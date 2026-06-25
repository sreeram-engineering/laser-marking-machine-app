# Laser Marking QR App

Lean Windows WinForms app for EZCAD2-based QR marking workflows.

The app is the source of truth for part data, user access, generated serials, engraving text construction, and production logging. EZCAD2 remains the marking engine and reads the generated payload from a text file.

## Deployment

This project targets .NET 8 Windows WinForms and is intended to be published as self-contained Windows executables:

- `win-x64`
- `win-x86`

The marking PC does not need a separate .NET install when using the published executables.

Build on a Windows machine with the .NET SDK installed:

```powershell
.\scripts\publish-win.ps1
```

Release executables are written to `dist\`.

## Download From GitHub Releases

The repository creates a GitHub Release after each successful build on `main`.

1. Open the GitHub repo.
2. Go to `Releases`.
3. Open the latest `Laser Marking App Build ...` release.
4. Download one of the assets:
   - `LaserMarkingApp-win-x64.exe`
   - `LaserMarkingApp-win-x86.exe`

Copy the `.exe` to the marking PC and run it.

## Download From GitHub Actions

The repository also keeps downloadable Windows app executables on each workflow run.

1. Open the GitHub repo.
2. Go to `Actions`.
3. Select `Build Windows App`.
4. Open the latest successful run.
5. Download one of the artifacts:
   - `laser-marking-machine-app-win-x64`
   - `laser-marking-machine-app-win-x86`

Each artifact contains the architecture-specific single executable.

## Default Paths

- QR output: `C:\Laser\QRDATA.TXT`
- Active template folder: `C:\Laser\ActiveTemplate`
- Database: `%LOCALAPPDATA%\LaserMarkingApp\laser_marking.db`

## Default Users

The first run creates these local users:

| Username | Password | Role |
| --- | --- | --- |
| `operator` | `operator123` | Operator |
| `setter` | `setter123` | Setter |
| `admin` | `admin123` | Admin |

Change these before production use from `Setter Login` using the `admin` account, then open `Users`.

## Operator Flow

1. Confirm the current part and item code on screen.
2. Enter the heat / lot number, for example `26-4B-21`.
3. Press `MARK` or hit Enter.
4. The app generates the next global serial, builds the full engraving string, writes `QRDATA.TXT`, logs the mark, clears the heat / lot field, and focuses the next entry.

The operator cannot edit part, item code, generated serial, date fields, QR format, or template settings.

## Setter Flow

1. Click `Setter Login`.
2. Log in with a Setter or Admin account.
3. Select or create a part.
4. Set vendor, plant, customer, QR format, and template path.
5. Press `Save` to store the part.
6. Press `Set Active` to activate the part and copy its `.ezd` template into the active template folder.

Setter access automatically logs out after 2 minutes of inactivity.

## User Management

Only Admin users can open `Users` from the setter screen. Admins can create users, change passwords, and set roles.

## QR Format

The app writes the full EZCAD text payload in this format:

```text
CustomerItemCode$PartNumber$DatePrefixSerial$Date$MonthLabel$HeatLot$Material$Pattern$Product$Supplier$
```

Example:

```text
7201097$B3F02001$26B-744$27.02.2026$FEB-26$26-4B-21$FG260$#.0$FLYWHEEL$SREERAMENGG$
```

The serial number is generated globally across all parts. The date prefix uses `YYM-`, where `A=Jan`, `B=Feb`, through `L=Dec`.

## EZCAD2 Setup

See [docs/EZCAD_SETUP.md](docs/EZCAD_SETUP.md).

## v1 Boundaries

- No EZCAD UI automation.
- No SDK integration.
- No OCR/barcode reader dependency.
- No database server.
- Optional external command hook is available but disabled by default.
