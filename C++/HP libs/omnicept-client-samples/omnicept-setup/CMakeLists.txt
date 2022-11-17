set(target "omnicept-setup")

add_executable(${target} OmniceptSetup.cpp)

target_link_libraries(${target} PRIVATE HPOmnicept::lib)
