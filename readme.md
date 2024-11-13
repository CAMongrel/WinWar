>
> With the release of Warcraft 1 Remastered on the 13th of November 2024 this project has lost its purpose, so it's getting archived now.
>

## Summary

WinWar is a multiplatform (Windows, Linux, macOS) port of the original DOS Warcraft: Orcs & Humans PC Game. It is completely rewritten from scratch using only the original art and level data.

It *requires* the original game; no game data is included. Since version 0.2.0 WinWar is compatible with the demo version (which you may find here: https://wow.gamepedia.com/Warcraft:_Orcs_%26_Humans_Shareware).  

You can (and should!) also buy Warcraft at GOG: https://www.gog.com/de/game/warcraft_bundle

## Supported platforms
- Desktop/OpenGL (Windows, Linux, macOS)
- (Note: iOS code is still there, but non-functional)

## Screenshots
![Intro](/../screenshots/Screenshots/Intro.png?raw=true "Intro")
![Orc1](/../screenshots/Screenshots/Orc1.png?raw=true "Orc1")
![iPad](/../screenshots/Screenshots/iPad.jpg?raw=true "iPad")  
(Note: iOS is no longer officially supported)

## Compiling and Running
### Compiling from source
- Clone the source code from the git repo including submodules, e.g. `git clone --recurse-submodules https://github.com/CAMongrel/WinWar`
- Open the solution (WinWar.Desktop.sln file) in Visual Studio 2022.
- Compile the code.

### Running
- Before running the game you have to copy over the data files from a Warcraft 1 retail or demo version. To do that, copy everything from the DATA directory of Warcraft 1 (contains 40 .WAR files in retail, but only one .WAR file in the demo version) to a subdirectory named "Data" below the "Assets" directory in the output folder containing the binary (usually bin/Debug, but that differs depending on platform).
- Run the WinWar executable either from your IDE or from disk.
- Enjoy

## Brief history
- Started project in 2003
- Originally written in C++
- Switched to C# sometime in 2008
- Later moved to MonoGame as base framework to support multiple platforms
- Updated to .NET 6 in 2022
- Note: Due to the age of the project, parts of the codebase are written in a way which I'm not particularly proud of. They will be refactored over time.

## Acknowledgements
Warcraft: Orcs & Humans is a trademark of Blizzard Entertainment.