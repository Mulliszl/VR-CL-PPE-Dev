set(target "omnicept-shared")

add_library(${target}
    SessionLicenseHelper.cpp
    SessionLicenseHelper.h
    ClientBuilderHelper.cpp
    ClientBuilderHelper.h
)

target_include_directories(${target} PUBLIC .)
target_link_libraries(${target} PRIVATE HPOmnicept::lib)
