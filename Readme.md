# .NET Core Configuration provider for OCI Key Vault

![Nuget](https://img.shields.io/nuget/v/Mcrio.Configuration.Provider.Docker.Secrets)

This package allows reading secrets from [OCI Key Vault](https://www.oracle.com/security/cloud-security/key-management/) and place there in IConfiguration for consuming.

```
dotnet add package Luansens.Configuration.Provider.Oci.KeyVault
```

## Usage

#### Simple usage
```cs
var provider = new ConfigFileAuthenticationDetailsProvider("DEFAULT");

var configuration = new ConfigurationBuilder()
                        .AddOciKeyVault(provider, compartmentId)
                        .Build();

var secretValue = configuration["mysecret"];
```

#### ASP.NET Core
```cs
// Program.cs
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configBuilder =>
                {

                    var provider = new ConfigFileAuthenticationDetailsProvider("DEFAULT");
                    
                    configBuilder
                        .AddOciKeyVault(provider, compartmentId)();

                    // allow command line arguments to override OCI secrets
                    if (args != null)
                    {
                        configBuilder.AddCommandLine(args);
                    }
                })
                .UseStartup<Startup>();
```

#### Only add secrets of specific vault

```cs
var provider = new ConfigFileAuthenticationDetailsProvider("DEFAULT");

configBuilder.AddOciKeyVault(provider, compartmentId, setup => {
    setup.VaultId = <VAULT_ID>
});
```
#### Only add secrets of specific stage

```cs
var provider = new ConfigFileAuthenticationDetailsProvider("DEFAULT");

configBuilder.AddOciKeyVault(provider, compartmentId, setup => {
    setup.Stage = StageEnum.Current // DEFAULT VALUE
});
```

### Configure Client

```cs
var provider = new ConfigFileAuthenticationDetailsProvider("DEFAULT");

configBuilder.AddOciKeyVault(provider, compartmentId, setup => {
    setup.ClientConfiguration = new ClientConfiguration(){
        TimeoutMillis = 500
    }
});
```