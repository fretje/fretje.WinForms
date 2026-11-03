using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace fretje.WinForms.MdiTabCtrl
{
    partial class TabControl
    {
        private IContainer components = null!;
        internal Panel pnlTop = null!;
        internal Panel pnlTabs = null!;
        internal Panel pnlBottom = null!;
        internal ContextMenuStrip WinMenu = null!;
        internal ControlButton DropButton = null!;
        internal ToolTip TabToolTip = null!;
        internal ControlButton CloseButton = null!;
        internal Panel pnlControls = null!;

        private void InitializeComponent()
        {
            components = new Container();
            pnlTop = new Panel();
            pnlControls = new Panel();
            DropButton = new ControlButton();
            CloseButton = new ControlButton();
            pnlTabs = new Panel();
            pnlBottom = new Panel();
            WinMenu = new ContextMenuStrip(components);
            TabToolTip = new ToolTip(components);
            pnlTop.SuspendLayout();
            pnlControls.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlTop.BackColor = Color.Transparent;
            pnlTop.Controls.Add(pnlControls);
            pnlTop.Controls.Add(pnlTabs);
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(200, 31);
            pnlTop.TabIndex = 6;
            pnlTop.SizeChanged += PnlTop_SizeChanged;
            pnlTop.Paint += PnlTop_Paint;
            // 
            // pnlControls
            // 
            pnlControls.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlControls.Controls.Add(DropButton);
            pnlControls.Controls.Add(CloseButton);
            pnlControls.Location = new Point(175, 0);
            pnlControls.Name = "pnlControls";
            pnlControls.Size = new Size(25, 30);
            pnlControls.TabIndex = 1;
            // 
            // DropButton
            // 
            DropButton.BackColor = Color.Transparent;
            DropButton.Location = new Point(4, 8);
            DropButton.Name = "DropButton";
            DropButton.Size = new Size(17, 15);
            DropButton.Style = ControlButton.ButtonStyle.Drop;
            DropButton.TabIndex = 0;
            DropButton.MouseDown += DropButton_MouseDown;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CloseButton.BackColor = Color.Transparent;
            CloseButton.Location = new Point(4, 8);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(17, 15);
            CloseButton.Style = ControlButton.ButtonStyle.Close;
            CloseButton.TabIndex = 0;
            CloseButton.Visible = false;
            CloseButton.MouseDown += CloseButton_MouseDown;
            // 
            // pnlTabs
            // 
            pnlTabs.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pnlTabs.BackColor = Color.Transparent;
            pnlTabs.Location = new Point(0, 3);
            pnlTabs.Name = "pnlTabs";
            pnlTabs.Size = new Size(200, 28);
            pnlTabs.TabIndex = 0;
            // 
            // pnlBottom
            // 
            pnlBottom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlBottom.Location = new Point(0, 31);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(200, 99);
            pnlBottom.TabIndex = 7;
            // 
            // WinMenu
            // 
            WinMenu.Name = "WinMenu";
            WinMenu.Size = new Size(61, 4);
            //
            // TabControl
            //
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
            Name = "TabControl";
            Size = new Size(200, 130);
            FontChanged += TabControl_FontChanged;
            ForeColorChanged += TabControl_ForeColorChanged;
            Paint += TabControl_Paint;
            Resize += TabControl_Resize;
            pnlTop.ResumeLayout(false);
            pnlControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
