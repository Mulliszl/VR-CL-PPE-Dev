// (c) Copyright 2021 HP Development Company, L.P.

#pragma once

#include "HP/lib-utils-cpp/ICryptUtil.h"
#include <windows.h>
#include <Wincrypt.h>
#include <string>
#include <memory>
#include <functional>
namespace HP {
namespace Utils {

class CryptUtil : public ICryptUtil
{
public:
    bool hasValidCert(const std::wstring& path) override;
    std::string getCert(const uint16_t port) override;
    std::string getCert(const std::wstring& path) override;
    std::wstring getCertIssuer(const std::wstring& path) override;
    std::wstring getCertIssuer(const uint16_t port) override;
    std::wstring getCertSubject(const std::wstring& path) override;
    std::wstring getCertSubject(const uint16_t port) override;
    std::string getCertThumbprint(const std::wstring& path) override;
    std::string getCertThumbprint(const uint16_t port) override;

private:
    std::unique_ptr<const CERT_CONTEXT, std::function<void(const CERT_CONTEXT*)>> getCertContextPtr(const std::wstring& path);
    std::wstring getCertAttribute(PCCERT_CONTEXT pCertContext, DWORD dwFlags);
};

}
}