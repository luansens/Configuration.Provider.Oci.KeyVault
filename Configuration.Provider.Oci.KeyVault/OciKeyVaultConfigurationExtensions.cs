using Microsoft.Extensions.Configuration;
using Oci.Common.Auth;

namespace Luansens.Configuration.Provider.Oci.KeyVault;

public static class OciKeyVaultConfigurationExtensions
{
    public static IConfigurationBuilder AddOciKeyVault(
        this IConfigurationBuilder builder,
        IAuthenticationDetailsProvider provider,
        string compartmentId,
        Action<OciKeyVaultConfiguration> setupConfiguration
        )
    {
        var configuration = new OciKeyVaultConfiguration();
        setupConfiguration(configuration);
        return builder.Add(new OciKeyVaultConfigurationSource(provider, compartmentId, configuration));
    }
}