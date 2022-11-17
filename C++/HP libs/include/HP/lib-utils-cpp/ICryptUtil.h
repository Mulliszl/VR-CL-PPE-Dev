// (c) Copyright 2021 HP Development Company, L.P.

#pragma once

#include <string>

namespace HP {
namespace Utils {

class ICryptUtil
{
public:
    virtual ~ICryptUtil() = default;

    virtual bool hasValidCert(const std::wstring& path) = 0;
    virtual std::string getCert(const uint16_t port) = 0;
    virtual std::string getCert(const std::wstring& path) = 0;
    virtual std::wstring getCertIssuer(const uint16_t port) = 0;
    virtual std::wstring getCertIssuer(const std::wstring& path) = 0;
    virtual std::wstring getCertSubject(const uint16_t port) = 0;
    virtual std::wstring getCertSubject(const std::wstring& path) = 0;
    virtual std::string getCertThumbprint(const uint16_t port) = 0;
    virtual std::string getCertThumbprint(const std::wstring& path) = 0;
};

}
}
