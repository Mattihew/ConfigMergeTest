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
        /// <summary>
        /// Adds a configuration merger to the builder that applies the specified configuration sources
        /// to the given array path.
        /// </summary>
        /// <param name="arrayPath">The configuration path where merged values should be placed.</param>
        /// <param name="builderAction">An action that configures the internal configuration builder.</param>
        /// <returns>The original <see cref="IConfigurationBuilder"/> for chaining.</returns>
        public IConfigurationBuilder AddConfigMerger(IEnumerable<string> arrayPath, Action<IConfigurationBuilder> builderAction)
        {
            var configBuilder = new ConfigurationBuilder();
            builderAction(configBuilder);
            return builder.AddConfigMerger(arrayPath, configBuilder.Sources);
        }

        /// <summary>
        /// Adds configuration sources to the builder, wrapping each in a merger that places values
        /// under the specified array path.
        /// </summary>
        /// <param name="arrayPath">The configuration path where merged values should be placed.</param>
        /// <param name="sources">The configuration sources to add.</param>
        /// <returns>The original <see cref="IConfigurationBuilder"/> for chaining.</returns>
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