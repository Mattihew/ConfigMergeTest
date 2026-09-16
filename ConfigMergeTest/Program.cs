// See https://aka.ms/new-console-template for more information

using ConfigMergeTest.ConfigMerger;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();

builder.AddConfigMerger("test", c =>
{
    c.AddInMemoryCollection(new Dictionary<string, string>()
    {
        { "test:0", "first" },
        { "test:1", "second" }
    }!);
    c.AddInMemoryCollection(new Dictionary<string, string>()
    {
        { "test:0", "third" },
        { "test:1", "fourth" }
    }!);
    c.AddInMemoryCollection(new Dictionary<string, string>()
    {
        { "test:0:name", "fifth" },
        { "test:0:value", "sixth" }
    }!);
});

var config = builder.Build();

Console.WriteLine(config.GetDebugView());