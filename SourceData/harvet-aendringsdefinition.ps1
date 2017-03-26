param($years= @(2017)) #2010,2011,2012,2013,2014,2016,
import-module (join-path $PSScriptRoot harvest-helpers.psm1) -force
$rootdir='love'

$paragrafIndledningPath = "//AendringsNummer/Aendring/AendringDefinition"
$elementToFind= 'AendringDefinition'
function Write-ToFile{
    param($list,$year)
    $dir=Join-Path $PSScriptRoot "..\UnitTest.MasterProject\AendringsDefinitionParser\RealTestData"
    mkdir $dir -force
    $file=join-path $dir "$year-cst-aendringsdefinitioner.txt"
    $list|Out-File $file -Encoding UTF8
}

$result= $years|%{
    $list=(Get-TargetElementTextNodes (join-path $rootdir $_) $paragrafIndledningPath)
    #Write defintion to file
    $resultWithSource =$list|
        select @{Name="Item";Expression={"$($_.File); $($_.Definition)"}}|
        select -ExpandProperty Item;
    
    Write-ToFile $resultWithSource $_

    #output definition for further processing
    $list|select -ExpandProperty Definition
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