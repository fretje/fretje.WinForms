using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace fretje.WinForms.MdiTabCtrl
{
    partial class TabControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null!;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
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
            components = new Container();
            _pnlTop = new Panel();
            _pnlControls = new Panel();
            _dropButton = new ControlButton();
            _closeButton = new ControlButton();
            _pnlTabs = new Panel();
            _pnlBottom = new Panel();
            _winMenu = new ContextMenuStrip(components);
            _tabToolTip = new ToolTip(components);
            _pnlTop.SuspendLayout();
            _pnlControls.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            _pnlTop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pnlTop.BackColor = Color.Transparent;
            _pnlTop.Controls.Add(_pnlControls);
            _pnlTop.Controls.Add(_pnlTabs);
            _pnlTop.Location = new Point(0, 0);
            _pnlTop.Name = "pnlTop";
            _pnlTop.Size = new Size(200, 31);
            _pnlTop.TabIndex = 6;
            _pnlTop.SizeChanged += PnlTop_SizeChanged;
            _pnlTop.Paint += PnlTop_Paint;
            // 
            // pnlControls
            // 
            _pnlControls.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            _pnlControls.Controls.Add(_dropButton);
            _pnlControls.Controls.Add(_closeButton);
            _pnlControls.Location = new Point(175, 0);
            _pnlControls.Name = "pnlControls";
            _pnlControls.Size = new Size(25, 30);
            _pnlControls.TabIndex = 1;
            // 
            // DropButton
            // 
            _dropButton.BackColor = Color.Transparent;
            _dropButton.Location = new Point(4, 8);
            _dropButton.Name = "DropButton";
            _dropButton.Size = new Size(17, 15);
            _dropButton.Style = ControlButton.ButtonStyle.Drop;
            _dropButton.TabIndex = 0;
            _dropButton.MouseDown += DropButton_MouseDown;
            // 
            // CloseButton
            // 
            _closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _closeButton.BackColor = Color.Transparent;
            _closeButton.Location = new Point(4, 8);
            _closeButton.Name = "CloseButton";
            _closeButton.Size = new Size(17, 15);
            _closeButton.Style = ControlButton.ButtonStyle.Close;
            _closeButton.TabIndex = 0;
            _closeButton.Visible = false;
            _closeButton.MouseDown += CloseButton_MouseDown;
            // 
            // pnlTabs
            // 
            _pnlTabs.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _pnlTabs.BackColor = Color.Transparent;
            _pnlTabs.Location = new Point(0, 3);
            _pnlTabs.Name = "pnlTabs";
            _pnlTabs.Size = new Size(200, 28);
            _pnlTabs.TabIndex = 0;
            // 
            // pnlBottom
            // 
            _pnlBottom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _pnlBottom.Location = new Point(0, 31);
            _pnlBottom.Name = "pnlBottom";
            _pnlBottom.Size = new Size(200, 99);
            _pnlBottom.TabIndex = 7;
            // 
            // WinMenu
            // 
            _winMenu.Name = "WinMenu";
            _winMenu.Size = new Size(61, 4);
            //
            // TabControl
            //
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(_pnlTop);
            Controls.Add(_pnlBottom);
            Name = "TabControl";
            Size = new Size(200, 130);
            FontChanged += TabControl_FontChanged;
            ForeColorChanged += TabControl_ForeColorChanged;
            Paint += TabControl_Paint;
            Resize += TabControl_Resize;
            _pnlTop.ResumeLayout(false);
            _pnlControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        internal Panel _pnlTop = null!;
        internal Panel _pnlTabs = null!;
        internal Panel _pnlBottom = null!;
        internal ContextMenuStrip _winMenu = null!;
        internal ControlButton _dropButton = null!;
        internal ToolTip _tabToolTip = null!;
        internal ControlButton _closeButton = null!;
        internal Panel _pnlControls = null!;
    }
}
