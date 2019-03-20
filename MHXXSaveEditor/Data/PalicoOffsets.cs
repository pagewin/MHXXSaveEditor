namespace MHXXSaveEditor.Data
{
    public class PalicoOffsets
    {
        public enum BiasType {
            Charisma,
            Fighting,
            Protection,
            Assist,
            Healing,
            Bombing,
            Gathering,
            Beast
        };

        public readonly string[] BIAS_TEXT = {
            "Charisma",
            "Fighting",
            "Protection",
            "Assist",
            "Healing",
            "Bombing",
            "Gathering",
            "Beast"
        };

        public const byte NAME = 0x0;
        public const byte LAST_MASTER = 0xBC;
        public const byte NAMEGIVER = 0x9C;
        public const byte SIZEOF_NAME = 32;

        public const byte EXP = 0x20;
        public const byte SIZEOF_EXP = 4;

        public const byte LEVEL = 0x24;
        public const byte SIZEOF_LEVEL = 1;
        public const byte BIAS = 0x25;
        public const byte SIZEOF_BIAS = 1;
        public const byte ENTHUSIASM = 0x26;
        public const byte SIZEOF_ENTHUSIASM = 1;
        public const byte TARGET = 0x27;
        public const byte SIZEOF_TARGET = 1;

        public const byte EQUIPPED_SUPPORT_MOVES = 0x28;
        public const byte SIZEOF_EQUIPPED_SUPPORT_MOVES = 8;
        public const byte EQUIPPED_SKILLS = 0x30;
        public const byte SIZEOF_EQUIPPED_SKILLS = 8;

        public const byte KNOWN_SUPPORT_MOVES = 0x38;
        public const byte SIZEOF_KNOWN_SUPPORT_MOVES = 16;
        public const byte KNOWN_SKILLS = 0x48;
        public const byte SIZEOF_KNOWN_SKILLS = 12;

        public const byte SUPPORT_MOVE_RNG = 0x54;
        public const byte SKILL_RNG = 0x56;
        public const byte SIZEOF_RNG = 2;

        public const byte ORIGINAL_OWNER_ID = 0x58;
        public const byte SIZEOF_ORIGINAL_OWNER_ID = 4;
        public const byte SCOUT_ID = 0x5C;
        public const byte SIZEOF_SCOUT_ID = 3;
        public const byte PALICO_ID = 0x5F;
        public const byte SIZEOF_PALICO_ID = 1;
        public const byte GREETING = 0x60;
        public const byte SIZEOF_GREETING = 60;

        public const byte STATUS = 0xE0;
        public const byte TRAINING = 0xE1;
        public const byte JOB = 0xE2;
        public const byte PROWLER = 0xE3;
        public const byte SIZEOF_STATUS = 1;

        public const ushort VOICE = 0x10F;
        public const ushort EYES = 0x110;
        public const ushort CLOTHING = 0x111;
        public const ushort COAT = 0x114;
        public const ushort EARS = 0x115;
        public const ushort TAIL = 0x116;
        public const byte SIZEOF_DESIGN = 1;

        public const ushort COAT_COLOR = 0x11A;
        public const ushort RIGHT_EYE_COLOR = 0x11E;
        public const ushort LEFT_EYE_COLOR = 0x122;
        public const ushort CLOTHING_COLOR = 0x126;
        public const byte SIZEOF_COLOR = 4;
    }
}
