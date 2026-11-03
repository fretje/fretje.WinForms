using System.Drawing.Drawing2D;

namespace fretje.WinForms.MdiTabCtrl;

/// <summary>
/// Helper class containing rendering utilities and color management.
/// </summary>
internal static class Helper
{
    /// <summary>
    /// Creates a glass gradient brush for tab backgrounds.
    /// </summary>
    public static LinearGradientBrush CreateGlassGradientBrush(Rectangle rectangle, Color color1, Color color2)
    {
        LinearGradientBrush brush = new(rectangle, color1, color2, LinearGradientMode.Vertical);
        Bitmap bitmap = new(1, rectangle.Height);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.FillRectangle(brush, new Rectangle(0, 0, 1, rectangle.Height));
        }

        ColorBlend blend = new(4);
        blend.Colors[0] = bitmap.GetPixel(0, 0);
        blend.Colors[1] = bitmap.GetPixel(0, bitmap.Height / 3);
        blend.Colors[2] = bitmap.GetPixel(0, bitmap.Height - 1);
        blend.Colors[3] = bitmap.GetPixel(0, bitmap.Height / 3);
        blend.Positions[0] = 0f;
        blend.Positions[1] = 0.335f;
        blend.Positions[2] = 0.335f;
        blend.Positions[3] = 1f;
        brush.InterpolationColors = blend;

        bitmap.Dispose();
        return brush;
    }

    /// <summary>
    /// Color management for different render modes.
    /// </summary>
    public static class RenderColors
    {
        public static Color BackHighColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? Color.Transparent :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.ToolStripGradientEnd :
            managedColor;

        public static Color BackLowColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? Color.Transparent :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.ToolStripGradientBegin :
            managedColor;

        public static Color BorderColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ControlDarkDark :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.GripDark :
            managedColor;

        public static Color BorderColorDisabled(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ControlDark :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.SeparatorDark :
            managedColor;

        public static Color ControlButtonBackHighColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ButtonHighlight :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.ButtonSelectedGradientBegin :
            managedColor;

        public static Color ControlButtonBackLowColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ButtonHighlight :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.ButtonSelectedGradientEnd :
            managedColor;

        public static Color ControlButtonBorderColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.HotTrack :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.ButtonPressedBorder :
            managedColor;

        public static Color TabBackHighColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.Control :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.MenuItemPressedGradientBegin :
            managedColor;

        public static Color TabBackLowColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.Control :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.MenuItemPressedGradientEnd :
            managedColor;

        public static Color TabBackHighColorDisabled(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.Control :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.ToolStripDropDownBackground :
            managedColor;

        public static Color TabBackLowColorDisabled(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.Control :
            renderMode == ToolStripRenderMode.Professional ? ProfessionalColors.ToolStripGradientMiddle :
            managedColor;

        public static Color TabCloseButtonBackHighColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? Color.Transparent :
            renderMode == ToolStripRenderMode.Professional ? Color.Transparent :
            managedColor;

        public static Color TabCloseButtonBackHighColorDisabled(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? Color.Transparent :
            renderMode == ToolStripRenderMode.Professional ? Color.Transparent :
            managedColor;

        public static Color TabCloseButtonBackHighColorHot(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ButtonHighlight :
            renderMode == ToolStripRenderMode.Professional ? Color.WhiteSmoke :
            managedColor;

        public static Color TabCloseButtonBackLowColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? Color.Transparent :
            renderMode == ToolStripRenderMode.Professional ? Color.Transparent :
            managedColor;

        public static Color TabCloseButtonBackLowColorDisabled(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? Color.Transparent :
            renderMode == ToolStripRenderMode.Professional ? Color.Transparent :
            managedColor;

        public static Color TabCloseButtonBackLowColorHot(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ButtonHighlight :
            renderMode == ToolStripRenderMode.Professional ? Color.LightGray :
            managedColor;

        public static Color TabCloseButtonBorderColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ControlDark :
            renderMode == ToolStripRenderMode.Professional ? Color.Transparent :
            managedColor;

        public static Color TabCloseButtonBorderColorDisabled(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.GrayText :
            renderMode == ToolStripRenderMode.Professional ? Color.Transparent :
            managedColor;

        public static Color TabCloseButtonBorderColorHot(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.HotTrack :
            renderMode == ToolStripRenderMode.Professional ? Color.Gray :
            managedColor;

        public static Color TabCloseButtonForeColor(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ControlText :
            renderMode == ToolStripRenderMode.Professional ? Color.Gray :
            managedColor;

        public static Color TabCloseButtonForeColorDisabled(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.GrayText :
            renderMode == ToolStripRenderMode.Professional ? Color.Gray :
            managedColor;

        public static Color TabCloseButtonForeColorHot(ToolStripRenderMode renderMode, Color managedColor) =>
            renderMode == ToolStripRenderMode.System ? SystemColors.ControlText :
            renderMode == ToolStripRenderMode.Professional ? Color.Firebrick :
            managedColor;
    }
}
