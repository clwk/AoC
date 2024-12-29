function New-InputFilesForDay {
    [CmdletBinding()]
    param (
        [int] 
        $Day = (Get-Date).Day,

        [int]
        $Year = (Get-Date).Year
    )

    $ErrorActionPreference = "Stop"

    $personalInputUri = "https://adventofcode.com/$Year/day/$Day/input"
    $exampleInputUri = "https://adventofcode.com/$Year/day/$Day"
    $sessionCookieValue = $env:AOC_SESSION_COOKIE

    # Create a new Web Session
    $webSession = New-Object Microsoft.PowerShell.Commands.WebRequestSession

    # Create a new Cookie
    $cookie = New-Object System.Net.Cookie

    # Set the properties of the Cookie
    $cookie.Name = "session"
    $cookie.Value = $sessionCookieValue
    $cookie.Domain = "adventofcode.com"

    # Add the Cookie to the Web Session
    $webSession.Cookies.Add($cookie)

    $day2Digits = Get-Date -Day $Day -UFormat "%d"
    # Download my input
    $outfile = "$PSScriptRoot\..\tests\Aoc$Year.Tests\Inputs\Day$day2Digits.input.txt"
    Write-Verbose "Fetching personal input $outfile from $personalInputUri"
    Invoke-WebRequest -Uri $personalInputUri -OutFile $outfile -WebSession $webSession
    Write-Output "Personal input for Day $Day saved to $outfile"

    # Get the example input
    $outfile = "$PSScriptRoot\..\tests\Aoc$Year.Tests\Inputs\Day$day2Digits.example.input.txt"

    try {
        Write-Verbose "Fetching example input $outfile from $exampleInputUri"
        $htmlContent = Invoke-WebRequest -Uri $exampleInputUri -WebSession $webSession
        $html = $htmlContent.Content

        # Use regex to extract the content inside the <pre><code>...</code></pre> tags
        $exampleInput = [regex]::Match($html, '<pre><code>(.*?)</code></pre>', 'Singleline').Groups[1].Value
        $exampleInput = $exampleInput.Trim()

        Set-Content -Path $outfile -Value $exampleInput
        Write-Output "Example input for Day $Day saved to $outfile"
    }
    catch {
        Write-Error "Failed to fetch or parse the example input: $_"
    }
}