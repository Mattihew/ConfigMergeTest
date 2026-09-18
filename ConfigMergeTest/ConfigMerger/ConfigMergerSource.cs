using Microsoft.Extensions.Configuration;

namespace ConfigMergeTest.ConfigMerger;

public class ConfigMergerSource(IEnumerable<string> arrayPath, IConfigurationSource source): IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new ConfigMergerProvider(arrayPath, source.Build(builder));
    }
}

public static class ConfigMergerSourceExtensions
{
    extension(IConfigurationBuilder builder)
    {
        public IConfigurationBuilder AddConfigMerger(IEnumerable<string> arrayPath, Action<IConfigurationBuilder> builderAction)
        {
            var configBuilder = new ConfigurationBuilder();
            builderAction(configBuilder);
            return builder.AddConfigMerger(arrayPath, configBuilder.Sources);
        }

        public IConfigurationBuilder AddConfigMerger(IEnumerable<string> arrayPath, IEnumerable<IConfigurationSource> sources)
        {
            foreach (var source in sources)
            {
                builder.Add(new ConfigMergerSource(arrayPath, source));
            }
            return builder;
        }
    }
}