#!/usr/bin/env python
import os
import sys

env = SConscript("godot-cpp/SConstruct")

# For reference:
# - CCFLAGS are compilation flags shared between C and C++
# - CFLAGS are for C-specific compilation flags
# - CXXFLAGS are for C++-specific compilation flags
# - CPPFLAGS are for pre-processor flags
# - CPPDEFINES are for pre-processor defines
# - LINKFLAGS are for linking flags

VariantDir('bin', 'src', duplicate=0)
env.Append(CPPPATH=["src/"])
# Use sources from the `bin` variant directory so SCons places object files there.
sources = Glob("bin/*.cpp")

if env["platform"] == "macos":
    library = env.SharedLibrary(
        "bin/libfog_game.{}.{}.framework/libfog_game.{}.{}".format(
            env["platform"], env["target"], env["platform"], env["target"]
        ),
        source=sources,
    )
elif env["platform"] == "ios":
    if env["ios_simulator"]:
        library = env.StaticLibrary(
            "bin/libfog_game.{}.{}.simulator.a".format(env["platform"], env["target"]),
            source=sources,
        )
    else:
        library = env.StaticLibrary(
            "bin/libfog_game.{}.{}.a".format(env["platform"], env["target"]),
            source=sources,
        )
else:
    # Non-mac/ios platforms: place the shared library into the project's `bin/`
    # and name it `libfog_game<suffix><shlibsuffix>` so it matches
    # `fog_game.gdextension` entries (e.g. libfog_game.windows.release.x86_64.dll).
    library = env.SharedLibrary(
        "bin/libfog_game{}{}".format(env["suffix"], env["SHLIBSUFFIX"]),
        source=sources,
    )

Default(library)
