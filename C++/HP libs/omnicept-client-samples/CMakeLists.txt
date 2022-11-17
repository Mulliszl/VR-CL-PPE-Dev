cmake_minimum_required(VERSION 3.14)

set(OMNICEPT_SAMPLES_PROJECT_NAME "omnicept-client-samples")
project(${OMNICEPT_SAMPLES_PROJECT_NAME})

if(DEFINED ENV{HP_OMNICEPT_SDK_INSTALL_PATH})
    set(HP_OMNICEPT_SDK_INSTALL "$ENV{HP_OMNICEPT_SDK_INSTALL_PATH}" CACHE STRING "Install location of the HP Omnicept SDK.")
    message(STATUS "HP Omnicept SDK install location set to: ${HP_OMNICEPT_SDK_INSTALL}")
elseif(DEFINED ENV{HP_OMNICEPT_INSTALL}) 
    set(HP_OMNICEPT_SDK_INSTALL "$ENV{HP_OMNICEPT_INSTALL}" CACHE STRING "Install location of the HP Omnicept SDK.")
    message(STATUS "HP Omnicept SDK install location set to: ${HP_OMNICEPT_SDK_INSTALL}")
else()
    message(FATAL_ERROR "ERROR: HP Omnicept SDK environment variable is not set.")
endif()

list(APPEND CMAKE_MODULE_PATH "${HP_OMNICEPT_SDK_INSTALL}/cmake")
find_package(HPOmnicept REQUIRED)

add_subdirectory(omnicept-shared)
add_subdirectory(omnicept-setup)
add_subdirectory(omnicept-lvc)
add_subdirectory(omnicept-callback)
add_subdirectory(omnicept-licensing)
add_subdirectory(omnicept-subscriptions)
