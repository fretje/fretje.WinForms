namespace fretje.WinForms.MdiTabCtrl.Sample;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using MainForm mainForm = new();
        Application.Run(mainForm);
    }
}

#if NETFRAMEWORK
file static class ApplicationConfiguration
{
    public static void Initialize()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
    }
}
#endif
