## GW2 Tiny WvW Kill Counter

![screenshot](https://user-images.githubusercontent.com/43114787/128597538-ea2f9690-d240-4f04-81ba-62ba0dc4fb51.jpg)

## Quickstart

 <h1 style="color:red;">Quickstart</h1>

- Download the Gw2TinyWvwKillCounter.exe: [cick here to download](https://github.com/Taschenbuch/Gw2TinyWvwKillCounter/releases)
- Double click on Gw2TinyWvwKillCounter.exe to start it. 
  - No installation required. 
  - The tool should appear on the top left corner of your screen.
  - If a windows warning appears. close it. rightclick on the exe -> settings -> General tab -> Security: tick the "allow" checkbox.
  - It is a tiny window, you may miss it at first.
  - Requirement: Guild Wars 2 runs in "Windowed Fullscreen" or "Window" mode.
- Click gear-icon and then plus-icon to enter one or more API keys.
  - API key permissions: account, characters, progression.
  - Create an API key here https://account.arena.net/applications applications tab -> new key -> check Account, Characters, Progression.
- Confirm dialog.
- Kills/deaths counters are updated roughly every 5+ minutes. There can be bigger delays of 15-20 minutes. Guild Wars 2 API updates the data really slow and the data can be outdated too.
- Optional: click 0-icon to reset kills/death counter to 0 again, change opacity and UI scaling in the settings.

## Features
- Tiny window: move it to where it does not annoy you.
- No installation needed: everything in a single exe file, no coping of .dlls and stuff.
- Not ToS breaking: it just displays information from the official Guild Wars 2 API and is not messing with Guild Wars 2 itself.
- Supports multiple accounts by adding multiple API keys
- Background opacity and UI scaling can be adjusted  [click for screenshot](https://user-images.githubusercontent.com/43114787/128633412-193582c7-4912-4261-b950-9123521cc175.jpg)

## FAQ

### Q: Where is the window? I started the .exe file but I dont see the tool!
- Look in the top left corner of your monitor. The tool is very small and easy to miss depending on the background. Especially when your monitor has a high resolution.
- The tool is only visible when Guild Wars 2 runs in "Window" or "Windowed Fullscreen" mode. You can turn that on ingame in the Guild Wars 2 graphics settings. Because otherwise Guild Wars 2 will push itself in the foreground.

### Q: How does the tool work?
- It uses the official Guild Wars 2 API like other tools like https://gw2efficiency.com/ or https://wvwintel.com/. 
- It reads the kill count from the realm avenger achievement (ultimate dominator title) and the characters deaths counts.

### Q: Is it safe to enter my API key?
- Yes. The API key is read-only and was designed by Guild Wars 2 to be used by third party software like this tool.
- I do not have access to your API key anyway. The tool stores your API key only locally on your computer and does not send it to me or someone else.

### Q: Is this tool allowed? Does it comply with the Terms of Service (ToS)?
- Yes. It only uses the data from the offical Guild Wars 2 API. It does not hack itself into Guild Wars 2 like tools like ArcDps.

### Q: Why are kills/deaths updated delayed, not live and can be outdated?
- Because the Guild Wars 2 API only refreshes this data every 5 minutes at best. But even then the data from the Guild Wars 2 API can be outdated. So dont expect to see your kills live (sadly). You can compare the total kills in the tooltip with the Realm Avenger achievement to see whether the data is up to date.

### Q: Can i move the window somewhere else?
- Yes. left click on the window, hold the left mouse button and drag the window around with the mouse.

### Q: Why is the .exe file so big?
- The tool itself is less than 1MB small. But I wanted to try out the newest version of Microsofts C# runtime (NET 5) which is not included in the current windows 10 version, i think. So i had to put it into that exe, too (microsoft calls that "self contained"). It wont be installed on your PC, but microsoft unpacks it in the background to run my tool everytime you start the exe. It is similiar to the java runtime you have to install for java software. Maybe in a future version i find a different way to solve this.

### Q: Does the tool affect Guild Wars 2 performance?
- It should not. The CPU and RAM usage is very low (0-5% CPU, ~50 MB RAM) compared to other Guild Wars 2 addons. Most of the time the tool does nothing, as it only polls the Guild Wars 2 API for new data every few minutes.
- Sometimes it feels like the tool is slow to respond when you interact with itbecause windows priorizes Guild Wars 2. The addon ArcDps on the other hand is not stand alone but injected into Guild Wars 2 and thus responding faster to user interaction.

### Q: Do I need to update the tool when Guild Wars 2 is updated/patched?
- No. The tool does not interact with Guild Wars 2 itself, but with the Guild Wars 2 API webserver. It may need an update when the API is updated, but that rarely happens or rarely affects existing API tools.

### Q: How to report an error or send a feature request?
- Create a new issue here: https://github.com/Taschenbuch/Gw2TinyWvwKillCounter/issues

### Q: Can Guild Wars 2 crash because of this tool?
- No. This tool is stand alone. It does not interact with Guild Wars 2 in anyway because it only access the Guild Wars 2 API webservers like tools like gw2efficiency.

### Q: Gsync does not work?
- It seems that overlays like TACO, BLISH and this tool interfere with the way gsync works. See Details here: https://github.com/Taschenbuch/Gw2TinyWvwKillCounter/issues/1

### Q: How to start it automatically with Guild Wars 2?
You can create a batch file to do this:
1. Right click on desktop -> new -> new Text file
2. Copy this text into the file:
```
@ echo off
start /d "C:\MyFolder" Gw2TinyWvwKillCounter.exe
start "GW2" "C:\Program Files\Guild Wars 2\Gw2-64.exe" -autologin
```
3. In the text adjust the file paths for the tool and Guild Wars 2 according to your system.
4. Save the file, close the text editor, change the file ending to .bat
5. Double click on the .bat file to start this tool and Guild Wars 2