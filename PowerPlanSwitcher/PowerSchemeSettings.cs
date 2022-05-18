namespace PowerPlanSwitcher
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Properties;

    internal static class PowerSchemeSettings
    {
        private class ImageConverter : JsonConverter
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
                    return Image.FromStream(
                        new MemoryStream(Convert.FromBase64String(base64)));
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
                    writer.WriteValue("");
                    return;
                }

                var ms = new MemoryStream();
                image.Save(ms, image.RawFormat);
                var imageBytes = ms.ToArray();
                writer.WriteValue(imageBytes);
            }

            public override bool CanConvert(Type objectType) =>
                objectType == typeof(Image);
        }

        public class Setting
        {
            public bool Visible { get; set; } = true;
            [JsonConverter(typeof(ImageConverter))]
            public Image? Icon { get; set; }
        }

        private static Dictionary<Guid, Setting>? settings;

        public static Setting? GetSetting(Guid schemaGuid)
        {
            LoadSettings();
            _ = settings!.TryGetValue(schemaGuid, out var setting);
            return setting;
        }

        public static void SetSetting(Guid schemaGuid, Setting setting)
        {
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

            settings = JsonConvert.DeserializeObject<Dictionary<Guid, Setting>>(
                    Settings.Default.PowerSchemeSettings) ??
                new Dictionary<Guid, Setting>();
        }
    }
}
