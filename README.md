# MHXXSaveEditor - 3DS version

Tweaking this in my _very_ limited free time. Release 0.09c is the last version tested on actual hardware.

If you are looking for a MHXX save editor for the Switch version, please refer to [this](https://github.com/Dawnshifter/MHXXSwitchSaveEditor) instead.  

If you're looking for a MHGU save editor, please refer to [this](https://gbatemp.net/threads/mhgu-save-editor.515460/) instead. 

## Description

A save editor for Monster Hunter XX for the Nintendo 3DS/2DS, not the Switch version.  
However, it *should be possible* to transfer your edited MHXX save file from the 3DS/2DS to the Switch and back. Refer [here](https://www.reddit.com/r/MonsterHunter/comments/6vtal5/mhxx_how_to_transfer_your_3ds_save_to_switchwith/).

## Features

- General Editor
- Character Editor
- Item Box Editor
- Equipment Editor (Transmogrify, Talisman/Charm (see section below), Kinsect, etc.)
- Palico Equipment Editor (Transmogrify too)
- Palico Editor (General stuffs, Learned Actions/Skills, etc.)
- Guild Card Editor (Monster Hunts, Arena, etc.)

---

Looking for a better talisman/charm editor? Look [here](https://gbatemp.net/threads/release-mh-talisman-editor-for-mhxx-mhx-mhgen-mh4g-mh4u.411182)

## Requirements

- Your MHXX `system` file obtained from your preferred save manager
  - Citra: ```sdmc/Nintendo 3DS/00000000000000000000000000000000/00000000000000000000000000000000/extdata/00000000/00001971/user/```
- Windows: [.NET Framework 4.0+](http://www.microsoft.com/en-us/download/details.aspx?id=17851), Visual C++ Redistributables
- Linux: [Mono](https://www.mono-project.com/)

## Building
Use your platform's version of [MSBuild](https://github.com/Microsoft/msbuild), usually installed with Visual Studio or Mono. Mono's deprecated xbuild may also work.

## Credits
- **Ukee** - For the previous iteration of [MHXXSaveEditor](https://github.com/mineminemine/MHXXSaveEditor).
- **APM** - For her initial work on a save editor for MHGen/MHX, some of the source code and GUI for this save editor is based off hers, see [here](https://github.com/ezapm/APMMHXSaveEditor)  
- **svanheulen** - For his research on the save file format/structure, see [here](https://github.com/svanheulen/mhff/wiki)
- **Kiranico** - For some references.
- MHX/MHGen/MHXX Save Editing threads in GBATemp
