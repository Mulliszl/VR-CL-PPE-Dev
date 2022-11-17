// (c) Copyright 2021 HP Development Company, L.P.

#include <Glia.h>
#include <SessionLicenseHelper.h>
#include <ClientBuilderHelper.h>

#include <vector>
#include <sstream>
#include <mutex>
#include <condition_variable>

using namespace HP::Omnicept;
std::vector<std::string> getTypesToPrintFromConsole();
template<class DomainType> void printCallback(std::shared_ptr<DomainType> toPrint);

std::mutex connectedMtx;
std::condition_variable connectedToOmnicept;
bool isConnectedToOmnicept = false;

/*! Omnicept Callback Client will print messages as they are received by registering callback functions for user requested DomainType messages
* - It will perform the following steps:
*    1. Parse an HP::Omnicept::Abi::SessionLicense from command line arguments using SessionLicenseHelper.
*    2. Create a HP::Omnicept::Client::StateCallback_T function to communicate the HP::Omnicept::Client's state to the main thread.
*    3. Start building a HP::Omnicept::Client asynchronously, using the license and state callback.
*    4. Use ClientBuilderHelper to finish building the HP::Omnicept::Client.
*    5. Prompt the user for messages they would like to receive.
*    6. Register a print callback function in the HP::Omnicept::Client for message types requested by the user.
*    7. Build an HP::Omnicept::Abi::SubscriptionList for all message types requested by the set them in the HP::Omnicept::Client with HP::Omnicept::Client::setSubscriptions.
*    8. Start the HP::Omnicept::Client to start receiving messages on it with HP::Omnicept::Client::startClient.
*    9. Make the main thread wait for the HP::Omnicept::Client to be disconnected from the %HP Omnicept Runtime or for the user to hit ctrl-c.
*    10. Shut down the HP::Omnicept::Client with HP::Omnicept::Client::disconnectClient.
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

    std::string clientName = "OmniceptCallbackClient";
    std::unique_ptr<Glia::AsyncClientBuilder> clientBuilder;

    Client::StateCallback_T stateCallback = [](const Client::State state)
    {
        std::unique_lock<std::mutex> lock{ connectedMtx };
        if (state == Client::State::RUNNING || state == Client::State::PAUSED)
        {
            isConnectedToOmnicept = true;
        }
        else if (state == Client::State::DISCONNECTED)
        {
            isConnectedToOmnicept = false;
        }
        lock.unlock();
        connectedToOmnicept.notify_all();
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

    std::vector<std::string> typesToPrint = getTypesToPrintFromConsole();

    std::shared_ptr<Abi::SubscriptionList> subList = Abi::SubscriptionList::GetSubscriptionListToNone();

    for (const auto& domainType : typesToPrint)
    {
        if (domainType == "h")
        {
            omniceptClient->registerCallback<Abi::HeartRate>(printCallback<Abi::HeartRate>);
            Abi::Subscription hrSub = Abi::Subscription::generateSubscriptionForDomainType<Abi::HeartRate>();
            subList->getSubscriptions().push_back(hrSub);
        }
        else if (domainType == "et")
        {
            omniceptClient->registerCallback<Abi::EyeTracking>(printCallback<Abi::EyeTracking>);
            Abi::Subscription eyeTrackingSub = Abi::Subscription::generateSubscriptionForDomainType<Abi::EyeTracking>();
            subList->getSubscriptions().push_back(eyeTrackingSub);
        }
        else if (domainType == "c")
        {
            omniceptClient->registerCallback<Abi::CognitiveLoad>(printCallback<Abi::CognitiveLoad>);
            Abi::Subscription cogLoadSub = Abi::Subscription::generateSubscriptionForDomainType<Abi::CognitiveLoad>();
            subList->getSubscriptions().push_back(cogLoadSub);
        }
        else if (domainType == "s")
        {
            omniceptClient->registerCallback<Abi::SubscriptionResultList>(printCallback<Abi::SubscriptionResultList>);
            Abi::Subscription subResultSubscription = Abi::Subscription::generateSubscriptionForDomainType<Abi::SubscriptionResultList>();
            subList->getSubscriptions().push_back(subResultSubscription);
        }
        else
        {
            std::cerr << "Cannot register callback for unrecognized type " << domainType << std::endl;
        }
    }

    omniceptClient->setSubscriptions(*subList);
    omniceptClient->startClient();

    //block until client is disconnected from omnicept
    std::unique_lock<std::mutex> disconnectLock{ connectedMtx };
    connectedToOmnicept.wait(disconnectLock, [] { return !isConnectedToOmnicept; });
    disconnectLock.unlock();

    omniceptClient->disconnectClient();
    std::cout << "Omnicept Runtime stopped client." << std::endl;

    return 0;
}

template<class DomainType>
void printCallback(std::shared_ptr<DomainType> toPrint)
{
    std::cout << *toPrint << std::endl;
}

std::vector<std::string> getTypesToPrintFromConsole()
{
    std::cout
        << "Enter all message types for continuous receiving delimited by a space. " << std::endl
        << "You will not receive messages you are not licensed for." << std::endl
        << "  (example: h et c s)" << std::endl
        << "    [h|et|c|s]" << std::endl
        << "    'h' for heart rate data" << std::endl
        << "    'et' for eye tracking data" << std::endl
        << "    'c' for cognitive load" << std::endl
        << "    's' for subscription result list" << std::endl
        << std::endl;

    std::vector<std::string> domainTypes;
    std::string input;
    std::getline(std::cin, input);
    std::stringstream sstream{ input };
    std::string domainType;
    while (std::getline(sstream, domainType, ' '))
    {
        domainTypes.push_back(domainType);
    }

    std::cout << std::endl << "Beginning continuous stream." << std::endl
        << "If some data is missing, check if you're licensed for that type of message." << std::endl
        << "Press ctrl+c to exit." << std::endl << std::endl;
    return domainTypes;
}