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
│   │   ├── UnixTime.cs                        # Primary struct definition
│   │   ├── DateTimeExtensions.cs              # Extension method: DateTime.ToUnixTime()
│   │   ├── UnixTimeJsonConverter.cs           # Newtonsoft.Json converter
│   │   ├── UnixTimeSystemTextJsonConverter.cs # System.Text.Json converter
│   │   └── UnixTimeTypeConverter.cs           # TypeConverter for design-time support
│   ├── System.UnixTime.Tests/       # NUnit unit-test project
│   │   ├── System.UnixTime.Tests.csproj
│   │   ├── UnixTimeTests.cs         # Test fixture (~1,000 lines, 2,000-item data-driven)
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
| `UnixTime.Zero` | `UnixTime` with a timestamp of `0` (same instant as `Epoch`) |
| `UnixTime.MinValue` | Smallest representable value (corresponds to `DateTime.MinValue` in UTC) |
| `UnixTime.MaxValue` | Largest representable value (corresponds to `DateTime.MaxValue` in UTC) |
| `UnixTime.FromDateTime(DateTime)` | Converts a `DateTime` to a `long` Unix timestamp |
| `UnixTime.ToUniversalDateTime(long/double)` | Timestamp → UTC `DateTime` |
| `UnixTime.ToLocalDateTime(long/double)` | Timestamp → local `DateTime` |
| `UnixTime.Parse(string)` | Parses a numeric string, a date-time string, or a `TimeSpan` string |

### Conversions

`UnixTime` supports the following conversions:

| Direction | long | double | DateTime | TimeSpan |
|---|---|---|---|---|
| `UnixTime` → target | **implicit** | **implicit** | **implicit** | **implicit** |
| target → `UnixTime` | **explicit** `(UnixTime)x` | **explicit** `(UnixTime)x` | **implicit** | **implicit** |

`long` and `double` to `UnixTime` are **explicit** (require a cast or `new UnixTime(x)`) to avoid operator ambiguity with the numeric offset operators introduced in C# 13 (`CS9342`). All other conversions remain implicit.

### Operator Overloads

Full sets of arithmetic (`+`, `-`) and comparison (`==`, `!=`, `<`, `<=`, `>`, `>=`) operators are defined for the following type pairs:

- `UnixTime` op `UnixTime`
- `UnixTime` op `long` (numeric second offset)
- `UnixTime` op `double` (fractional second offset)
- `UnixTime` op `TimeSpan` (and `TimeSpan` op `UnixTime`)

### Interface Implementations

| Interface | Notes |
|---|---|
| `IComparable` | Object-level comparison |
| `IComparable<UnixTime>` | Generic typed comparison |
| `IEquatable<UnixTime>` | Typed equality |
| `IFormattable` | `ToString(format)` / `ToString(format, culture)` delegate to `DateTime` |
| `ISpanFormattable` | Allocation-free `TryFormat` into a `Span<char>` |
| `IConvertible` | Conversion to all BCL primitive types via `Convert` |
| `ISpanParsable<UnixTime>` | Parsing from `ReadOnlySpan<char>` with an optional `IFormatProvider` |

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

### `UnixTimeSystemTextJsonConverter`

A `System.Text.Json.JsonConverter<UnixTime>` for use with `System.Text.Json`. It writes a `UnixTime` as a JSON number (the raw Unix timestamp) and reads back either a JSON number or a JSON string (parsed via `UnixTime.TryParse`). Register it via `JsonSerializerOptions.Converters`.

### `UnixTimeTypeConverter`

A `System.ComponentModel.TypeConverter` that enables design-time and data-binding support (e.g. in WinForms property grids). It delegates conversion logic through the `double` TypeConverter.

---

## Testing

The test project targets **net10.0** and uses **NUnit 4** with the `NUnit3TestAdapter`.

- **`TestDataItem`** generates a random `double` Unix timestamp and derives the corresponding `DateTime`, `long`, and `int` representations from it.
- **`UnixTimeTests`** loads **2,000 random test-data items** at fixture startup and drives every constructor, property, implicit operator, arithmetic operator, comparison operator, and interface method with that data set (~1,000 lines).
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
│  long ts    = u;             │  ← implicit conversion (UnixTime→long)
│  DateTime d = u;             │  ← implicit conversion
│  UnixTime v = (UnixTime)ts;  │  ← explicit conversion (long→UnixTime)
│  UnixTime w = u + 3600L;     │  ← numeric offset operator
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
┌───────────────────────────────────────┐
│  Optional integrations                │
│  • UnixTimeJsonConverter              │  (Newtonsoft.Json)
│  • UnixTimeSystemTextJsonConverter    │  (System.Text.Json)
│  • UnixTimeTypeConverter              │  (TypeConverter / design-time support)
└───────────────────────────────────────┘
```
