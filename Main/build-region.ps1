$buildregion=$args[0]

$currentFolder = Split-Path $MyInvocation.MyCommand.Path

$path = Join-Path $currentFolder \\LoaderFiles\Bars.Gkh.RegionLoader.Console.exe

echo "Start Region Builder"
Start-Process -FilePath $path -ArgumentList "-region ""$buildregion"" -auto" -Wait