using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;

namespace fretje.WinForms.MdiTabCtrl;

[DesignTimeVisible(true)]
public partial class TabControl : UserControl
{
    internal static readonly Padding _defaultPadding = new(0, 0, 0, 0);
    internal static readonly Color _defaultBackLowColor = SystemColors.ControlLightLight;
    internal static readonly Color _defaultBackHighColor = SystemColors.Control;
    internal static readonly Color _defaultBorderColor = SystemColors.ControlDarkDark;
    internal static readonly Color _defaultTabBackHighColor = SystemColors.Window;
    internal static readonly Color _defaultTabBackLowColor = SystemColors.Control;
    internal static readonly Color _defaultTabBackHighColorDisabled = SystemColors.Control;
    internal static readonly Color _defaultTabBackLowColorDisabled = SystemColors.ControlDark;
    internal static readonly Color _defaultBorderColorDisabled = SystemColors.ControlDark;
    internal static readonly Color _defaultForeColorDisabled = SystemColors.ControlText;
    internal static readonly Color _defaultControlButtonBackHighColor = SystemColors.GradientInactiveCaption;
    internal static readonly Color _defaultControlButtonBackLowColor = SystemColors.GradientInactiveCaption;
    internal static readonly Color _defaultControlButtonBorderColor = SystemColors.HotTrack;
    internal static readonly Color _defaultControlButtonForeColor = SystemColors.ControlText;
    internal static readonly Size _defaultTabCloseButtonSize = new(17, 17);
    internal static readonly Size _defaultTabIconSize = new(16, 16);
    internal static readonly Color _defaultTabCloseButtonBackHighColor = Color.IndianRed;
    internal static readonly Color _defaultTabCloseButtonBackHighColorDisabled = Color.LightGray;
    internal static readonly Color _defaultTabCloseButtonBackHighColorHot = Color.LightCoral;
    internal static readonly Color _defaultTabCloseButtonBackLowColor = Color.Firebrick;
    internal static readonly Color _defaultTabCloseButtonBackLowColorDisabled = Color.DarkGray;
    internal static readonly Color _defaultTabCloseButtonBackLowColorHot = Color.IndianRed;
    internal static readonly Color _defaultTabCloseButtonBorderColor = Color.DarkRed;
    internal static readonly Color _defaultTabCloseButtonBorderColorDisabled = Color.Gray;
    internal static readonly Color _defaultTabCloseButtonBorderColorHot = Color.Firebrick;
    internal static readonly Color _defaultTabCloseButtonForeColor = Color.White;
    internal static readonly Color _defaultTabCloseButtonForeColorDisabled = Color.White;
    internal static readonly Color _defaultTabCloseButtonForeColorHot = Color.White;
    internal static readonly ToolStripRenderMode _defaultRenderMode = ToolStripRenderMode.ManagerRenderMode;

    private readonly int _leftOffset = 3;
    private readonly Panel _background = new();
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
        DropButton.RenderMode = RenderMode;
        CloseButton.RenderMode = RenderMode;
        ResumeLayout();
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
    public object? SelectedForm => pnlBottom.Controls.Count > 0 ? pnlBottom.Controls[0] : null;

    [Category("Layout"), DefaultValue(FlowDirection.LeftToRight), Description("Gets or sets the the direction which the tabs are drawn.")]
    public FlowDirection TabsDirection
    {
        get;
        set
        {
            field = value;
            SelectItem(null);
        }
    }

    [Category("Appearance"), DefaultValue(false), Description("Gets or sets if the tab background will paint with glass style.")]
    public bool TabGlassGradient
    {
        get;
        set
        {
            field = value;
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
                pnlTabs.Height = field;
                pnlTabs.Top = pnlTop.Height - pnlTabs.Height;
                AdjustHeight();
                foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
            foreach (TabPage t in TabPages)
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
        get => CloseButton.Visible;
        set
        {
            if (CloseButton.Visible != value)
            {
                CloseButton.Visible = value;
                SetControlsSizeLocation();
            }
        }
    }

    [Category("Layout"), DefaultValue(true), Description("Gets or sets a value indicating whether the drop button is displayed or not.")]
    public bool DropButtonVisible
    {
        get => DropButton.Visible;
        set
        {
            if (DropButton.Visible != value)
            {
                DropButton.Visible = value;
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
            foreach (TabPage t in TabPages)
            {
                t.Alignment = value;
            }
        }
    }

        = TabAlignment.Top;

    [Category("Layout"), Description("Gets or sets the amount of space around the form on the control's tab pages.")]
    public new Padding Padding
    {
        get => pnlBottom.Padding;
        set => pnlBottom.Padding = value;
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the starting color of the Background linear gradient for the control button.")]
    public Color ControlButtonBackHighColor
    {
        get => DropButton.BackHighColor;
        set
        {
            DropButton.BackHighColor = value;
            CloseButton.BackHighColor = value;
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ending color of the Background linear gradient for the control button.")]
    public Color ControlButtonBackLowColor
    {
        get => DropButton.BackLowColor;
        set
        {
            DropButton.BackLowColor = value;
            CloseButton.BackLowColor = value;
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the border color for the control button.")]
    public Color ControlButtonBorderColor
    {
        get => DropButton.BorderColor;
        set
        {
            DropButton.BorderColor = value;
            CloseButton.BorderColor = value;
        }
    }

    [Category("Appearance"), Description("Gets or sets the System.Drawing.Color structure that represents the ForeColor for the control button.")]
    public Color ControlButtonForeColor
    {
        get => DropButton.ForeColor;
        set
        {
            DropButton.ForeColor = value;
            CloseButton.ForeColor = value;
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
            foreach (TabPage t in TabPages)
            {
                t.BorderColor = value;
            }

            pnlTabs.Invalidate();
            pnlTop.Invalidate();
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
            DropButton.RenderMode = value;
            CloseButton.RenderMode = value;
            WinMenu.RenderMode = value;
            foreach (TabPage t in TabPages)
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
            if (value != null)
            {
                WinMenu.Renderer = value;
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
            foreach (TabPage t in TabPages)
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

    public bool ShouldSerializePadding() => pnlBottom.Padding != _defaultPadding;

    public void ResetPadding() => pnlBottom.Padding = _defaultPadding;

    public bool ShouldSerializeControlButtonBackHighColor() => DropButton.BackHighColor != _defaultControlButtonBackHighColor;

    public void ResetControlButtonBackHighColor()
    {
        DropButton.BackHighColor = _defaultControlButtonBackHighColor;
        CloseButton.BackHighColor = _defaultControlButtonBackHighColor;
    }

    public bool ShouldSerializeControlButtonBackLowColor() => DropButton.BackLowColor != _defaultControlButtonBackLowColor;

    public void ResetControlButtonBackLowColor()
    {
        DropButton.BackLowColor = _defaultControlButtonBackLowColor;
        CloseButton.BackLowColor = _defaultControlButtonBackLowColor;
    }

    public bool ShouldSerializeControlButtonBorderColor() => DropButton.BorderColor != _defaultControlButtonBorderColor;

    public void ResetControlButtonBorderColor()
    {
        DropButton.BorderColor = _defaultControlButtonBorderColor;
        CloseButton.BorderColor = _defaultControlButtonBorderColor;
    }

    public bool ShouldSerializeControlButtonForeColor() => DropButton.ForeColor != _defaultControlButtonForeColor;

    public void ResetControlButtonForeColor()
    {
        DropButton.ForeColor = _defaultControlButtonForeColor;
        CloseButton.ForeColor = _defaultControlButtonForeColor;
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
        pnlControls.Width = DropButton.Visible && CloseButton.Visible ? 43 : DropButton.Visible || CloseButton.Visible ? 25 : 3;
        pnlControls.Left = Width - pnlControls.Width;
        CheckVisibility();
    }

    private void AdjustHeight()
    {
        if (Alignment == TabAlignment.Top)
        {
            pnlTop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlTop.Height = pnlTabs.Height + TabTop;
            pnlTop.Top = TopSeparator ? 2 : 0;
            pnlTabs.Top = TabTop;
            pnlBottom.Height = Height - (pnlTop.Height + (TopSeparator ? 2 : 0));
            pnlBottom.Top = Height - pnlBottom.Height;
        }
        else
        {
            pnlTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlTop.Height = pnlTabs.Height + TabTop;
            pnlTop.Top = Height - pnlTop.Height;
            pnlTabs.Top = 0;
            pnlBottom.Height = Height - (pnlTop.Height + (TopSeparator ? 2 : 0));
            pnlBottom.Top = TopSeparator ? 2 : 0;
        }

        pnlTop.Invalidate();
    }

    private void ArrangeItems()
    {
        if (TabPages.Count == 0)
        {
            return;
        }

        pnlTabs.SuspendLayout();
        var x = _leftOffset;
        for (var i = 0; i < TabPages.Count; i++)
        {
            TabPages[i].TabVisible = x + TabPages[i].Width < pnlControls.Left;
            if (TabPages[i].IsSelected && !TabPages[i].TabVisible)
            {
                SelectItem(TabPages[i]);
                pnlTabs.ResumeLayout();
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

        pnlTabs.ResumeLayout();
    }

    private void CheckVisibility()
    {
        if (TabPages == null)
        {
            return;
        }

        var x = _leftOffset;
        for (var i = 0; i < TabPages.Count; i++)
        {
            if (TabPages[i].TabVisible != (x + TabPages[i].Width < pnlControls.Left))
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
            if (x > pnlControls.Left)
            {
                return;
            }
        }
    }

    private void ShowTab(int i)
    {
        TabPages[i].Visible = TabPages[i].TabVisible;
        if (TabPages[0].Width != 1)
        {
            TabPages[i].Left = TabPages[i].TabLeft;
        }
    }

    private void SelectItem(TabPage? tabPage)
    {
        foreach (TabPage t in TabPages)
        {
            t.IsSelected = false;
        }

        if (tabPage != null)
        {
            foreach (TabPage t in TabPages)
            {
                if (TabsDirection == FlowDirection.LeftToRight)
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
            if (pnlBottom.Controls.Count > 1)
            {
                pnlBottom.Controls[1].Visible = false;
                pnlBottom.Controls[1].Dock = DockStyle.None;
            }

            tabPage.IsSelected = true;
            if (!tabPage.TabVisible && TabPages.IndexOf(tabPage) != 0)
            {
                TabPages.SetIndex(tabPage, 0);
            }
        }
        else if (pnlTabs.Controls.Count > 0)
        {
            foreach (TabPage t in TabPages)
            {
                if (t.InternalForm.Equals(pnlBottom.Controls[0]))
                {
                    t.Select();
                    break;
                }
            }
        }
    }

    private void TabControl_FontChanged(object? sender, EventArgs e)
    {
        foreach (TabPage t in TabPages)
        {
            t.Font = Font;
        }
    }

    private void TabControl_ForeColorChanged(object? sender, EventArgs e)
    {
        foreach (TabPage t in TabPages)
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
        DropButton.Top = (int)Math.Ceiling((pnlTop.Height - DropButton.Height) / 2.0) + ((Alignment == TabAlignment.Top && TopSeparator) ? -1 : 0);
        CloseButton.Top = DropButton.Top;
    }

    private void PnlTop_Paint(object? sender, PaintEventArgs e)
    {
        using (LinearGradientBrush brush = new(new Point(0, 0), new Point(0, pnlTop.Height), Helper.RenderColors.BackHighColor(_renderMode, BackHighColor), Helper.RenderColors.BackLowColor(_renderMode, BackLowColor)))
        {
            e.Graphics.FillRectangle(brush, new Rectangle(0, 0, pnlTop.Width, pnlTop.Height));
        }

        using Pen pen = new(Helper.RenderColors.BorderColor(_renderMode, BorderColor));
        if (Alignment == TabAlignment.Top)
        {
            e.Graphics.DrawLine(pen, 0, pnlTop.Height - 1, pnlTop.Width + 1, pnlTop.Height - 1);
        }
        else
        {
            e.Graphics.DrawLine(pen, 0, 0, pnlTop.Width + 1, 0);
        }
    }

    private void DropButton_MouseDown(object? sender, MouseEventArgs e)
    {
        WinMenu.Items.Clear();
        for (var i = 0; i < TabPages.Count; i++)
        {
            WinMenu.Items.Add(TabPages[i].MenuItem);
            TabPages[i].MenuItem.Click += MenuClick;
        }

        WinMenu.Show(pnlTop, pnlTop.Width - WinMenu.Width, pnlTop.Height - 1);
    }

    private void MenuClick(object? sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem menuItem && menuItem.Tag is TabPage tabPage)
        {
            tabPage.Select();
        }
    }

    private void CloseButton_MouseDown(object? sender, MouseEventArgs e) => TabPages.SelectedTab()?.InternalForm.Close();

    private void TabPages_GetTabRegion(object? sender, GetTabRegionEventArgs e) => GetTabRegion?.Invoke(sender, e);

    private void TabPages_SelectedChanged(object? sender, EventArgs e) => SelectedTabChanged?.Invoke(sender, e);

    private void TabPages_TabPaintBackground(object? sender, TabPaintEventArgs e) => TabPaintBackground?.Invoke(sender, e);

    private void TabPages_TabPaintBorder(object? sender, TabPaintEventArgs e) => TabPaintBorder?.Invoke(sender, e);

    private void TabPages_TabRemoved(object? sender, TabPage e) => TabRemoved?.Invoke(sender, EventArgs.Empty);

    private void Owner_KeyDown(object? sender, KeyEventArgs e)
    {
        if (TabPages.Count == 0)
        {
            return;
        }

        KeyHandledEventArgs ke = new();
        int index;
        if (KeyTabEnabled && e.KeyCode == Keys.Tab && e.Control && !e.Alt)
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
        else if (KeyCloseEnabled && e.KeyCode == Keys.F4 && e.Control && !e.Shift && !e.Alt)
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
    public class TabPageCollection : CollectionBase
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

        internal event EventHandler<TabPage>? TabRemoved;

        [Description("Gets a TabPage in the position Index from the collection.")]
        public TabPage this[int index] => (TabPage)List[index]!;

        [Description("Gets a TabPage associated with the Form from the collection.")]
        public TabPage? this[Form form]
        {
            get
            {
                var x = IndexOf(form);
                return x == -1 ? null : this[x];
            }
        }

        [Description("Returns the index of the specified TabPage in the collection.")]
        public int IndexOf(TabPage tabPage) => List.IndexOf(tabPage);

        public void SetIndex(TabPage tabPage, int value)
        {
            _isReorder = true;
            List.Remove(tabPage);
            List.Insert(value, tabPage);
            _tabControl.ArrangeItems();
            _isReorder = false;
        }

        [Description("Returns the index of the specified TabPage associated with the Form in the collection.")]
        public int IndexOf(Form form)
        {
            var ret = -1;
            for (var i = 0; i < List.Count; i++)
            {
                if (((TabPage)List[i]!).InternalForm.Equals(form))
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

            _tabControl.TabToolTip.SetToolTip(tabPage, tabPage.InternalForm.Text);

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

            List.Add(tabPage);
            _tabControl.ResumeLayout();
            tabPage.ResumeLayout();
            return tabPage;
        }

        [Description("Removes a TabPage from the collection.")]
        public void Remove(TabPage tabPage)
        {
            try
            {
                _tabControl._isDelete = true;
                if (_tabControl.pnlBottom.Controls.Count > 1)
                {
                    _tabControl.pnlBottom.Controls[1].Dock = DockStyle.Fill;
                    _tabControl.pnlBottom.Controls[1].Visible = true;
                }

                List.Remove(tabPage);
                TabRemoved?.Invoke(this, tabPage);
            }
            catch { }
        }

        public Form TearOff(TabPage tabPage)
        {
            var f = tabPage.InternalForm;
            _tabControl.pnlBottom.Controls.Remove(f);
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
            foreach (TabPage t in List)
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
            for (var i = 0; i < List.Count; i++)
            {
                if (((TabPage)List[i]!).IsSelected)
                {
                    return i;
                }
            }

            return -1;
        }

        protected override void OnInsertComplete(int index, object? value)
        {
            base.OnInsertComplete(index, value);
            if (_isReorder)
            {
                return;
            }

            _tabControl.pnlBottom.Controls.Add(((TabPage)value!).InternalForm);
            _tabControl.pnlTabs.Controls.Add((TabPage)value);
            ((TabPage)value).Select();
            _tabControl._addingPage = false;
            _tabControl.ArrangeItems();
            _tabControl._background.Visible = false;
        }

        protected override void OnRemoveComplete(int index, object? value)
        {
            base.OnRemoveComplete(index, value);
            if (_isReorder)
            {
                return;
            }

            if (List.Count == 0)
            {
                _tabControl._background.Visible = true;
            }

            _tabControl.ArrangeItems();
            _tabControl.pnlBottom.Controls.Remove(((TabPage)value!).InternalForm);
            ((TabPage)value).InternalForm.Dispose();
            _tabControl.pnlTabs.Controls.Remove((TabPage)value);
            ((TabPage)value).Dispose();
            _tabControl.SelectItem(null);
        }

        protected override void OnClear()
        {
            base.OnClear();
            _tabControl._background.Visible = true;
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            _tabControl.pnlBottom.Controls.Clear();
            _tabControl.pnlTabs.Controls.Clear();
        }

        private void TabPage_Clicked(object? sender, EventArgs e)
        {
            _tabControl.SelectItem((TabPage)sender!);
            SelectedChanged?.Invoke(sender, e);
        }

        private void TabPage_Closed(object? sender, EventArgs e) => Remove((TabPage)sender!);

        private void TabPage_GetTabRegion(object? sender, GetTabRegionEventArgs e) => GetTabRegion?.Invoke(sender, e);

        private void TabPage_TabPaintBackground(object? sender, TabPaintEventArgs e) => TabPaintBackground?.Invoke(sender, e);

        private void TabPage_TabPaintBorder(object? sender, TabPaintEventArgs e) => TabPaintBorder?.Invoke(sender, e);

        private void TabPage_SizeChanged(object? sender, EventArgs e) => _tabControl.ArrangeItems();

        private void TabPage_Dragging(object? sender, MouseEventArgs e)
        {
            if (_tabControl.AllowTabReorder && e.Button == MouseButtons.Left)
            {
                var t = GetTabPage((TabPage)sender!, e.X, e.Y);
                if (t == null)
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
            for (var i = 0; i < List.Count; i++)
            {
                var page = (TabPage)List[i]!;
                if (!ReferenceEquals(page, tabPage) && page.TabVisible
                    && page.RectangleToScreen(page.ClientRectangle).Contains(tabPage.PointToScreen(new Point(x, y))))
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
