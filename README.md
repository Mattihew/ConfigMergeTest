# Config Array Merge POC

A proof-of-concept project exploring seamless configuration array merging in .NET. This implementation provides a custom `IConfigurationProvider` wrapper that intercepts configuration key operations, dynamically adjusts array indices, and prevents collisions when combining multiple configuration sources.

## 📖 Overview

Standard .NET configuration providers do not natively support merging array-based settings without index conflicts or overwriting. This POC demonstrates a strategy to:
- Wrap existing `IConfigurationProvider` instances
- Track and offset array indices across multiple sources
- Map configuration paths transparently during read/write operations
- Maintain compatibility with `Microsoft.Extensions.Configuration` change tokens and reloading mechanisms

## 🛠 Requirements

- .NET 10.0
- C# 14.0
- `Microsoft.Extensions.Configuration` (and related packages)

## 🔍 How It Works

The core component is a provider wrapper that intercepts configuration operations:
1. **Path Mapping:** Identifies keys belonging to designated array paths.
2. **Index Offset Calculation:** Dynamically calculates an offset based on existing array indices to prevent collisions.
3. **Transparent Interception:** Overrides `Set`, `TryGet`, and `GetChildKeys` to apply offsets only to matching array paths, leaving non-array configurations untouched.
4. **Change Token Support:** Delegates to the underlying provider's reload token, ensuring configuration reloading works as expected.

## 📦 Usage (Conceptual)

While this is a proof of concept, integration typically follows the standard .NET configuration builder pattern:

```csharp
var builder = new ConfigurationBuilder();

builder.AddConfigMerger(["test", "test2"], c =>
{
    c.AddJsonFile("appsettings.json");
    c.AddInMemoryCollection(new Dictionary<string, string>()
    {
        { "test:0", "first" },
        { "test:1", "second" },
        {"test2:1", "foo" }
    });
```

> 💡 **Note:** This is a POC. The exact registration and API surface may evolve as the concept is refined for production use.

## ⚠️ POC Disclaimer

This project is currently a proof of concept. It has not undergone rigorous production testing, performance benchmarking, or comprehensive edge-case validation. Features, APIs, and internal implementation details are subject to change.
