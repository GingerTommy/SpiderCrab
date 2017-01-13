[![Build status](https://ci.appveyor.com/api/projects/status/3j8ihrkf1lq2k1eh?svg=true)](https://ci.appveyor.com/project/GingerTommy/spidercrab)

# SpiderCrab

Execute PowerShell scripts via an ASP.NET Web API endpoint

![SpiderCrab Logo](spidercrab.gif?raw=true "SpiderCrab Logo")

## Setup

The SpiderCrab agent install a Windows service.
### Install with Chocolatey
```powershell
choco install spidercrab -y
```

## Customize

Modify SpiderCrab.Agent.exe.config to specify:
- the port that Web API listens on
