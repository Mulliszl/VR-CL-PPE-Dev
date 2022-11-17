// (c) Copyright 2021 HP Development Company, L.P.

#pragma once

#include <omnicept/lib-abi-cpp/DomainData.h>
#include <omnicept/lib-abi-cpp/DataFrame.h>
#include <map>

namespace HP {
namespace Omnicept {
namespace Abi {

    enum class Blendshape : uint32_t
    {
        none = 0 // Spike to identify enumerations for blendshapes (Apple vs. Reallusion vs. Wolf3d/Ready-player-me)
    };

    enum class MacroExpressionStates : uint32_t
    {
        smiling = 0,
        frowning = 1,
        talking = 2
    };
    /*! @brief Domain type for Expression data.

     */
    class Expression : public DomainData
    {

    public:
        Expression();
        std::map<Blendshape, float> blendshapes = {
            {Blendshape::none, 0.0F}
        };
        std::map<MacroExpressionStates, float> macroExpressions{
            {MacroExpressionStates::smiling, 0.0F},
            {MacroExpressionStates::frowning, 0.0F},
            {MacroExpressionStates::talking, 0.0F}
        };

        virtual ~Expression() = default;

        bool operator == (const Expression& other) const;
        bool dataEquals(const DomainData& other) const override;
        std::string toString() const;
        MessageVersion getLatestMessageVersion() const override;
        MessageType getMessageType() const override;
    };

    std::ostream& operator<< (std::ostream &out, const Expression&);

    /*! @brief Domain type for Expression Frames.
    */
    class ExpressionFrame : public DataFrame<Expression>
    {
    public:
        ExpressionFrame();
        virtual ~ExpressionFrame() = default;
        MessageType getMessageType() const override;
        MessageVersion getLatestMessageVersion() const override;
    };
}
}
}