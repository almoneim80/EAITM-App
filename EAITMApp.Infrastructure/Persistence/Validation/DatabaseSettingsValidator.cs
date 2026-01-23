using EAITMApp.Application.Persistence;
using EAITMApp.Application.Persistence.Validation;
using EAITMApp.SharedKernel.Enums;
using EAITMApp.SharedKernel.Errors;
using EAITMApp.SharedKernel.Errors.Registries;
using EAITMApp.SharedKernel.Exceptions;

namespace EAITMApp.Infrastructure.Persistence.Validation
{
    public class DatabaseSettingsValidator : IDatabaseSettingsValidator
    {
        public void Validate(IDataStoresSettings settings)
        {
            if (settings == null) throw new PersistenceConfigurationException(PersistenceErrors.SettingsNotConfigured);

            var writeDbSettings = settings.WriteDatabaseSettings;
            var readDbSettings = settings.ReadDatabaseSettings;

            if (!string.Equals(writeDbSettings.ProviderType, readDbSettings.ProviderType, StringComparison.OrdinalIgnoreCase))
                throw new PersistenceConfigurationException(PersistenceErrors.ProviderTopologyMismatch);

            ValidateConnectionSettings(writeDbSettings, DataStoreRole.Primary);
            ValidateConnectionSettings(readDbSettings, DataStoreRole.Secondary);
        }

        private void ValidateConnectionSettings(IDatabaseConnectionSettings settings, DataStoreRole role)
        {
            if (string.IsNullOrWhiteSpace(settings.ProviderType)) throw WithRole(PersistenceErrors.ProviderNotConfigured, role);

            if (string.IsNullOrWhiteSpace(settings.Host)) throw WithRole(PersistenceErrors.ConnectionHostNotConfigured, role);

            if (settings.Port <= 0 || settings.Port > 65535) throw WithRole(PersistenceErrors.ConnectionPortNotConfigured, role);

            if (string.IsNullOrWhiteSpace(settings.Database)) throw WithRole(PersistenceErrors.ConnectionDataStoreNameNotConfigured, role);

            if (string.IsNullOrWhiteSpace(settings.Username)) throw WithRole(PersistenceErrors.ConnectionDataStoreUsernameNotConfigured, role);

            if (string.IsNullOrWhiteSpace(settings.Password)) throw WithRole(PersistenceErrors.ConnectionDataStorePasswordNotConfigured, role);

            var validSslModes = new[] { "Disable", "Allow", "Prefer", "Require", "VerifyCA", "VerifyFull" };
            if (!validSslModes.Contains(settings.SslMode, StringComparer.OrdinalIgnoreCase))
            {
                var metadata = new Dictionary<string, object>
                {
                    ["PassedSslMode"] = settings.SslMode,
                    ["ValidValues"] = string.Join(", ", validSslModes)
                };

                throw WithRole(PersistenceErrors.ConnectionSslModeInvalid, role, metadata);
            }
        }
        private static PersistenceConfigurationException WithRole(
            ErrorDescriptor descriptor, DataStoreRole role, Dictionary<string, object>? additionalData  = null)
        {
            var metadata = new Dictionary<string, object>
            {
                ["DataStoreRole"] = role.ToString()
            };

            if (additionalData is not null && additionalData.Any())
            {
                foreach(var item in additionalData)
                    metadata[item.Key] = item.Value;
            }

            return new PersistenceConfigurationException(descriptor, metadata);
                
        }
    }
}
