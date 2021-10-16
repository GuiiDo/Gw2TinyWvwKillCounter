# Gw2TinyWvwKillCounter
Tiny window which shows kills and death of current WvW session ingame for Guild Wars 2.
![screenshot](https://user-images.githubusercontent.com/43114787/128597538-ea2f9690-d240-4f04-81ba-62ba0dc4fb51.jpg)

## Just want to use it?
[cick here to download](https://github.com/Taschenbuch/Gw2TinyWvwKillCounter/releases)

[click here to download (under construction)](https://taschenbuch.github.io/Gw2TinyWvwKillCounter/)

- double click on Gw2TinyWvwKillCounter.exe to start it. The tool should appear on the top left of your screen.
 - If a windows warning appears. close it. rightclick on the exe -> settings -> General tab -> Security: tick the "allow" checkbox
 - It is a tiny window, you may miss it at first
 - requirement: gw2 in windowed fullscreen or Window mode
- click gear-icon and then plus-icon to enter one or more api keys
 - api key permissions: account, characters, progression
 - you can create an api key here https://account.arena.net/applications -> applications tab -> new key -> check Account, Characters, Progression
- confirm dialog
- Kills/deaths counters are updated roughly every 5+ minutes. There can be bigger delays of 15-20 minutes. Gw2 api updates the data really slow and the data can be outdated too
- optional: click 0-icon to reset kills/death counter to 0 again



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
- to reset the saved settings for the tool to default (api key etc.), copy this path into the windows explorer path field and press enter: ```%localappdata%\Gw2TinyWvwKillCounter```
- Then delete the content or the whole folder

### Contributing
If you find a bug or have a feature request, feel free to write me directly or open an issue.
