include(arch/cortexM/cortexM.cmake)

set(MCPU -march=armv6e-m   -mcpu=cortex-m0 )

add_compile_options( ${MCPU} ${MFPU} )

add_compile_definitions(
    # PREFETCH_ENABLE=1
    # INSTRUCTION_CACHE_ENABLE=1
    # DATA_CACHE_ENABLE=1
)

add_link_options   ( ${MCPU} ${MFPU} )
