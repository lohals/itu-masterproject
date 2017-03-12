$targetSubDir= 'love/2017'
$url = "https://www.retsinformation.dk/eli/regel/lov/2017"
$regex = 'http://www.retsinformation.dk/eli/[a-z]*/(?<year>[0-9]{4})/(?<number>[0-9]*)';

mkdir $targetSubDir -Force

$webResponse = (Invoke-Webrequest $url)

$webResponse.Links|
    where {$_.typeof -eq 'LegalResource'}|
    select -expandproperty href|
    Select-String -Pattern $regex -AllMatches |  
    % { $_.Matches } |
    select `
        value,
        @{Name='Year';Expression={$_.Groups['year']}},
        @{Name='Number';Expression={$_.Groups['number']}} | 
    %{Invoke-WebRequest -Uri "$($_.Value)/xml" -OutFile "$targetSubDir/$($_.Year)-$($_.Number).xml"}
    