using Microsoft.Extensions.Configuration;
using Oci.Common.Auth;

namespace Luansens.Configuration.Provider.Oci.KeyVault;

internal sealed class OciKeyVaultConfigurationSource : IConfigurationSource
{
    internal readonly IAuthenticationDetailsProvider _provider;
    internal readonly string _compartmentId;
    internal readonly OciKeyVaultConfiguration _configuration;
    public OciKeyVaultConfigurationSource(IAuthenticationDetailsProvider provider, string compartmentId, OciKeyVaultConfiguration configuration)
    {
        _provider = provider;
        _compartmentId = compartmentId;
        _configuration = configuration;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new OciKeyVaultConfigurationProvider(this);
    }
}