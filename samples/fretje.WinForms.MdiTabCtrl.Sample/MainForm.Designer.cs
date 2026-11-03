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
    /// The tab control.
    /// </summary>
    private fretje.WinForms.MdiTabCtrl.TabControl _tabControl;

    /// <summary>
    /// The menu strip.
    /// </summary>
    private MenuStrip menuStrip;

    /// <summary>
    /// The file menu.
    /// </summary>
    private ToolStripMenuItem fileMenu;

    /// <summary>
    /// The new tab menu item.
    /// </summary>
    private ToolStripMenuItem newTabItem;

    /// <summary>
    /// The exit menu item.
    /// </summary>
    private ToolStripMenuItem exitItem;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        menuStrip = new MenuStrip();
        fileMenu = new ToolStripMenuItem();
        newTabItem = new ToolStripMenuItem();
        exitItem = new ToolStripMenuItem();
        menuStrip.SuspendLayout();
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
        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(738, 24);
        menuStrip.TabIndex = 1;
        menuStrip.Text = "MenuStrip";
        // 
        // fileMenu
        // 
        fileMenu.DropDownItems.AddRange(new ToolStripItem[] { newTabItem, exitItem });
        fileMenu.Name = "fileMenu";
        fileMenu.Size = new Size(37, 20);
        fileMenu.Text = "&File";
        // 
        // newTabItem
        // 
        newTabItem.Name = "newTabItem";
        newTabItem.Size = new Size(119, 22);
        newTabItem.Text = "&New Tab";
        newTabItem.Click += OnNewTab;
        // 
        // exitItem
        // 
        exitItem.Name = "exitItem";
        exitItem.Size = new Size(119, 22);
        exitItem.Text = "E&xit";
        exitItem.Click += OnExit;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(738, 487);
        Controls.Add(_tabControl);
        Controls.Add(menuStrip);
        KeyPreview = true;
        MainMenuStrip = menuStrip;
        Name = "MainForm";
        Text = "MdiTabCtrl Sample - Demonstrating all features";
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
