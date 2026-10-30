# ScreenQR

[English](README.md)

ScreenQR 是一款轻量的 Windows 屏幕二维码识别工具。按下全局快捷键后框选屏幕上的一个或多个二维码，即可在本机完成识别并自动复制结果；截图和识别内容不会上传。

![ScreenQR 框选界面](https://github.com/user-attachments/assets/6544b7b4-f805-4b3a-8131-48386cfa0dc8)

## 功能

- 在 Windows 虚拟桌面中框选任意区域
- 支持逐显示器 DPI 感知，并对混合缩放显示器进行坐标映射
- 同一区域识别一个或多个二维码
- 自动复制识别结果，并给出明确的成功或失败反馈
- 只将绝对 HTTP/HTTPS 地址视为可打开链接，打开前必须再次确认
- 常驻系统托盘，支持可配置的全局快捷键
- 可选当前用户登录后自启动，无需管理员权限
- 所有截图和二维码识别都在本机完成

## 技术栈

- C#、Windows Forms
- .NET 10 for Windows
- [ZXing.Net](https://github.com/micjahn/ZXing.Net)，通过 `ZXing.Net.Bindings.Windows.Compatibility` 0.16.16 使用
- 全局快捷键使用 Win32 API；可选自启动使用当前用户注册表项

除发布包内自带的 .NET 运行时外，不需要额外应用框架或后台服务。

## 系统要求

- Windows 10 或 Windows 11，x64
- 可用的图形桌面会话
- 已发布的便携版包含 .NET 运行时

从源码构建需要 Windows 环境和 .NET 10 SDK。

## 安装与使用

从 [ScreenQR 1.3.0 Release](https://github.com/Erodedbone/ScreenQR/releases/tag/v1.3.0) 下载已发布的便携版：

- [便携版 `ScreenQR-1.3.0-portable.exe`](https://github.com/Erodedbone/ScreenQR/releases/download/v1.3.0/ScreenQR-1.3.0-portable.exe)
- [SHA-256 校验值 `ScreenQR-1.3.0-SHA256SUMS.txt`](https://github.com/Erodedbone/ScreenQR/releases/download/v1.3.0/ScreenQR-1.3.0-SHA256SUMS.txt)

Release 页面只包含从已审核标签构建的版本；默认分支可能包含尚未形成发布二进制的新修改。

1. 启动 `ScreenQR.exe` 或便携版程序，应用会进入通知区域。
2. 按 **Win + Alt + Q**，也可以双击托盘图标。
3. 拖动框选一个或多个二维码；按 **Esc** 取消。
4. 所有结果会自动复制。若识别到多个二维码，可在结果窗口中选择单条结果。
5. 右键托盘图标可扫描、修改设置或退出。

## 从源码构建

```powershell
dotnet restore .\ScreenQR\ScreenQR.csproj
dotnet build .\ScreenQR\ScreenQR.csproj -c Release
dotnet run --project .\ScreenQR.Tests\ScreenQR.Tests.csproj -c Release
```

生成包含运行时的 x64 单文件便携版：

```powershell
dotnet publish .\ScreenQR\ScreenQR.csproj -c Release -r win-x64 `
  --self-contained true -p:PublishSingleFile=true `
  -p:EnableCompressionInSingleFile=true -o .\artifacts\portable
```

## 设置与本地数据

- 设置文件：`%LocalAppData%\ScreenQR\settings.json`
- 可选自启动项：`HKCU\Software\Microsoft\Windows\CurrentVersion\Run`
- 默认快捷键：**Win + Alt + Q**

启用自启动后如果移动了程序文件，请关闭再重新打开自启动开关，以更新 Windows 保存的路径。

## 已知限制

- 仅识别二维码，不识别其他条码格式。
- 暂不支持摄像头或导入图片文件。
- 当前已发布程序仅提供 Windows x64 版本。
- 二维码过小、模糊、低对比度、被遮挡或严重变形时可能识别失败。
- 混合 DPI 坐标映射已有自动测试，但仍需在不同的真实多显示器布局上验证。
- 当前发布程序没有代码签名，Windows 可能显示信誉提示。

## 许可

ScreenQR 使用 [MIT License](LICENSE)。ZXing.Net 使用 Apache License 2.0，详见[第三方许可说明](ScreenQR/THIRD-PARTY-NOTICES.txt)。
