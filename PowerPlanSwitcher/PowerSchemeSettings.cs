namespace PowerPlanSwitcher;

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Properties;
using SkiaSharp;

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

                using var skData = SKData.CreateCopy(imageBytes);
                using var skImage = SKImage.FromEncodedData(skData)
                    ?? throw new InvalidOperationException(
                        "Failed to decode image data.");

                using var encoded = skImage.Encode(
                    SKEncodedImageFormat.Png,
                    100);
                using var pngStream = new MemoryStream(encoded.ToArray());
                return Image.FromStream(pngStream);
            }
            catch (FormatException)
            {
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
            image.Save(ms, ImageFormat.Png);
            _ = ms.Seek(0, SeekOrigin.Begin);

            using var skData = SKData.Create(ms);
            using var skImage = SKImage.FromEncodedData(skData);

            using var encoded = skImage.Encode(SKEncodedImageFormat.Png, 100);
            writer.WriteValue(Convert.ToBase64String(encoded.ToArray()));
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

            settings = JsonConvert.DeserializeObject<Dictionary<Guid, Setting>>(
                    Settings.Default.PowerSchemeSettings) ??
                [];
        }
    }
}
