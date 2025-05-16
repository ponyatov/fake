#![no_std]
#![no_main]
// #![allow(unused_variables)]
#![allow(dead_code)]
#![allow(unused_imports)]

// pick a panicking behavior
// use panic_halt as _; // you can put a breakpoint on `rust_begin_unwind` to catch panics
// use panic_abort as _; // requires nightly
// use panic_itm as _; // logs messages over ITM; requires ITM support
use panic_semihosting as _; // logs messages to the host stderr; requires a debugger

use cortex_m::asm;
use cortex_m_rt::entry;

// #[cfg(target_arch = "arm")]
fn main() -> ! {
    asm::nop(); // To not have main optimize to abort in release mode, remove when you add code

    loop {
        // your code goes here
    }
}

#[cfg(target_os = "linux")]
fn main() {
    // \ args
    let argv: Vec<String> = std::env::args().collect();
    let argc = argv.len();
    let ini = if argc > 1 { &argv[1] } else { "lib/fake.ini" };
    let src = std::fs::read_to_string(ini).unwrap();
    println!("#{:?} {:?} -> {:?}", argc, argv, ini);
    // / args
    println!("\n{:?}", src);
}
