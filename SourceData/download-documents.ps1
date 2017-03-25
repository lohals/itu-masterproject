param($years= @(2010,2011,2012,2013))


$targetRootDir= 'love'

$regex = 'http://www.retsinformation.dk/eli/[a-z]*/(?<year>[0-9]{4})/(?<number>[0-9]*)';

mkdir $targetSubDir -Force

$years|%{

    $year= $_
    $targetDir =join-path $targetRootDir $year
    mkdir $targetDir -Force
    $searchUrl = "https://www.retsinformation.dk/eli/regel/lov/$year"

    $webResponse = (Invoke-Webrequest $searchUrl)

    $webResponse.Links|
        where {$_.typeof -eq 'LegalResource'}|
        select -expandproperty href|
        Select-String -Pattern $regex -AllMatches |  
        % { $_.Matches } |
        select `
            value,
            @{Name='Year';Expression={$_.Groups['year']}},
            @{Name='Number';Expression={$_.Groups['number']}} | 
        %{Invoke-WebRequest -Uri "$($_.Value)/xml" -OutFile "$targetDir/$($_.Year)-$($_.Number).xml"}
 
}   