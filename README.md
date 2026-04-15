![GitHub Workflow Status (with branch)](https://img.shields.io/github/actions/workflow/status/porrey/UnixTime/.github/workflows/dotnet.yml?style=for-the-badge)

[![Nuget](https://img.shields.io/nuget/v/UnixTime?label=UnixTime%20-%20NuGet&style=for-the-badge)
![Nuget](https://img.shields.io/nuget/dt/UnixTime?label=Downloads&style=for-the-badge)](https://www.nuget.org/packages/UnixTime/)

# UnixTime

`System.UnixTime` is a first-class .NET value type (struct) that represents a Unix timestamp — the number of seconds elapsed since **January 1, 1970 00:00:00 UTC** (the Unix Epoch). It lives in the `System` namespace alongside `DateTime` and provides seamless, type-safe conversion between Unix time and .NET `DateTime`, `TimeSpan`, `long`, and `double`.

---

## Table of Contents

- [What is Unix Time?](#what-is-unix-time)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Constructors](#constructors)
- [Properties](#properties)
- [Static Members](#static-members)
- [Conversions](#conversions)
- [Operator Overloads](#operator-overloads)
- [Parsing](#parsing)
- [String Formatting](#string-formatting)
- [Extension Methods](#extension-methods)
- [JSON Serialization](#json-serialization)
- [Boundary Values](#boundary-values)
- [Version History](#version-history)

---

## What is Unix Time?

Unix time (also called POSIX time or Epoch time) is a system for describing a point in time as a single number: the total number of seconds that have elapsed since midnight UTC on January 1, 1970, not counting leap seconds. It is widely used in computing, logging, APIs, and embedded systems.

```
Unix Epoch  →  0
One hour after epoch  →  3600
January 1, 2000 00:00:00 UTC  →  946684800
```

---

## Installation

Install via the .NET CLI:

```sh
dotnet add package UnixTime
```

Or via the NuGet Package Manager:

```
Install-Package UnixTime
```

Because `UnixTime` is in the `System` namespace, no additional `using` statement is needed in files that already reference the BCL.

---

## Quick Start

```csharp
// Create from the current local time
UnixTime now = DateTime.Now;
Console.WriteLine($"Current Unix timestamp: {now}");

// Create from a known timestamp
UnixTime epoch2k = new(946684800);
Console.WriteLine($"Y2K was: {epoch2k.DateTimeUtc:u}");

// Convert back to DateTime
DateTime localTime = now.DateTime;         // local time
DateTime utcTime   = now.DateTimeUtc;      // UTC

// Convert to a raw number
long   ts = now;                           // implicit: UnixTime → long
double td = now;                           // implicit: UnixTime → double

// Add/subtract offsets
UnixTime later     = now + 3600L;          // 1 hour later  (long seconds)
UnixTime earlier   = now - 86400L;         // 1 day earlier
UnixTime fractional = now + 0.5;           // half a second later (double)
UnixTime withSpan  = now + TimeSpan.FromHours(2);

// Parse from a string
UnixTime fromNum  = UnixTime.Parse("1700000000");
UnixTime fromDate = UnixTime.Parse("2023-11-14T22:13:20Z");
UnixTime fromSpan = UnixTime.Parse("1.00:00:00");   // 1 day after epoch
```

---

## Constructors

| Constructor | Description |
|---|---|
| `UnixTime(long timestamp)` | Creates an instance from a 64-bit integer timestamp. |
| `UnixTime(int timestamp)` | Creates an instance from a 32-bit integer timestamp. |
| `UnixTime(double timestamp)` | Creates an instance from a floating-point timestamp (supports sub-second precision). |
| `UnixTime(DateTime datetime)` | Creates an instance from a `DateTime`. The `DateTime` must have `Kind` set to `Utc` or `Local`; `DateTimeKind.Unspecified` throws `ArgumentException`. |

```csharp
UnixTime a = new(1700000000L);
UnixTime b = new(1700000000.5);
UnixTime c = new(DateTime.UtcNow);
```

---

## Properties

| Property | Type | Description |
|---|---|---|
| `Timestamp` | `double` | The raw Unix timestamp (seconds since the Epoch). Supports sub-second precision. Readable and writable. |
| `DateTime` | `DateTime` | The timestamp converted to a **local-time** `DateTime`. |
| `DateTimeUtc` | `DateTime` | The timestamp converted to a **UTC** `DateTime`. |

```csharp
UnixTime t = new(1700000000L);
Console.WriteLine(t.Timestamp);    // 1700000000
Console.WriteLine(t.DateTimeUtc);  // 14/11/2023 22:13:20
Console.WriteLine(t.DateTime);     // (your local equivalent)
```

---

## Static Members

### Epoch and Boundary Values

| Member | Type | Description |
|---|---|---|
| `UnixTime.Epoch` | `DateTime` | `1970-01-01 00:00:00 UTC` — the origin of Unix time. |
| `UnixTime.Zero` | `UnixTime` | A `UnixTime` with a timestamp of `0` (same instant as `Epoch`). |
| `UnixTime.MinValue` | `UnixTime` | Smallest representable value (corresponds to `DateTime.MinValue` in UTC). |
| `UnixTime.MaxValue` | `UnixTime` | Largest representable value (corresponds to `DateTime.MaxValue` in UTC). |

### Conversion Methods

| Method | Description |
|---|---|
| `UnixTime.FromDateTime(DateTime)` | Converts a `DateTime` to a `long` Unix timestamp. Requires `Kind` to be `Utc` or `Local`. |
| `UnixTime.ToUniversalDateTime(long)` | Converts a `long` timestamp to a UTC `DateTime`. |
| `UnixTime.ToUniversalDateTime(double)` | Converts a `double` timestamp to a UTC `DateTime`. |
| `UnixTime.ToLocalDateTime(long)` | Converts a `long` timestamp to a local-time `DateTime`. |
| `UnixTime.ToLocalDateTime(double)` | Converts a `double` timestamp to a local-time `DateTime`. |

```csharp
long ts = UnixTime.FromDateTime(DateTime.UtcNow);
DateTime utc   = UnixTime.ToUniversalDateTime(ts);
DateTime local = UnixTime.ToLocalDateTime(ts);
```

---

## Conversions

`UnixTime` supports the following implicit and explicit conversions:

| From → To | Conversion kind |
|---|---|
| `UnixTime` → `double` | **Implicit** |
| `UnixTime` → `long` | **Implicit** (truncates fractional seconds) |
| `UnixTime` → `DateTime` | **Implicit** (returns local time) |
| `UnixTime` → `TimeSpan` | **Implicit** (total seconds from Epoch as `TimeSpan`) |
| `double` → `UnixTime` | **Explicit** — use a cast `(UnixTime)value` or constructor `new(value)` |
| `long` → `UnixTime` | **Explicit** — use a cast `(UnixTime)value` or constructor `new(value)` |
| `DateTime` → `UnixTime` | **Implicit** |
| `TimeSpan` → `UnixTime` | **Implicit** (total seconds of the span become the timestamp) |

> **Breaking change in 10.1.0:** `long → UnixTime` and `double → UnixTime` conversions changed from **implicit** to **explicit**. Code that relied on implicit numeric assignment must add a constructor call or explicit cast. See [Version History](#version-history).

```csharp
// Implicit (UnixTime → numeric/DateTime)
double d = someUnixTime;
long   l = someUnixTime;
DateTime dt = someUnixTime;

// Explicit (numeric → UnixTime) — requires cast or constructor
UnixTime fromLong   = new(1700000000L);
UnixTime fromDouble = new(1700000000.0);
// or equivalently:
UnixTime fromLong2   = (UnixTime)1700000000L;
UnixTime fromDouble2 = (UnixTime)1700000000.0;

// Implicit (DateTime / TimeSpan → UnixTime)
UnixTime fromDateTime = DateTime.UtcNow;
UnixTime fromTimeSpan = TimeSpan.FromDays(1);
```

---

## Operator Overloads

### Arithmetic

All arithmetic operators return a new `UnixTime` instance.

| Expression | Description |
|---|---|
| `t1 + t2` | Adds two `UnixTime` timestamps. |
| `t1 - t2` | Subtracts one `UnixTime` from another. |
| `t + seconds` | Adds `long` seconds offset. |
| `t - seconds` | Subtracts `long` seconds offset. |
| `t + seconds` | Adds `double` (fractional) seconds offset. |
| `t - seconds` | Subtracts `double` (fractional) seconds offset. |
| `t + timespan` | Adds a `TimeSpan`. |
| `t - timespan` | Subtracts a `TimeSpan`. |
| `timespan + t` | Adds a `UnixTime` to a `TimeSpan`. |
| `timespan - t` | Subtracts a `UnixTime` from a `TimeSpan`. |
| `t + datetime` | Adds a `DateTime` (as a `UnixTime`) to a `UnixTime`. |
| `t - datetime` | Subtracts a `DateTime` (as a `UnixTime`) from a `UnixTime`. |

```csharp
UnixTime t = new(1700000000L);

UnixTime oneHourLater     = t + 3600L;
UnixTime oneDayEarlier    = t - 86400L;
UnixTime halfSecondLater  = t + 0.5;
UnixTime withSpan         = t + TimeSpan.FromMinutes(30);
```

### Comparison

Full sets of comparison operators (`==`, `!=`, `<`, `<=`, `>`, `>=`) are defined for:

- `UnixTime` op `UnixTime`
- `UnixTime` op `TimeSpan` (and `TimeSpan` op `UnixTime`)
- `UnixTime` op `DateTime` (and `DateTime` op `UnixTime`)

```csharp
UnixTime t1 = new(1000L);
UnixTime t2 = new(2000L);

bool earlier = t1 < t2;   // true
bool same    = t1 == t2;  // false
```

---

## Parsing

`UnixTime` can be parsed from any of these string formats:

| String format | Parsed as | Example |
|---|---|---|
| Numeric string (integer or decimal) | Unix timestamp (`double`) | `"1700000000"`, `"1700000000.5"` |
| Date/time string | `DateTime` → `UnixTime` | `"2023-11-14T22:13:20Z"`, `"January 1, 2013 12:34 AM"` |
| `TimeSpan` string (`d.hh:mm:ss`) | Seconds in span → `UnixTime` | `"1.00:00:00"` (86400 s) |

> **Breaking change in 10.1.0:** Parse order changed. Numeric parsing is attempted **first** (before date-time parsing). A string like `"2024"` will now parse as the number `2024` (seconds after the Epoch, i.e., 1970-01-01 00:33:44 UTC) rather than January 1, 2024. If you need to parse years or ambiguous date strings, provide a fully qualified date-time format. See [Version History](#version-history).

### `Parse`

```csharp
UnixTime t1 = UnixTime.Parse("1700000000");
UnixTime t2 = UnixTime.Parse("2023-11-14T22:13:20Z");
UnixTime t3 = UnixTime.Parse("1.00:00:00");   // 1 day after epoch
```

Throws `FormatException` if the string cannot be parsed.

### `TryParse`

```csharp
if (UnixTime.TryParse("1700000000", out UnixTime result))
{
    Console.WriteLine(result.DateTimeUtc);
}
```

Overloads are also available accepting `IFormatProvider` and `ReadOnlySpan<char>`:

```csharp
UnixTime.TryParse("1700000000", CultureInfo.InvariantCulture, out UnixTime result);
UnixTime.TryParse("1700000000".AsSpan(), CultureInfo.InvariantCulture, out UnixTime span);
```

### `CanParse`

Returns `true` if the string can be successfully parsed without throwing:

```csharp
bool ok = UnixTime.CanParse("1700000000");
```

---

## String Formatting

### `ToString()`

Returns the raw timestamp as a culture-invariant decimal string.

> **Breaking change in 10.1.0:** `ToString()` now uses `CultureInfo.InvariantCulture`, ensuring the numeric value always round-trips correctly regardless of the machine locale. See [Version History](#version-history).

```csharp
UnixTime t = new(1700000000L);
Console.WriteLine(t.ToString());   // "1700000000"
```

### `ToString(string format)`

Formats the equivalent **local-time** `DateTime` using the standard `DateTime` format string:

```csharp
UnixTime t = new(1700000000L);
Console.WriteLine(t.ToString("f"));    // e.g. "Tuesday, 14 November 2023 22:13"
Console.WriteLine(t.ToString("yyyy-MM-dd"));  // e.g. "2023-11-14"
```

### `TryFormat` (`ISpanFormattable`)

Writes the formatted value into a `Span<char>` for allocation-free formatting:

```csharp
UnixTime t = new(1700000000L);
Span<char> buffer = stackalloc char[64];
if (t.TryFormat(buffer, out int written, "f", CultureInfo.CurrentCulture))
{
    Console.WriteLine(buffer[..written]);
}
```

When `format` is empty, the raw timestamp is written using the invariant culture.

---

## Extension Methods

The `DateTimeExtensions` class adds a `ToUnixTime()` extension method to all `DateTime` instances:

```csharp
DateTime now = DateTime.Now;
long unixTs = now.ToUnixTime();
```

This is equivalent to calling `UnixTime.FromDateTime(now)`.

---

## JSON Serialization

### Newtonsoft.Json

The `UnixTimeJsonConverter` serializes a `UnixTime` as its numeric string representation and deserializes it back via `UnixTime.Parse()`.

Decorate a property with `[JsonConverter(typeof(UnixTimeJsonConverter))]`:

```csharp
using Newtonsoft.Json;

public class Event
{
    public string Name { get; set; }

    [JsonConverter(typeof(UnixTimeJsonConverter))]
    public UnixTime Timestamp { get; set; }
}

var e = new Event { Name = "Launch", Timestamp = new(1700000000L) };
string json = JsonConvert.SerializeObject(e);
// {"Name":"Launch","Timestamp":"1700000000"}

Event e2 = JsonConvert.DeserializeObject<Event>(json);
```

### System.Text.Json

A `UnixTimeSystemTextJsonConverter` is also provided for use with `System.Text.Json`:

```csharp
using System.Text.Json;

var options = new JsonSerializerOptions();
options.Converters.Add(new UnixTimeSystemTextJsonConverter());

string json = JsonSerializer.Serialize(new(1700000000L), options);
UnixTime t = JsonSerializer.Deserialize<UnixTime>(json, options);
```

---

## Boundary Values

```csharp
// The Unix Epoch as a DateTime
DateTime epoch = UnixTime.Epoch;           // 1970-01-01 00:00:00 UTC

// Zero (same instant as Epoch, as a UnixTime)
UnixTime zero = UnixTime.Zero;             // Timestamp == 0

// Limits of the 32-bit Unix timestamp (the "Year 2038 problem")
DateTime y2038 = UnixTime.ToUniversalDateTime(int.MaxValue);
// 2038-01-19 03:14:07 UTC

// Maximum value the library supports (bounded by DateTime.MaxValue)
UnixTime maxDate = DateTime.MaxValue.ToUniversalTime().Subtract(TimeSpan.FromSeconds(1));
Console.WriteLine($"Max supported: {maxDate.DateTimeUtc:u}");
```

---

## Version History

### 10.1.0

This release introduces four **breaking changes** for correctness and C# 13 compatibility. See the migration notes below if upgrading from 10.0.x.

#### Breaking Change — Explicit `long → UnixTime` and `double → UnixTime` conversions

`implicit operator UnixTime(long)` and `implicit operator UnixTime(double)` are now **explicit** to avoid operator ambiguity (`CS9342`) introduced in C# 13.

**Migration:** Replace implicit numeric assignments with an explicit cast or constructor call.

```csharp
// Before (no longer compiles)
UnixTime t = 1204343210L;

// After
UnixTime t = new(1204343210L);
// or
UnixTime t = (UnixTime)1204343210L;
```

`UnixTime → long` and `UnixTime → double` remain **implicit** and are unaffected.

New `+` and `-` operators accepting `long` and `double` offsets make arithmetic unambiguous:

```csharp
UnixTime t = new(1700000000L);
UnixTime later     = t + 3600L;     // operator+(UnixTime, long)
UnixTime fractional = t + 0.5;     // operator+(UnixTime, double)
```

#### Breaking Change — `DateTimeKind.Unspecified` throws `ArgumentException`

Passing a `DateTime` with `Kind == DateTimeKind.Unspecified` to `FromDateTime`, the `UnixTime(DateTime)` constructor, or the `DateTime → UnixTime` implicit conversion now throws `ArgumentException` instead of silently treating the value as UTC.

**Migration:** Specify the `DateTimeKind` explicitly.

```csharp
// Before — silently treated as UTC, now throws
DateTime d = new(2024, 1, 15, 0, 0, 0);  // Kind == Unspecified
UnixTime t = d;

// After — be explicit
DateTime utc   = new(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);
DateTime local = new(2024, 1, 15, 0, 0, 0, DateTimeKind.Local);
UnixTime tUtc   = utc;
UnixTime tLocal = local;
```

#### Breaking Change — `TryParse` parse order: numeric before date-time

All `TryParse` overloads now attempt numeric (`double`) parsing **first**, followed by `DateTime`, then `TimeSpan`. Previously, `DateTime` was attempted first.

**Impact:** An ambiguous string like `"2024"` now parses as the number `2024` (seconds after the Epoch = 1970-01-01 00:33:44 UTC) rather than January 1, 2024.

**Migration:** Use fully-qualified ISO 8601 date strings (e.g. `"2024-01-01T00:00:00Z"`) to ensure date-time strings are not misinterpreted as timestamps.

#### Breaking Change — Culture-invariant `ToString()` and `TryParse`

`ToString()` now uses `CultureInfo.InvariantCulture` to format the numeric timestamp. The basic `TryParse(string, out UnixTime)` overload also uses invariant culture for all three parse attempts.

**Impact:** On machines with locales that use a comma as the decimal separator (e.g. German), `ToString()` previously returned `"1700000000,5"` for a fractional timestamp. It now always returns `"1700000000.5"`, enabling reliable round-tripping across machines.

**Migration:** Most callers are unaffected. If you were relying on locale-sensitive numeric output from `ToString()`, switch to `ToString("R")` or use `Timestamp.ToString(yourCulture)` directly.
