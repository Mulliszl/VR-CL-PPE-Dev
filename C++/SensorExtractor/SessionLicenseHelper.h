// (c) Copyright 2019-2021 HP Development Company, L.P.

#include <omnicept/Glia.h>
#include <vector>
#include <memory>

/*! @brief Utility class to prompt the user for formatted license input, and parse an HP::Omnicept::Abi::SessionLicense from command line arguments.
*
*/
class SessionLicenseHelper final
{
    SessionLicenseHelper() = delete;
    ~SessionLicenseHelper() = delete;
public:
    /*! @brief Prints the required format for license information to be correctly parsed.
    */
    static void printCommandHelp();

    /*! @brief Parses command line input for an HP::Omnicept::Abi::SessionLicense
    *
    * @param licenseCommandLineArgs a collection of strings that can be parsed into an HP::Omnicept::Abi::SessionLicense.
    * @returns an HP::Omnicept::Abi::SessionLicense if \p licenseCommandLineArgs was correctly formatted, or a nullptr if it was not. 
    */
    static std::unique_ptr<HP::Omnicept::Abi::SessionLicense> getSessionLicenseFromCommandLineArgs(const std::vector<std::string>& licenseCommandLineArgs);
};
