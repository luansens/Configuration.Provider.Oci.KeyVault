using Oci.Common;
using static Oci.SecretsService.Requests.GetSecretBundleRequest;

namespace Luansens.Configuration.Provider.Oci.KeyVault;

public sealed class OciKeyVaultConfiguration
{
    /// <summary>
    /// The OCID of the vault in which to fetch secrets. When empty all vaults in the compartment are fetched.
    /// </summary>
    public string? VaultId { get; set; }
    /// <summary>
    /// Secrets stage, only secrets with this stage will be considered. Defaults to <see cref="StageEnum.Current"/>.
    /// </summary>
    public StageEnum Stage { get; set; } = StageEnum.Current;

    /// <summary>
    /// Client configuration to use when making requests to OCI.
    /// </summary>
    /// <remarks>
    /// When null, the default client configuration is used.
    /// </remarks>  
    public ClientConfiguration? ClientConfiguration { get; set; }
}