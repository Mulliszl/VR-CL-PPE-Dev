// (c) Copyright 2021 HP Development Company, L.P.

#pragma once

#include "HP/lib-utils-cpp/IClientUtil.h"

#include <stdint.h>

#include <string>

namespace HP {
namespace Utils {

class ClientUtil : public IClientUtil
{
public:
    uint64_t getPid(const uint16_t port) override;
    std::wstring getPath(const uint16_t port) override;
    std::string getHash(const uint16_t port) override;
    std::string getHash(const std::wstring& path) override;
};

}
}