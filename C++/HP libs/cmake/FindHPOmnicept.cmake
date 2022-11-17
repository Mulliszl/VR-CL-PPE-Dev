# (c) Copyright 2020-2021 HP Development Company, L.P.

# - Try to find the HPOmnicept library
# Once done this will define
#
#  HPOmnicept_FOUND - system has HPOmnicept
#  HPOmnicept_LIB - HPOmnicept library to link
#  HPOmnicept_INCLUDE_DIR - HPOmnicept include directory
#  HPOmnicept::lib - HPOmnicept library target
#  HPOmnicept::jsoncpp - jsoncpp library target (dependency)
#  HPOmnicept::zeromq - zeromq library target (dependency)
#  HPOmnicept::log4cplus - log4cplus library target (dependency)

option(USE_OMNICEPT_EXPERIMENTAL "Should Omnicept experimental features be enabled?" OFF)

if(USE_OMNICEPT_EXPERIMENTAL)
  add_definitions(-DOMNICEPT_EXPERIMENTAL)
endif()

if(HPOmnicept_FOUND)
    return()
endif()

find_library(HPOmnicept_LIB_DEBUG
    NAMES hp_omniceptd.lib
    PATHS "${CMAKE_CURRENT_LIST_DIR}/../lib/Debug/msvc2019_64"
)

find_library(HPOmnicept_LIB_RELEASE
    NAMES hp_omnicept.lib
    PATHS "${CMAKE_CURRENT_LIST_DIR}/../lib/Release/msvc2019_64"
)

set(HPOmnicept_LIB
    debug ${HPOmnicept_LIB_DEBUG}
    optimized ${HPOmnicept_LIB_RELEASE}
)

set(HPOmnicept_INCLUDE_DIR "${CMAKE_CURRENT_LIST_DIR}/../include")

include(FindPackageHandleStandardArgs)
find_package_handle_standard_args(
    HPOmnicept DEFAULT_MSG
    HPOmnicept_LIB HPOmnicept_INCLUDE_DIR
)
mark_as_advanced(HPOmnicept_LIB HPOmnicept_INCLUDE_DIR)

if(HPOmnicept_FOUND AND NOT TARGET HPOmnicept::lib)
    add_library(HPOmnicept::lib STATIC IMPORTED)

    set_target_properties(HPOmnicept::lib PROPERTIES
        INTERFACE_INCLUDE_DIRECTORIES "${HPOmnicept_INCLUDE_DIR}"
        IMPORTED_LOCATION_DEBUG "${HPOmnicept_LIB_DEBUG}"
        IMPORTED_LOCATION_RELEASE "${HPOmnicept_LIB_RELEASE}"
        MAP_IMPORTED_CONFIG_MINSIZEREL Release
        MAP_IMPORTED_CONFIG_RELWITHDEBINFO Release
    )

    target_link_libraries(HPOmnicept::lib
        INTERFACE wintrust
        INTERFACE Pathcch
        INTERFACE wbemuuid
        INTERFACE Rpcrt4
        INTERFACE Version
        INTERFACE bcrypt
        INTERFACE crypt32
        INTERFACE iphlpapi
        INTERFACE ws2_32
    )
endif()

if(HPOmnicept_FOUND AND NOT TARGET HPOmnicept::jsoncpp)
    add_library(HPOmnicept::jsoncpp SHARED IMPORTED)
    set_target_properties(HPOmnicept::jsoncpp PROPERTIES
        IMPORTED_IMPLIB_DEBUG "${HPOmnicept_LIB_DEBUG}"
        IMPORTED_LOCATION_DEBUG "${CMAKE_CURRENT_LIST_DIR}/../bin/Debug/jsoncpp.dll"
        IMPORTED_IMPLIB_RELEASE "${HPOmnicept_LIB_RELEASE}"
        IMPORTED_LOCATION_RELEASE "${CMAKE_CURRENT_LIST_DIR}/../bin/Release/jsoncpp.dll"
        MAP_IMPORTED_CONFIG_MINSIZEREL Release
        MAP_IMPORTED_CONFIG_RELWITHDEBINFO Release
    )
endif()

if(HPOmnicept_FOUND AND NOT TARGET HPOmnicept::zeromq)
    add_library(HPOmnicept::zeromq SHARED IMPORTED)
    set_target_properties(HPOmnicept::zeromq PROPERTIES
        IMPORTED_IMPLIB_DEBUG "${HPOmnicept_LIB_DEBUG}"
        IMPORTED_LOCATION_DEBUG "${CMAKE_CURRENT_LIST_DIR}/../bin/Debug/libzmq-mt-gd-4_3_3.dll"
        IMPORTED_IMPLIB_RELEASE "${HPOmnicept_LIB_RELEASE}"
        IMPORTED_LOCATION_RELEASE "${CMAKE_CURRENT_LIST_DIR}/../bin/Release/libzmq-mt-4_3_3.dll"
        MAP_IMPORTED_CONFIG_MINSIZEREL Release
        MAP_IMPORTED_CONFIG_RELWITHDEBINFO Release
    )
endif()

if(HPOmnicept_FOUND AND NOT TARGET HPOmnicept::log4cplus)
    add_library(HPOmnicept::log4cplus SHARED IMPORTED)
    set_target_properties(HPOmnicept::log4cplus PROPERTIES
        IMPORTED_IMPLIB_DEBUG "${HPOmnicept_LIB_DEBUG}"
        IMPORTED_LOCATION_DEBUG "${CMAKE_CURRENT_LIST_DIR}/../bin/Debug/log4cplusUD.dll"
        IMPORTED_IMPLIB_RELEASE "${HPOmnicept_LIB_RELEASE}"
        IMPORTED_LOCATION_RELEASE "${CMAKE_CURRENT_LIST_DIR}/../bin/Release/log4cplusU.dll"
        MAP_IMPORTED_CONFIG_MINSIZEREL Release
        MAP_IMPORTED_CONFIG_RELWITHDEBINFO Release
    )
endif()
