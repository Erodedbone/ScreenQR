# ScreenQR 1.3.0 — draft release notes

This draft is prepared for review. Do not publish it until the pull request is approved and the release artifacts are rebuilt from the approved commit.

## Highlights

- Select and decode multiple QR codes in one scan.
- More reliable selection coordinates when monitors use different DPI scaling.
- Clearer success and failure feedback.
- Safer URL handling: only HTTP/HTTPS links are eligible, embedded credentials are rejected, and the full destination must be confirmed before opening.
- Source code, tests, license, changelog, and bilingual documentation are now maintained directly in the repository.

## Verification before publishing

- Build and run the automated test program from the final release commit.
- Test selection on physical multi-monitor arrangements at 100%, 125%, and 150% scaling, including monitors with negative virtual coordinates.
- Smoke-test the global hotkey, tray menu, clipboard, startup setting, result window, failure notification, and multi-code selection.
- Build fresh installer and portable artifacts; verify hashes, version metadata, and behavior on a clean Windows 10/11 x64 machine.
- Rebuild the installer from a documented installer configuration. Do not reuse the v1.2.0 wrapper without fixing and retesting its silent install/uninstall behavior.
- If the binaries remain unsigned, state this clearly in the release notes.

## Proposed assets

- `ScreenQR-Setup-1.3.0.exe`
- `ScreenQR-1.3.0-portable.exe`
