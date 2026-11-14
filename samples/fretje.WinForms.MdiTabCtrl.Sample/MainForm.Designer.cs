using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace fretje.WinForms.MdiTabCtrl.Sample;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _tabControl = new TabControl();
        _menuStrip = new MenuStrip();
        _fileMenu = new ToolStripMenuItem();
        _newTabItem = new ToolStripMenuItem();
        _exitItem = new ToolStripMenuItem();
        _menuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // _tabControl
        // 
        _tabControl.Dock = DockStyle.Fill;
        _tabControl.Location = new Point(0, 24);
        _tabControl.Name = "_tabControl";
        _tabControl.Size = new Size(738, 463);
        _tabControl.TabIndex = 0;
        // 
        // menuStrip
        // 
        _menuStrip.Items.AddRange(new ToolStripItem[] { _fileMenu });
        _menuStrip.Location = new Point(0, 0);
        _menuStrip.Name = "menuStrip";
        _menuStrip.Size = new Size(738, 24);
        _menuStrip.TabIndex = 1;
        _menuStrip.Text = "MenuStrip";
        // 
        // fileMenu
        // 
        _fileMenu.DropDownItems.AddRange(new ToolStripItem[] { _newTabItem, _exitItem });
        _fileMenu.Name = "fileMenu";
        _fileMenu.Size = new Size(37, 20);
        _fileMenu.Text = "&File";
        // 
        // newTabItem
        // 
        _newTabItem.Name = "newTabItem";
        _newTabItem.Size = new Size(119, 22);
        _newTabItem.Text = "&New Tab";
        _newTabItem.Click += OnNewTab;
        // 
        // exitItem
        // 
        _exitItem.Name = "exitItem";
        _exitItem.Size = new Size(119, 22);
        _exitItem.Text = "E&xit";
        _exitItem.Click += OnExit;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(738, 487);
        Controls.Add(_tabControl);
        Controls.Add(_menuStrip);
        KeyPreview = true;
        MainMenuStrip = _menuStrip;
        Name = "MainForm";
        Text = "MdiTabCtrl Sample - Demonstrating all features";
        _menuStrip.ResumeLayout(false);
        _menuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private fretje.WinForms.MdiTabCtrl.TabControl _tabControl;
    private MenuStrip _menuStrip;
    private ToolStripMenuItem _fileMenu;
    private ToolStripMenuItem _newTabItem;
    private ToolStripMenuItem _exitItem;
}
