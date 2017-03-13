param($years= @(2015,2016,2017))

$rootdir='love'

$paragrafIndledningPath = "//AendringCentreretParagraf/Exitus"
$elementToFind= 'AendringCentreretParagraf'

$dateRegexPart ='nr\. (?<number>[0-9]*) af [0-9]{1,2}\. [a-z]* (?<year>[0-9]*)'
$docTypePart ='(?<doctype>lov\u00AD?bekendt\u00AD?gørelse|lov)';

$lovFormat1= "I (?<title>[^,]*),( jf.)? $docTypePart $dateRegexPart, .*"

$lovFormat2= "I $docTypePart $dateRegexPart (?<title>.*):"
function Get-AllAendringParagrafIndledning{
    param($targetSubDir)
    get-childitem $targetSubDir -File |
    select-xml $paragrafIndledningPath|
    #Remove whitespace, transform citations
    % { $_.Node.InnerText `
        -replace '\s+', ' ' `
    }|%{$_.Trim()}

}

function Parse{
    param($pattern,$taragetDir)
    Get-AllAendringParagrafIndledning $taragetDir|
    select-string $pattern -AllMatches -CaseSensitive|
    %{$_.Matches}|
    select `
            value,
            @{Name='Number';Expression={$_.Groups['number']}},
            @{Name='Year';Expression={$_.Groups['year']}},
            @{Name='Title';Expression={$_.Groups['title']}},
            @{Name='DocType';Expression={$_.Groups['doctype']}}  
}
$years|%{
    $year=$_
    $targetDir=join-path $rootdir $year
    Parse $lovFormat1 $targetDir |Export-Csv "..\UnitTest.MasterProject\ParagrafIndledningParser\TestData\$year-format1.csv" -Delimiter ';' -Encoding UTF8
    Parse $lovFormat2 $targetDir |Export-Csv "..\UnitTest.MasterProject\ParagrafIndledningParser\TestData\$year-format2.csv" -Delimiter ';' -Encoding UTF8


}





