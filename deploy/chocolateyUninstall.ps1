$serviceName = "SpiderCrab.Agent"
$groupName = "SpiderCrab Operators"

try {
	Write-Output "Searching for service: $serviceName"
	$service = Get-Service -Name $serviceName -ErrorAction Stop
}
catch [Microsoft.PowerShell.Commands.ServiceCommandException] {
	Write-Output "Service not installed, skipping uninstall"
}

if ($service) {
	$serviceWmi = Get-WmiObject win32_service `
		| Where-Object { $_.Name -eq $serviceName } `
		| Select-Object Name, @{ Name="Path"; Expression={ $_.PathName.Split('"')[1] } }
	Write-Output "Removing service: $serviceName"
	& $serviceWmi.Path uninstall
}

$computer = $env:COMPUTERNAME
$adsi = [ADSI]"WinNT://$computer,computer"
try {
	Write-Output "Searching for local security group: $groupName"
	$group = $adsi.Children.Find($groupName, "Group")
}
catch [System.Runtime.InteropServices.COMException] {
	Write-Output "Local security group does not exist, skipping deletion"
}

if ($group) {
	try {
		Write-Output "Removing local security group: $groupName"
		$adsi.Children.Remove($group)
	}
	catch [System.Runtime.InteropServices.COMException] {
		Write-Warning "Error while attempting to remove local security group: $groupName"
	}
}