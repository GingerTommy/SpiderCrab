[![Build status](https://ci.appveyor.com/api/projects/status/3j8ihrkf1lq2k1eh?svg=true)](https://ci.appveyor.com/project/GingerTommy/spidercrab)

# SpiderCrab

Execute PowerShell scripts via an ASP.NET Web API endpoint

![SpiderCrab Logo](spidercrab.gif?raw=true "SpiderCrab Logo")

## Setup

The SpiderCrab agent installs a Windows service.
### Install with Chocolatey
```powershell
choco install spidercrab -y
```

> A local security group `SpiderCrab Operators` is created. Grant users and groups permissions to
  invoke scripts by adding them here.

## Customize

Modify SpiderCrab.Agent.exe.config to specify:
- the port that Web API listens on

Modify the Windows service entry to configure:
- the account the service runs as (you will probably have to configure urlacl for accounts other
  than `Local System`)