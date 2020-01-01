<#
.SYNOPSIS
Wait for a URL to return a known statuscode. Will throw an exception if the status code is never returned. 

.PARAMETER TimeoutInSeconds
Number of seconds to wait before the operation fails. 

.PARAMETER Uri
URI of the endpoint to ping

.EXAMPLE
. \wait-until-uri-is-ready.ps1 -TimeoutInSeconds 60 -Uri "http://localhost:80/whatever" -ExpectStatusCode 200
#>
Param(
    [Parameter(Mandatory,HelpMessage="Specify timeout in seconds")]
    [int] $TimeoutInSeconds = [System.Environment]::ExpandEnvironmentVariables("WAIT_UNTIL_URI_IS_READY_TIMEOUT_IN_SECONDS"),
    [Parameter(Mandatory,HelpMessage="Uri of endpoint to query")]
    [ValidateNotNullOrEmpty()]
    [string] $Uri = [System.Environment]::ExpandEnvironmentVariables("WAIT_UNTIL_URI_IS_READY_URI"),
    [Parameter(Mandatory,HelpMessage="Specify successful StatusCode")]
    [int] $ExpectStatusCode = [System.Environment]::ExpandEnvironmentVariables("WAIT_UNTIL_URI_IS_READY_EXPECT_STATUS_CODE")
)
PROCESS
{
    Write-Verbose "TimeoutInSeconds: $($TimeoutInSeconds)"
    Write-Verbose "Uri: $($Uri)"
    Write-Verbose "ExpectStatusCode: $($ExpectStatusCode)"

    $until = [System.DateTime]::Now.AddSeconds($timeoutInSeconds)

    do 
    {
        Write-Verbose "TRY: To GET $($Uri)"

        try {
            $response = Invoke-WebRequest -Method GET -Uri $Uri -ErrorAction SilentlyContinue

            Write-Verbose "DONE: Response.StatusCode = $($response.StatusCode)"
            if($response.StatusCode -eq $ExpectStatusCode) {
                Write-Verbose "Service is returning $($ExpectStatusCode) from $($Uri). Exiting..."
                return;
            }
        }
        catch
        {
            Write-Verbose "NOT READY ERROR: Unable to GET $($Uri). $($_)"
        }

        Start-Sleep -Seconds 1
    }
    while( [System.DateTime]::Now -lt $until )

    throw "ERROR: The endpoint $($uri) did not return $($expectStatusCode) within $($timeoutInSeconds) seconds."
}
