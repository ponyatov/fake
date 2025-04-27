include(arch/cortexM/cortexM.cmake)

set(MCPU -march=armv7e-m   -mcpu=cortex-m4)
set(FCPU -mfpu=fpv4-sp-d16 -mfloat-abi=hard)

add_compile_options(
    ${MCPU} ${MFPU}
)

add_compile_definitions(
)

add_link_options(
    ${MCPU} ${MFPU}
)
