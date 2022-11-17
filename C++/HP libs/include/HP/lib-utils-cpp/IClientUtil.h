// (c) Copyright 2021 HP Development Company, L.P.

#pragma once

#include <string>

namespace HP {
namespace Utils {

class IClientUtil
{
public:
    virtual ~IClientUtil() = default;

    virtual uint64_t getPid(const uint16_t port) = 0;
    virtual std::wstring getPath(const uint16_t port) = 0;
    virtual std::string getHash(const uint16_t port) = 0;
    virtual std::string getHash(const std::wstring& path) = 0;
};

}
}
