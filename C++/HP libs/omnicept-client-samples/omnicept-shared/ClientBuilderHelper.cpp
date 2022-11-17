// (c) Copyright 2021 HP Development Company, L.P.

#include "ClientBuilderHelper.h"

using namespace HP::Omnicept;

ClientBuilderHelper::BuildResult ClientBuilderHelper::finishBuildingClientHandleExceptions(std::unique_ptr<Glia::AsyncClientBuilder> builder)
{
    BuildResult result;
    if (builder == nullptr)
    {
        result.errorMsg = "Error builder was a nullptr";
    }
    else
    {
        try
        {
            result.client = builder->getBuildClientResultOrThrow();
            builder.reset();
        }
        catch (const Abi::HandshakeError& e)
        {
            result.errorMsg = "Could not connect to runtime HandshakeError : " + std::string{ e.what() };
        }
        catch (const Abi::TransportError& e)
        {
            result.errorMsg = "Could not connect to runtime TransportError : " + std::string{ e.what() };
        }
        catch (const Abi::ProtocolError& e)
        {
            result.errorMsg = "Could not connect to runtime ProtocolError : " + std::string{ e.what() };
        }
        catch (const std::invalid_argument& e)
        {
            result.errorMsg = "Could not connect to runtime invalid_argument : " + std::string{ e.what() };
        }
        catch (const std::logic_error& e)
        {
            result.errorMsg = "Could not connect to runtime logic_error : " + std::string{ e.what() };
        }
    }
    return result;
}
