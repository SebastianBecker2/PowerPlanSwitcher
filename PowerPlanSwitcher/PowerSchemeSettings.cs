namespace PowerPlanSwitcher;

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Properties;
using Serilog;

internal static class PowerSchemeSettings
{
    private sealed class ImageConverter : JsonConverter
    {
        public override object? ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer)
        {
            var base64 = reader.Value as string;
            if (string.IsNullOrWhiteSpace(base64))
            {
                return null;
            }

            try
            {
                var imageBytes = Convert.FromBase64String(base64);
                using var imageStream = new MemoryStream(imageBytes);
                using var sourceImage = Image.FromStream(
                    imageStream,
                    useEmbeddedColorManagement: true,
                    validateImageData: true);
                return IconUtilities.NormalizeForPowerSchemeIcon(sourceImage);
            }
            catch (Exception ex)
            {
                Log.Warning(
                    ex,
                    "Failed to decode power scheme icon from settings. Payload length: {PayloadLength}",
                    base64.Length);
                return null;
            }
        }

        public override void WriteJson(
            JsonWriter writer,
            object? value,
            JsonSerializer serializer)
        {
            if (value is not Image image)
            {
                writer.WriteValue(string.Empty);
                return;
            }

            // !Deterministically! serializing image
            // Important right now to recognize changes in settings.

            using var ms = new MemoryStream();
            using var normalizedImage = IconUtilities.NormalizeForPowerSchemeIcon(image);
            normalizedImage.Save(ms, ImageFormat.Png);
            writer.WriteValue(Convert.ToBase64String(ms.ToArray()));
        }

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Image);
    }

    public class Setting
    {
        public bool Visible { get; set; } = true;
        [JsonConverter(typeof(ImageConverter))]
        public Image? Icon { get; set; }
        public Hotkey? Hotkey { get; set; }
    }

    private static Dictionary<Guid, Setting>? settings;
    private static readonly object SettingsLock = new();

    public static Setting? GetSetting(Guid schemaGuid)
    {
        LoadSettings();
        _ = settings!.TryGetValue(schemaGuid, out var setting);
        if (setting is not null)
        {
            return setting;
        }
        if (schemaGuid == Vanara.PInvoke.PowrProf.GUID_MAX_POWER_SAVINGS)
        {
            return new Setting { Visible = true, Icon = Resources.green };
        }
        if (schemaGuid == Vanara.PInvoke.PowrProf.GUID_MIN_POWER_SAVINGS)
        {
            return new Setting { Visible = true, Icon = Resources.red };
        }
        if (schemaGuid == Vanara.PInvoke.PowrProf.GUID_TYPICAL_POWER_SAVINGS)
        {
            return new Setting { Visible = true, Icon = Resources.yellow };
        }
        return null;
    }

    public static void SetSetting(Guid schemaGuid, Setting setting)
    {
        if (schemaGuid == Guid.Empty)
        {
            return;
        }

        LoadSettings();
        settings![schemaGuid] = setting;
    }

    public static void SaveSettings()
    {
        if (settings is null)
        {
            return;
        }
        Settings.Default.PowerSchemeSettings =
            JsonConvert.SerializeObject(settings);
        Settings.Default.Save();
    }

    private static void LoadSettings()
    {
        if (settings is not null)
        {
            return;
        }

        lock (SettingsLock)
        {
            if (settings is not null)
            {
                return;
            }

            try
            {
                settings = JsonConvert.DeserializeObject<Dictionary<Guid, Setting>>(
                        Settings.Default.PowerSchemeSettings) ??
                    [];
            }
            catch (Exception ex)
            {
                Log.Warning(
                    ex,
                    "Failed to load power scheme settings JSON. Falling back to defaults.");
                settings = [];
            }
        }
    }
}
