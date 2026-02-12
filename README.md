
# Run

## From GitHub

You can download the [latest](https://github.com/tversteeg/castle-game/releases/latest) version from [releases](https://github.com/tversteeg/castle-game/releases). It should work by just running the executable, if not, [create an issue](https://github.com/tversteeg/castle-game/issues/new).

## Rust

Or if you have Rust installed you can do the following:

```bash
cargo install --force castle-game
castle-game
```

# Building

## Prerequisites

To build the project you need to have [Rust](https://www.rustup.rs/) installed.

### Linux (Debian based)

    sudo apt install xorg-dev cmake

### Windows & Mac

You need to install [CMake](https://cmake.org/) and make sure it's in your path.

## Run

Check out the repository with git and build:

    git clone https://github.com/tversteeg/castle-game && cd castle-game
    
Build & run:
    
    cargo run --release

# Contributing

Contributions are more than welcome!
