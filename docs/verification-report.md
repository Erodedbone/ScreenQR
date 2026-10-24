# Verification report

Date: 2026-10-24  
Environment: Windows x64, .NET SDK 10.0.302

This report records what was actually checked while preparing the 1.3.0 changes. It is not a substitute for clean-machine or physical multi-monitor testing.

## Source and automated checks

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
- GitHub Actions `build-and-test` passed on `windows-latest` for the pull request.

## Existing v1.2.0 release assets

The downloaded assets matched the sizes and SHA-256 digests reported by the GitHub Release API:

| Asset | Size | SHA-256 | Version | Signature |
| --- | ---: | --- | --- | --- |
| `ScreenQR-1.2.0-portable.exe` | 52,029,775 bytes | `FB29F0D413694036FBAE83C7C05AA9927808D7806B35313FF233735F05428CC3` | 1.2.0 | Not signed |
| `ScreenQR-Setup-1.2.0.exe` | 52,115,456 bytes | `C20A761F78434744F32AF498EE9C847AB430B2CDE6821DC0D61716059F08BC53` | 1.2.0 | Not signed |

The portable executable started and remained running for a controlled four-second process smoke test.

## v1.2.0 installer findings

The installer contains an NSIS marker. A controlled silent test found that it:

- ignored the requested test install directory and used `%LocalAppData%\Programs\ScreenQR`;
- automatically launched `ScreenQR.exe`;
- did not terminate after the launched application was stopped;
- created an uninstall entry for version 1.2.0;
- installed the expected executable, license, README, third-party notices, and uninstaller;
- did not complete silent removal within 45 seconds when invoked as `Uninstall.exe /uninstall /S`.

The test-created install directory and uninstall registry entry were manually removed after validating their exact paths. Two older ScreenQR shortcuts and an older 1.1.0 startup entry were deliberately preserved because their timestamps and targets predated this test.

These observations cover silent automation only. The interactive installer UI could not be inspected in the available automation environment, so interactive install and uninstall behavior remains unverified.

## Still required before publishing

- Physical multi-monitor testing at 100%, 125%, and 150% scaling, including displays positioned left of or above the primary display.
- End-to-end interaction checks for the tray menu, global hotkey, clipboard, failure notification, URL confirmation, startup setting, and multiple-result window.
- A reproducible installer configuration committed to source control.
- Clean-machine tests of freshly built 1.3.0 installer and portable artifacts.
- Code signing, or an explicit unsigned-binary warning in the release notes.
