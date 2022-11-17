// (c) Copyright 2021 HP Development Company, L.P.

#pragma once
#include <omnicept/Glia.h>
#include <string>
#include <memory>

/*! @brief Utility class to take an asynchronously building HP::Omnicept::Client and finishes building it, handling all exceptions that may get thrown. 
*
*/
class ClientBuilderHelper final
{
private:
    ClientBuilderHelper() = delete;
    ~ClientBuilderHelper() = delete;
public:
    
    /*! @brief Stores the result of HP::Omnicept::Glia::AsyncClientBuilder::getBuildClientResultOrThrow
    * 
    */
    struct BuildResult
    {
        std::unique_ptr<HP::Omnicept::Client> client; /*!< @brief A client. Will be a nullptr if an exception occurred. */
        std::string errorMsg; /*!< @brief Message of exception that was thrown when building the HP::Omnicept::Client, will be empty if building the client was successful. */
    };

    /*! @brief Wraps HP::Omnicept::Glia::AsyncClientBuilder::getBuildClientResultOrThrow
    *   catching all exceptions that can be thrown, and stores the result in a ClientBuilderHelper::BuildResult
    *   
    * @param builder An in-progress client builder
    * @returns ClientBuilderHelper::BuildResult which stores the result of HP::Omnicept::Glia::AsyncClientBuilder::getBuildClientResultOrThrow
    */
    static BuildResult finishBuildingClientHandleExceptions(std::unique_ptr<HP::Omnicept::Glia::AsyncClientBuilder> builder);
};
