namespace PowerPlanSwitcher
{
    using System;
    using Hotkeys;

    public class Hotkey : IEquatable<Hotkey>
    {
        public Keys Key { get; set; } = Keys.None;
        public ModifierKeys Modifier { get; set; } = ModifierKeys.None;

        public override bool Equals(object? obj) =>
            Equals(obj as Hotkey);
        public bool Equals(Hotkey? other) =>
            other is not null
            && Key == other.Key
            && Modifier == other.Modifier;

        public override int GetHashCode() =>
            HashCode.Combine(Key, Modifier);

        public override string ToString() =>
            HotkeyManager.ToString(Key, Modifier);
    }
}
