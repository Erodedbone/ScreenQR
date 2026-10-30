# Verification report

Initial verification date: 2026-10-24  
Release refresh: 2026-10-30  
Environment: Windows x64, .NET SDK 10.0.302

This report records what was actually checked while preparing ScreenQR 1.3.0. It does not claim clean-machine or exhaustive physical multi-monitor validation.

## Source and automated checks

The following checks were repeated successfully on 2026-10-30:

- `dotnet restore` completed successfully.
- Release build completed with zero warnings and zero errors.
- The regression program passed checks for:
  - single QR decoding;
  - multiple QR decoding from one image;
  - blank-image false positives;
  - result-window selection and link-button behavior;
  - scaled and out-of-bounds selection coordinate mapping;
  - HTTP/HTTPS URL policy and unsafe input rejection;
  - default global-hotkey validity and display text.
- `dotnet format --verify-no-changes` passed.
- NuGet reported no known vulnerable direct or transitive packages from the configured source.
- GitHub Actions `build-and-test` passed on `windows-latest` for merge commit `1f2995067bfd99b86efdadcd01ad56c2b81cd746`.

## ScreenQR 1.3.0 portable asset

A fresh self-contained, single-file `win-x64` portable asset was built and checked on 2026-10-30:

| Asset | Size | SHA-256 | Product version | File version | Signature |
| --- | ---: | --- | --- | --- | --- |
| `ScreenQR-1.3.0-portable.exe` | 52,033,871 bytes | `BD9F54EED7BC1875C6946F3284420C232EF9CF82840BC5F674A2FCF267E4CA89` | 1.3.0 | 1.3.0.0 | Not signed |

The final portable executable started and remained running for a controlled five-second process smoke test. It was then stopped by the verification script.

## Existing v1.2.0 release assets

The downloaded assets matched the sizes and SHA-256 digests reported by the GitHub Release API:

| Asset | Size | SHA-256 | Version | Signature |
| --- | ---: | --- | --- | --- |
| `ScreenQR-1.2.0-portable.exe` | 52,029,775 bytes | `FB29F0D413694036FBAE83C7C05AA9927808D7806B35313FF233735F05428CC3` | 1.2.0 | Not signed |
| `ScreenQR-Setup-1.2.0.exe` | 52,115,456 bytes | `C20A761F78434744F32AF498EE9C847AB430B2CDE6821DC0D61716059F08BC53` | 1.2.0 | Not signed |

## v1.2.0 installer findings

The installer contains an NSIS marker. A controlled silent test found that it:

- ignored the requested test install directory and used `%LocalAppData%\Programs\ScreenQR`;
- automatically launched `ScreenQR.exe`;
- did not terminate after the launched application was stopped;
- created an uninstall entry for version 1.2.0;
- installed the expected executable, license, README, third-party notices, and uninstaller;
- did not complete silent removal within 45 seconds when invoked as `Uninstall.exe /uninstall /S`.

The test-created install directory and uninstall registry entry were manually removed after validating their exact paths. Two older ScreenQR shortcuts and an older 1.1.0 startup entry were deliberately preserved because their timestamps and targets predated this test.

The v1.2.0 installer was not reused or renamed for v1.3.0. Version 1.3.0 is released as a portable executable only.

## Verification boundaries

The following areas remain outside the completed automated and process-level checks:

- exhaustive physical multi-monitor layouts at 100%, 125%, and 150% scaling, including displays positioned left of or above the primary display;
- full manual interaction coverage for the tray menu, global hotkey, clipboard, failure notification, URL confirmation, startup setting, and multiple-result window;
- clean-machine testing on every supported Windows 10/11 configuration;
- code signing.

The public release notes explicitly state the unsigned status and mixed-DPI limitation.
