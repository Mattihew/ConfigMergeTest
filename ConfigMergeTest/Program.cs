// See https://aka.ms/new-console-template for more information

using ConfigMergeTest.ConfigMerger;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();

builder.AddConfigMerger(["test", "test2"], c =>
{
    c.AddInMemoryCollection(new Dictionary<string, string>()
    {
        { "test:0", "first" },
        { "test:1", "second" },
        {"test2:1", "foo" }
    }!);
    c.AddInMemoryCollection(new Dictionary<string, string>()
    {
        { "test:0", "third" },
        { "test:1:name", "fourth" },
        { "test2:0", "bar" }
    }!);
    c.AddInMemoryCollection(new Dictionary<string, string>()
    {
        { "test:0:name", "fifth" },
        { "test:0:value", "sixth" },
        { "test2:0:name", "baz"}
    }!);
});

var config = builder.Build();

Console.WriteLine(config.GetDebugView());