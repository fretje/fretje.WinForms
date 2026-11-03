# fretje.WinForms

A collection of Windows Forms controls and components.

## Projects

### [fretje.WinForms.MdiTabCtrl](src/fretje.WinForms.MdiTabCtrl/README.md)

A highly configurable MDI tab control for Windows Forms, converted from [JacksiroKe/MdiTabCtrl](https://github.com/JacksiroKe/MdiTabCtrl).

A modern, tabbed interface for Multiple Document Interface (MDI) applications in Windows Forms. Each tab represents an MDI child form, offering a familiar tabbed-document experience similar to modern IDEs and browsers.

This library is a C# port of the original [MdiTabCtrl](https://github.com/JacksiroKe/MdiTabCtrl) by JacksiroKe, which was written in VB.NET. The port updates the codebase for current .NET standards.

## Building

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build
```

## Project Structure

```
fretje.WinForms/
├── .github/workflows/       # CI/CD pipelines
├── src/                     # Source projects
│   └── fretje.WinForms.MdiTabCtrl/   # MDI tab control
└── samples/                 # Sample applications
    └── fretje.WinForms.MdiTabCtrl.Sample/   # Demo for MDI tab control
```

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch
3. Ensure code passes all analyzers (dotnet build treats warnings as errors)
4. Submit a pull request

For significant changes, please open an issue first to discuss the proposed changes. See [CONTRIBUTING.md](CONTRIBUTING.md) for details, including the release process.
## License

MIT License — see [LICENSE](LICENSE) for details.
