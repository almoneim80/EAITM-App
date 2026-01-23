using EAITMApp.SharedKernel.Enums;
using EAITMApp.SharedKernel.Errors.Enums;

namespace EAITMApp.SharedKernel.Errors.Registries
{
    public class PersistenceErrors
    {
        private const string CodePrefix = "INFRASTRUCTURE.PERSISTENCE";
        private const string MessagePrefix = "Persistence configuration error: ";

        // =========================
        // SETTINGS
        // =========================
        public static readonly ErrorDescriptor SettingsNotConfigured = new(
            code: $"{CodePrefix}.SETTINGS.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Persistence settings section is missing from application configuration.",
            isSafeToExpose: false);

        // =========================
        // PROVIDER
        // =========================
        public static readonly ErrorDescriptor ProviderNotConfigured = new(
            code: $"{CodePrefix}.PROVIDER.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Data store provider is not configured.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ProviderUnsupported = new(
            code: $"{CodePrefix}.PROVIDER.UNSUPPORTED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}The specified data store provider is not supported.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ProviderTopologyMismatch = new(
            code: $"{CodePrefix}.PROVIDER.TOPOLOGY_MISMATCH",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Data store providers are misconfigured and do not match the expected topology.",
            isSafeToExpose: false);

        // =========================
        // CONNECTION
        // =========================
        public static readonly ErrorDescriptor ConnectionStringNotConfigured = new(
            code: $"{CodePrefix}.CONNECTION.STRING.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Connection string is missing or empty.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ConnectionHostNotConfigured = new(
            code: $"{CodePrefix}.CONNECTION.HOST.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Data store host is not configured.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ConnectionPortNotConfigured = new(
            code: $"{CodePrefix}.CONNECTION.PORT.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Data store port is not configured or must be between 1 and 65535.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ConnectionDataStoreNameNotConfigured = new(
            code: $"{CodePrefix}.CONNECTION.DATA_STORE_NAME.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Data store name is not configured",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ConnectionDataStoreUsernameNotConfigured = new(
            code: $"{CodePrefix}.CONNECTION.DATA_STORE_USERNAME.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Data store username is not configured",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ConnectionDataStorePasswordNotConfigured = new(
            code: $"{CodePrefix}.CONNECTION.DATA_STORE_PASSWORD.NOT_CONFIGURED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Data store password is not configured",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ConnectionSslModeInvalid = new(
            code: $"{CodePrefix}.CONNECTION.SSL_MODE.INVALID",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}SSL Mode is invalid.",
            isSafeToExpose: false);

        public static readonly ErrorDescriptor ConnectionFailed = new(
            code: $"{CodePrefix}.CONNECTION.FAILED",
            category: ErrorCategory.Infrastructure,
            severity: ErrorSeverity.Critical,
            httpStatus: AppHttpStatus.InternalServerError,
            defaultMessage: $"{MessagePrefix}Failed to establish a connection to the data store.",
            isSafeToExpose: false);
    }
}
