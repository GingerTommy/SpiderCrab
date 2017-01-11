$path = Split-Path -Parent $MyInvocation.MyCommand.Definition
$installExe = "$path\..\lib\SpiderCrab.Agent.exe"
$packageName = "spidercrab"
$serviceName = "SpiderCrab.Agent"
$groupName = "SpiderCrab Operators"

$service = Get-Service $serviceName -ErrorAction SilentlyContinue
if ($service)
{
	$serviceWmi = Get-WmiObject win32_service `
		| Where-Object { $_.Name -eq $serviceName } `
		| Select-Object Name, @{ Name="Path"; Expression={ $_.PathName.Split('"')[1] } }
	Write-Output "Removing service: $serviceName"
	& $serviceWmi.Path uninstall
}
else
{
	Write-Output "Service not installed, skipping uninstall"
}

try
{
	$computer = $env:COMPUTERNAME
	$adsi = [ADSI]"WinNT://$computer,computer"
	$group = $adsi.Children.Find($groupName, "Group")
	Write-Output "Local security group [$groupName] already exists, skipping creation"
}
catch
{
	Write-Output "Creating local security group: $groupName"
	$group = $adsi.Create("Group", $groupName)
	$group.SetInfo()
	$group.Description = "Members in this group can remotely post scripts to the $serviceName service for local execution"
	$group.SetInfo()
}

Write-Output "Installing service: $serviceName"
Write-Output "Executable: $installExe"

try
{
	& $installExe install
	Start-Service -Name $serviceName
	Write-ChocolateySuccess -PackageName $packageName
}
catch
{
	Write-ChocolateyFailure `
		-PackageName $packageName `
		-FailureMessage $_.Exception.Message
}