# ScreenQR

[简体中文](README.zh-CN.md)

ScreenQR is a lightweight Windows utility that reads QR codes directly from your screen. Press a global hotkey, drag over one or more QR codes, and ScreenQR copies the decoded content locally without uploading the screenshot.

![ScreenQR selection overlay](https://github.com/user-attachments/assets/6544b7b4-f805-4b3a-8131-48386cfa0dc8)

## Features

- Capture any region across the Windows virtual desktop
- Per-monitor DPI awareness with coordinate mapping for mixed-scale displays
- Decode one or multiple QR codes from the same selection
- Copy decoded content to the clipboard automatically
- Show clear success and failure feedback
- Open only absolute HTTP/HTTPS results, after an explicit confirmation
- Run from the system tray with a configurable global hotkey
- Optional per-user startup without administrator privileges
- Process screenshots entirely on the local computer

## Technology

- C# and Windows Forms
- .NET 10 for Windows
- [ZXing.Net](https://github.com/micjahn/ZXing.Net) through `ZXing.Net.Bindings.Windows.Compatibility` 0.16.16
- Win32 APIs for the global hotkey and Windows Registry for optional startup

No application framework or background service is required beyond the self-contained release package.

## System requirements

- Windows 10 or Windows 11, x64
- A graphical desktop session
- The .NET runtime is included in the published portable executable

Building from source requires the .NET 10 SDK on Windows.

## Install and use

Download the published portable build from the [ScreenQR 1.3.0 release](https://github.com/Erodedbone/ScreenQR/releases/tag/v1.3.0):

- [Portable executable (`ScreenQR-1.3.0-portable.exe`)](https://github.com/Erodedbone/ScreenQR/releases/download/v1.3.0/ScreenQR-1.3.0-portable.exe)
- [SHA-256 checksums (`ScreenQR-1.3.0-SHA256SUMS.txt`)](https://github.com/Erodedbone/ScreenQR/releases/download/v1.3.0/ScreenQR-1.3.0-SHA256SUMS.txt)

Release pages contain builds made from reviewed tags. The default branch may contain newer changes that are not yet available as published binaries.

1. Start `ScreenQR.exe` or the portable executable. ScreenQR remains in the notification area.
2. Press **Win + Alt + Q**, or double-click the tray icon.
3. Drag over the QR code or codes. Press **Esc** to cancel.
4. ScreenQR copies all decoded results. Select an individual result in the result window if several codes were found.
5. Right-click the tray icon to scan, change settings, or exit.

## Build from source

```powershell
dotnet restore .\ScreenQR\ScreenQR.csproj
dotnet build .\ScreenQR\ScreenQR.csproj -c Release
dotnet run --project .\ScreenQR.Tests\ScreenQR.Tests.csproj -c Release
```

Create a self-contained x64 portable build:

```powershell
dotnet publish .\ScreenQR\ScreenQR.csproj -c Release -r win-x64 `
  --self-contained true -p:PublishSingleFile=true `
  -p:EnableCompressionInSingleFile=true -o .\artifacts\portable
```

## Settings and local data

- Settings: `%LocalAppData%\ScreenQR\settings.json`
- Optional startup entry: `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`
- Default hotkey: **Win + Alt + Q**

If the executable is moved after startup is enabled, disable and enable startup again so Windows stores the new path.

## Known limitations

- Only QR Code symbols are decoded; other barcode formats are intentionally disabled.
- There is no camera or image-file import mode.
- Published builds currently target Windows x64.
- Very small, blurred, low-contrast, partially covered, or heavily distorted codes may fail to decode.
- Mixed-DPI coordinate mapping has automated coverage, but each physical multi-monitor layout still needs hardware verification.
- Release executables are not currently code-signed, so Windows may show a reputation warning.

## License

ScreenQR is available under the [MIT License](LICENSE). ZXing.Net is licensed under Apache License 2.0; see [third-party notices](ScreenQR/THIRD-PARTY-NOTICES.txt).
