param($years= @(2010))
import-module (join-path $PSScriptRoot harvest-helpers.psm1) -force

$rootdir='love'

$paragrafIndledningPath = "//AendringCentreretParagraf/Exitus"
$elementToFind= 'AendringCentreretParagraf'

$dateRegexPart ='nr\. (?<number>[0-9]*) af [0-9]{1,2}\. [a-z]* (?<year>[0-9]*)'
$docTypePart ='(?<doctype>lov\u00AD?bekendt\u00AD?gørelse|lov)';

$lovFormat1= "I (?<title>[^,]*),( jf.)? $docTypePart $dateRegexPart, .*"

$lovFormat2= "I $docTypePart $dateRegexPart (?<title>.+?(?=( foretages følgende ændring)|(, som ændret ved)))"

function Parse{
    param($pattern,$taragetDir)
    Get-TargetElementTextNodes $taragetDir $paragrafIndledningPath|
    select-string $pattern -AllMatches -CaseSensitive|
   %{        
       $res= %{$_.Matches}|
        select @{Name='Number';Expression={$_.Groups['number']}},
             @{Name='Year';Expression={$_.Groups['year']}},
             @{Name='Title';Expression={$_.Groups['title']}},
             @{Name='DocType';Expression={$_.Groups['doctype']}}
       #}
      $output=New-Object psobject
      $output | add-member Noteproperty "Text"  $_.Line 
      $output | add-member Noteproperty "Number"  $res.Number 
      $output | add-member Noteproperty "Year"  $res.Year 
      $output | add-member Noteproperty "Title"  $res.Title.ToString().Trim()
      $output | add-member Noteproperty "DocType"  $res.DocType 
      
      $output
     } 
}
$years|%{
    $year=$_
    $targetDir=join-path $rootdir $year
    Parse $lovFormat1 $targetDir |Export-Csv (Join-Path $PSScriptRoot "..\UnitTest.MasterProject\ParagrafIndledningParser\TestData\$year-format1.csv") -Delimiter ';' -Encoding UTF8
    
    Parse $lovFormat2 $targetDir|%{$_.Title="$((Get-Culture).TextInfo.ToTitleCase($_.DocType)) $($_.Title)";$_}|Export-Csv (join-path $PSScriptRoot "..\UnitTest.MasterProject\ParagrafIndledningParser\TestData\$year-format2.csv") -Delimiter ';' -Encoding UTF8


}
#"$((Get-Culture).TextInfo.ToTitleCase($res.DocType)) $($res.Title)"