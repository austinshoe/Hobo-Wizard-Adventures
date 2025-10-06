using System.Collections.Generic;

public class SystemInfo
{
    public enum ElementType
    {
        Water,
        Ice,
        Fire,
        Lightning,
        Earth,
        Air,
        Shadow,
        Light,
        None
    }

    public enum Mob
    {
        Archerfish,
        None
    }

    public static readonly Dictionary<ElementType, string> ElementTypeToString =
        new Dictionary<ElementType, string>
        {
            { ElementType.Water, "Water" },
            { ElementType.Ice, "Ice" },
            { ElementType.Fire, "Fire" },
            { ElementType.Lightning, "Lightning" },
            { ElementType.Earth, "Earth" },
            { ElementType.Air, "Air" },
            { ElementType.Shadow, "Shadow" },
            { ElementType.Light, "Light" },
            { ElementType.None, "None" }
        };
}