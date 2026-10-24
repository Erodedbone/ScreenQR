namespace ScreenQR;

internal sealed class ResultForm : Form
{
    private readonly IReadOnlyList<string> _results;
    private readonly ListBox _resultList;
    private readonly TextBox _text;
    private readonly Button _open;

    private string SelectedResult => _results[Math.Max(0, _resultList.SelectedIndex)];

    internal ResultForm(IReadOnlyList<string> results)
    {
        ArgumentOutOfRangeException.ThrowIfZero(results.Count);
        _results = results;
        Text = "ScreenQR · 识别成功";
        Icon = AppIcon.Create();
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = true;
        TopMost = true;
        ClientSize = new Size(560, results.Count > 1 ? 330 : 250);
        Font = new Font("Segoe UI", 10f);

        var title = new Label
        {
            Text = results.Count == 1 ? "二维码内容（已复制到剪贴板）" : $"识别到 {results.Count} 个二维码（全部内容已复制）",
            AutoSize = true,
            Location = new Point(18, 16)
        };
        _resultList = new ListBox
        {
            Visible = results.Count > 1,
            Location = new Point(18, 45),
            Size = new Size(524, 74)
        };
        for (var index = 0; index < results.Count; index++)
            _resultList.Items.Add($"二维码 {index + 1}: {Shorten(results[index], 72)}");
        _resultList.SelectedIndex = 0;

        var textTop = results.Count > 1 ? 128 : 45;
        _text = new TextBox
        {
            Text = results[0],
            ReadOnly = true,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            Location = new Point(18, textTop),
            Size = new Size(524, 110)
        };
        var buttonTop = ClientSize.Height - 50;
        var copy = new Button { Text = results.Count > 1 ? "复制所选" : "再次复制", Location = new Point(242, buttonTop), Size = new Size(94, 32) };
        copy.Click += (_, _) => ClipboardHelper.SetText(SelectedResult);
        var copyAll = new Button { Text = "复制全部", Visible = results.Count > 1, Location = new Point(140, buttonTop), Size = new Size(94, 32) };
        copyAll.Click += (_, _) => ClipboardHelper.SetText(string.Join(Environment.NewLine, _results));
        var close = new Button { Text = "关闭", DialogResult = DialogResult.OK, Location = new Point(448, buttonTop), Size = new Size(94, 32) };
        AcceptButton = close;
        CancelButton = close;

        _open = new Button { Text = "打开链接", Location = new Point(344, buttonTop), Size = new Size(94, 32) };
        _open.Click += (_, _) =>
        {
            if (UrlHelper.TryGetHttpUrl(SelectedResult, out var uri))
                UrlHelper.ConfirmAndOpen(this, uri!);
        };

        _resultList.SelectedIndexChanged += (_, _) => RefreshSelectedResult();
        Controls.AddRange(new Control[] { title, _resultList, _text, copyAll, copy, _open, close });
        RefreshSelectedResult();
    }

    private void RefreshSelectedResult()
    {
        _text.Text = SelectedResult;
        _open.Enabled = UrlHelper.TryGetHttpUrl(SelectedResult, out _);
    }

    private static string Shorten(string value, int maximumLength)
    {
        var singleLine = value.Replace('\r', ' ').Replace('\n', ' ');
        if (singleLine.Length <= maximumLength) return singleLine;
        return string.Concat(singleLine.AsSpan(0, maximumLength - 1), "…");
    }
}
