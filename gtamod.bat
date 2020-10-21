
@echo off
:: Change to the directory with the game files
D:
cd Steam\steamapps\common\

:: Ask the user for input

SET /P _userPrompt= ENABLE MODS? (y/n): 

IF /I "%_userPrompt%"=="Y" (
	IF NOT EXIST "Grand Theft Auto V - clean" (
		ECHO ENABLING MODS...
		ren "Grand Theft Auto V" "Grand Theft Auto V - clean"
		ren "Grand Theft Auto V - mods" "Grand Theft Auto V"
	)
) ELSE (
	IF NOT EXIST "Grand Theft Auto V - mods" (
		ECHO DISABLING MODS...
		ren "Grand Theft Auto V" "Grand Theft Auto V - mods"
		ren "Grand Theft Auto V - clean" "Grand Theft Auto V"
	)
)

:end

SET /P _runGame= Run Game? (y/n): 

IF /I "%_runGame%"=="Y" (
	ECHO Starting Grand Theft Auto V...
	IF /I "%_userPrompt%"=="Y" (
		:: step into the game directory
		cd "Grand Theft Auto V"
		IF NOT EXIST "RAGEPluginHook.exe" (
			ECHO Something went wrong. RPH does not exist!
		) ELSE (
			"RAGEPluginHook.exe"
		)
	) ELSE (
		start steam://rungameid/271590
	)
)

ECHO Program Complete...
SET /P _confirm= Press ENTER to exit...

@echo on

