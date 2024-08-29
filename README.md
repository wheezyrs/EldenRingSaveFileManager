---

# Elden Ring Save Manager

This is a simple console application designed to manage save files for the game **Elden Ring**, specifically tailored for users of the Seamless Co-op mod. While the mod itself has built-in features for managing `.sl2` save files, this tool was created for fun and practice.

## Features

- **Backup Save Files**: Create a backup of your `.sl2` or `.co2` save files in a designated directory.
- **Duplicate Save Files**: Duplicate `.sl2` save files to `.co2` (and vice versa) within the save directory.
- **Automated Directory Management**: Automatically identifies and handles the Elden Ring save file directory based on the user's system configuration.

## Requirements

- [NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/wheezyrs/EldenRingSaveFileManager.git
   cd EldenRingSaveFileManager
   ```

2. **Build the Project**:
   Open the solution file in Visual Studio and build the project, or use the Developer Command Prompt:
   ```bash
   msbuild EldenRingSaveManager.sln
   ```

3. **Run the Program**:
   After building, you can run the program directly from the command line:
   ```bash
   EldenRingSaveManager.exe
   ```

## Usage

When you run the program, you'll be presented with a menu that allows you to perform several operations:

1. **Backup your `.sl2` save files to the Backup Directory**: This option will copy the current `.sl2` save files to a backup directory located at `C:\Users\<YourUsername>\AppData\Roaming\EldenRingSaveBackUp`.

2. **Backup your `.co2` save files to the Backup Directory**: Similar to the `.sl2` option but for `.co2` files.

3. **Duplicate `.sl2` save files to `.co2` in the game directory**: This option duplicates the `.sl2` save files and renames them to `.co2` in the Elden Ring save directory.

4. **Duplicate `.co2` save files to `.sl2` in the game directory**: This option duplicates the `.co2` save files and renames them to `.sl2` in the Elden Ring save directory.

5. **Restore `.sl2` save files from the Backup Directory to the game directory**: This option restores `.sl2` save files from the backup directory back to the game directory.

6. **Restore `.co2` save files from the Backup Directory to the game directory**: This option restores `.co2` save files from the backup directory back to the game directory.

7. **Show Additional Information**: Displays additional information about the save files, such as the last time they were accessed.

8. **Exit**: Closes the program.

### Backup Directory

By default, all backups are stored in `C:\Users\<YourUsername>\AppData\Roaming\EldenRingSaveBackUp`. This directory is automatically created if it does not already exist.

### Elden Ring Save Directory

The program automatically detects the Elden Ring save directory by searching within `C:\Users\<YourUsername>\AppData\Roaming\EldenRing`. It uses a regular expression to identify the correct subdirectory containing your save files.

## Contributing

If you'd like to contribute to this project, feel free to fork the repository and submit a pull request. Any contributions, whether for bug fixes, additional features, or improvements to the code, are welcome!

## License

This project is licensed under the **GNU Affero General Public License (AGPL) Version 3**. See the [LICENSE](https://github.com/wheezyrs/EldenRingSaveFileManager/blob/main/LICENSE.md) file for details.

## Credits

Created by [wheezyrs](https://github.com/wheezyrs).

## Social medias

[YOUTUBE](https://www.youtube.com/@Wheezyrs)

[X](https://x.com/wheezyrs_)
---
