// (c) Copyright 2019-2021 HP Development Company, L.P.
#include<windows.h>


#include <omnicept/Glia.h>
#include <SessionLicenseHelper.h>
#include <ClientBuilderHelper.h>

using namespace HP::Omnicept;

/*! Omnicept Licensing Client will attempt to build an HP::Omnicept::Client with commnad line licensing arguments and will print out the result.
* - It will perform the following steps: 
*    1. Parse an HP::Omnicept::Abi::SessionLicense from command line arguments using SessionLicenseHelper.
*    2. Create an empty HP::Omnicept::Client::StateCallback_T function @warning This is not recommended in cases where the HP::Omnicept::Client is going to be used. 
*    3. Start building a HP::Omnicept::Client asynchronously, using the license and state callback.
*    4. Use ClientBuilderHelper to finish building the HP::Omnicept::Client.
*    5. Print a success message if the HP::Omnicept::Client was built successfully
*    6. Shut down the HP::Omnicept::Client with HP::Omnicept::Client::disconnectClient.
*/
int main(int argc, char** argv)
{
    std::vector<std::string> licenseCommandLineArgs(argv + 1, argv + argc);
    std::unique_ptr<Abi::SessionLicense> sessionLicense = SessionLicenseHelper::getSessionLicenseFromCommandLineArgs(licenseCommandLineArgs);

    if(sessionLicense == nullptr)
    {
        SessionLicenseHelper::printCommandHelp();
        return 1;
    }

    std::string clientName = "OmniceptLicensingClient";
    std::unique_ptr<Glia::AsyncClientBuilder> clientBuilder;
    Client::StateCallback_T emptyStateCallback = [](const Client::State state) {};

    try
    {
        clientBuilder = Glia::StartBuildClient_Async(clientName, std::move(sessionLicense), emptyStateCallback);
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
    std::cout << "Successfully connected to runtime" << std::endl;
    Sleep(2000);
    omniceptClient->disconnectClient();

    return 0;
}
