# MHXXSaveEditor - 3DS version

Tweaking this in my _very_ limited free time. Release 0.09c is the last version tested on actual hardware.

## Description

A save editor for Monster Hunter XX for the Nintendo 3DS.

Need an editor for MHXX Switch or MHGU?
1. [MHXX Switch Save Editor by Dawnshifter](https://github.com/Dawnshifter/MHXXSwitchSaveEditor) based off of v0.09c of this project.
2. Instructions for editing MHGU saves with Dawnshifter's fork [here](https://gbatemp.net/threads/mhgu-save-editor.515460/).
3. It may be possible to transfer your edited MHXX save file from the 3DS/2DS to the Switch and back. Refer [to this thread](https://www.reddit.com/r/MonsterHunter/comments/6vtal5/mhxx_how_to_transfer_your_3ds_save_to_switchwith/) for instructions.

An editor targeted specifically at talismans is available [here](https://gbatemp.net/threads/release-mh-talisman-editor-for-mhxx-mhx-mhgen-mh4g-mh4u.411182).

## Features

- General Editor
  - HR points
  - Funds and village points
  - Hunter name
  - Playtime
  - Shoutouts
- Guild Card Editor
  - Number of quests run
  - Weapon usage records
  - Monster slay/capture/size records
  - Arena records
- Hunter Editor
  - Appearance
- Item Box Editor
  - Add new items or change quantities
  - Increase quantity of all items in box to maximum
  - Set quantity of all items in box to a specific number
  - Remove all duplicate items
  - Empty entire box
  - Export and import item boxes
- Equipment Editor
  - Add new equipment or modify existing equipment
  - Transmogrify
  - Talismans
  - Kinsects
  - Decorations
  - Upgrade levels
  - Export and import equipment boxes
- Palico Equipment Editor
  - Add new equipment
  - Transmogrify
- Palico Editor
  - Name, Level, Experience, Enthusiasm, Greeting, Previous Owner, Name Giver
  - Appearance (voice, eyes, coat, etc.)
  - Bias
  - Support Moves
  - Skills
  - Export and import Palicoes
- Automatic backup of save files
  - **Before trusting automatic backups, verify that they will load in game and in the editor.**
  - These are stored in your OS's application data folder. The following are defaults and can be different if you've modified your environment.
  - Windows/Wine: `%APPDATA%\MHXXSaveEditor`
    - Enter `%APPDATA%` into the File Explorer address bar to find where your version of Windows keeps it.
  - Linux: `~/.config/MHXXSaveEditor`

## Requirements

- Your MHXX `system` file obtained from your preferred save manager
  - Citra: ```sdmc/Nintendo 3DS/00000000000000000000000000000000/00000000000000000000000000000000/extdata/00000000/00001971/user/```
- Windows: [.NET Framework 4.0+](http://www.microsoft.com/en-us/download/details.aspx?id=17851), [Visual C++ Redistributables](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)
- Linux: [Mono](https://www.mono-project.com/) or [Wine](https://www.winehq.org) with at least `dotnet40`

## Building
Use your platform's version of [MSBuild](https://github.com/Microsoft/msbuild), usually installed with Visual Studio or Mono. Mono's deprecated xbuild may also work. Ensure that your chosen build tool supports at least .NET 4.0.

## Credits
- **Ukee** - For the previous iteration of [MHXXSaveEditor](https://github.com/mineminemine/MHXXSaveEditor).
- **APM** - For her initial work on a save editor for MHGen/MHX, some of the source code and GUI for this save editor is based off hers, see [here](https://github.com/ezapm/APMMHXSaveEditor)  
- **svanheulen** - For his research on the save file format/structure, see [here](https://github.com/svanheulen/mhff/wiki)
- **Kiranico** - For some references.
- MHX/MHGen/MHXX Save Editing threads in GBATemp
