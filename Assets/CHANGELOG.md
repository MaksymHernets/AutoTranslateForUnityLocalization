# Changelog

## [0.3.9]
 
## [0.3.8] 
- Added the ability to select elements for localization;
- Сode optimization;
- Fixed some bugs.
## [0.3.7] 
- Added support TextMeshPro;
- Сode optimization;
- Fixed some bugs.
## [0.3.6] 
- Added service selection for translation;
- Fixed prefab and text search bug.
## [0.3.5] 
- Added the ability to search for text in prefab to translate text;
- Upgrade Localization 1.3.2
- Upgrade Addressables 1.20.5
## [0.3.4] 
- Added the ability to search for text in scenes to translate text;
- Unity support 2019.4.0;
- Removed not needed packages;
- Fixed some bugs;
- Code optimization.
## [0.3.3] 
- Fixes an error that occurs when compiling a project (library not found InterfaceTranslate)
## [0.3.2] 
- Fixed a bug in the definition of text tables collections.
## [0.3.1] 
- Added compatibility with unity version 2019.4.33 and newer
- And updated screenshots
## [0.3.0] 
- Package compatibility with unity version 2020.3.23
## [0.2.9] 
- Fixed parsing error after server response.
## [0.2.8] 
- Bug fixes with compatibility old version dotnet
- Added Display a message if there is no internet
- Updated screenshots
## [0.2.7] 
- Correction of the problem with the search for files and the consequences of a system crash
## [0.2.6] 
- Fixed the problem associated with not found localization files, which resulted in a crash
## [0.2.5] 
- Fixed a bug with the search for the auto-translation settings file
- Updated document file and added document file for editing
## [0.2.4] 
- Server query optimization, now one query per table with all keys.
- Fixed a bug when a negative server response did not close the progress bar
## [0.2.3] 
- Added "Auto Translation" window to project settings. To select a translation platform.
- Fixed a bug where when opening the translation menu, the selected language for translation is not selected
## [0.2.2] 
-Refactoring code. Splitting the "WindowAutoTrsnslate" class into several classes.
## [0.2.1] 
- Added AssetStoreTools
- Relocaled scripts onto folder AutoTranslate
- Added Demo.scene
- Added icon.jpg, thumbnail.jpg
- Rename package to "autotranslateforlocalizationunity"
## [0.2.0] 
- Fixed a bug when when starting the project, the window gave errors
- Remove Sample.meta
## [0.1.9] 
- Deleted folder "Samples"
- Added folder "Samples" in gitignore
## [0.1.8] 
- Remove package.json
- Rename name in package
## [0.1.7] 
- Restore package.json in main folder
- New logo and screenshots
## [0.1.6] 
- Relocate file package.json
- Made npm package
## [0.1.5] 
- Added the ability to cancel the transfer;
- Added the ability to select for the user which collection tables need to be translated
- Added blocks-errors when there is no localization of the setting, there is no language table and tables
- Fixed a bug when the translation was not saved after exiting the project;
- Fixed a bug when table keys were taken from a random table, as a result of which keys were duplicated in all tables;
- Fixed error displaying the translation progress bar and added additional information about the status of the translation.
## [0.1.4] 
- Fixed hovering when load tables.
- Added handling of server error 429
- Fixed system language detection
- Added information block
- Added progress bar to show translation status
- Fixed incorrect reading from tables
- Fixed a bug where the language table was deleted







