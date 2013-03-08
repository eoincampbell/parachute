$scripts = [IO.Directory]::GetFiles($args[0], $args[1], [IO.SearchOption]::AllDirectories)

foreach($item in $scripts)
{
	$prop = Get-ItemProperty $item
	#inser major and minor version
	$rename = $prop.DirectoryName + "\" +$prop.CreationTime.ToString("dd.MM.yyyy.hhmmss") + "_" + $prop.Name
	Rename-Item $item $rename
}