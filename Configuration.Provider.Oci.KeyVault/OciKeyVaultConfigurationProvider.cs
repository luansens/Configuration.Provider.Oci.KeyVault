using System.Text;
using Microsoft.Extensions.Configuration;
using Oci.SecretsService;
using Oci.SecretsService.Models;
using Oci.SecretsService.Requests;
using Oci.VaultService;
using Oci.VaultService.Models;
using Oci.VaultService.Requests;

namespace Luansens.Configuration.Provider.Oci.KeyVault;

internal sealed class OciKeyVaultConfigurationProvider : ConfigurationProvider
{
    private readonly OciKeyVaultConfigurationSource _source;
    public OciKeyVaultConfigurationProvider(OciKeyVaultConfigurationSource source)
    {
        _source = source;
    }
    public override void Load()
    {
        var secrets = GetSecrets();

        foreach (var secret in secrets)
        {
            Data.Add(secret.Key, secret.Value);
        }
    }

    private IEnumerable<SecretSummary> GetSecretSummaries()
    {
        VaultsClient vaultsClient;
        if (_source._configuration.ClientConfiguration is not null)
        {
            vaultsClient = new VaultsClient(_source._provider, _source._configuration.ClientConfiguration);
        }
        else
        {
            vaultsClient = new VaultsClient(_source._provider);
        }


        var request = new ListSecretsRequest
        {
            CompartmentId = _source._compartmentId,
            LifecycleState = SecretSummary.LifecycleStateEnum.Active
        };

        if (!string.IsNullOrWhiteSpace(_source._configuration.VaultId))
        {
            request.VaultId = _source._configuration.VaultId;
        }

        var vaultsPaginator = new VaultsPaginators(vaultsClient);

        var secretSummaries = vaultsPaginator.ListSecretsRecordEnumerator(request);
        return secretSummaries;
    }

    private IEnumerable<KeyValuePair<string, string>> GetSecrets()
    {
        var secretSummaries = GetSecretSummaries();

        SecretsClient secretsClient;
        if (_source._configuration.ClientConfiguration is not null)
        {
            secretsClient = new SecretsClient(_source._provider, _source._configuration.ClientConfiguration);
        }
        else
        {
            secretsClient = new SecretsClient(_source._provider);
        }

        foreach (var secretSummary in secretSummaries)
        {
            var request = new GetSecretBundleRequest
            {
                SecretId = secretSummary.Id,
                Stage = _source._configuration.Stage
            };

            var response = secretsClient.GetSecretBundle(request)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            var base64Content = (Base64SecretBundleContentDetails)response.SecretBundle.SecretBundleContent;
            yield return KeyValuePair.Create(
                secretSummary.SecretName,
                ConvertFromBase64ToRaw(base64Content.Content)
            );
        }
    }

    private string ConvertFromBase64ToRaw(string base64String)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64String);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

}