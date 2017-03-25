param($years= @(2015,2016,2017))
import-module (join-path $PSScriptRoot harvest-helpers.psm1) -force
$rootdir='love'

$paragrafIndledningPath = "//AendringsNummer/Aendring/AendringDefinition"
$elementToFind= 'AendringDefinition'


$result= $years|%{
    Get-TargetElementTextNodes (join-path $rootdir $_) $paragrafIndledningPath
} | %{
    $_ -replace "».+?«", '[CiteretTekst]' `
       -replace "Stk\. \d+", '[Stk]' `
       -replace "\d+\. pkt\.", '[Sætning]' `
       -replace "§ ?\d+( ([a-z]|[A-Z])(?!\w))?", '[Paragraf]' `
       -replace "(\[Stk\],) (nr\. \d+)", '$1 [NummerElement]' `
}| %{
    #Multi element replacements
    $_ -replace "\[Stk\]( og |-)\d+", '[MultiStk]' `
       -replace "\d+\.(-| og )\[Sætning\]", '[MultiSætning]' `
   
} | 
group |
select Count,Name |
sort Count -Descending
#measure -sum -Maximum count

$result