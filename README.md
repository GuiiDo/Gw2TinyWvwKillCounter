# Gw2TinyWvwKillCounter
A tiny tool for the computer game Guild Wars 2. It shows current kills and deaths in a tiny window for the Guild Wars 2 World vs World game mode.

![screenshot](https://user-images.githubusercontent.com/43114787/128597538-ea2f9690-d240-4f04-81ba-62ba0dc4fb51.jpg)

## Just want to use it?
[Click here to download and for quickstart guide](https://taschenbuch.github.io/Gw2TinyWvwKillCounter/)

## For developers

### General
- C#
- UI in WPF with MVVM pattern
- icons from https://materialdesignicons.com/
- uses [gw2sharp nuget](https://archomeda.github.io/Gw2Sharp/master/guides/introduction.html) to get data from the [official gw2 API](https://wiki.guildwars2.com/wiki/API:Main)  

### Build it
- clone repo
- open the solution in Visual Studio 2019 with .NET 5 (e.g. Version 16.8 or later) 
- you can now simply build it with Build -> Build solution
- if you want to have a single file .exe-file instead, do the following steps
  - Tools -> command line -> developer command prompt
  - copy this command into the command prompt and press enter:  
 ```dotnet publish --runtime win-x64 -c  Release --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true```
  - the .exe will be in \Gw2TinyWvwKillCounter\bin\Release\net5.0-windows\win-x64\publish
  - the command has to be used because the Build -> publish dialog in visual studio is not able to do that yet (version 16.10.2).

### Delete Settings
- to reset the saved settings for the tool to default (API key etc.), copy this path into the windows explorer path field and press enter: ```%localappdata%\Gw2TinyWvwKillCounter```
- Then delete the content or the whole folder

### Contributing
If you find a bug or have a feature request, feel free to write me directly or open an issue.
