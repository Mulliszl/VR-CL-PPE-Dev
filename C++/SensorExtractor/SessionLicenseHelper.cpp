// (c) Copyright 2019-2021 HP Development Company, L.P.

#include "SessionLicenseHelper.h"
#include <iostream>

using namespace HP::Omnicept::Abi;
void SessionLicenseHelper::printCommandHelp()
{
    std::cout
        << "Usage: " << std::endl
        << "    --clientId <client id> --accessKey <access key> --licenseModel <license model number=[ 1|2|3|4 ]>" << std::endl
        << "    '--clientId' <client id>" << std::endl
        << "    '--accessKey' <access key>" << std::endl
        << "    '--licenseModel' <license model number=[ 1|2|3|4 ]>. License model option: CORE = 1, TRIAL = 2, ENTERPRISE = 3, REV_SHARE = 4" << std::endl
        << "    e.g." << std::endl
        << "        OmniceptConsole.exe --clientId myId --accessKey myKey --licenseModel 1" << std::endl
        << "  If no options are given, only CORE messages can be printed." << std::endl;
}

std::unique_ptr<SessionLicense> SessionLicenseHelper::getSessionLicenseFromCommandLineArgs(const std::vector<std::string>& licenseCommandLineArgs)
{
    bool gotIllegalArguments = false;
    std::string clientId;
    std::string accessKey;
    LicensingModel requestedLicense = LicensingModel::CORE;
    std::unique_ptr<SessionLicense> sessionLicense = nullptr;
    const int licenseEnumMax = 4;
    const int licenseEnumMin = 0;

    std::string value = std::string();

    for (size_t index = 0; index < licenseCommandLineArgs.size(); ++index)
    {
        value = licenseCommandLineArgs[index];

        if (value == "--clientId")
        {
            if ((index + 1) < licenseCommandLineArgs.size())
            {
                clientId = licenseCommandLineArgs[index + 1];
                ++index;
            }
        }
        else if (value == "--accessKey")
        {
            if ((index + 1) < licenseCommandLineArgs.size())
            {
                accessKey = licenseCommandLineArgs[index + 1];
                ++index;
            }
        }
        else if (value =="--licenseModel")
        {
            if ((index + 1) < licenseCommandLineArgs.size())
            {
                int licenseIntValue = std::stoi(licenseCommandLineArgs[index + 1], nullptr);
                if(licenseEnumMin <= licenseIntValue && licenseIntValue <= licenseEnumMax )
                {
                    requestedLicense = static_cast<LicensingModel>(licenseIntValue);
                }
                else
                {
                    std::cerr << "Error: Unrecognized license model " << licenseIntValue << std::endl;
                    break;
                }
                ++index;
            }
        }
        else
        {
            std::cerr << "Error: Unrecognized argument: " << value << std::endl;
            gotIllegalArguments = true;
            break;
        }
    }

    const bool gaveClientIdButNotAccessKey = !clientId.empty() && accessKey.empty();
    const bool gaveAccessKeyButNotClientId = clientId.empty() && !accessKey.empty();
    const bool requestedLicensedFeaturesWithoutClientInfo = requestedLicense != LicensingModel::CORE
        && (clientId.empty() || accessKey.empty());

    if (requestedLicensedFeaturesWithoutClientInfo)
    {
        std::cerr << "Error: The license type requested requires client information, but not all information required to checkout that license was provided" << std::endl;
        gotIllegalArguments = true;
    }

    if (gaveClientIdButNotAccessKey)
    {
        std::cerr << "Error: A clientId was provided, but an accessKey was not" << std::endl;
        gotIllegalArguments = true;
    }

    if (gaveAccessKeyButNotClientId)
    {
        std::cerr << "Error: An accessKey was provided, but a clientId was not" << std::endl;
        gotIllegalArguments = true;
    }

    if (!gotIllegalArguments)
    {
        sessionLicense = std::make_unique<SessionLicense>(clientId, accessKey, requestedLicense, true);
    }

    return sessionLicense;
}
