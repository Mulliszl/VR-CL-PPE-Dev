// (c) Copyright 2019-2021 HP Development Company, L.P.

#include <omnicept/Glia.h>
#include <SessionLicenseHelper.h>
#include <ClientBuilderHelper.h>
#include <atomic>

using namespace HP::Omnicept;

void inputPrompt();
void printLastValueCachedForType(const std::string& domainType, Client& client);
template<class DomainType> void printLastValueCached(Client& client);

/** Synchronization variables to communicate a HP::Omnicept::Client::State to the main thread. */
std::atomic_bool isConnectedToOmnicept = false;

/*! Omnicept Last Value Cached Client will print DomainType messages upon user request.
* - It will perform the following steps:
*    1. Parse an HP::Omnicept::Abi::SessionLicense from command line arguments using SessionLicenseHelper.
*    2. Create a HP::Omnicept::Client::StateCallback_T function to communicate the HP::Omnicept::Client's state to the main thread.
*    3. Start building a HP::Omnicept::Client asynchronously, using the license and state callback.
*    4. Use ClientBuilderHelper to finish building the HP::Omnicept::Client.
*    5. Build an HP::Omnicept::Abi::SubscriptionList for all message types a user can request and set them in the HP::Omnicept::Client with HP::Omnicept::Client::setSubscriptions.
*    6. Start the HP::Omnicept::Client to start receiving messages on it with HP::Omnicept::Client::startClient.
*    7. Prompt the user for messages they would like to receive.
*    8. Prints the HP::Omnicept::Client::LastValueCached for the message type requested by the user, if one has been received.
*    9. Runs steps 7 and 8 until the HP::Omnicept::Client is disconnected from the %HP Omnicept Runtime or the user quits.
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

    std::string clientName = "OmniceptLastValueCachedClient";
    std::unique_ptr<Glia::AsyncClientBuilder> clientBuilder;

    Client::StateCallback_T stateCallback = [&](const Client::State state)
    {
        if (state == Client::State::RUNNING || state == Client::State::PAUSED)
        {
            isConnectedToOmnicept = true;
        }
        else if (state == Client::State::DISCONNECTED)
        {
            std::cerr << "Omnicept Client Disconnected." << std::endl;
            isConnectedToOmnicept = false;
        }
    };
    
    try
    {
        clientBuilder = Glia::StartBuildClient_Async(clientName, std::move(sessionLicense), stateCallback);
        sessionLicense.reset();
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
    
    Abi::Subscription hrSub = Abi::Subscription::generateSubscriptionForDomainType<Abi::HeartRate>();
    subList->getSubscriptions().push_back(hrSub);

    Abi::Subscription eyeTrackingSub = Abi::Subscription::generateSubscriptionForDomainType<Abi::EyeTracking>();
    subList->getSubscriptions().push_back(eyeTrackingSub);

    Abi::Subscription cogLoadSub = Abi::Subscription::generateSubscriptionForDomainType<Abi::CognitiveLoad>();
    subList->getSubscriptions().push_back(cogLoadSub);

    omniceptClient->setSubscriptions(*subList);
    omniceptClient->startClient();

    inputPrompt();
    std::string input;
    std::cin >> input;

    while (input != "q" && isConnectedToOmnicept)
    {
        std::cout << std::endl;
        printLastValueCachedForType(input, *omniceptClient);
        inputPrompt();
        std::cin >> input;
    }
    return 0;
}

void inputPrompt()
{
    std::cout
        << "Usage: " << std::endl
        << "    [h|et|c|q]" << std::endl
        << "    'h' for heart rate data" << std::endl
        << "    'et' for eye tracking data" << std::endl
        << "    'c' for cognitive load" << std::endl
        << "    'q' to quit" << std::endl << std::endl;
}

void printLastValueCachedForType(const std::string& domainType, Client& client)
{

    if (domainType == "h")
    {
        printLastValueCached<Abi::HeartRate>(client);
    }
    else if (domainType == "et")
    {
        printLastValueCached<Abi::EyeTracking>(client);
    }
    else if (domainType == "c")
    {
        printLastValueCached<Abi::CognitiveLoad>(client);
    }
    else
    {
        std::cerr << "input " << domainType << " does not map to a valid data type" << std::endl;
    }
}

template<class DomainType>
void printLastValueCached(Client& client)
{
    Client::LastValueCached<DomainType> lvc = client.getLastData<DomainType>();
    if (lvc.valid)
    {
        std::cout << lvc.data << std::endl;
    }
    else
    {
        std::cout << "No messages received. Are you licensed for this type of message?" << std::endl;
    }
}
