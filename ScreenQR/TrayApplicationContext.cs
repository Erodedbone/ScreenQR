namespace ScreenQR;

using System.Media;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _trayIcon;
    private readonly Icon _appIcon;
    private readonly HotkeyWindow _hotkey;
    private readonly ToolStripMenuItem _scanMenuItem;
    private AppSettings _settings;
    private bool _scanning;

    internal TrayApplicationContext()
    {
        _settings = SettingsStore.Load();
        _appIcon = AppIcon.Create();
        var menu = new ContextMenuStrip();
        _scanMenuItem = new ToolStripMenuItem();
        _scanMenuItem.Click += (_, _) => Scan();
        menu.Items.Add(_scanMenuItem);
        menu.Items.Add("设置…", null, (_, _) => ShowSettings());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("退出", null, (_, _) => ExitThread());

        _trayIcon = new NotifyIcon
        {
            Icon = _appIcon,
            Text = "ScreenQR",
            ContextMenuStrip = menu,
            Visible = true
        };
        _trayIcon.DoubleClick += (_, _) => Scan();

        _hotkey = new HotkeyWindow(Scan, _settings.Hotkey);
        RefreshHotkeyText();
        if (!_hotkey.IsRegistered)
        {
            MessageBox.Show($"无法注册快捷键 {_settings.Hotkey}，可能已被其他程序占用。\n仍可双击托盘图标扫描，并在设置中更换快捷键。",
                "ScreenQR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        _trayIcon.ShowBalloonTip(3000, "ScreenQR", "ScreenQR 已打开", ToolTipIcon.Info);
    }

    private void ShowSettings()
    {
        var current = _hotkey.Current ?? _settings.Hotkey;
        using var form = new SettingsForm(current, StartupManager.IsEnabled);
        if (form.ShowDialog() != DialogResult.OK) return;
        var selected = form.SelectedHotkey;
        if (!selected.IsValid)
        {
            MessageBox.Show("请至少选择 Ctrl、Alt、Shift 或 Win 中的一个修饰键。",
                "ScreenQR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!_hotkey.TryRegister(selected))
        {
            MessageBox.Show($"快捷键 {selected} 无法注册，可能已被其他程序占用。原快捷键保持不变。",
                "ScreenQR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _settings.HotkeyModifiers = selected.Modifiers;
            _settings.HotkeyKey = selected.Key;
            SettingsStore.Save(_settings);
        }
        catch (Exception exception)
        {
            _hotkey.TryRegister(current);
            MessageBox.Show($"保存设置失败：{exception.Message}", "ScreenQR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            StartupManager.SetEnabled(form.StartWithWindows);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"快捷键已保存，但修改自启动失败：{exception.Message}", "ScreenQR",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        RefreshHotkeyText();
    }

    private void RefreshHotkeyText()
    {
        var text = (_hotkey.Current ?? _settings.Hotkey).ToString();
        _scanMenuItem.Text = $"扫描二维码    {text}";
        _trayIcon.Text = $"ScreenQR · {text}";
    }

    private void Scan()
    {
        if (_scanning) return;
        _scanning = true;
        try
        {
            using var screen = ScreenCapture.CaptureVirtualScreen();
            using var overlay = new SelectionOverlay(screen);
            if (overlay.ShowDialog() != DialogResult.OK) return;

            using var crop = screen.Clone(overlay.SelectedArea, screen.PixelFormat);
            var results = QrDecoder.DecodeAll(crop);
            if (results.Count == 0)
            {
                _trayIcon.ShowBalloonTip(2500, "ScreenQR", "所选区域中没有识别到二维码。", ToolTipIcon.Warning);
                SystemSounds.Exclamation.Play();
                return;
            }

            ClipboardHelper.SetText(string.Join(Environment.NewLine, results));
            _trayIcon.ShowBalloonTip(2000, "ScreenQR",
                results.Count == 1 ? "二维码识别成功，内容已复制。" : $"已识别 {results.Count} 个二维码，全部内容已复制。",
                ToolTipIcon.Info);
            using var resultForm = new ResultForm(results);
            resultForm.ShowDialog();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"扫描失败：{exception.Message}", "ScreenQR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _scanning = false;
        }
    }

    protected override void ExitThreadCore()
    {
        _hotkey.Dispose();
        _trayIcon.Visible = false;
        _trayIcon.Dispose();
        _appIcon.Dispose();
        base.ExitThreadCore();
    }
}
