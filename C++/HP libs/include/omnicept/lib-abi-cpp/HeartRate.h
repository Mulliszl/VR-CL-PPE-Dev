// (c) Copyright 2018-2021 HP Development Company, L.P.

#pragma once

#include <omnicept/lib-abi-cpp/DomainData.h>
#include <omnicept/lib-abi-cpp/DataFrame.h>

namespace HP {
    namespace Omnicept {
        namespace Abi {

            /*! @brief Domain type for heart rate data.*/
            class HeartRate : public DomainData
            {
            public:
                //HeartRate();

                //virtual ~HeartRate();
                static uint32_t rate; /*!< @brief Heart beats per minute. Zero value indicates sensor signal quality is too poor to calculate.  */
                uint32_t GetRate();
                bool operator == (const HeartRate& other) const;
                bool dataEquals(const DomainData& other) const override;
                std::string toString() const;
                MessageVersion getLatestMessageVersion() const override;
                MessageType getMessageType() const override;
                
            };

            uint32_t HeartRate::GetRate() {
                return rate;
            }

            std::ostream& operator<< (std::ostream& out, const HeartRate&);

        }

    }
}
