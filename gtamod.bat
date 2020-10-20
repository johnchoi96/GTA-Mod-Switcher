
@echo off
:: Change to the directory with the game files
D:
cd Steam\steamapps\common\

:: Ask the user for input
ECHO Do you want to play with mods or just online?
SET /P _userPrompt= Type either "mods" or "online": 

ECHO %_userPrompt%
IF "%_userPrompt%"=="mods" (

	ECHO ENABLING MODS...
	ren "Grand Theft Auto V" "Grand Theft Auto V - clean"
	ren "Grand Theft Auto V - mods" "Grand Theft Auto V"

) ELSE (

	ECHO DISABLING MODS...
	ren "Grand Theft Auto V" "Grand Theft Auto V - mods"
	ren "Grand Theft Auto V - clean" "Grand Theft Auto V"

)

:end

ECHO Task finished...
SET /P _confirm= Press ENTER to finish...
@echo off

