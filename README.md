# Make 10 Game

A beginner-friendly C# console math puzzle game. Use all four given numbers exactly once, with `+`, `-`, `*`, `/`, and parentheses, to make the target number 10.

## Features

- Multiple levels
- Expression parsing without using `eval`
- Checks that all supplied numbers are used exactly once
- Supports parentheses and operator precedence
- Cross-platform .NET console app
- No external NuGet packages required

## Requirements for development

- .NET 10 SDK
- VS Code (Visual Studio is not required)

## Run from source

```bash
dotnet restore
dotnet run
```

## Publish for macOS Apple Silicon

```bash
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true
```

## Publish for Windows x64

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Upload the complete contents of each platform's `publish` folder as a ZIP asset in a GitHub Release.

## How to play

Enter an arithmetic expression such as:

```text
1 + 2 + 3 + 4
```

Every provided number must be used exactly once. The expression must evaluate to 10. Type `q` to quit.

## License

Choose and add a license before public distribution.
