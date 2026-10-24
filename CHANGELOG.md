# Changelog

All notable changes to ScreenQR are documented here. The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and versions follow [Semantic Versioning](https://semver.org/).

## [Unreleased]

### Added

- Multiple QR code detection within one selected region.
- Explicit confirmation showing the destination and host before opening HTTP/HTTPS links.
- Automated checks for multiple-code decoding, blank images, mixed-DPI coordinate mapping, and URL safety policy.
- English and Simplified Chinese project documentation.

### Changed

- Mapped overlay coordinates to screenshot pixels when display scaling makes their dimensions differ.
- Improved scan success and failure feedback.
- Moved the source tree into the repository and removed the source ZIP archive.

### Security

- Restricted openable results to absolute HTTP/HTTPS URLs with a host and no embedded credentials.

## [1.2.0] - 2026-09-13

### Added

- Initial public release.
- Screen-region selection and local QR code decoding.
- Automatic clipboard copy.
- Configurable global hotkey, system tray controls, and optional per-user startup.
- Installer and portable x64 executables.

[Unreleased]: https://github.com/Erodedbone/ScreenQR/compare/v1.2.0...HEAD
[1.2.0]: https://github.com/Erodedbone/ScreenQR/releases/tag/v1.2.0
