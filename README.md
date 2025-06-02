# ⚡ RandomShock

**RandomShock** is a .NET (C#) NuGet package that interfaces with the **OpenShock API** to deliver random electric shocks. Designed for novelty applications, behavior conditioning, or just chaotic experimentation, this package lets you schedule or trigger shocks at unpredictable intervals.

> ⚠️ **Warning:** Use this package with extreme caution. It interacts with hardware that can cause physical discomfort or harm. Not recommended for individuals with health conditions. Use at your own risk.

---

## 📦 Installation

Install via NuGet:

```bash
dotnet add package RandomShock
```

Or via the Package Manager Console:

```powershell
Install-Package RandomShock
```

## 🤝 Contributing
Contributions are welcome! Please open an issue or submit a pull request with ideas, fixes, or enhancements.

## 🛠️ Usage
Quick Start

Create a ```config.toml```
```toml
api_token = ""
check_interval = 500 # in milliseconds

[shocker.left_thigh]
uuid = "left"
[shocker.right_thigh]
uuid = "right"

[action.weak]
type = "Shock"
intensity = 20 # intensity 1-100
duration = 3 # duration in seconds
weight = 1
shockers = ["left_thigh"]

[action.strong]
type = "Shock"
intensity = 60
duration = 1
weight = 1
```

```csharp
using RandomShock;

RandomShock.Start()
RandomShockSetDetection(true)
```

## 🧠 Why?
Because you're curious, reckless, or trying to train yourself not to procrastinate. No judgment. We support creative experimentation.

## ⚠️ Safety Notice
RandomShock can trigger real physical devices. Always:

- Use with consent
- Never shock others without explicit agreement
- Stop immediately if pain or discomfort exceeds tolerable levels
- Avoid use near water, pacemakers, or other sensitive electronics

