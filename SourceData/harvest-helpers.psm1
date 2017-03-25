function Get-TargetElementTextNodes{
    param($targetDir, $xpath)
    get-childitem $targetDir -File |
    select-xml $xpath|
    #Remove whitespace, transform citations
    % { $_.Node.InnerText `
        -replace '\s+', ' ' `
    }|%{$_.Trim()}

}
