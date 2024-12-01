function New-ClassFilesForDay {
    param (
        [Parameter(
            Mandatory = $true, 
            ValueFromPipeline = $true, 
            ValueFromPipelineByPropertyName = $true)
        ]
        [string]
        $FullName,

        [Parameter(Mandatory = $false)]
        [int]
        $Day = (Get-Date).Day,
    
        [string]
        $OutputPath
    )

    process {
        $ErrorActionPreference = "Stop"

        # Get the current day number
        $dayNumber = Get-Date -UFormat "%d"

        # Read the content of the file
        $content = Get-Content -Path $FullName

        # Replace "__DD__" with the day number in the content
        $content = $content -replace "__DD__", $dayNumber

        # Correct namespace
        $content = $content -replace "TemplateFiles", "Days"

        # Generate the new file name
        $newFileName = Split-Path -Leaf ($FullName -replace "__DD__", $dayNumber)
        $parentDirectory = Split-Path -Parent (Split-Path -Parent $FullName)

        if ($OutputPath) {
            # If an output path is provided, use it
            $newFileName = Join-Path -Path $OutputPath -ChildPath $newFileName
        }
        else {
            # Otherwise, use the same directory as the input file
            $newFileFullName = Join-Path $parentDirectory 'Days' $newFileName
        }

        # Check if the file already exists
        if (Test-Path -Path $newFileFullName) {
            Write-Warning "The file '$newFileFullName' already exists and will be overwritten." -WarningAction Inquire
        }

        # Write the updated content to the new file
        Set-Content -Path $newFileFullName -Value $content

        Write-Output "New file created: $newFileFullName"
    }
}