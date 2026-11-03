using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace fretje.WinForms.MdiTabCtrl;

[DesignTimeVisible(false), Description("Represents a single tab page in a MdiTabCtrl.TabControl.")]
public
#if NET
partial
#endif
class TabPage : Control
{
    private bool _hot;
    private Size _closeButtonSize;
    private Size _iconSize;
    private bool _painting;
    private bool _mouseOverCloseButton;

    public TabPage(Form form)
    {
        InitializeComponent();
        SuspendLayout();
        SetStyle(ControlStyles.DoubleBuffer, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Visible = false;
        Size = new(1, 1);

        form.TopLevel = false;
        form.MdiParent = null;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        InternalForm = form;

        MenuItem.Text = form.Text;
        if (form.Icon != null)
        {
            MenuItem.Image = form.Icon.ToBitmap();
        }

        MenuItem.Tag = this;

        InternalForm.Enter += TabContents_Enter;
        InternalForm.FormClosed += TabContent_FormClosed;
        InternalForm.Leave += Form_Leave;
        InternalForm.TextChanged += TabContent_TextChanged;

        ResumeLayout(false);
    }

    [Description("Occurs when the user clicks the Tab Control.")]
    public new event EventHandler? Click;

    internal event EventHandler? Close;

    internal event EventHandler<TabControl.GetTabRegionEventArgs>? GetTabRegion;

    internal event EventHandler<TabControl.TabPaintEventArgs>? TabPaintBackground;

    internal event EventHandler<TabControl.TabPaintEventArgs>? TabPaintBorder;

    internal event EventHandler<MouseEventArgs>? Dragging;

    internal event EventHandler<MouseEventArgs>? EndDrag;

    internal event EventHandler? EnterForm;

    internal event EventHandler? LeaveForm;

    [Description("Gets the form associated with the tab page")]
    public object Form => InternalForm;

    [Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for the tab.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color BackHighColor
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for the tab.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color BackLowColor
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for a non selected tab.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color BackHighColorDisabled
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for a non selected tab.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color BackLowColorDisabled
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [Description("Gets or sets the System.Drawing.Color structure that represents the border color of the tab when not selected.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color BorderColorDisabled
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [Description("Gets or sets the System.Drawing.Color structure that represents the fore color of the tab when not selected.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color ForeColorDisabled
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [Description("Returns whether the tab is selected or not.")]
    public bool Selected => IsSelected;

    [Description("Gets or sets whether the tab close button is visible or not.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public bool CloseButtonVisible
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                CalculateWidth();
                Invalidate();
            }
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Image? CloseButtonImage
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Image? CloseButtonImageHot
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Image? CloseButtonImageDisabled
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBackHighColor { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBackLowColor { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBorderColor { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonForeColor { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBackHighColorDisabled { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBackLowColorDisabled { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBorderColorDisabled { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonForeColorDisabled { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBackHighColorHot { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBackLowColorHot { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonBorderColorHot { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Color CloseButtonForeColorHot { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Icon? Icon
    {
        get => InternalForm.Icon;
        set
        {
            InternalForm.Icon = value;
            using (Region r = new(new Rectangle(PadLeft, (Height / 2) - (_iconSize.Height / 2), _iconSize.Width, _iconSize.Height)))
            {
                Invalidate(r);
            }

            MenuItem.Image = value?.ToBitmap();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool IsSelected
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                if (field)
                {
                    _hot = false;
                }

                Invalidate();
            }
        }
    }

    [Description("Gets or sets the System.Drawing.Color structure that represents the border color.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal Color BorderColor
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal int MaximumWidth
    {
        get;
        set
        {
            field = value;
            CalculateWidth();
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal int MinimumWidth
    {
        get;
        set
        {
            field = value;
            CalculateWidth();
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal int PadLeft
    {
        get;
        set
        {
            field = value;
            CalculateWidth();
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal int PadRight
    {
        get;
        set
        {
            field = value;
            CalculateWidth();
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool HotTrack
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal Size CloseButtonSize
    {
        get => _closeButtonSize;
        set
        {
            _closeButtonSize = value;
            CalculateWidth();
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool FontBoldOnSelect
    {
        get;
        set
        {
            field = value;
            CalculateWidth();
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal Size IconSize
    {
        get => _iconSize;
        set
        {
            _iconSize = value;
            CalculateWidth();
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal SmoothingMode SmoothingMode
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal TabControl.TabAlignment Alignment
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool GlassGradient { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool BorderEnhanced { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal ToolStripRenderMode RenderMode
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal TabControl.Weight BorderEnhanceWeight { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal Form InternalForm { get; set; } = null!;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool TabVisible { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal int TabLeft { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal ToolStripMenuItem MenuItem { get; set; } = new();

    public new void Select()
    {
        if (!IsSelected)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (_painting)
        {
            return;
        }

        _painting = true;

        SuspendLayout();
        Color renderBorderColor, renderBottomColor, renderHighColor, renderLowColor;
        using (GraphicsPath gp = new())
        {
            var w = Width;
            CalculateWidth();
            if (w != Width)
            {
                _painting = false;
                ResumeLayout();
                return;
            }

            if (IsSelected)
            {
                renderBorderColor = Helper.RenderColors.BorderColor(RenderMode, BorderColor);
                renderHighColor = Helper.RenderColors.TabBackHighColor(RenderMode, BackHighColor);
                renderLowColor = Helper.RenderColors.TabBackLowColor(RenderMode, BackLowColor);
                renderBottomColor = Helper.RenderColors.TabBackLowColor(RenderMode, BackLowColor);
            }
            else if (_hot)
            {
                renderBorderColor = Helper.RenderColors.BorderColor(RenderMode, BorderColor);
                renderHighColor = Helper.RenderColors.TabBackHighColor(RenderMode, BackHighColor);
                renderLowColor = Helper.RenderColors.TabBackLowColor(RenderMode, BackLowColor);
                renderBottomColor = Helper.RenderColors.BorderColor(RenderMode, BorderColor);
            }
            else
            {
                renderBorderColor = Helper.RenderColors.BorderColorDisabled(RenderMode, BorderColorDisabled);
                renderHighColor = Helper.RenderColors.TabBackHighColorDisabled(RenderMode, BackHighColorDisabled);
                renderLowColor = Helper.RenderColors.TabBackLowColorDisabled(RenderMode, BackLowColorDisabled);
                renderBottomColor = Helper.RenderColors.BorderColor(RenderMode, BorderColor);
            }

            e.Graphics.SmoothingMode = SmoothingMode;
            gp.AddPolygon(GetRegion(Width - 1, Height - 1, IsSelected ? Height : Height - 1));

            if (Alignment == TabControl.TabAlignment.Bottom)
            {
                MirrorPath(gp);
                (renderLowColor, renderHighColor) = (renderHighColor, renderLowColor);
            }

            using (Region r = new(gp))
            using (Region r1 = new(gp))
            using (Region r2 = new(gp))
            using (Region r3 = new(gp))
            using (Matrix m1 = new())
            using (Matrix m2 = new())
            using (Matrix m3 = new())
            {
                m1.Translate(0, -0.5f);
                m2.Translate(0, 0.5f);
                m3.Translate(1, 0);
                r1.Transform(m1);
                r2.Transform(m2);
                r3.Transform(m3);
                r.Union(r1);
                r.Union(r2);
                r.Union(r3);
                Region = r;

                var rf = r.GetBounds(e.Graphics);
                Rectangle rec = new(0, 0, (int)rf.Width, (int)rf.Height);

                using (TabControl.TabPaintEventArgs te = new(e.Graphics, rec, IsSelected, _hot, gp, Width, Height))
                {
                    TabPaintBackground?.Invoke(this, te);
                    if (!te.Handled)
                    {
                        using var gb = CreateGradientBrush(new Rectangle(0, 0, Width, Height), renderHighColor, renderLowColor);
                        e.Graphics.FillPath(gb, gp);
                    }
                }

                using (TabControl.TabPaintEventArgs te = new(e.Graphics, rec, IsSelected, _hot, gp, Width, Height))
                {
                    TabPaintBorder?.Invoke(this, te);
                    if (!te.Handled)
                    {
                        if (BorderEnhanced)
                        {
                            var c = (Alignment == TabControl.TabAlignment.Bottom) ? renderLowColor : renderHighColor;
                            using Pen p = new(c, (float)BorderEnhanceWeight);
                            e.Graphics.DrawLines(p, gp.PathPoints);
                        }

                        using Pen p1 = new(renderBorderColor);
                        e.Graphics.DrawLines(p1, gp.PathPoints);
                    }
                }
            }

            e.Graphics.SmoothingMode = SmoothingMode.None;
            using (Pen pen = new(renderBottomColor))
            {
                e.Graphics.DrawLine(pen, gp.PathPoints[0], gp.PathPoints[gp.PointCount - 1]);
            }

            e.Graphics.SmoothingMode = SmoothingMode;

            DrawIcon(e.Graphics);
            DrawText(e.Graphics);
            if (CloseButtonVisible)
            {
                DrawCloseButton(e.Graphics);
            }
        }

        ResumeLayout();
        _painting = false;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            MouseDown -= Tab_MouseDown;
            MouseEnter -= Tab_MouseEnter;
            MouseLeave -= Tab_MouseLeave;
            MouseMove -= Tab_MouseMove;

            if (InternalForm != null)
            {
                InternalForm.Enter -= TabContents_Enter;
                InternalForm.FormClosed -= TabContent_FormClosed;
                InternalForm.Leave -= Form_Leave;
                InternalForm.TextChanged -= TabContent_TextChanged;
            }

            MenuItem.Dispose();
        }

        base.Dispose(disposing);
    }

#if NET
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool DestroyIcon(IntPtr handle);
#else
    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr handle);
#endif

    private LinearGradientBrush CreateGradientBrush(Rectangle rectangle, Color color1, Color color2) =>
        GlassGradient
            ? Helper.CreateGlassGradientBrush(rectangle, color1, color2)
            : new LinearGradientBrush(rectangle, color1, color2, LinearGradientMode.Vertical);

    private void TabContents_Enter(object? sender, EventArgs e) => EnterForm?.Invoke(this, e);

    private void TabContent_FormClosed(object? sender, FormClosedEventArgs e) => Close?.Invoke(this, EventArgs.Empty);

    private void Form_Leave(object? sender, EventArgs e) => LeaveForm?.Invoke(this, e);

    private void TabContent_TextChanged(object? sender, EventArgs e)
    {
        CalculateWidth();
        Invalidate();
        MenuItem.Text = InternalForm.Text;
    }

    private void Tab_MouseDown(object? sender, MouseEventArgs e)
    {
        if (IsSelected && !(_mouseOverCloseButton && CloseButtonVisible))
        {
            return;
        }

        if (e.Button == MouseButtons.Left)
        {
            if (_mouseOverCloseButton && CloseButtonVisible)
            {
                InternalForm.Close();
            }
            else
            {
                Select();
            }
        }
    }

    private void Tab_MouseEnter(object? sender, EventArgs e)
    {
        if (IsSelected)
        {
            return;
        }

        if (HotTrack)
        {
            _hot = true;
        }

        Invalidate();
    }

    private void Tab_MouseLeave(object? sender, EventArgs e)
    {
        _mouseOverCloseButton = false;
        _hot = false;
        Invalidate();
    }

    private void Tab_MouseMove(object? sender, MouseEventArgs e)
    {
        var state = false;
        if (CloseButtonVisible)
        {
            var x = Width - PadRight - _closeButtonSize.Width - 2;
            var y = (Height / 2) - (_closeButtonSize.Height / 2);
            _mouseOverCloseButton = e.X >= x && e.X <= x + _closeButtonSize.Width - 1 && e.Y >= y && e.Y <= y + _closeButtonSize.Height - 1;
            if (state != _mouseOverCloseButton && CloseButtonVisible)
            {
                state = _mouseOverCloseButton;
                using Region r = new(new Rectangle(x, y, _closeButtonSize.Width, _closeButtonSize.Height));
                Invalidate(r);
            }
        }

        if (RectangleToScreen(ClientRectangle).Contains(PointToScreen(new Point(e.X, e.Y))))
        {
            Cursor = Cursors.Default;
            EndDrag?.Invoke(this, e);
        }
        else
        {
            Dragging?.Invoke(this, e);
            Cursor = Cursors.No;
        }
    }

    private void DrawText(Graphics g)
    {
        using Font f = new(Font, (IsSelected && FontBoldOnSelect) ? FontStyle.Bold : FontStyle.Regular);
        using SolidBrush b = new((IsSelected || _hot) ? ForeColor : ForeColorDisabled);
        var iw = (InternalForm.Icon == null) ? 0 : _iconSize.Width;
        var cbw = CloseButtonVisible ? _closeButtonSize.Width : 0;
        RectangleF bounds = new(
            PadLeft + iw + 2,
            1,
            Width - PadLeft - iw - 5 - cbw - PadRight,
            DisplayRectangle.Height);

        using StringFormat format = new();
        format.FormatFlags = StringFormatFlags.NoWrap;
        format.LineAlignment = StringAlignment.Center;
        format.Trimming = StringTrimming.EllipsisCharacter;
        g.DrawString(InternalForm.Text, f, b, bounds, format);
    }

    private void DrawIcon(Graphics g)
    {
        try
        {
            if (InternalForm.Icon == null)
            {
                return;
            }

            Rectangle r = new(PadLeft, (Height / 2) - (_iconSize.Height / 2), _iconSize.Width, _iconSize.Height);
            using Icon i = new(InternalForm.Icon, _iconSize);
            g.DrawIcon(i, r);
            DestroyIcon(i.Handle);
        }
        catch { }
    }

    private void DrawCloseButton(Graphics g)
    {
        try
        {
            Bitmap? img = null;
            var x = Width - (_closeButtonSize.Width + PadRight + 2);
            var y = (Height / 2) - (_closeButtonSize.Height / 2);

            img = _mouseOverCloseButton
                ? CloseButtonImageHot as Bitmap
                : IsSelected ? CloseButtonImage as Bitmap : CloseButtonImageDisabled as Bitmap;

            var isDisposable = false;
            if (img == null)
            {
                img = GetButton();
                isDisposable = true;
            }

            var hIcon = img.GetHicon();
            using (var icon = Icon.FromHandle(hIcon))
            {
                Rectangle r = new(x, y, _closeButtonSize.Width, _closeButtonSize.Height);
                g.DrawIcon(icon, r);
                DestroyIcon(icon.Handle);
            }

            if (isDisposable)
            {
                img.Dispose();
            }
        }
        catch { }
    }

    private Bitmap GetButton()
    {
        Point[] points = [
            new(1, 0), new(3, 0), new(5, 2), new(7, 0), new(9, 0),
            new(6, 3), new(6, 4), new(9, 7), new(7, 7), new(5, 5),
            new(3, 7), new(1, 7), new(4, 4), new(4, 3),
        ];

        Color bch, bcl, bc, fc;

        if (_mouseOverCloseButton)
        {
            bch = Helper.RenderColors.TabCloseButtonBackHighColorHot(RenderMode, CloseButtonBackHighColorHot);
            bcl = Helper.RenderColors.TabCloseButtonBackLowColorHot(RenderMode, CloseButtonBackLowColorHot);
            bc = Helper.RenderColors.TabCloseButtonBorderColorHot(RenderMode, CloseButtonBorderColorHot);
            fc = Helper.RenderColors.TabCloseButtonForeColorHot(RenderMode, CloseButtonForeColorHot);
        }
        else if (IsSelected)
        {
            bch = Helper.RenderColors.TabCloseButtonBackHighColor(RenderMode, CloseButtonBackHighColor);
            bcl = Helper.RenderColors.TabCloseButtonBackLowColor(RenderMode, CloseButtonBackLowColor);
            bc = Helper.RenderColors.TabCloseButtonBorderColor(RenderMode, CloseButtonBorderColor);
            fc = Helper.RenderColors.TabCloseButtonForeColor(RenderMode, CloseButtonForeColor);
        }
        else
        {
            bch = Helper.RenderColors.TabCloseButtonBackHighColorDisabled(RenderMode, CloseButtonBackHighColorDisabled);
            bcl = Helper.RenderColors.TabCloseButtonBackLowColorDisabled(RenderMode, CloseButtonBackLowColorDisabled);
            bc = Helper.RenderColors.TabCloseButtonBorderColorDisabled(RenderMode, CloseButtonBorderColorDisabled);
            fc = Helper.RenderColors.TabCloseButtonForeColorDisabled(RenderMode, CloseButtonForeColorDisabled);
        }

        Bitmap bmp = new(17, 17);
        bmp.MakeTransparent();

        using (var g = Graphics.FromImage(bmp))
        {
            Point[] path = [
                new(0, 1), new(1, 0), new(15, 0), new(16, 1),
                new(16, 14), new(15, 15), new(1, 15), new(0, 14),
            ];

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (LinearGradientBrush l = new(new Point(0, 0), new Point(0, 15), bch, bcl))
            {
                g.FillPolygon(l, path);
            }

            using (Pen p = new(bc))
            {
                g.DrawPolygon(p, path);
            }

            g.SmoothingMode = SmoothingMode.Default;

            using GraphicsPath gp = new();
            using Matrix m = new();
            gp.AddPolygon(points);
            m.Translate(3, 4);
            gp.Transform(m);

            using (Pen p = new(fc))
            {
                g.DrawPolygon(p, gp.PathPoints);
            }

            using SolidBrush sb = new(fc);
            g.FillPolygon(sb, gp.PathPoints);
        }

        return bmp;
    }

    private void CalculateWidth()
    {
        using var g = CreateGraphics();
        var iw = (InternalForm.Icon == null) ? 0 : _iconSize.Width;
        var cbw = CloseButtonVisible ? _closeButtonSize.Width : 0;
        var w = Width;
        using (Font f = new(Font, FontBoldOnSelect ? FontStyle.Bold : FontStyle.Regular))
        {
            w = PadLeft + iw + 3 + (int)g.MeasureString(InternalForm.Text, f).Width + 3 + cbw + PadRight + 2;
        }

        if (w < MinimumWidth + 1)
        {
            w = MinimumWidth + 1;
        }
        else if (w > MaximumWidth + 1)
        {
            w = MaximumWidth + 1;
        }

        if (w != Width)
        {
            Width = w;
        }
    }

    private Point[] GetRegion(int w, int h, int h1)
    {
        Point[] r = [
            new(0, h), new(0, 2), new(2, 0),
            new(w - 3, 0), new(w - 1, 2), new(w - 1, h),
        ];

        TabControl.GetTabRegionEventArgs e = new(r, w, h, IsSelected);
        GetTabRegion?.Invoke(this, e);

        var points = e.Points;
        Array.Resize(ref points, points.Length + 2);
        Array.Copy(points, 0, points, 1, points.Length - 1);
        points[0] = new(points[1].X, h1);
        points[^1] = new(points[^2].X, h1);
        return points;
    }

    private void MirrorPath(GraphicsPath gp)
    {
        using Matrix m = new();
        m.Translate(0, Height - 1);
        m.Scale(1, -1);
        gp.Transform(m);
    }

    private void InitializeComponent()
    {
        MouseDown += Tab_MouseDown;
        MouseEnter += Tab_MouseEnter;
        MouseLeave += Tab_MouseLeave;
        MouseMove += Tab_MouseMove;
    }
}
