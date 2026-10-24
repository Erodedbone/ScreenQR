using System.Drawing.Drawing2D;

namespace ScreenQR;

internal sealed class SelectionOverlay : Form
{
    private readonly Bitmap _screen;
    private Point _start;
    private Point _current;
    private bool _dragging;

    internal Rectangle SelectedArea { get; private set; }

    internal SelectionOverlay(Bitmap screen)
    {
        _screen = screen;
        var virtualScreen = SystemInformation.VirtualScreen;
        AutoScaleMode = AutoScaleMode.None;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        Bounds = virtualScreen;
        ShowInTaskbar = false;
        TopMost = true;
        DoubleBuffered = true;
        KeyPreview = true;
        Cursor = Cursors.Cross;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Activate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.DrawImage(_screen, ClientRectangle);
        using var shade = new SolidBrush(Color.FromArgb(115, 0, 0, 0));
        e.Graphics.FillRectangle(shade, ClientRectangle);

        var selection = NormalizedSelection();
        if (!selection.IsEmpty)
        {
            var source = SelectionGeometry.MapToPixels(selection, ClientSize, _screen.Size);
            e.Graphics.DrawImage(_screen, selection, source, GraphicsUnit.Pixel);
            using var border = new Pen(Color.FromArgb(0, 120, 212), 2f) { DashStyle = DashStyle.Solid };
            e.Graphics.DrawRectangle(border, selection);

            var label = $"{selection.Width} × {selection.Height}";
            using var labelBackground = new SolidBrush(Color.FromArgb(220, 24, 24, 24));
            var labelSize = e.Graphics.MeasureString(label, Font);
            var labelY = Math.Max(4, selection.Top - (int)labelSize.Height - 8);
            var labelRect = new RectangleF(selection.Left, labelY, labelSize.Width + 12, labelSize.Height + 4);
            e.Graphics.FillRectangle(labelBackground, labelRect);
            e.Graphics.DrawString(label, Font, Brushes.White, selection.Left + 6, labelY + 2);
        }
        else
        {
            const string hint = "拖动框选二维码  ·  Esc 取消";
            using var hintFont = new Font("Segoe UI", 14f, FontStyle.Regular);
            var size = e.Graphics.MeasureString(hint, hintFont);
            var rect = new RectangleF((ClientSize.Width - size.Width) / 2 - 16, 28, size.Width + 32, size.Height + 16);
            using var background = new SolidBrush(Color.FromArgb(220, 24, 24, 24));
            e.Graphics.FillRectangle(background, rect);
            e.Graphics.DrawString(hint, hintFont, Brushes.White, rect.Left + 16, rect.Top + 8);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        _start = _current = e.Location;
        _dragging = true;
        Capture = true;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!_dragging) return;
        _current = Clamp(e.Location);
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (!_dragging || e.Button != MouseButtons.Left) return;
        Capture = false;
        _dragging = false;
        _current = Clamp(e.Location);
        var selected = NormalizedSelection();
        if (selected.Width < 8 || selected.Height < 8)
        {
            _start = _current = Point.Empty;
            Invalidate();
            return;
        }
        SelectedArea = SelectionGeometry.MapToPixels(selected, ClientSize, _screen.Size);
        DialogResult = DialogResult.OK;
        Close();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private Point Clamp(Point point) => new(
        Math.Clamp(point.X, 0, ClientSize.Width - 1),
        Math.Clamp(point.Y, 0, ClientSize.Height - 1));

    private Rectangle NormalizedSelection()
    {
        if (_start == _current) return Rectangle.Empty;
        return Rectangle.FromLTRB(
            Math.Min(_start.X, _current.X), Math.Min(_start.Y, _current.Y),
            Math.Max(_start.X, _current.X), Math.Max(_start.Y, _current.Y));
    }
}
