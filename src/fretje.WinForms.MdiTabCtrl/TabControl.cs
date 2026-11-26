using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace fretje.WinForms.MdiTabCtrl;

[DesignTimeVisible(true)]
public partial class TabControl : UserControl
{
    private const ToolStripRenderMode _defaultRenderMode = ToolStripRenderMode.ManagerRenderMode;
    private static readonly Padding _defaultPadding = new(0, 0, 0, 0);
    private static readonly Color _defaultBackLowColor = SystemColors.ControlLightLight;
    private static readonly Color _defaultBackHighColor = SystemColors.Control;
    private static readonly Color _defaultBorderColor = SystemColors.ControlDarkDark;
    private static readonly Color _defaultTabBackHighColor = SystemColors.Window;
    private static readonly Color _defaultTabBackLowColor = SystemColors.Control;
    private static readonly Color _defaultTabBackHighColorDisabled = SystemColors.Control;
    private static readonly Color _defaultTabBackLowColorDisabled = SystemColors.ControlDark;
    private static readonly Color _defaultBorderColorDisabled = SystemColors.ControlDark;
    private static readonly Color _defaultForeColorDisabled = SystemColors.ControlText;
    private static readonly Color _defaultControlButtonBackHighColor = SystemColors.GradientInactiveCaption;
    private static readonly Color _defaultControlButtonBackLowColor = SystemColors.GradientInactiveCaption;
    private static readonly Color _defaultControlButtonBorderColor = SystemColors.HotTrack;
    private static readonly Color _defaultControlButtonForeColor = SystemColors.ControlText;
    private static readonly Size _defaultTabCloseButtonSize = new(17, 17);
    private static readonly Size _defaultTabIconSize = new(16, 16);
    private static readonly Color _defaultTabCloseButtonBackHighColor = Color.IndianRed;
    private static readonly Color _defaultTabCloseButtonBackHighColorDisabled = Color.LightGray;
    private static readonly Color _defaultTabCloseButtonBackHighColorHot = Color.LightCoral;
    private static readonly Color _defaultTabCloseButtonBackLowColor = Color.Firebrick;
    private static readonly Color _defaultTabCloseButtonBackLowColorDisabled = Color.DarkGray;
    private static readonly Color _defaultTabCloseButtonBackLowColorHot = Color.IndianRed;
    private static readonly Color _defaultTabCloseButtonBorderColor = Color.DarkRed;
    private static readonly Color _defaultTabCloseButtonBorderColorDisabled = Color.Gray;
    private static readonly Color _defaultTabCloseButtonBorderColorHot = Color.Firebrick;
    private static readonly Color _defaultTabCloseButtonForeColor = Color.White;
    private static readonly Color _defaultTabCloseButtonForeColorDisabled = Color.White;
    private static readonly Color _defaultTabCloseButtonForeColorHot = Color.White;

    [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "It is disposed in the Disposed handler")]
    private readonly Panel _background = new();
    private readonly int _leftOffset = 3;
    private bool _addingPage;
    private bool _isDelete;
    private Color _backLowColor;
    private Color _backHighColor;
    private Color _borderColor;
    private Color _tabBackHighColor;
    private Color _tabBackLowColor;
    private Color _tabBackHighColorDisabled;
    private Color _tabBackLowColorDisabled;
    private Color _torderColorDisabled;
    private Color _foreColorDisabled;
    private Size _tabIconSize = new(16, 16);
    private Size _tabCloseButtonSize = new(17, 17);
    private ToolStripRenderMode _renderMode;
    private bool _focused;

    public TabControl()
    {
        InitializeComponent();

        SuspendLayout();
        TabPages = new(this);
        TabPages.GetTabRegion += TabPages_GetTabRegion;
        TabPages.SelectedChanged += TabPages_SelectedChanged;
        TabPages.TabPaintBackground += TabPages_TabPaintBackground;
        TabPages.TabPaintBorder += TabPages_TabPaintBorder;
        TabPages.TabRemoved += TabPages_TabRemoved;
        _background.BackColor = SystemColors.AppWorkspace;
        _background.BorderStyle = BorderStyle.Fixed3D;
        _background.Dock = DockStyle.Fill;
        Controls.Add(_background);
        _background.BringToFront();
        ResetBackLowColor();
        ResetBackHighColor();
        ResetBorderColor();
        ResetTabBackHighColor();
        ResetTabBackLowColor();
        ResetTabBackHighColorDisabled();
        ResetTabBackLowColorDisabled();
        ResetBorderColorDisabled();
        ResetForeColorDisabled();
        ResetControlButtonBackHighColor();
        ResetControlButtonBackLowColor();
        ResetControlButtonBorderColor();
        ResetControlButtonForeColor();
        ResetTabCloseButtonBackHighColor();
        ResetTabCloseButtonBackLowColor();
        ResetTabCloseButtonBorderColor();
        ResetTabCloseButtonForeColor();
        ResetTabCloseButtonBackHighColorDisabled();
        ResetTabCloseButtonBackLowColorDisabled();
        ResetTabCloseButtonBorderColorDisabled();
        ResetTabCloseButtonForeColorDisabled();
        ResetTabCloseButtonBackHighColorHot();
        ResetTabCloseButtonBackLowColorHot();
        ResetTabCloseButtonBorderColorHot();
        ResetTabCloseButtonForeColorHot();
        ResetPadding();
        ResetTabCloseButtonSize();
        ResetTabIconSize();
        ResetRenderMode();
        AdjustHeight();
        _dropButton.RenderMode = RenderMode;
        _closeButton.RenderMode = RenderMode;
        ResumeLayout();
        Disposed += (_, _) => _background.Dispose();
    }

    [Description("Occurs when the Tab Page requests the tab region.")]
    public event EventHandler<GetTabRegionEventArgs>? GetTabRegion;

    [Description("Occurs when the Tab Background has been painted.")]
    public event EventHandler<TabPaintEventArgs>? TabPaintBackground;

    [Description("Occurs when the Tab Border has been painted.")]
    public event EventHandler<TabPaintEventArgs>? TabPaintBorder;

    [Description("Occurs when the TabControl Focus changes.")]
    public event EventHandler? FocusedChanged;

    public event EventHandler? SelectedTabChanged;

    [Description("Occurs when a tab page is removed from the control.")]
    public event EventHandler? TabRemoved;

    public event EventHandler<KeyHandledEventArgs>? TabPressed;

    public event EventHandler<KeyHandledEventArgs>? F4Pressed;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
    public override bool Focused => _focused;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
    public object? SelectedForm => _pnlBottom.Controls.Count > 0 ? _pnlBottom.Controls[0] : null;

    [Category("Layout"), DefaultValue(FlowDirection.LeftToRight), Description("Gets or sets the the direction which the tabs are drawn.")]
    public FlowDirection TabsDirection
    {
        get;
        set
        {
            field = value;
            SelectItem(tabPage: null);
        }
    }

    [Category("Appearance"), DefaultValue(false), Description("Gets or sets if the tab background will paint with glass style.")]
    public bool TabGlassGradient
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.GlassGradient = value;
            }
        }
    }

    [Category("Appearance"), DefaultValue(false), Description("Gets or sets if the tab border will paint with enhanced style.")]
    public bool TabBorderEnhanced
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.BorderEnhanced = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for the tab close button.")]
    public Color TabCloseButtonBackHighColor { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for the tab close button.")]
    public Color TabCloseButtonBackLowColor { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the border color for the tab close button.")]
    public Color TabCloseButtonBorderColor { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the fore color for the tab close button.")]
    public Color TabCloseButtonForeColor { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for the disabled tab close button.")]
    public Color TabCloseButtonBackHighColorDisabled { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for the disabled tab close button.")]
    public Color TabCloseButtonBackLowColorDisabled { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the border color for the disabled tab close button.")]
    public Color TabCloseButtonBorderColorDisabled { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the disabled fore color for the tab close button.")]
    public Color TabCloseButtonForeColorDisabled { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for the Hot tab close button.")]
    public Color TabCloseButtonBackHighColorHot { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for the Hot tab close button.")]
    public Color TabCloseButtonBackLowColorHot { get; set; }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the border color for the Hot tab close button.")]
    public Color TabCloseButtonBorderColorHot { get; set; }

    [Category("Appearance"), Description("Gets or sets the Hot fore color for the tab close button.")]
    public Color TabCloseButtonForeColorHot { get; set; }

    [Category("Appearance"), Description("Gets or sets the tab close button image.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Image? TabCloseButtonImage
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.CloseButtonImage = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the tab close button image in hot state.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Image? TabCloseButtonImageHot
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.CloseButtonImageHot = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the tab close button image in disabled state.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Image? TabCloseButtonImageDisabled
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.CloseButtonImageDisabled = value;
            }
        }
    }

    [Category("Layout"), DefaultValue(true), Description("Gets or sets whether the tab close button is visible or not.")]
    public bool TabCloseButtonVisible
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.CloseButtonVisible = value;
            }
        }
    }

        = true;

    [Category("Appearance"), Description("Gets or sets the size of the icon displayed at the tab.")]
    public Size TabIconSize
    {
        get => _tabIconSize;
        set
        {
            _tabIconSize = value;
            foreach (var t in TabPages)
            {
                t.IconSize = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the size of the close button displayed at the tab.")]
    public Size TabCloseButtonSize
    {
        get => _tabCloseButtonSize;
        set
        {
            _tabCloseButtonSize = value;
            foreach (var t in TabPages)
            {
                t.CloseButtonSize = value;
            }
        }
    }

    [Category("Appearance"), DefaultValue(SmoothingMode.None), Description("Specifies whether smoothing (antialiasing) is applied to lines and curves and the edges of filled areas.")]
    public SmoothingMode SmoothingMode
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.SmoothingMode = value;
            }
        }
    }

        = SmoothingMode.None;

    [Category("Layout"), DefaultValue(5), Description("Gets or sets the amount of space on the right side of the tab.")]
    public int TabPadRight
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.PadRight = value;
            }
        }
    }

        = 5;

    [Category("Layout"), DefaultValue(5), Description("Gets or sets the amount of space on the left side of the tab.")]
    public int TabPadLeft
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.PadLeft = value;
            }
        }
    }

        = 5;

    [Category("Layout"), DefaultValue(3), Description("Gets or sets the amount of space between the tabs.")]
    public int TabOffset
    {
        get;
        set
        {
            field = value;
            ArrangeItems();
        }
    }

        = 3;

    [Category("Layout"), DefaultValue(28), Description("Gets or sets the height of the tab.")]
    public int TabHeight
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                _pnlTabs.Height = field;
                _pnlTabs.Top = _pnlTop.Height - _pnlTabs.Height;
                AdjustHeight();
                foreach (var t in TabPages)
                {
                    t.Height = value;
                }
            }
        }
    }

        = 28;

    [Category("Layout"), DefaultValue(3), Description("Gets or sets the distance between the top of the control and the top of the tab.")]
    public int TabTop
    {
        get;
        set
        {
            field = value;
            AdjustHeight();
        }
    }

        = 3;

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient.")]
    public Color TabBackHighColor
    {
        get => _tabBackHighColor;
        set
        {
            _tabBackHighColor = value;
            foreach (var t in TabPages)
            {
                t.BackHighColor = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient.")]
    public Color TabBackLowColor
    {
        get => _tabBackLowColor;
        set
        {
            _tabBackLowColor = value;
            foreach (var t in TabPages)
            {
                t.BackLowColor = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for a non selected tab.")]
    public Color TabBackHighColorDisabled
    {
        get => _tabBackHighColorDisabled;
        set
        {
            _tabBackHighColorDisabled = value;
            foreach (var t in TabPages)
            {
                t.BackHighColorDisabled = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for a non selected tab.")]
    public Color TabBackLowColorDisabled
    {
        get => _tabBackLowColorDisabled;
        set
        {
            _tabBackLowColorDisabled = value;
            foreach (var t in TabPages)
            {
                t.BackLowColorDisabled = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the border color of the tab when not selected.")]
    public Color BorderColorDisabled
    {
        get => _torderColorDisabled;
        set
        {
            _torderColorDisabled = value;
            foreach (var t in TabPages)
            {
                t.BorderColorDisabled = value;
            }
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the fore color of the tab when not selected.")]
    public Color ForeColorDisabled
    {
        get => _foreColorDisabled;
        set
        {
            _foreColorDisabled = value;
            foreach (var t in TabPages)
            {
                t.ForeColorDisabled = value;
            }
        }
    }

    [Category("Layout"), DefaultValue(100), Description("Gets or sets the minimum width for the tab.")]
    public int TabMinimumWidth
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.MinimumWidth = value;
            }
        }
    }

        = 100;

    [Category("Layout"), DefaultValue(200), Description("Gets or sets the maximum width for the tab.")]
    public int TabMaximumWidth
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.MaximumWidth = value;
            }
        }
    }

        = 200;

    [Category("Appearance"), DefaultValue(true), Description("Gets or sets whether the font on the selected tab is displayed in bold.")]
    public bool FontBoldOnSelect
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.FontBoldOnSelect = value;
            }
        }
    }

        = true;

    [Category("Behavior"), DefaultValue(true), Description("Gets or sets a value indicating whether the control's tabs change in appearance when the mouse passes over them.")]
    public bool HotTrack
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.HotTrack = value;
            }
        }
    }

        = true;

    [Category("Behavior"), DefaultValue(true), Description("Gets or sets a value indicating whether the user can reorder tabs dragging.")]
    public bool AllowTabReorder { get; set; } = true;

    [Category("Layout"), DefaultValue(false), Description("Gets or sets a value indicating whether the close button is displayed or not.")]
    public bool CloseButtonVisible
    {
        get => _closeButton.Visible;
        set
        {
            if (_closeButton.Visible != value)
            {
                _closeButton.Visible = value;
                SetControlsSizeLocation();
            }
        }
    }

    [Category("Layout"), DefaultValue(true), Description("Gets or sets a value indicating whether the drop button is displayed or not.")]
    public bool DropButtonVisible
    {
        get => _dropButton.Visible;
        set
        {
            if (_dropButton.Visible != value)
            {
                _dropButton.Visible = value;
                SetControlsSizeLocation();
            }
        }
    }

    [Category("Appearance"), DefaultValue(true), Description("Gets or sets a value indicating whether a double line separator is displayed at the top of the control.")]
    public bool TopSeparator
    {
        get;
        set
        {
            field = value;
            AdjustHeight();
        }
    }

        = true;

    [Category("Behavior"), DefaultValue(TabAlignment.Top), Description("Gets or sets the area of the control (for example, along the top) where the tabs are aligned.")]
    public TabAlignment Alignment
    {
        get;
        set
        {
            field = value;
            AdjustHeight();
            PositionButtons();
            foreach (var t in TabPages)
            {
                t.Alignment = value;
            }
        }
    }

        = TabAlignment.Top;

    [Category("Layout"), Description("Gets or sets the amount of space around the form on the control's tab pages.")]
    new public Padding Padding
    {
        get => _pnlBottom.Padding;
        set => _pnlBottom.Padding = value;
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for the control button.")]
    public Color ControlButtonBackHighColor
    {
        get => _dropButton.BackHighColor;
        set
        {
            _dropButton.BackHighColor = value;
            _closeButton.BackHighColor = value;
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for the control button.")]
    public Color ControlButtonBackLowColor
    {
        get => _dropButton.BackLowColor;
        set
        {
            _dropButton.BackLowColor = value;
            _closeButton.BackLowColor = value;
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the border color for the control button.")]
    public Color ControlButtonBorderColor
    {
        get => _dropButton.BorderColor;
        set
        {
            _dropButton.BorderColor = value;
            _closeButton.BorderColor = value;
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ForeColor for the control button.")]
    public Color ControlButtonForeColor
    {
        get => _dropButton.ForeColor;
        set
        {
            _dropButton.ForeColor = value;
            _closeButton.ForeColor = value;
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for the tabs region.")]
    public Color BackLowColor
    {
        get => _backLowColor;
        set
        {
            _backLowColor = value;
            Invalidate();
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for the tabs region.")]
    public Color BackHighColor
    {
        get => _backHighColor;
        set
        {
            _backHighColor = value;
            Invalidate();
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the border color.")]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            foreach (var t in TabPages)
            {
                t.BorderColor = value;
            }

            _pnlTabs.Invalidate();
            _pnlTop.Invalidate();
        }
    }

    [Browsable(false), Description("Gets the collection of tab pages in this tab control.")]
    public TabPageCollection TabPages { get; }

    [Category("Appearance"), Description("The painting style applied to the control.")]
    public ToolStripRenderMode RenderMode
    {
        get => _renderMode;
        set
        {
            _renderMode = value;
            _dropButton.RenderMode = value;
            _closeButton.RenderMode = value;
            _winMenu.RenderMode = value;
            foreach (var t in TabPages)
            {
                t.RenderMode = value;
            }
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ToolStripRenderer? MenuRenderer
    {
        get;
        set
        {
            field = value;
            if (value is { })
            {
                _winMenu.Renderer = value;
            }
        }
    }

    [Category("Appearance"), DefaultValue(Weight.Medium), Description("The weight of the border.")]
    public Weight TabBorderEnhanceWeight
    {
        get;
        set
        {
            field = value;
            foreach (var t in TabPages)
            {
                t.BorderEnhanceWeight = value;
            }
        }
    }

        = Weight.Medium;

    [Category("Behavior"), DefaultValue(true), Description("Gets or sets if the CTRL+TAB/CTRL+SHIFT+TAB will select the next/previous tab.")]
    public bool KeyTabEnabled { get; set; } = true;

    [Category("Behavior"), DefaultValue(true), Description("Gets or sets if the CTRL+F4 will close the selected tab.")]
    public bool KeyCloseEnabled { get; set; } = true;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
    internal bool SetFocus
    {
        set
        {
            _focused = value;
            FocusedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool ShouldSerializeTabCloseButtonBackHighColor() => TabCloseButtonBackHighColor != _defaultTabCloseButtonBackHighColor;

    public void ResetTabCloseButtonBackHighColor() => TabCloseButtonBackHighColor = _defaultTabCloseButtonBackHighColor;

    public bool ShouldSerializeTabCloseButtonBackLowColor() => TabCloseButtonBackLowColor != _defaultTabCloseButtonBackLowColor;

    public void ResetTabCloseButtonBackLowColor() => TabCloseButtonBackLowColor = _defaultTabCloseButtonBackLowColor;

    public bool ShouldSerializeTabCloseButtonBorderColor() => TabCloseButtonBorderColor != _defaultTabCloseButtonBorderColor;

    public void ResetTabCloseButtonBorderColor() => TabCloseButtonBorderColor = _defaultTabCloseButtonBorderColor;

    public bool ShouldSerializeTabCloseButtonForeColor() => TabCloseButtonForeColor != _defaultTabCloseButtonForeColor;

    public void ResetTabCloseButtonForeColor() => TabCloseButtonForeColor = _defaultTabCloseButtonForeColor;

    public bool ShouldSerializeTabCloseButtonBackHighColorDisabled() => TabCloseButtonBackHighColorDisabled != _defaultTabCloseButtonBackHighColorDisabled;

    public void ResetTabCloseButtonBackHighColorDisabled() => TabCloseButtonBackHighColorDisabled = _defaultTabCloseButtonBackHighColorDisabled;

    public bool ShouldSerializeTabCloseButtonBackLowColorDisabled() => TabCloseButtonBackLowColorDisabled != _defaultTabCloseButtonBackLowColorDisabled;

    public void ResetTabCloseButtonBackLowColorDisabled() => TabCloseButtonBackLowColorDisabled = _defaultTabCloseButtonBackLowColorDisabled;

    public bool ShouldSerializeTabCloseButtonBorderColorDisabled() => TabCloseButtonBorderColorDisabled != _defaultTabCloseButtonBorderColorDisabled;

    public void ResetTabCloseButtonBorderColorDisabled() => TabCloseButtonBorderColorDisabled = _defaultTabCloseButtonBorderColorDisabled;

    public bool ShouldSerializeTabCloseButtonForeColorDisabled() => TabCloseButtonForeColorDisabled != _defaultTabCloseButtonForeColorDisabled;

    public void ResetTabCloseButtonForeColorDisabled() => TabCloseButtonForeColorDisabled = _defaultTabCloseButtonForeColorDisabled;

    public bool ShouldSerializeTabCloseButtonBackHighColorHot() => TabCloseButtonBackHighColorHot != _defaultTabCloseButtonBackHighColorHot;

    public void ResetTabCloseButtonBackHighColorHot() => TabCloseButtonBackHighColorHot = _defaultTabCloseButtonBackHighColorHot;

    public bool ShouldSerializeTabCloseButtonBackLowColorHot() => TabCloseButtonBackLowColorHot != _defaultTabCloseButtonBackLowColorHot;

    public void ResetTabCloseButtonBackLowColorHot() => TabCloseButtonBackLowColorHot = _defaultTabCloseButtonBackLowColorHot;

    public bool ShouldSerializeTabIconSize() => _tabIconSize != _defaultTabIconSize;

    public void ResetTabIconSize() => _tabIconSize = _defaultTabIconSize;

    public bool ShouldSerializeTabBackLowColor() => _tabBackLowColor != _defaultTabBackLowColor;

    public void ResetTabBackLowColor() => _tabBackLowColor = _defaultTabBackLowColor;

    public bool ShouldSerializeTabCloseButtonBorderColorHot() => TabCloseButtonBorderColorHot != _defaultTabCloseButtonBorderColorHot;

    public void ResetTabCloseButtonBorderColorHot() => TabCloseButtonBorderColorHot = _defaultTabCloseButtonBorderColorHot;

    public bool ShouldSerializeTabCloseButtonForeColorHot() => TabCloseButtonForeColorHot != _defaultTabCloseButtonForeColorHot;

    public void ResetTabCloseButtonForeColorHot() => TabCloseButtonForeColorHot = _defaultTabCloseButtonForeColorHot;

    public bool ShouldSerializeTabCloseButtonSize() => _tabCloseButtonSize != _defaultTabCloseButtonSize;

    public void ResetTabCloseButtonSize() => _tabCloseButtonSize = _defaultTabCloseButtonSize;

    public bool ShouldSerializeTabBackHighColor() => _tabBackHighColor != _defaultTabBackHighColor;

    public void ResetTabBackHighColor() => _tabBackHighColor = _defaultTabBackHighColor;

    public bool ShouldSerializeTabBackHighColorDisabled() => _tabBackHighColorDisabled != _defaultTabBackHighColorDisabled;

    public void ResetTabBackHighColorDisabled() => _tabBackHighColorDisabled = _defaultTabBackHighColorDisabled;

    public bool ShouldSerializeTabBackLowColorDisabled() => _tabBackLowColorDisabled != _defaultTabBackLowColorDisabled;

    public void ResetTabBackLowColorDisabled() => _tabBackLowColorDisabled = _defaultTabBackLowColorDisabled;

    public bool ShouldSerializeBorderColorDisabled() => _torderColorDisabled != _defaultBorderColorDisabled;

    public void ResetBorderColorDisabled() => _torderColorDisabled = _defaultBorderColorDisabled;

    public bool ShouldSerializeForeColorDisabled() => _foreColorDisabled != _defaultForeColorDisabled;

    public void ResetForeColorDisabled() => _foreColorDisabled = _defaultForeColorDisabled;

    public bool ShouldSerializePadding() => _pnlBottom.Padding != _defaultPadding;

    public void ResetPadding() => _pnlBottom.Padding = _defaultPadding;

    public bool ShouldSerializeControlButtonBackHighColor() => _dropButton.BackHighColor != _defaultControlButtonBackHighColor;

    public void ResetControlButtonBackHighColor()
    {
        _dropButton.BackHighColor = _defaultControlButtonBackHighColor;
        _closeButton.BackHighColor = _defaultControlButtonBackHighColor;
    }

    public bool ShouldSerializeControlButtonBackLowColor() => _dropButton.BackLowColor != _defaultControlButtonBackLowColor;

    public void ResetControlButtonBackLowColor()
    {
        _dropButton.BackLowColor = _defaultControlButtonBackLowColor;
        _closeButton.BackLowColor = _defaultControlButtonBackLowColor;
    }

    public bool ShouldSerializeControlButtonBorderColor() => _dropButton.BorderColor != _defaultControlButtonBorderColor;

    public void ResetControlButtonBorderColor()
    {
        _dropButton.BorderColor = _defaultControlButtonBorderColor;
        _closeButton.BorderColor = _defaultControlButtonBorderColor;
    }

    public bool ShouldSerializeControlButtonForeColor() => _dropButton.ForeColor != _defaultControlButtonForeColor;

    public void ResetControlButtonForeColor()
    {
        _dropButton.ForeColor = _defaultControlButtonForeColor;
        _closeButton.ForeColor = _defaultControlButtonForeColor;
    }

    public bool ShouldSerializeBackLowColor() => _backLowColor != _defaultBackLowColor;

    public void ResetBackLowColor() => _backLowColor = _defaultBackLowColor;

    public bool ShouldSerializeBackHighColor() => _backHighColor != _defaultBackHighColor;

    public void ResetBackHighColor() => _backHighColor = _defaultBackHighColor;

    public bool ShouldSerializeBorderColor() => _borderColor != _defaultBorderColor;

    public void ResetBorderColor() => _borderColor = _defaultBorderColor;

    public bool ShouldSerializeRenderMode() => _renderMode != _defaultRenderMode;

    public void ResetRenderMode() => _renderMode = _defaultRenderMode;

    public void SetColors(ProfessionalColorTable colorTable)
    {
        BackHighColor = colorTable.ToolStripGradientEnd;
        BackLowColor = colorTable.ToolStripGradientBegin;
        BorderColor = colorTable.GripDark;
        BorderColorDisabled = colorTable.SeparatorDark;
        ControlButtonBackHighColor = colorTable.ButtonSelectedGradientBegin;
        ControlButtonBackLowColor = colorTable.ButtonSelectedGradientEnd;
        ControlButtonBorderColor = colorTable.ButtonPressedBorder;
        TabBackHighColor = colorTable.MenuItemPressedGradientBegin;
        TabBackLowColor = colorTable.MenuItemPressedGradientEnd;
        TabBackHighColorDisabled = colorTable.ToolStripDropDownBackground;
        TabBackLowColorDisabled = colorTable.ToolStripGradientMiddle;
        TabCloseButtonBackHighColor = Color.Transparent;
        TabCloseButtonBackHighColorDisabled = Color.Transparent;
        TabCloseButtonBackHighColorHot = Color.WhiteSmoke;
        TabCloseButtonBackLowColor = Color.Transparent;
        TabCloseButtonBackLowColorDisabled = Color.Transparent;
        TabCloseButtonBackLowColorHot = Color.LightGray;
        TabCloseButtonBorderColor = Color.Transparent;
        TabCloseButtonBorderColorDisabled = Color.Transparent;
        TabCloseButtonBorderColorHot = Color.Gray;
        TabCloseButtonForeColor = Color.Gray;
        TabCloseButtonForeColorDisabled = Color.Gray;
        TabCloseButtonForeColorHot = Color.Firebrick;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (ParentForm is Form parentForm)
        {
            parentForm.KeyPreview = true;
            parentForm.KeyDown += Owner_KeyDown;
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        if (ParentForm is Form parentForm)
        {
            parentForm.KeyDown -= Owner_KeyDown;
        }

        base.OnHandleDestroyed(e);
    }

    private void SetControlsSizeLocation()
    {
        _pnlControls.Width = _dropButton.Visible && _closeButton.Visible ? 43 : _dropButton.Visible || _closeButton.Visible ? 25 : 3;
        _pnlControls.Left = Width - _pnlControls.Width;
        CheckVisibility();
    }

    private void AdjustHeight()
    {
        if (Alignment is TabAlignment.Top)
        {
            _pnlTop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlTop.Height = _pnlTabs.Height + TabTop;
            _pnlTop.Top = TopSeparator ? 2 : 0;
            _pnlTabs.Top = TabTop;
            _pnlBottom.Height = Height - (_pnlTop.Height + (TopSeparator ? 2 : 0));
            _pnlBottom.Top = Height - _pnlBottom.Height;
        }
        else
        {
            _pnlTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _pnlTop.Height = _pnlTabs.Height + TabTop;
            _pnlTop.Top = Height - _pnlTop.Height;
            _pnlTabs.Top = 0;
            _pnlBottom.Height = Height - (_pnlTop.Height + (TopSeparator ? 2 : 0));
            _pnlBottom.Top = TopSeparator ? 2 : 0;
        }

        _pnlTop.Invalidate();
    }

    private void ArrangeItems()
    {
        if (TabPages.Count is 0)
        {
            return;
        }

        _pnlTabs.SuspendLayout();
        var x = _leftOffset;
        for (var i = 0; i < TabPages.Count; i++)
        {
            TabPages[i].TabVisible = x + TabPages[i].Width < _pnlControls.Left;
            if (TabPages[i].IsSelected && !TabPages[i].TabVisible)
            {
                SelectItem(TabPages[i]);
                _pnlTabs.ResumeLayout();
                return;
            }

            TabPages[i].TabLeft = x;
            x += TabPages[i].Width + TabOffset - 1;
        }

        if (!_addingPage)
        {
            if (_isDelete)
            {
                for (var i = TabPages.Count - 1; i >= 0; i--)
                {
                    ShowTab(i);
                }

                _isDelete = false;
            }
            else
            {
                for (var i = 0; i < TabPages.Count; i++)
                {
                    ShowTab(i);
                }
            }
        }

        _pnlTabs.ResumeLayout();
    }

    private void CheckVisibility()
    {
        if (TabPages is null)
        {
            return;
        }

        var x = _leftOffset;
        for (var i = 0; i < TabPages.Count; i++)
        {
            if (TabPages[i].TabVisible != (x + TabPages[i].Width < _pnlControls.Left))
            {
                if (TabPages[i].TabVisible)
                {
                    TabPages[i].TabVisible = false;
                    if (TabPages[i].IsSelected)
                    {
                        SelectItem(TabPages[i]);
                        return;
                    }

                    ShowTab(i);
                    return;
                }

                TabPages[i].TabVisible = true;
                TabPages[i].TabLeft = x;
                ShowTab(i);
            }
            else if (!TabPages[i].TabVisible)
            {
                return;
            }

            x += TabPages[i].Width + TabOffset - 1;
            if (x > _pnlControls.Left)
            {
                return;
            }
        }
    }

    private void ShowTab(int i)
    {
        TabPages[i].Visible = TabPages[i].TabVisible;
        if (TabPages[0].Width is not 1)
        {
            TabPages[i].Left = TabPages[i].TabLeft;
        }
    }

    private void SelectItem(TabPage? tabPage)
    {
        foreach (var t in TabPages)
        {
            t.IsSelected = false;
        }

        if (tabPage is { })
        {
            foreach (var t in TabPages)
            {
                if (TabsDirection is FlowDirection.LeftToRight)
                {
                    t.SendToBack();
                }
                else
                {
                    t.BringToFront();
                }
            }

            tabPage.InternalForm.Dock = DockStyle.Fill;
            tabPage.InternalForm.Visible = true;
            tabPage.BringToFront();
            tabPage.InternalForm.BringToFront();
            tabPage.InternalForm.Focus();
            if (_pnlBottom.Controls.Count > 1)
            {
                _pnlBottom.Controls[1].Visible = false;
                _pnlBottom.Controls[1].Dock = DockStyle.None;
            }

            tabPage.IsSelected = true;
            if (!tabPage.TabVisible && TabPages.IndexOf(tabPage) is not 0)
            {
                TabPages.SetIndex(tabPage, 0);
            }
        }
        else if (_pnlTabs.Controls.Count > 0)
        {
            foreach (var t in TabPages)
            {
                if (t.InternalForm.Equals(_pnlBottom.Controls[0]))
                {
                    t.Select();
                    break;
                }
            }
        }
    }

    private void TabControl_FontChanged(object? sender, EventArgs e)
    {
        foreach (var t in TabPages)
        {
            t.Font = Font;
        }
    }

    private void TabControl_ForeColorChanged(object? sender, EventArgs e)
    {
        foreach (var t in TabPages)
        {
            t.ForeColor = ForeColor;
        }
    }

    private void TabControl_Paint(object? sender, PaintEventArgs e)
    {
        if (TopSeparator)
        {
            ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(-2, 0, Width + 4, 1), Border3DStyle.Etched);
        }
    }

    private void TabControl_Resize(object? sender, EventArgs e) => CheckVisibility();

    private void PnlTop_SizeChanged(object? sender, EventArgs e) => PositionButtons();

    private void PositionButtons()
    {
        _dropButton.Top = (int)Math.Ceiling((_pnlTop.Height - _dropButton.Height) / 2.0) + (Alignment is TabAlignment.Top && TopSeparator ? -1 : 0);
        _closeButton.Top = _dropButton.Top;
    }

    private void PnlTop_Paint(object? sender, PaintEventArgs e)
    {
        using (LinearGradientBrush brush = new(new Point(0, 0), new Point(0, _pnlTop.Height), Helper.RenderColors.BackHighColor(_renderMode, BackHighColor), Helper.RenderColors.BackLowColor(_renderMode, BackLowColor)))
        {
            e.Graphics.FillRectangle(brush, new Rectangle(0, 0, _pnlTop.Width, _pnlTop.Height));
        }

        using Pen pen = new(Helper.RenderColors.BorderColor(_renderMode, BorderColor));
        if (Alignment is TabAlignment.Top)
        {
            e.Graphics.DrawLine(pen, 0, _pnlTop.Height - 1, _pnlTop.Width + 1, _pnlTop.Height - 1);
        }
        else
        {
            e.Graphics.DrawLine(pen, 0, 0, _pnlTop.Width + 1, 0);
        }
    }

    private void DropButton_MouseDown(object? sender, MouseEventArgs e)
    {
        _winMenu.Items.Clear();
        for (var i = 0; i < TabPages.Count; i++)
        {
            _winMenu.Items.Add(TabPages[i].MenuItem);
            TabPages[i].MenuItem.Click += MenuClick;
        }

        _winMenu.Show(_pnlTop, _pnlTop.Width - _winMenu.Width, _pnlTop.Height - 1);
    }

    private void MenuClick(object? sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem menuItem && menuItem.Tag is TabPage tabPage)
        {
            tabPage.Select();
        }
    }

    private void CloseButton_MouseDown(object? sender, MouseEventArgs e) => TabPages.SelectedTab()?.InternalForm.Close();

    private void TabPages_GetTabRegion(object? sender, GetTabRegionEventArgs e) => GetTabRegion?.Invoke(this, e);

    private void TabPages_SelectedChanged(object? sender, EventArgs e) => SelectedTabChanged?.Invoke(this, e);

    private void TabPages_TabPaintBackground(object? sender, TabPaintEventArgs e) => TabPaintBackground?.Invoke(this, e);

    private void TabPages_TabPaintBorder(object? sender, TabPaintEventArgs e) => TabPaintBorder?.Invoke(this, e);

    private void TabPages_TabRemoved(object? sender, EventArgs e) => TabRemoved?.Invoke(this, e);

    private void Owner_KeyDown(object? sender, KeyEventArgs e)
    {
        if (TabPages.Count is 0)
        {
            return;
        }

        KeyHandledEventArgs ke = new();
        int index;
        if (KeyTabEnabled && e.KeyCode is Keys.Tab && e.Control && !e.Alt)
        {
            if (e.Shift)
            {
                index = TabPages.SelectedIndex() - 1;
                if (index < 0)
                {
                    index = TabPages.Count - 1;
                }

                ke.Shift = true;
            }
            else
            {
                index = TabPages.SelectedIndex() + 1;
                if (index > TabPages.Count - 1)
                {
                    index = 0;
                }
            }

            ke.Index = index;
            ke.Handled = false;
            TabPressed?.Invoke(this, ke);
            if (!ke.Handled)
            {
                TabPages[index].Select();
            }
        }
        else if (KeyCloseEnabled && e.KeyCode is Keys.F4 && e.Control && !e.Shift && !e.Alt)
        {
            ke.Index = TabPages.SelectedIndex();
            ke.Handled = false;
            F4Pressed?.Invoke(this, ke);
            if (!ke.Handled)
            {
                TabPages.SelectedTab()?.InternalForm.Close();
            }
        }
    }

    public enum TabAlignment
    {
        Top = 0,
        Bottom = 1,
    }

    public enum FlowDirection
    {
        LeftToRight = 0,
        RightToLeft = 2,
    }

    public enum Weight
    {
        Soft = 2,
        Medium = 3,
        Strong = 4,
        Strongest = 5,
    }

    public class KeyHandledEventArgs : HandledEventArgs
    {
        public int Index { get; set; }

        public bool Shift { get; set; }
    }

    [Description("Provides data for the MdiTabCtrl.TabControl.GetTabRegion event.")]
    public class GetTabRegionEventArgs : EventArgs
    {
        private readonly bool _selected;

        [Description("Initializes a new instance of the MdiTabCtrl.TabControl.GetTabRegionEventArgs class.")]
        public GetTabRegionEventArgs(Point[] points, int width, int height, bool selected)
        {
            Points = points;
            TabWidth = width;
            TabHeight = height;
            _selected = selected;
        }

        private GetTabRegionEventArgs() => Points = [];

        [Description("Returns whether the tab is selected or not.")]
        public int Selected => _selected ? 1 : 0;

        [Description("Returns the tab width.")]
        public int TabWidth { get; }

        [Description("Returns the tab height.")]
        public int TabHeight { get; }

        [Description("Gets or sets an array of System.Drawing.Point structures that represents the points through which the tab path is constructed.")]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Won't change interface now")]
        public Point[] Points { get; set; }
    }

    [Description("Provides data for the MdiTabCtrl.TabControl.TabPaint event.")]
    [method: Description("Initializes a new instance of the MdiTabCtrl.TabControl.GetTabRegionEventArgs class.")]
    public class TabPaintEventArgs(Graphics graphics, Rectangle clipRect, bool selected, bool hot, GraphicsPath graphicPath, int width, int height)
        : PaintEventArgs(graphics, clipRect)
    {
        [Description("Returns the tab's hot state.")]
        public bool Hot { get; } = hot;

        [Description("Returns whether the tab is selected or not.")]
        public bool Selected { get; } = selected;

        [Description("Gets or sets a value that indicates whether the event handler has completely handled the paint or whether the system should continue its own processing.")]
        public bool Handled { get; set; }

        [Description("Returns the tab width.")]
        public int TabWidth { get; } = width;

        [Description("Returns the tab height.")]
        public int TabHeight { get; } = height;

        [Description("Represents a series of connected lines and curves which the tab path is constructed.")]
        public GraphicsPath GraphicPath { get; } = graphicPath;
    }

    [Description("Contains a collection of MdiTabCtrl.TabPage objects.")]
    public sealed class TabPageCollection : Collection<TabPage>
    {
        private readonly TabControl _tabControl;
        private bool _isReorder;
        private TabPage? _currentTab;

        internal TabPageCollection(TabControl owner) => _tabControl = owner;

        internal event EventHandler<GetTabRegionEventArgs>? GetTabRegion;

        [Description("Occurs when the Tab Background has been painted.")]
        internal event EventHandler<TabPaintEventArgs>? TabPaintBackground;

        [Description("Occurs when the Tab Border has been painted.")]
        internal event EventHandler<TabPaintEventArgs>? TabPaintBorder;

        internal event EventHandler? SelectedChanged;

        internal event EventHandler? TabRemoved;

        [Description("Gets a TabPage associated with the Form from the collection.")]
        public TabPage? this[Form form]
        {
            get
            {
                var x = IndexOf(form);
                return x == -1 ? null : this[x];
            }
        }

        public void SetIndex(TabPage tabPage, int value)
        {
            _isReorder = true;
            Items.Remove(tabPage);
            Items.Insert(value, tabPage);
            _tabControl.ArrangeItems();
            _isReorder = false;
        }

        [Description("Returns the index of the specified TabPage associated with the Form in the collection.")]
        public int IndexOf(Form form)
        {
            var ret = -1;
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i]!.InternalForm.Equals(form))
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }

        [Description("Create a new TabPage and adds it to the collection whit the Form associated and returns the created TabPage.")]
        public TabPage Add(Form form)
        {
            TabPage tabPage = new(form);
            tabPage.SuspendLayout();
            _tabControl.SuspendLayout();

            _tabControl._addingPage = true;
            tabPage.BackHighColor = _tabControl.TabBackHighColor;
            tabPage.BackHighColorDisabled = _tabControl.TabBackHighColorDisabled;
            tabPage.BackLowColor = _tabControl.TabBackLowColor;
            tabPage.BackLowColorDisabled = _tabControl.TabBackLowColorDisabled;
            tabPage.BorderColor = _tabControl.BorderColor;
            tabPage.BorderColorDisabled = _tabControl.BorderColorDisabled;
            tabPage.ForeColor = _tabControl.ForeColor;
            tabPage.ForeColorDisabled = _tabControl.ForeColorDisabled;
            tabPage.MaximumWidth = _tabControl.TabMaximumWidth;
            tabPage.MinimumWidth = _tabControl.TabMinimumWidth;
            tabPage.PadLeft = _tabControl.TabPadLeft;
            tabPage.PadRight = _tabControl.TabPadRight;
            tabPage.CloseButtonVisible = _tabControl.TabCloseButtonVisible;
            tabPage.CloseButtonImage = _tabControl.TabCloseButtonImage;
            tabPage.CloseButtonImageHot = _tabControl.TabCloseButtonImageHot;
            tabPage.CloseButtonImageDisabled = _tabControl.TabCloseButtonImageDisabled;
            tabPage.CloseButtonSize = _tabControl.TabCloseButtonSize;
            tabPage.CloseButtonBackHighColor = _tabControl.TabCloseButtonBackHighColor;
            tabPage.CloseButtonBackLowColor = _tabControl.TabCloseButtonBackLowColor;
            tabPage.CloseButtonBorderColor = _tabControl.TabCloseButtonBorderColor;
            tabPage.CloseButtonForeColor = _tabControl.TabCloseButtonForeColor;
            tabPage.CloseButtonBackHighColorDisabled = _tabControl.TabCloseButtonBackHighColorDisabled;
            tabPage.CloseButtonBackLowColorDisabled = _tabControl.TabCloseButtonBackLowColorDisabled;
            tabPage.CloseButtonBorderColorDisabled = _tabControl.TabCloseButtonBorderColorDisabled;
            tabPage.CloseButtonForeColorDisabled = _tabControl.TabCloseButtonForeColorDisabled;
            tabPage.CloseButtonBackHighColorHot = _tabControl.TabCloseButtonBackHighColorHot;
            tabPage.CloseButtonBackLowColorHot = _tabControl.TabCloseButtonBackLowColorHot;
            tabPage.CloseButtonBorderColorHot = _tabControl.TabCloseButtonBorderColorHot;
            tabPage.CloseButtonForeColorHot = _tabControl.TabCloseButtonForeColorHot;
            tabPage.HotTrack = _tabControl.HotTrack;
            tabPage.Font = _tabControl.Font;
            tabPage.FontBoldOnSelect = _tabControl.FontBoldOnSelect;
            tabPage.IconSize = _tabControl.TabIconSize;
            tabPage.SmoothingMode = _tabControl.SmoothingMode;
            tabPage.Alignment = _tabControl.Alignment;
            tabPage.GlassGradient = _tabControl.TabGlassGradient;
            tabPage.BorderEnhanced = _tabControl.TabBorderEnhanced;
            tabPage.RenderMode = _tabControl.RenderMode;
            tabPage.BorderEnhanceWeight = _tabControl.TabBorderEnhanceWeight;

            tabPage.Top = 0;
            tabPage.Left = _tabControl._leftOffset;
            tabPage.Height = _tabControl.TabHeight;

            _tabControl._tabToolTip.SetToolTip(tabPage, tabPage.InternalForm.Text);

            tabPage.Click += TabPage_Clicked;
            tabPage.Close += TabPage_Closed;
            tabPage.GetTabRegion += TabPage_GetTabRegion;
            tabPage.TabPaintBackground += TabPage_TabPaintBackground;
            tabPage.TabPaintBorder += TabPage_TabPaintBorder;
            tabPage.SizeChanged += TabPage_SizeChanged;
            tabPage.Dragging += TabPage_Dragging;
            tabPage.EndDrag += TabPage_EndDrag;
            tabPage.EnterForm += TabPage_Enter;
            tabPage.LeaveForm += TabPage_Leave;

            Add(tabPage);
            _tabControl.ResumeLayout();
            tabPage.ResumeLayout();
            return tabPage;
        }

        [Description("Removes a TabPage from the collection.")]
        new public void Remove(TabPage tabPage)
        {
            try
            {
                _tabControl._isDelete = true;
                if (_tabControl._pnlBottom.Controls.Count > 1)
                {
                    _tabControl._pnlBottom.Controls[1].Dock = DockStyle.Fill;
                    _tabControl._pnlBottom.Controls[1].Visible = true;
                }

                base.Remove(tabPage);
                TabRemoved?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        public Form TearOff(TabPage tabPage)
        {
            var f = tabPage.InternalForm;
            _tabControl._pnlBottom.Controls.Remove(f);
            tabPage.InternalForm = new();
            Remove(tabPage);
            f.TopLevel = true;
            f.Dock = DockStyle.None;
            f.FormBorderStyle = FormBorderStyle.Sizable;
            return f;
        }

        [Description("Returns the selected TabPage.")]
        public TabPage? SelectedTab()
        {
            foreach (var t in Items)
            {
                if (t.IsSelected)
                {
                    return t;
                }
            }

            return null;
        }

        [Description("Returns the index of the selected TabPage.")]
        public int SelectedIndex()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i]!.IsSelected)
                {
                    return i;
                }
            }

            return -1;
        }

        protected override void InsertItem(int index, TabPage item)
        {
            base.InsertItem(index, item);
            if (_isReorder)
            {
                return;
            }

            _tabControl._pnlBottom.Controls.Add(item.InternalForm);
            _tabControl._pnlTabs.Controls.Add(item);
            item.Select();
            _tabControl._addingPage = false;
            _tabControl.ArrangeItems();
            _tabControl._background.Visible = false;
        }

        protected override void RemoveItem(int index)
        {
            var value = this[index];

            base.RemoveItem(index);

            if (_isReorder)
            {
                return;
            }

            if (Items.Count is 0)
            {
                _tabControl._background.Visible = true;
            }

            _tabControl.ArrangeItems();
            _tabControl._pnlBottom.Controls.Remove(value!.InternalForm);
            value.InternalForm.Dispose();
            _tabControl._pnlTabs.Controls.Remove(value);
            value.Dispose();
            _tabControl.SelectItem(tabPage: null);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            _tabControl._background.Visible = true;
            _tabControl._pnlBottom.Controls.Clear();
            _tabControl._pnlTabs.Controls.Clear();
        }

        private void TabPage_Clicked(object? sender, EventArgs e)
        {
            _tabControl.SelectItem((TabPage)sender!);
            SelectedChanged?.Invoke(this, e);
        }

        private void TabPage_Closed(object? sender, EventArgs e) => Remove((TabPage)sender!);

        private void TabPage_GetTabRegion(object? sender, GetTabRegionEventArgs e) => GetTabRegion?.Invoke(this, e);

        private void TabPage_TabPaintBackground(object? sender, TabPaintEventArgs e) => TabPaintBackground?.Invoke(this, e);

        private void TabPage_TabPaintBorder(object? sender, TabPaintEventArgs e) => TabPaintBorder?.Invoke(this, e);

        private void TabPage_SizeChanged(object? sender, EventArgs e) => _tabControl.ArrangeItems();

        private void TabPage_Dragging(object? sender, MouseEventArgs e)
        {
            if (_tabControl.AllowTabReorder && e.Button is MouseButtons.Left)
            {
                var t = GetTabPage((TabPage)sender!, e.X, e.Y);
                if (t is null)
                {
                    _currentTab = null;
                }
                else if (t != _currentTab)
                {
                    SetIndex((TabPage)sender!, IndexOf(t));
                    _currentTab = t;
                }
            }
        }

        private void TabPage_EndDrag(object? sender, MouseEventArgs e) => _currentTab = null;

        [SuppressMessage("Style", "IDE0370:Remove unnecessary suppression", Justification = "Suppression is necessary for NETFRAMEWORK")]
        private TabPage? GetTabPage(TabPage tabPage, int x, int y)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var page = Items[i]!;
                if (!ReferenceEquals(page, tabPage) && page.TabVisible
                    && page.RectangleToScreen(page.ClientRectangle).Contains(tabPage.PointToScreen(new(x, y))))
                {
                    return page;
                }
            }

            return null;
        }

        private void TabPage_Enter(object? sender, EventArgs e)
        {
            if (!_tabControl._focused)
            {
                _tabControl.SetFocus = true;
            }
        }

        private void TabPage_Leave(object? sender, EventArgs e)
        {
            if (_tabControl._focused)
            {
                _tabControl.SetFocus = false;
            }
        }
    }
}
