# fretje.WinForms.MdiTabCtrl

[![NuGet](https://img.shields.io/nuget/v/fretje.WinForms.MdiTabCtrl.svg)](https://www.nuget.org/packages/fretje.WinForms.MdiTabCtrl/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-10.0%20%7C%204.7.2-blue.svg)](https://dotnet.microsoft.com/)

A highly configurable MDI tab control for Windows Forms, converted from [JacksiroKe/MdiTabCtrl](https://github.com/JacksiroKe/MdiTabCtrl).

## Overview

`fretje.WinForms.MdiTabCtrl` provides a modern, tabbed interface for Multiple Document Interface (MDI) applications in Windows Forms. Each tab represents an MDI child form, offering a familiar tabbed-document experience similar to modern IDEs and browsers.

This library is a C# port of the original [MdiTabCtrl](https://github.com/JacksiroKe/MdiTabCtrl) by JacksiroKe, which was written in VB.NET. The port updates the codebase for current .NET standards with centralized package management and analyzers.

## Features

- **Tabbed MDI Interface** — Each MDI child form appears as a tab
- **Drag-and-Drop Reordering** — Rearrange tabs by dragging
- **Overflow Handling** — Hidden tabs accessible via dropdown menu when space is limited
- **Close Buttons** — Per-tab close buttons with hover/disabled states
- **Extensive Customization** — Colors, gradients, borders, fonts, padding, and sizing
- **Owner Drawing** — Custom paint events for tabs, borders, and backgrounds
- **Keyboard Navigation** — Ctrl+Tab, Ctrl+F4, and configurable shortcuts
- **Tear-Off Support** — Detach tabs into floating windows
- **High DPI Aware** — Works correctly on high-DPI displays
- **Design-Time Support** — Full Visual Studio designer integration

## Customization Options

The control exposes a comprehensive set of properties for visual customization:

| Category | Properties |
|----------|------------|
| **Tab Appearance** | `TabHeight`, `TabTop`, `TabOffset`, `TabPadLeft`, `TabPadRight`, `TabMinimumWidth`, `TabMaximumWidth`, `TabIconSize` |
| **Colors & Gradients** | `TabBackHighColor`, `TabBackLowColor`, `TabBackHighColorDisabled`, `TabBackLowColorDisabled`, `BorderColor`, `BorderColorDisabled`, `ForeColorDisabled` |
| **Close Button** | `TabCloseButtonVisible`, `TabCloseButtonSize`, `TabCloseButtonImage`, `TabCloseButtonImageHot`, `TabCloseButtonImageDisabled`, and 9 color properties for states |
| **Control Buttons** | `CloseButtonVisible`, `DropButtonVisible`, `ControlButtonBackHighColor`, `ControlButtonBackLowColor`, `ControlButtonBorderColor`, `ControlButtonForeColor` |
| **Behavior** | `AllowTabReorder`, `TabGlassGradient`, `TabBorderEnhanced`, `TabBorderEnhanceWeight`, `FontBoldOnSelect`, `HotTrack`, `KeyTabEnabled`, `KeyCloseEnabled`, `TabsDirection`, `Alignment`, `TopSeparator` |
| **Rendering** | `SmoothingMode`, `RenderMode`, `MenuRenderer`, `Padding` |

## Events

| Event | Description |
|-------|-------------|
| `SelectedTabChanged` | Fired when the active tab changes |
| `TabRemoved` | Fired when a tab is closed |
| `TabPaintBackground` | Custom background painting for tabs |
| `TabPaintBorder` | Custom border painting for tabs |
| `GetTabRegion` | Custom hit-testing region for tabs |
| `TabPressed` | Keyboard shortcut handling (Ctrl+Tab) |
| `F4Pressed` | F4 key handling |
| `FocusedChanged` | Focus state changes |

## Installation

```powershell
# Package Manager Console
Install-Package fretje.WinForms.MdiTabCtrl

# .NET CLI
dotnet add package fretje.WinForms.MdiTabCtrl
```

## Quick Start

```csharp
using fretje.WinForms.MdiTabCtrl;

public partial class MainForm : Form
{
    private readonly TabControl _tabControl = new();

    public MainForm()
    {
        InitializeComponent();

        _tabControl.Dock = DockStyle.Fill;
        Controls.Add(_tabControl);

        // Add MDI child forms as tabs
        var childForm = new ChildForm { Text = "Document 1" };
        _tabControl.TabPages.Add(childForm);
    }
}
```

### Adding Tabs Programmatically

```csharp
// Add a new form as a tab
var form = new MyMdiChildForm();
var tabPage = _tabControl.TabPages.Add(form);

// Select a specific tab
tabPage.Select();

// Get the currently selected tab
var currentTab = _tabControl.TabPages.SelectedTab();
var currentIndex = _tabControl.TabPages.SelectedIndex();

// Remove a tab
_tabControl.TabPages.Remove(tabPage);

// Tear off a tab into a floating window
var floatingForm = _tabControl.TabPages.TearOff(tabPage);
```

## Sample Application

The [`../../samples/MdiTabCtrl.Sample`](../../samples/MdiTabCtrl.Sample) folder contains a complete WinForms application demonstrating:

- Basic tab creation and management
- Drag-and-drop tab reordering
- Custom styling and theming
- Close button functionality
- Dropdown menu for overflow tabs
- Icon support on tabs
- Custom painting via events
- Keyboard navigation

To run the sample:

```bash
cd ../../samples/MdiTabCtrl.Sample
dotnet run
```

## Requirements

- **.NET 10.0** (Windows) or later
- **.NET Framework 4.7.2** or later
- Windows Forms (included in both targets)

## Building from Source
```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run sample
dotnet run --project ../../samples/MdiTabCtrl.Sample/MdiTabCtrl.Sample.csproj
```

## License

MIT License — see [LICENSE](LICENSE) for details.

## Acknowledgments

- **Original project**: [JacksiroKe/MdiTabCtrl](https://github.com/JacksiroKe/MdiTabCtrl) by JacksiroKe
- This conversion modernizes the codebase for current .NET standards while preserving the original functionality and design
