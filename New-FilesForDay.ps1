[CmdletBinding()]
param (
    [int] 
    $Day = (Get-Date).Day,

    [int]
    $Year = (Get-Date).Year
)

. $PSScriptRoot\scripts\New-InputFilesForDay.ps1
. $PSScriptRoot\scripts\New-ClassFilesForDay.ps1

New-InputFilesForDay -Day $Day -Year $Year

Get-ChildItem -Path $PSScriptRoot\src\Aoc2024\TemplateFiles\Day__DD__.cs 
| New-ClassFilesForDay -Day $Day

Get-ChildItem -Path $PSScriptRoot\tests\Aoc2024.Tests\TemplateFiles\Day__DD__Tests.cs
| New-ClassFilesForDay -Day $Day
