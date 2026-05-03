# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Cinemabox is a cross-platform desktop application built with:
- **C# / .NET 10.0** with ASP.NET Core Blazor (server-side rendering)
- **ElectronNET** (`ElectronNET.Core` + `ElectronNET.Core.AspNet` v0.4.1) wrapping the web app in a native Electron window
- **TailwindCSS v4** + **DaisyUI v5** for styling

## Commands

```bash
# Build
dotnet build ../Cinemabox.sln

# Run in development (opens browser at http://localhost:5202)
dotnet run --project Cinemabox.csproj

# Publish self-contained (targeting Windows, macOS ARM, macOS x64)
dotnet publish -r win-x64 --self-contained
dotnet publish -r osx-arm64 --self-contained
dotnet publish -r osx-x64 --self-contained
```

There are no test projects configured yet.

## Architecture

**ElectronNET bridge**: `Program.cs` calls `UseElectron(args)` and `ElectronBootstrap()`, which starts an Electron host process that manages a 1280×800 native window around the ASP.NET Core web server. The native window talks to the .NET process over Socket.io (see the local `electron-host-hook` dependency in the Electron layer).

**Blazor Server**: All UI is Razor components under `Components/`. The server renders HTML and pushes updates over a SignalR connection — no separate API layer.

**Component layout**:
- `Components/App.razor` — root HTML document (links CSS, scripts)
- `Components/Routes.razor` — Blazor router, applies `MainLayout` by default
- `Components/Layout/MainLayout.razor` — master shell wrapping `@Body`
- `Components/Layout/ReconnectModal.*` — custom reconnect UX with JS interop when SignalR drops
- `Components/Pages/` — individual routed pages (Home, Error, NotFound)

**Key project settings** (`Cinemabox.csproj`):
- Nullable enabled, implicit usings enabled
- `BlazorDisableThrowNavigationException = true` — navigation exceptions are swallowed silently; handle navigation errors explicitly in components rather than relying on exceptions
- Runtime identifiers: `win-x64`, `osx-arm64`, `osx-x64`

**Electron packaging** is driven by `Properties/electron-builder.json` (portable Windows `.exe`, Linux `.tar.xz`, macOS). Build targets and compression level are configured there.

## Styling (TailwindCSS + DaisyUI)

**Source**: `../Frontend/app.css` — the only CSS you edit. It imports Tailwind v4, points the content scanner at `../Cinemabox/Components`, and loads DaisyUI as a plugin:

```css
@import "tailwindcss";
@source "../Cinemabox/Components";

@plugin "./node_modules/daisyui" {
    themes: light --default, dark --prefersdark;
}
```

**Output**: `wwwroot/app.css` — compiled by the .NET build pipeline (MSBuild target in `Cinemabox.csproj`) via `npx @tailwindcss/cli`. Debug builds are unminified; Release builds add `--minify`. Never edit `wwwroot/app.css` directly.

**npm packages** live in `../Frontend/` (`package.json`):
- `tailwindcss` + `@tailwindcss/cli` v4.2.4
- `daisyui` v5.5.19 (devDependency)
- `esbuild` 0.28.0 (for JS bundling)

**Themes**: `light` (default) and `dark` (activated via `prefers-color-scheme: dark`). To add or change themes, edit the `@plugin` block in `../Frontend/app.css`.

**No `tailwind.config.js`**: Tailwind v4 is configured entirely through the CSS file — no separate config file is needed.
