// (c) Copyright 2021 HP Development Company, L.P.

#include <omnicept/Glia.h>
#include <SessionLicenseHelper.h>
#include <ClientBuilderHelper.h>
#include <mutex>
#include <condition_variable>

using namespace HP::Omnicept;

void printSubscriptionResults(std::shared_ptr<Abi::SubscriptionResultList> subResult);
/** Synchronization variables to communicate a HP::Omnicept::Client::State to the main thread. */
bool isConnectedToOmnicept = false;
bool receivedSubResultList = false;
std::mutex mtx;
std::condition_variable connectedAndWaitingForSubResult;

/*! Omnicept Subscriptions Client will subscribe to cognitive load version 1.1.0 messages and print the subscription list result. 
* - It will perform the following steps:
*    1. Parse an HP::Omnicept::Abi::SessionLicense from command line arguments using SessionLicenseHelper.
*    2. Create a HP::Omnicept::Client::StateCallback_T function to communicate the HP::Omnicept::Client's state to the main thread.
*    3. Start building a HP::Omnicept::Client asynchronously, using the license and state callback.
*    4. Use ClientBuilderHelper to finish building the HP::Omnicept::Client.
*    5. Registers a callback function for receiving an HP::Omnicept::Abi::SubscriptionResultList
*    5. Build an HP::Omnicept::Abi::SubscriptionList for CognitiveLoad message types with message version 1.1.0 a user can request and set them in the HP::Omnicept::Client with HP::Omnicept::Client::setSubscriptions.
*    6. Start the HP::Omnicept::Client to start receiving messages on it with HP::Omnicept::Client::startClient.
*    8. Waits until the HP::Omnicept::Abi::SubscriptionResultList callback is called is called, the client is disconnected or the wait timesout.
*    9. Shut down the HP::Omnicept::Client with HP::Omnicept::Client::disconnectClient.
*/

int main(int argc, char** argv)
{
    std::vector<std::string> licenseCommandLineArgs(argv + 1, argv + argc);
    std::unique_ptr<Abi::SessionLicense> sessionLicense = SessionLicenseHelper::getSessionLicenseFromCommandLineArgs(licenseCommandLineArgs);
    if (sessionLicense == nullptr)
    {
        SessionLicenseHelper::printCommandHelp();
        return 1;
    }

    std::string clientName = "OmniceptMessageVersionClient";
    std::unique_ptr<Glia::AsyncClientBuilder> clientBuilder;

    Client::StateCallback_T stateCallback = [](const Client::State state)
    {
        std::unique_lock<std::mutex> lock{ mtx };
        if (state == Client::State::RUNNING || state == Client::State::PAUSED)
        {
            isConnectedToOmnicept = true;
        }
        else if (state == Client::State::DISCONNECTED)
        {
            std::cerr << "Omnicept Client Disconnected." << std::endl;
            isConnectedToOmnicept = false;
        }
        lock.unlock();
        connectedAndWaitingForSubResult.notify_one();
    };
    
    try
    {
        clientBuilder = Glia::StartBuildClient_Async(clientName, std::move(sessionLicense), stateCallback);
    }
    catch (const std::invalid_argument& e)
    {
        std::cerr << "Failed to start asynchronously connecting to Omnicept. invalid_argument: " << e.what() << std::endl;
        return 1;
    }

    std::unique_ptr<Client> omniceptClient;


    ClientBuilderHelper::BuildResult cbResult = ClientBuilderHelper::finishBuildingClientHandleExceptions(std::move(clientBuilder));

    if (cbResult.client == nullptr)
    {
        std::cerr << "Error building client: " << cbResult.errorMsg << std::endl;
        return 1;
    }

    omniceptClient = std::move(cbResult.client);

    std::shared_ptr<Abi::SubscriptionList> subList = Abi::SubscriptionList::GetSubscriptionListToNone();
   
    omniceptClient->registerCallback<Abi::SubscriptionResultList>(printSubscriptionResults);

    /*Note: will be rejected for clients with core licenses*/
    Abi::Subscription cogLoadSub = Abi::Subscription::generateSubscriptionForDomainType<Abi::CognitiveLoad>();
    
    /* Note: when subscribing to a message an empty version semantic will subscribe to the latest message version available.
       It is recommended to leave it empty unless your client depends on a specific version of a message.
    */
    cogLoadSub.setVersionSemantic(Abi::MessageVersionSemantic("1.1.0"));
    subList->getSubscriptions().push_back(cogLoadSub);

    omniceptClient->setSubscriptions(*subList);
    omniceptClient->startClient();

    std::unique_lock<std::mutex> lock{ mtx };
    if (isConnectedToOmnicept && !receivedSubResultList)
    {
        connectedAndWaitingForSubResult.wait_for
        (
            lock, std::chrono::milliseconds{ 500 },
            []()
            {
                return !isConnectedToOmnicept || receivedSubResultList;
            }
        );
    }
    lock.unlock();
    omniceptClient->disconnectClient();
    return 0;
}


void printSubscriptionResults(std::shared_ptr<Abi::SubscriptionResultList> subResultList)
{
    std::unique_lock<std::mutex> lock{ mtx };
    for (const Abi::SubscriptionResult& subResult : subResultList->getSubscriptionResults())
    {
        if (subResult.result == Abi::SubscriptionResultType::APPROVED)
        {
            std::cout << "Subscription Approved: " << subResult << std::endl;
        }
        else if (subResult.result == Abi::SubscriptionResultType::REJECTED)
        {
            std::cout << "Subscription Rejected: " << subResult << std::endl;
        }
        else if (subResult.result == Abi::SubscriptionResultType::PENDING)
        {
            std::cout << "Subscription Pending: " << subResult << std::endl;
        }
    }
    receivedSubResultList = true;
    lock.unlock();
    connectedAndWaitingForSubResult.notify_one();
}
