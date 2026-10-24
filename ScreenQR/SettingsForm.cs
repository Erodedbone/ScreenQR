namespace ScreenQR;

internal sealed class SettingsForm : Form
{
    private readonly CheckBox _ctrl = new() { Text = "Ctrl", AutoSize = true };
    private readonly CheckBox _alt = new() { Text = "Alt", AutoSize = true };
    private readonly CheckBox _shift = new() { Text = "Shift", AutoSize = true };
    private readonly CheckBox _win = new() { Text = "Win", AutoSize = true };
    private readonly ComboBox _key = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 105 };
    private readonly CheckBox _startup = new() { Text = "登录 Windows 后自动启动", AutoSize = true };

    internal HotkeyGesture SelectedHotkey
    {
        get
        {
            uint modifiers = 0;
            if (_ctrl.Checked) modifiers |= NativeMethods.ModControl;
            if (_alt.Checked) modifiers |= NativeMethods.ModAlt;
            if (_shift.Checked) modifiers |= NativeMethods.ModShift;
            if (_win.Checked) modifiers |= NativeMethods.ModWin;
            return new HotkeyGesture(modifiers, (Keys)(_key.SelectedItem ?? Keys.Q));
        }
    }

    internal bool StartWithWindows => _startup.Checked;

    internal SettingsForm(HotkeyGesture hotkey, bool startWithWindows)
    {
        Text = "ScreenQR 设置";
        Icon = AppIcon.Create();
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = true;
        TopMost = true;
        ClientSize = new Size(430, 215);
        Font = new Font("Segoe UI", 10f);

        var title = new Label { Text = "全局扫描快捷键", AutoSize = true, Location = new Point(20, 18) };
        var modifiers = new FlowLayoutPanel
        {
            Location = new Point(20, 50),
            Size = new Size(275, 32),
            WrapContents = false
        };
        modifiers.Controls.AddRange(new Control[] { _ctrl, _alt, _shift, _win });

        var keys = Enumerable.Range('A', 26).Select(value => (Keys)value)
            .Concat(Enumerable.Range(1, 12).Select(value => Keys.F1 + value - 1))
            .Concat(new[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 })
            .ToArray();
        _key.Items.AddRange(keys.Cast<object>().ToArray());
        _key.Location = new Point(300, 48);

        _ctrl.Checked = (hotkey.Modifiers & NativeMethods.ModControl) != 0;
        _alt.Checked = (hotkey.Modifiers & NativeMethods.ModAlt) != 0;
        _shift.Checked = (hotkey.Modifiers & NativeMethods.ModShift) != 0;
        _win.Checked = (hotkey.Modifiers & NativeMethods.ModWin) != 0;
        _key.SelectedItem = keys.Contains(hotkey.Key) ? hotkey.Key : Keys.Q;
        _startup.Checked = startWithWindows;
        _startup.Location = new Point(20, 102);

        var note = new Label
        {
            Text = "至少选择一个修饰键。若快捷键已被占用，原快捷键会继续生效。",
            AutoSize = true,
            ForeColor = SystemColors.GrayText,
            Location = new Point(20, 137)
        };
        var save = new Button { Text = "保存", DialogResult = DialogResult.OK, Location = new Point(230, 170), Size = new Size(86, 32) };
        var cancel = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Location = new Point(324, 170), Size = new Size(86, 32) };
        AcceptButton = save;
        CancelButton = cancel;
        Controls.AddRange(new Control[] { title, modifiers, _key, _startup, note, save, cancel });
    }
}
