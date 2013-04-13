set builder_dir=C:\Program Files (x86)\Steam\steamapps\common\terraria
set modpack_dir=C:\Users\Surfpup\Documents\My Games\Terraria\ModPacks

cd "%builder_dir%"

"Modpack Builder.exe" "%~dp0\" "Portal Mod" "%~dp0..\\" true
copy "%~dp0\..\*.obj" "%modpack_dir%"