; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

; starfrost's Track Maker Version 2.0.1
; Setup File v2.0.000.00006

; v2.0.000.00000          Initial version                                           2020-09-26
; v2.0.000.00001          Now compiles                                              2020-12-22
; v2.0.000.00002          Added the DLls required for the Track Maker to launch.    2020-12-22
; v2.0.000.00003          Changed icon, fixed install path                          2020-12-22
; v2.0.000.00003 rev 2    Added this comment block                                  2020-12-22
; v2.0.000.00003 rev 3    Added more comments                                       2020-12-22 22:48
; v2.0.000.00004          Dynamic output dir                                        2020-12-22 22:5x
; v2.0.000.00005          Update for Priscilla final version                        2021-01-10
; v2.0.000.00006          Update for v2.0.1                                         2021-01-11
                 
; CHANGE TO APP NAME AND VERSION FOR FINAL
#define MyAppName "starfrost's Track Maker 2.0"
#define MyAppVersion "2.0.656.21011" ; 2.0.463.20268 cb as of 2020/9/26
; END CHANGE TO NAME AND VERSION FOR FINAL
#define MyAppPublisher "starfrost"
#define MyAppURL "https://www.medicanecentre.org"
#define MyAppExeName "Track Maker.exe"
;#define DEBUG

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{21A27285-FFB3-4294-98A6-85ADF89C5CFE}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; The [Icons] "quicklaunchicon" entry uses {userappdata} but its [Tasks] entry has a proper IsAdminInstallMode Check.
UsedUserAreasWarning=no
#ifdef DEBUG
InfoBeforeFile=C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\help_info.txt
#else
InfoBeforeFile=C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\help_info.txt
#endif
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest

OutputDir=C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Installer\Install
#ifdef DEBUG
OutputBaseFilename=TrackMaker-Priscilla-Debug-{#MyAppVersion}
#else
OutputBaseFilename=TrackMaker-Priscilla-Release-{#MyAppVersion}
#endif
SetupIconFile=C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\V2Icon_32x32.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]

#ifdef DEBUG
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Track Maker.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\BetterWin32Errors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Dano.ACECalculator.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Dano.AdvisoryGenerator.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\DanoUI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Starfrost.UL5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Updater.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\Basins.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\CategorySystems.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\help_info.txt"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\Settings.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\StormTypes.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
;Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\UpdateComplete.cmd"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Debug\Data\BasinImages\*"; DestDir: "{app}\Data\BasinImages"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
#else
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Track Maker.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\BetterWin32Errors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Dano.ACECalculator.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Dano.AdvisoryGenerator.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\DanoUI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\help_info.txt"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Starfrost.UL5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Updater.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\Basins.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\CategorySystems.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\Settings.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\StormTypes.xml"; DestDir: "{app}\Data"; Flags: ignoreversion
;Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\UpdateComplete.cmd"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "C:\Users\FiercePC\HHWTools\TrackMaker\TrackMaker\Track Maker\bin\Priscilla_Release\Data\BasinImages\*"; DestDir: "{app}\Data\BasinImages"; Flags: ignoreversion recursesubdirs createallsubdirs
#endif

[Icons]
Name: "{autoprograms}\{#MyAppName}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autoprograms}\{#MyAppName}\starfrost's ACE Calculator"; Filename: "{app}\{#MyAppExeName}"; Parameters: "-initacecalc"
Name: "{autoprograms}\{#MyAppName}\starfrost's Advisory Generator"; Filename: "{app}\{#MyAppExeName}"; Parameters: "-initadvgen"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

