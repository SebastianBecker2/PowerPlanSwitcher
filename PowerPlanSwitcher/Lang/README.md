# Translations

UI strings are loaded at runtime from JSON files in this folder.

## File naming

Each file is named after a .NET culture, matching `CultureInfo.CurrentUICulture.Name`:

| Language | File |
|---|---|
| Simplified Chinese | `zh-CN.json` |
| Traditional Chinese (Taiwan) | `zh-TW.json` |
| Japanese | `ja-JP.json` |
| Russian | `ru-RU.json` |
| German | `de-DE.json` |
| French | `fr-FR.json` |

Use the standard `ll-CC` form (`zh-CN`, not `zh_CN` or `zh-cn`). File names are case-sensitive on Linux CI.

## Adding a new language

```powershell
dotnet run --project tools\SyncLangJson -- --culture ja-JP --create
```

This creates `ja-JP.json` with every key from `Strings.resx`, using the English value as a placeholder. Translate the values and commit the file.

## Missing keys

Missing keys fall back to the built-in English `Strings.resx`. A missing or malformed JSON file falls back to English instead of crashing.

## Keeping files in sync

`Strings.resx` is the source of truth for keys. When a new key is added:

- Local builds run `tools/SyncLangJson --sync` and append the new key to every `Lang/*.json`.
- CI runs `tools/SyncLangJson --check` and fails if a translation file is out of sync.

You normally do not need to edit keys by hand. Only edit values.