set(target "omnicept-subscriptions")

add_executable(${target}
    OmniceptSubscriptions.cpp
)

target_include_directories(${target} PRIVATE .)
target_link_libraries(${target} PRIVATE HPOmnicept::lib)
target_link_libraries(${target} PRIVATE omnicept-shared)

add_custom_command(TARGET ${target} POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:HPOmnicept::jsoncpp> $<TARGET_FILE_DIR:${target}>
    COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:HPOmnicept::zeromq> $<TARGET_FILE_DIR:${target}>
    COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:HPOmnicept::log4cplus> $<TARGET_FILE_DIR:${target}>
)
