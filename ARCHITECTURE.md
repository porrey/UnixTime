# Architecture

## Overview

UnixTime is a .NET class library that introduces `System.UnixTime` — a first-class value type (struct) for representing Unix timestamps. It provides seamless, bidirectional conversion between Unix time (seconds since the Unix Epoch, January 1 1970 00:00:00 UTC) and the standard .NET `System.DateTime` type. The library is published as the [UnixTime NuGet package](https://www.nuget.org/packages/UnixTime/).

---

## Repository Layout

```
UnixTime/
├── Assets/                          # NuGet package icon (Unix-Time-Stamp.png)
├── README.md                        # Project overview and NuGet badge
├── Src/
│   ├── Unix-Time-Solution.sln       # Visual Studio solution file
│   ├── System.UnixTime/             # Core library project (NuGet package)
│   │   ├── System.UnixTime.csproj
│   │   ├── UnixTime.cs              # Primary struct definition
│   │   ├── DateTimeExtensions.cs    # Extension method: DateTime.ToUnixTime()
│   │   ├── UnixTimeJsonConverter.cs # Newtonsoft.Json converter
│   │   └── UnixTimeTypeConverter.cs # TypeConverter for legacy .NET targets
│   ├── System.UnixTime.Tests/       # NUnit unit-test project
│   │   ├── System.UnixTime.Tests.csproj
│   │   ├── UnixTimeTests.cs         # Test fixture (~600 lines, 2000-item data-driven)
│   │   ├── TestDataItem.cs          # Test data model
│   │   └── TestDecorator.cs        # Assert2 helper shim
│   └── Demo Console/                # Console application demonstrating the library
│       ├── Demo Console.csproj
│       └── Program.cs
└── .github/
    └── workflows/
        └── dotnet.yml               # GitHub Actions CI pipeline
```

---

## Key Technologies

| Technology | Role |
|---|---|
| **C# / .NET 10** | Target framework for all three projects |
| **`struct` value type** | `UnixTime` is a lightweight stack-allocated struct |
| **Newtonsoft.Json 13.x** | Optional JSON serialization support via `UnixTimeJsonConverter` |
| **NUnit 4.x** | Unit-testing framework |
| **coverlet** | Code-coverage collection during test runs |
| **GitHub Actions** | CI pipeline (`dotnet restore → build → test`) |
| **NuGet** | Package distribution (`GeneratePackageOnBuild = true`) |

---

## Core Component: `System.UnixTime` struct

`UnixTime` lives in the `System` namespace (same as `DateTime`) so it integrates naturally without extra `using` statements.

### Internal Storage

```
private double _timestamp;   // seconds since Unix Epoch (Jan 1 1970 00:00:00 UTC)
```

Storing the value as a `double` allows sub-second precision while remaining compatible with integer-based Unix timestamps.

### Constructors

| Constructor | Accepts |
|---|---|
| `UnixTime(long)` | 64-bit integer timestamp |
| `UnixTime(int)` | 32-bit integer timestamp |
| `UnixTime(double)` | Floating-point timestamp |
| `UnixTime(DateTime)` | Converts from a `DateTime` |

### Public Properties

| Property | Returns |
|---|---|
| `Timestamp` | The raw `double` Unix timestamp |
| `DateTime` | Local-time `DateTime` equivalent |
| `DateTimeUtc` | UTC `DateTime` equivalent |

### Static Members

| Member | Description |
|---|---|
| `UnixTime.Epoch` | `DateTime` of `1970-01-01 00:00:00 UTC` |
| `UnixTime.FromDateTime(DateTime)` | Converts a `DateTime` to a `long` Unix timestamp |
| `UnixTime.ToUniversalDateTime(long/double)` | Timestamp → UTC `DateTime` |
| `UnixTime.ToLocalDateTime(long/double)` | Timestamp → local `DateTime` |
| `UnixTime.Parse(string)` | Parses a numeric string, a date-time string, or a `TimeSpan` string |

### Implicit Conversions

`UnixTime` can be assigned to/from all of the following without an explicit cast:

```
UnixTime  ↔  double
UnixTime  ↔  long
UnixTime  ↔  DateTime
UnixTime  ↔  TimeSpan
```

### Operator Overloads

Full sets of arithmetic (`+`, `-`) and comparison (`==`, `!=`, `<`, `<=`, `>`, `>=`) operators are defined for the following type pairs:

- `UnixTime` op `UnixTime`
- `UnixTime` op `TimeSpan` (and `TimeSpan` op `UnixTime`)

### Interface Implementations

| Interface | Notes |
|---|---|
| `IComparable` | Object-level comparison |
| `IComparable<UnixTime>` | Generic typed comparison |
| `IEquatable<UnixTime>` | Typed equality |
| `IFormattable` | `ToString(format)` / `ToString(format, culture)` delegate to `DateTime` |
| `IConvertible` *(legacy)* | Only compiled for `NET20`–`NET451` targets |
| `ISerializable` *(legacy)* | Binary serialization for `NET20`–`NET451` targets |

---

## Supporting Components

### `DateTimeExtensions`

A single extension method:

```csharp
// Converts a DateTime to a long Unix timestamp
long unixTs = myDateTime.ToUnixTime();
```

### `UnixTimeJsonConverter`

A `Newtonsoft.Json.JsonConverter` subclass. It serializes a `UnixTime` as its numeric string representation and deserializes by calling `UnixTime.Parse()`. Decorate a property with `[JsonConverter(typeof(UnixTimeJsonConverter))]` to use it.

### `UnixTimeTypeConverter` *(legacy)*

A `System.ComponentModel.TypeConverter` compiled only for older .NET Framework targets (`NET20`–`NET451`). It delegates conversion logic through the `double` TypeConverter, enabling design-time support in WinForms and similar frameworks.

---

## Testing

The test project targets **net10.0** and uses **NUnit 4** with the `NUnit3TestAdapter`.

- **`TestDataItem`** generates a random `double` Unix timestamp and derives the corresponding `DateTime`, `long`, and `int` representations from it.
- **`UnixTimeTests`** loads **2,000 random test-data items** at fixture startup and drives every constructor, property, implicit operator, arithmetic operator, comparison operator, and interface method with that data set.
- **`Assert2`** is a thin shim wrapping `Assert.That` in the NUnit 4 constraint model, used to provide a consistent assertion API across the test file.

Coverage is collected by the **coverlet** collector during `dotnet test`.

---

## CI / CD

The GitHub Actions workflow (`.github/workflows/dotnet.yml`) runs on every push and pull-request to `main`:

1. **Restore** – `dotnet restore` (in `Src/`)
2. **Build** – `dotnet build --no-restore`
3. **Test** – `dotnet test --no-build`

Package publication is handled separately via `NuGet.Publish.cmd` (manual release script) with `GeneratePackageOnBuild = true` set in the library project.

---

## Data Flow

```
┌──────────────────────────────┐
│  Caller / Application        │
│                              │
│  UnixTime u = DateTime.Now;  │  ← implicit conversion
│  long ts    = u;             │  ← implicit conversion
│  DateTime d = u;             │  ← implicit conversion
└──────────┬───────────────────┘
           │
           ▼
┌─────────────────────────────────────────────┐
│  System.UnixTime  (struct)                  │
│                                             │
│  _timestamp: double  ──► DateTimeExtensions │
│      │                   .ToUnixTime()      │
│      ▼                                      │
│  UnixTime.Epoch (1970-01-01 UTC)            │
│  + TimeSpan.FromSeconds(_timestamp)         │
│  = DateTime (UTC or Local)                  │
└─────────────────────────────────────────────┘
           │
           ▼
┌───────────────────────────────┐
│  Optional integrations        │
│  • UnixTimeJsonConverter      │  (Newtonsoft.Json)
│  • UnixTimeTypeConverter      │  (legacy .NET Framework)
└───────────────────────────────┘
```
