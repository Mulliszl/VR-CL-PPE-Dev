// (c) Copyright 2021 HP Development Company, L.P.

#pragma once
#include <omnicept/lib-abi-cpp/MessageVersion.h>
#include <string>

namespace HP {
namespace Omnicept {
namespace Abi {

    /*! @brief Message version semantic class for storing version specification logics.*/
    class MessageVersionSemantic
    {
    public:
    
        explicit MessageVersionSemantic(const std::string& semantic);
        MessageVersionSemantic(const MessageVersion& semantic);
        
        virtual ~MessageVersionSemantic() = default;

        /*! @brief Gets the version in std::string.
        */
        virtual std::string toString() const;

        /*! @brief Check a MessageVersion matches the semantic logic.
*/
        bool satisfy(const HP::Omnicept::Abi::MessageVersion &messageVer) const;

    protected:
        std::string m_versionSemanticString;

        bool checkFormat(const std::string &semantic) const;
        bool checkFormatHelper(std::string::const_iterator begin, std::string::const_iterator end) const;
        bool isOperator(std::string::const_iterator op, int len, bool allowOr = true) const;
        bool checkTailingIsOperator(std::string::const_iterator begin, std::string::const_iterator end) const;
        std::string trim(std::string str) const;
    };

    namespace MessageVersionSemanticConstants
    {
        const MessageVersionSemantic MessageVersionSemantic_WildCard{ "" };
    }
}

}
}