$targetSubDir= 'love/2017'
$paragrafIndledningPath = "//AendringCentreretParagraf/Exitus"
$elementToFind= 'AendringCentreretParagraf'

$dateRegexPart ='nr\. (?<number>[0-9]*) af [0-9]{1,2}\. [a-z]* (?<year>[0-9]*)'
$docTypePart ='(?<doctype>lov\u00AD??bekendt\u00AD??gørelse|lov)';

$lovFormat1= "I (?<title>.*),( jf.)? $docTypePart $dateRegexPart, .*"

$lovFormat2= "I $docTypePart $dateRegexPart (?<title>.*):"

function Get-AllAendringParagrafIndledning{
    get-childitem $targetSubDir -File |
    select-xml $paragrafIndledningPath|
    #Remove whitespace, transform citations
    % { $_.Node.InnerText `
        -replace '\s+', ' ' `
    }|%{$_.Trim()}

}

function Parse{
    param($pattern)
    Get-AllAendringParagrafIndledning|
    select-string $pattern -AllMatches -CaseSensitive|
    %{$_.Matches}|
    select `
            value,
            @{Name='Year';Expression={$_.Groups['year']}},
            @{Name='Number';Expression={$_.Groups['number']}},
            @{Name='Title';Expression={$_.Groups['title']}},
            @{Name='DocType';Expression={$_.Groups['doctype']}}  
}

Parse $lovFormat1 |Export-Csv '..\UnitTest.MasterProject\ParagrafIndledningParser\TestData\2017-format1.csv' -Delimiter ';' -Encoding UTF8
Parse $lovFormat2 |Export-Csv '..\UnitTest.MasterProject\ParagrafIndledningParser\2017-format2.csv' -Delimiter ';' -Encoding UTF8


