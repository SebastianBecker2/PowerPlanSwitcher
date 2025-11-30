namespace RuleManagement.Rules;

public record MigrationPolicy(
    // Attention: MigratedPowerRulesToRules says if the migration
    // has already been executed. So True means, no migration necessary
    // anymore. While False means the migration has to be done.
    bool MigratedPowerRulesToRules,
    Guid AcPowerSchemeGuid,
    Guid BatterPowerSchemeGuid);
