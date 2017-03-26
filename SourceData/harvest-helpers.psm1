function Get-TargetElementTextNodes{
    param($targetDir, $xpath)
    get-childitem $targetDir -File | 
    select-xml $xpath|    
    #Remove whitespace, transform citations
    % { 
        New-Object psobject -property @{
           File=(Get-item $_.Path).BaseName
           Definition = ($_.Node.InnerText -replace '\s+', ' ').Trim()
        }
    }

}
