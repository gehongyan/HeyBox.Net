# HeyBox.Net

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/gehongyan/HeyBox.Net/push.yml?branch=master)
![GitHub Top Language](https://img.shields.io/github/languages/top/gehongyan/HeyBox.Net)
[![Nuget Version](https://img.shields.io/nuget/v/HeyBox.Net)](https://www.nuget.org/packages/HeyBox.Net)
[![Nuget](https://img.shields.io/nuget/dt/HeyBox.Net?color=%230099ff)](https://www.nuget.org/packages/HeyBox.Net)
[![License](https://img.shields.io/github/license/gehongyan/HeyBox.Net)](https://github.com/gehongyan/HeyBox.Net/blob/master/LICENSE)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fgehongyan%2FHeyBox.Net.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fgehongyan%2FHeyBox.Net?ref=badge_shield)
<a href="https://chat.xiaoheihe.cn/ihtpuxhq">
    <img src="https://imgheybox.max-c.com/oa/2023/03/21/47912df9f48f030c784dd6115b91274b.png" alt="drawing" height="20" alt="Chat on HeyBox"/>
</a>

---

**English** | [简体中文](./README.zh-CN.md)

---

**HeyBox.Net** is an unofficial C# .NET implementation for [HeyBox API](https://apifox.com/apidoc/shared-43256fe4-9a8c-4f22-949a-74a3f8b431f5).

---

## Source & Documentation

Source code is available on [GitHub](https://github.com/gehongyan/HeyBox.Net).

Documents are available on [heyboxnet.dev](https://heyboxnet.dev). (Simplified Chinese available only)

---

## Targets

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)

> [!TIP]
> Targets other than .NET 8.0 have not been fully tested.

---

## Installation

### Main Package

The main package provides all implementations of official APIs.

- HeyBox.Net: [NuGet](https://www.nuget.org/packages/HeyBox.Net/), [GitHub Packages](https://github.com/gehongyan/HeyBox.Net/pkgs/nuget/HeyBox.Net)

### Individual Packages

Individual components of the main package can be installed separately. These packages are included in the main package.

- HeyBox.Net.Core: [NuGet](https://www.nuget.org/packages/HeyBox.Net.Core/),
  [GitHub Packages](https://github.com/gehongyan/HeyBox.Net/pkgs/nuget/HeyBox.Net.Core)
- HeyBox.Net.Rest: [NuGet](https://www.nuget.org/packages/HeyBox.Net.Rest/),
  [GitHub Packages](https://github.com/gehongyan/HeyBox.Net/pkgs/nuget/HeyBox.Net.Rest)
- HeyBox.Net.WebSocket: [NuGet](https://www.nuget.org/packages/HeyBox.Net.WebSocket/),
  [GitHub Packages](https://github.com/gehongyan/HeyBox.Net/pkgs/nuget/HeyBox.Net.WebSocket)
- HeyBox.Net.Interactions: [NuGet](https://www.nuget.org/packages/HeyBox.Net.Interactions/),
  [GitHub Packages](https://github.com/gehongyan/HeyBox.Net/pkgs/nuget/HeyBox.Net.Interactions)

---

## License & Copyright

This package is open-source and is licensed under the [MIT license](LICENSE).

HeyBox.Net was developed with reference to **[Discord.Net](https://github.com/discord-net/Discord.Net)**.

[Discord.Net contributors](https://github.com/discord-net/Discord.Net/graphs/contributors) holds the copyright
for portion of the code in this repository according to [this license](https://github.com/discord-net/Discord.Net/blob/dev/LICENSE).

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fgehongyan%2FHeyBox.Net.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fgehongyan%2FHeyBox.Net?ref=badge_large)

---

## Acknowledgements

<img src="./assets/Discord.Net_Logo.svg" alt="drawing" height="50"/>

Special thanks to [Discord.Net](https://github.com/discord-net/Discord.Net) for such a great project.

<p>
  <img src="./assets/Rider_Icon.svg" height="50" alt="RiderIcon"/>
  <img src="./assets/ReSharper_Icon.png" height="50" alt="Resharper_Icon"/>
</p>

Special thanks to [JetBrains](https://www.jetbrains.com) for providing free licenses for their awesome tools -
[Rider](https://www.jetbrains.com/rider/) and [ReSharper](https://www.jetbrains.com/resharper/) -
to develop HeyBox.Net.
