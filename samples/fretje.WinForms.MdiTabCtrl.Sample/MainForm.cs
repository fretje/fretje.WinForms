namespace fretje.WinForms.MdiTabCtrl.Sample;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        AddSampleTabs();
    }

    private static Form CreateSampleForm(string title, Color backColor)
    {
        Form form = new() { Text = title, BackColor = backColor };

        form.Controls.Add(new Label
        {
            Text = $"""
                   This is {title}

                   Features demonstrated:
                   • Tab creation and management
                   • Drag and drop reordering
                   • Close button functionality
                   • Dropdown menu for hidden tabs
                   • Icon support
                   • Custom styling

                   Try:
                   • Drag tabs to reorder them
                   • Click the close button (X) on tabs
                   • Use Ctrl+Tab to navigate
                   • Use Ctrl+F4 to close tabs
                   • Click the dropdown button to see hidden tabs
                   """,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new("Segoe UI", 12)
        });

        return form;
    }

    private void AddSampleTabs()
    {
        // Tab 1: Basic form
        var form1 = CreateSampleForm("Basic Form", Color.LightBlue);
        _tabControl.TabPages.Add(form1);

        // Tab 2: Form with icon
        var form2 = CreateSampleForm("Form with Icon", Color.LightGreen);
        form2.Icon = SystemIcons.Information;
        _tabControl.TabPages.Add(form2);

        // Tab 3: Form with custom text
        var form3 = CreateSampleForm("Custom Styled Tab", Color.LightYellow);
        _tabControl.TabPages.Add(form3);

        // Select first tab
        if (_tabControl.TabPages.Count > 0)
        {
            _tabControl.TabPages[0].Select();
        }
    }

    private void OnNewTab(object? sender, EventArgs e)
    {
        Random random = new();
        var colors = new[] { Color.LightBlue, Color.LightGreen, Color.LightYellow, Color.LightPink, Color.LightCoral, Color.LightCyan };
        var color = colors[random.Next(colors.Length)];

        var form = CreateSampleForm($"New Tab {DateTime.Now:HH:mm:ss}", color);
        _tabControl.TabPages.Add(form);
        _tabControl.TabPages[^1].Select();
    }

    private void OnExit(object? sender, EventArgs e) => Close();
}
