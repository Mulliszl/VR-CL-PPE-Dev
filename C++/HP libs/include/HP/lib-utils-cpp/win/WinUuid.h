// (c) Copyright 2021 HP Development Company, L.P.

#pragma once
#include <rpc.h>
#include <string>
#include <HP/lib-utils-cpp/IUuid.h>

namespace HP {
namespace Utils {

    struct WinUuid : public IUuid
    {
        WinUuid();
        WinUuid(const std::string&);
        virtual ~WinUuid() = default;

        virtual operator std::string() const override;

    protected:
        void init();
        void init(const std::string& uuid);

    private:
        UUID m_uuid;
        std::string m_asString;
    };

}
}
