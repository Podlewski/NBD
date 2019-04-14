param (
    [string]$dbname = "", 
    [string]$collection = "",    
    [string]$jsonFile = "",
    [string]$path = "",
    [switch]$interactive = $false
)

if($interactive -eq $true){
	$path = Read-Host 'Path to mongo bin folder (skip if added to path)'
	$dbname = Read-Host 'Database name'
	$collection = Read-Host 'Collection name'
	$jsonFile = Read-Host 'Path to JSON file'
}elseif([string]::IsNullOrEmpty($dbname) -or [string]::IsNullOrEmpty($collection) -or [string]::IsNullOrEmpty($jsonFile)){
	Get-Help .\importData.ps1
	Exit 1
}

$env:Path += ';' + $path

$command = 'mongoimport --jsonArray --db ' + $dbname + ' --collection ' + $collection + ' --file ' + $jsonFile
Start-Process mongod
Invoke-Expression $command

<#
.SYNOPSIS
    Imports data to mongo database.
.DESCRIPTION	
	Starts 'mongod' daemon and executes 'mongoimport' command with given parameters. If '-interactive' parameters is set, switches to interactive mode. See parameter description	
.PARAMETER path
    Path to mongodb bin folder. Set this parameter if it is not already in you PATH
.PARAMETER dbname
    A name of the database. If it doesn't exist, it will be created
.PARAMETER collection
	A name of the collection inside the database. If it doesn't exist, it will be created	
.PARAMETER jsonFile
	Path to JSON file which will be imported into the database	
.PARAMETER interactive
	Switches to interactive mode in which parameters are put via standard input. Note that previously given parameters are deleted
.EXAMPLE
    .\importData.ps1 -dbname MyDatabase -collection MyCollection -jsonFile data.json -path C:\path\to\mongodb\bin
	use this when MongoDB bin folder is not in PATH
.EXAMPLE
	.\importData.ps1 -dbname MyDatabase -collection MyCollection -jsonFile data.json
	use this when MongoDB bin folder is in PATH
.EXAMPLE
	.\importData.ps1 - interactive
	Use interactive mode
#>