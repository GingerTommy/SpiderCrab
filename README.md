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

- the port that Web API listens on (don't forget to open the incoming port if you have a firewall
  enabled)

Modify the Windows service entry to configure:

- the account the service runs as (you will probably have to configure urlacl for accounts other
  than `Local System`)

## Usage

If you have followed the steps above, you will now have a Web API endpoint that will accept
PowerShell scripts when sent to `http://{machine-name}:9990/api/agent` per the following example:

```json
{
  "correlationId": "{any string value}",
  "scriptBlock": "Get-Process | Select-Object -ExpandProperty Name -Unique"
}
```

or, if you prefer XML:

```XML
<ExecuteScriptRequest>
  <CorrelationId>{any string value}</CorrelationId>
  <ScriptBlock>Get-Process | Select-Object -ExpandProperty Name -Unique</ScriptBlock>
</ExecuteScriptRequest>
```

## Next steps

- Hook into your service desk application to automate customer requests
- Automate DevOps tasks: collect script parameters and approvals in your workflow system (
  SharePoint, K2, etc.) then call out to SpiderCrab to spin up environments in Azure or VMWare