# PyPyDanceRPC
 A rich status for Discord while playing PyPyDance on VRChat. </br></br>
![Image of Yaktocat](https://github.com/ZuwaiiVR/PyPyDanceRPC/blob/main/Discord_3AAIQXExmh.png)
![Image of Yaktocat](https://github.com/ZuwaiiVR/PyPyDanceRPC/blob/main/PyPy_Console_dLASVgksBM.png)

# How to use
Simple, get the latest release in the release page.
Extract it, and run it. You need to have discord running ofcourse.</br>
Run VRChat with command line "--enable-sdk-log-levels" and go to PyPyDance world and play :). It will automatic update your discord rich presence status on each song, and load the latest logfile after a restart of VRChat.
</br>
After the song has finish playing, status will change to "idle", after 30 seconds, the discord rich presence status will be cleared.
</br>
# Config file.
When there's no config file, the config file will be generated with the following settings.

```
DiscordRPCEnabled=true   (enable or disable Rich presence on discord)
LogSongsToFile=true      (Log's the current song into a text file (pypylog.txt), might be usefull for later if you like a song)
Logo=true                (Just me somewhere in the logo lol)
```


# Build yourself
I used Visual Studio 2017 and the following dependencies:
```
Newtonsoft.Json
https://github.com/Lachee/discord-rpc-csharp
```

# My comments or issues
It's kinda a mess in the code, but I tried to clean up as much as possible.</br>
There is a youtube button, to view the current video on youtube, it might in some cases that the youtube video is broken/removed/invalid.</br>
Sometimes there could be a parse error, I fill fix this later on. </br></br>
Thanks pypy & Natsumi-sama!
