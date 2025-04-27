include(arch/cortexM/cortexM.cmake)

set(MCPU -march=armv7-m -mcpu=cortex-m3)
set(FCPU -mfloat-abi=soft)

add_compile_options(
    ${MCPU} ${MFPU}
)

add_compile_definitions(
)

add_link_options(
    ${MCPU} ${MFPU}
)
