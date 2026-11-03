using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace fretje.WinForms.MdiTabCtrl;

/// <summary>
/// A custom control button (close or dropdown) for the tab control.
/// </summary>
internal sealed class ControlButton : Control
{
    private readonly Color _defaultBackHighColor = SystemColors.ControlLightLight;
    private readonly Color _defaultBackLowColor = SystemColors.ControlDark;
    private readonly Color _defaultBorderColor = SystemColors.ControlDarkDark;
    private bool _hot;
    private Color _backHighColor;
    private Color _backLowColor;
    private Color _borderColor;

    public ControlButton()
    {
        InitializeComponent();
        SuspendLayout();
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        ResetBackHighColor();
        ResetBackLowColor();
        ResetBorderColor();
        ResumeLayout();
    }

    [Browsable(true), Category("Appearance")]
    public Color BackHighColor
    {
        get => _backHighColor;
        set
        {
            _backHighColor = value;
            Invalidate();
        }
    }

    [Browsable(true), Category("Appearance")]
    public Color BackLowColor
    {
        get => _backLowColor;
        set
        {
            _backLowColor = value;
            Invalidate();
        }
    }

    [Browsable(true), Category("Appearance")]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ButtonStyle Style
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ToolStripRenderMode RenderMode
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    public bool ShouldSerializeBackHighColor() => _backHighColor != _defaultBackHighColor;

    public void ResetBackHighColor() => _backHighColor = _defaultBackHighColor;

    public bool ShouldSerializeBackLowColor() => _backLowColor != _defaultBackLowColor;

    public void ResetBackLowColor() => _backLowColor = _defaultBackLowColor;

    public bool ShouldSerializeBorderColor() => _borderColor != _defaultBorderColor;

    public void ResetBorderColor() => _borderColor = _defaultBorderColor;

    protected override void OnPaint(PaintEventArgs e)
    {
        Point[] dropPoints = [new(0, 0), new(11, 0), new(5, 6)];
        Point[] closePoints = [
            new(0, 0), new(2, 0), new(5, 3), new(8, 0), new(10, 0),
            new(6, 4), new(10, 8), new(8, 8), new(5, 5), new(2, 8),
            new(0, 8), new(4, 4),
        ];

        Rectangle rec = new(0, 0, Width - 1, Height - 1);

        if (_hot)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (LinearGradientBrush brush = new(
                new Point(0, 0), new Point(0, Height),
                Helper.RenderColors.ControlButtonBackHighColor(RenderMode, _backHighColor),
                Helper.RenderColors.ControlButtonBackLowColor(RenderMode, _backLowColor)))
            {
                e.Graphics.FillRectangle(brush, rec);
            }

            using (Pen pen = new(Helper.RenderColors.ControlButtonBorderColor(RenderMode, _borderColor)))
            {
                e.Graphics.DrawRectangle(pen, rec);
            }

            e.Graphics.SmoothingMode = SmoothingMode.Default;
        }

        using GraphicsPath g = new();
        using Matrix m = new();
        var x = (Width - 11) / 2;
        var y = ((Height - 11) / 2) + 1;

        if (Style == ButtonStyle.Drop)
        {
            e.Graphics.FillRectangle(new SolidBrush(ForeColor), x, y, 11, 2);
            g.AddPolygon(dropPoints);
            m.Translate(x, y + 3);
            g.Transform(m);
            using SolidBrush brush = new(ForeColor);
            e.Graphics.FillPolygon(brush, g.PathPoints);
        }
        else
        {
            g.AddPolygon(closePoints);
            m.Translate(x, y);
            g.Transform(m);
            using (Pen pen = new(ForeColor))
            {
                e.Graphics.DrawPolygon(pen, g.PathPoints);
            }

            using SolidBrush brush = new(ForeColor);
            e.Graphics.FillPolygon(brush, g.PathPoints);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            MouseEnter -= ControlButton_MouseEnter;
            MouseLeave -= ControlButton_MouseLeave;
        }

        base.Dispose(disposing);
    }

    private void ControlButton_MouseEnter(object? sender, EventArgs e)
    {
        _hot = true;
        Invalidate();
    }

    private void ControlButton_MouseLeave(object? sender, EventArgs e)
    {
        _hot = false;
        Invalidate();
    }

    private void InitializeComponent()
    {
        MouseEnter += ControlButton_MouseEnter;
        MouseLeave += ControlButton_MouseLeave;
    }

    public enum ButtonStyle
    {
        Close = 0,
        Drop = 1,
    }
}
