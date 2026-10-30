# ScreenQR 1.3.0

ScreenQR 1.3.0 improves multi-code scanning, mixed-DPI accuracy, user feedback, and URL safety. Source code, automated checks, licensing, changelog, and bilingual documentation are now maintained directly in the repository.

## Highlights

- Decode one or multiple QR codes from the same screen selection.
- Improve selection-coordinate mapping on mixed-DPI displays.
- Show clearer success, failure, and multiple-result feedback.
- Open only absolute HTTP/HTTPS links with a host, reject embedded credentials, and require confirmation before opening.
- Add regression coverage for decoding, blank images, result behavior, DPI mapping, URL policy, and the default hotkey.

## Download

- Portable, Windows x64: `ScreenQR-1.3.0-portable.exe`
- SHA-256 checksums: `ScreenQR-1.3.0-SHA256SUMS.txt`

The portable build is self-contained and includes the required .NET runtime.

> Security notice: the executable is not code-signed. Windows may show a Microsoft Defender SmartScreen or reputation warning. Verify the SHA-256 checksum before running it.

## Requirements and limitations

- Windows 10 or Windows 11, x64.
- QR Code symbols only; camera and image-file import are not included.
- Very small, blurred, low-contrast, partially covered, or heavily distorted codes may not decode.
- Mixed-DPI mapping has automated coverage, but individual physical multi-monitor layouts may behave differently.

**Full changelog:** https://github.com/Erodedbone/ScreenQR/compare/v1.2.0...v1.3.0

---

## ScreenQR 1.3.0（简体中文）

ScreenQR 1.3.0 改进了多二维码识别、混合 DPI 坐标准确性、操作反馈和 URL 安全策略。源码、自动检查、许可证、更新日志和中英文文档现已直接维护在仓库中。

### 主要变化

- 同一次屏幕框选可识别一个或多个二维码。
- 改进混合 DPI 显示器上的框选坐标映射。
- 提供更清楚的成功、失败和多结果反馈。
- 只允许打开带主机名的绝对 HTTP/HTTPS 地址，拒绝嵌入凭据，并在打开前要求确认。
- 增加二维码识别、空白图、结果窗口、DPI 映射、URL 策略和默认快捷键的回归检查。

### 下载

- Windows x64 便携版：`ScreenQR-1.3.0-portable.exe`
- SHA-256 校验值：`ScreenQR-1.3.0-SHA256SUMS.txt`

便携版已经包含所需的 .NET 运行时，无需单独安装。

> 安全提示：该程序目前没有代码签名，Windows 可能显示 Microsoft Defender SmartScreen 或信誉警告。运行前请核对 SHA-256 校验值。

### 系统要求与限制

- Windows 10 或 Windows 11，x64。
- 仅识别二维码；暂不提供摄像头或图片文件导入。
- 二维码过小、模糊、低对比度、被遮挡或严重变形时可能无法识别。
- 混合 DPI 映射已有自动测试，但不同的真实多显示器布局仍可能存在差异。

**完整变更：** https://github.com/Erodedbone/ScreenQR/compare/v1.2.0...v1.3.0
