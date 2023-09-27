using Microsoft.Extensions.Configuration;
using Oci.Common.Auth;

namespace Luansens.Configuration.Provider.Oci.KeyVault;

public static class OciKeyVaultConfigurationExtensions
{
    /// <summary>
    /// Add OCI Key Vault as configuration source.
    /// </summary>
    /// <param name="provider">Authentication provider, must contains valid credentials to connect to OCI.</param>
    /// <param name="compartmentId">The OCID of the compartment that includes the vaults.</param>
    /// <param name="setupConfiguration">Configure options or default values are used.</param>
    /// <returns>The configuration builder.</returns>
    public static IConfigurationBuilder AddOciKeyVault(
        this IConfigurationBuilder builder,
        IBasicAuthenticationDetailsProvider provider,
        string compartmentId,
        Action<OciKeyVaultConfiguration> setupConfiguration
        )
    {
        var configuration = new OciKeyVaultConfiguration();
        setupConfiguration(configuration);
        return builder.Add(new OciKeyVaultConfigurationSource(provider, compartmentId, configuration));
    }
    /// <summary>
    /// Add OCI Key Vault as configuration source.
    /// </summary>
    /// <param name="provider">Authentication provider, must contains valid credentials to connect to OCI.</param>
    /// <param name="compartmentId">The OCID of the compartment that includes the vaults.</param>
    /// <returns>The configuration builder.</returns>
    public static IConfigurationBuilder AddOciKeyVault(
        this IConfigurationBuilder builder,
        IAuthenticationDetailsProvider provider,
        string compartmentId
        )
    {
        var configuration = new OciKeyVaultConfiguration();
        return builder.Add(new OciKeyVaultConfigurationSource(provider, compartmentId, configuration));
    }
}