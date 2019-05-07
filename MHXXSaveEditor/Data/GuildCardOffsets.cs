using System;
namespace MHXXSaveEditor.Data
{
    public class GuildCardOffsets
    {
        public const byte NAME_FULLWIDTH = 0x0000;
        public const byte NAME_HALFWIDTH = 0x000C;
        public const int SIZEOF_NAME = 12;

        public const byte WEAPON_TYPE = 0x0018;
        public const int SIZEOF_WEAPON_TYPE = 1;
        public const byte HUNTING_STYLE = 0x001D;

        public const byte VOICE = 0x0019;
        public const byte EYE_COLOR = 0x001A;
        public const byte CLOTHING = 0x001B;
        public const byte GENDER = 0x001C;
        public const byte HAIRSTYLE = 0x001E;
        public const byte FACE = 0x001F;
        public const byte FEATURES = 0x0020;
        public const int SIZEOF_DESIGN = 1;

        public const byte ARMOR_PIGMENT_CHEST = 0x0024;
        public const byte ARMOR_PIGMENT_ARMS = 0x0028;
        public const byte ARMOR_PIGMENT_WAIST = 0x002C;
        public const byte ARMOR_PIGMENT_LEGS = 0x0030;
        public const byte ARMOR_PIGMENT_HEAD = 0x0034;
        public const byte SKIN_COLOR = 0x0038;
        public const byte HAIR_COLOR = 0x003C;
        public const byte FEATURES_COLOR = 0x0040;
        public const byte CLOTHING_COLOR = 0x0044;
        public const int SIZEOF_COLOR = 4;

        public const short PROWLER = 0x0188;
        public const short FIRST_PALICO = 0x03CC;
        public const short SECOND_PALICO = 0x610;
        public const int SIZEOF_PALICO = 580;

        public const short COMPLETED_VILLAGE_LOW = 0x085E;
        public const short COMPLETED_VILLAGE_HIGH = 0x0860;
        public const short COMPLETED_HUB_LOW = 0x0862;
        public const short COMPLETED_HUB_HIGH = 0x0864;
        public const short COMPLETED_G_RANK = 0x0866;
        public const short COMPLETED_SPECIAL_PERMIT = 0x0868;
        public const short COMPLETED_ARENA = 0x086A;
        public const int SIZEOF_COMPLETED_QUESTS = 2;

        public const short GUILD_CARD_ID = 0x08B0;
        public const int SIZEOF_GUILD_CARD_ID = 8;

        public const short WEAPON_USAGE_VILLAGE = 0x08BA;
        public const short WEAPON_USAGE_HUB = 0x08D8;
        public const short WEAPON_USAGE_ARENA = 0x08F6;
        public const int SIZEOF_SINGLE_WEAPON_USAGE = 2;
        public const int SIZEOF_TOTAL_WEAPON_USAGE = 30;

        public const short PLAYTIME = 0x0914;
        public const int SIZEOF_PLAYTIME = 4;

        public const short HUNTING_LOG = 0x918;
        public const int SIZEOF_SINGLE_HUNTING_LOG = 160;
        public const int SIZEOF_TOTAL_HUNTING_LOG = 1600; // 160 * 10

        public const short MONSTER_HUNTS = 0x0F58; // ?
        public const int SIZEOF_MONSTER_HUNTS = 716;

        /* 4 bytes each, 5 best times recorded per quest, 10 arena quests */
        public const short ARENA_HIGH_SCORES = 0x1224;
        public const int SIZEOF_ARENA_SCORE_ENTRY = 4;
        public const int SIZEOF_TOTAL_ARENA_SCORES = 342;
    }
}
