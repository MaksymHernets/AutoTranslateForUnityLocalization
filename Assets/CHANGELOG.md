# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)

## [0.4.9] - 2025-03-19
### Changed
- Updated Unity 2023
- Updated Addressables into 1.21.21
- Updated Localization into 1.4.5
### Fixed
- Fixed crashwindow

## [0.4.8] - 2023-07-09
### Added
- Added ability Undo for AutoTranslate, Search and CleanUp
### Changed
- Updated Addressables into 1.21.14 and remove ai.navigation
### Fixed
- Fixed some bug guipro

## [0.4.7] - 2023-06-27
### Added
- Added the ability to skip text that has parent components of type ui.
- Added filter for dropdown of string table
### Changed
- Updated searchTextSceneWindow screenshot
### Fixed
- Fixed some bug guipro

## [0.4.6] - 2023-06-26
### Added
- Added search and localization for TextMeshLegacy and TextMeshPro
- Added ability skip variant prefab for search
- Added message when completed
- Added instruction to Demo.scene
### Changed
- Changed default parameters (skip empty text) for Search Text
- Remove option dropdown for search
- Updated ResultStringTable screenshot
### Fixed
- Fixed detect variant prefab
- Fixed detect prefab in scene or prefab
- Fixed some trouble GUI

## [0.4.5] - 2023-06-24
### Changed
- Updated screenshots
### Fixed
- Fixed parsing of google translate response
- Fixed show scenes in project in cleanuplocalizationscenes
- Fixed dont save after remove localization in scene
- Fixed dont work to search TextMeshPro
- Fixed nullreferense prefab when autosave searchtext in prerab
- Fixed CheckList when default parameters be false

## [0.4.4] - 2023-06-23
### Added
- Added window ClearUpLocalization for Scenes and Prefabs
- Added tips for search text in scenes or prefabs
### Changed
- Renamed window Auto Translate for Unity Localization
### Fixed
- Fixed swap word into key when translate smart word
- Fixed when code key to replace string key in smartword

## [0.4.3] - 2023-06-22
### Added
- Added mapping dialects of languages for google free api
### Changed
- Change default translate parameters
### Fixed
- Fixed translate SmartWord when key translated
- Fixed dropdown when update options

## [0.4.2] - 2023-06-20
### Added
- Added ability select target languages
### Changed
- Up mimimum version Unity 2020.3.48
### Fixed
- Fixed build errors
- Fixed bug smartwords
- Fixed some UI
- Some refactoring...

## [0.4.1] - 2023-06-20
### Added
- Added autotests for GoogleApiFree;
- Added new windows for cleanup Localization, search audio and texture;
- Added the ability to auto save;
- Added the ability to remove unnecessary "string event" components;
### Fixed
- Fixed a bug when the delimiter character was not found when using GoogleApiFree;
- Refactiring.

## [0.4.0] - 2023-06-20
- Added ability to skip empty text while searching for localization.
- Code refactoring

## [0.3.9] - 2023-06-20
- Renamed GoogleApiFree;
- Renamed asmdef AutoTranslatel;
- Relocated Tutorial scene into folder Sample~
- Filled changelog, added documentation and addition package.

## [0.3.8] - 2023-06-20
- Added the ability to select elements for localization;
- Сode optimization;
- Fixed some bugs.

## [0.3.7] - 2023-06-20
- Added support TextMeshPro;
- Сode optimization;
- Fixed some bugs.

## [0.3.6] - 2023-06-20
- Added service selection for translation;
- Fixed prefab and text search bug.

## [0.3.5] - 2023-06-20 
- Added the ability to search for text in prefab to translate text;
- Upgrade Localization 1.3.2
- Upgrade Addressables 1.20.5

## [0.3.4] - 2023-06-20
- Added the ability to search for text in scenes to translate text;
- Unity support 2019.4.0;
- Removed not needed packages;
- Fixed some bugs;
- Code optimization.

## [0.3.3] - 2023-06-20
- Fixes an error that occurs when compiling a project (library not found InterfaceTranslate)

## [0.3.2] - 2023-06-20
- Fixed a bug in the definition of text tables collections.

## [0.3.1] - 2023-06-20
- Added compatibility with unity version 2019.4.33 and newer
- And updated screenshots

## [0.3.0] - 2023-06-20
- Package compatibility with unity version 2020.3.23

## [0.2.9] - 2023-06-20
- Fixed parsing error after server response.

## [0.2.8] - 2023-06-20
- Bug fixes with compatibility old version dotnet
- Added Display a message if there is no internet
- Updated screenshots

## [0.2.7] - 2023-06-20
- Correction of the problem with the search for files and the consequences of a system crash
## [0.2.6] - 2023-06-20
- Fixed the problem associated with not found localization files, which resulted in a crash
## [0.2.5] - 2023-06-20
- Fixed a bug with the search for the auto-translation settings file
- Updated document file and added document file for editing
## [0.2.4] - 2023-06-20
- Server query optimization, now one query per table with all keys.
- Fixed a bug when a negative server response did not close the progress bar
## [0.2.3] - 2023-06-20
- Added "Auto Translation" window to project settings. To select a translation platform.
- Fixed a bug where when opening the translation menu, the selected language for translation is not selected
## [0.2.2] - 2023-06-20
-Refactoring code. Splitting the "WindowAutoTrsnslate" class into several classes.
## [0.2.1] - 2023-06-20
- Added AssetStoreTools
- Relocaled scripts onto folder AutoTranslate
- Added Demo.scene
- Added icon.jpg, thumbnail.jpg
- Rename package to "autotranslateforlocalizationunity"
## [0.2.0] - 2023-06-20
- Fixed a bug when when starting the project, the window gave errors
- Remove Sample.meta
## [0.1.9] - 2023-06-20
- Deleted folder "Samples"
- Added folder "Samples" in gitignore
## [0.1.8] - 2023-06-20
- Remove package.json
- Rename name in package
## [0.1.7] - 2023-06-20
- Restore package.json in main folder
- New logo and screenshots
## [0.1.6] - 2023-06-20
- Relocate file package.json
- Made npm package
## [0.1.5] - 2023-06-20
- Added the ability to cancel the transfer;
- Added the ability to select for the user which collection tables need to be translated
- Added blocks-errors when there is no localization of the setting, there is no language table and tables
- Fixed a bug when the translation was not saved after exiting the project;
- Fixed a bug when table keys were taken from a random table, as a result of which keys were duplicated in all tables;
- Fixed error displaying the translation progress bar and added additional information about the status of the translation.
## [0.1.4] - 2023-06-20
- Fixed hovering when load tables.
- Added handling of server error 429
- Fixed system language detection
- Added information block
- Added progress bar to show translation status
- Fixed incorrect reading from tables
- Fixed a bug where the language table was deleted







