﻿using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MHXXSaveEditor.Data;

namespace MHXXSaveEditor.Util
{
    class DataExtractor
    {
        public void getInfo(byte[] saveFile, int slot, Player player)
        {
            byte[] charNameByte = new byte[32];
            byte[] playTimeByte = new byte[4];
            byte[] fundsByte = new byte[4];
            byte[] rankByte = new byte[2];
            byte[] hrPointsByte = new byte[4];
            byte[] acaPointsByte = new byte[4];
            byte[] villagePointsByte = new byte[4];
            player.SkinColorRGBA = new byte[4];
            player.HairColorRGBA = new byte[4];
            player.FeaturesColorRGBA = new byte[4];
            player.ClothingColorRGBA = new byte[4];
            player.itemId = new string[Constants.TOTAL_ITEM_SLOTS];
            player.itemCount = new string[Constants.TOTAL_ITEM_SLOTS];
            player.equipmentInfo = new byte[Constants.SIZEOF_EQUIPBOX];
            player.equipmentPalico = new byte[Constants.SIZEOF_PALICOEQUIPBOX];
            player.PalicoData = new byte[Constants.SIZEOF_PALICOES];
            byte[] itemBytes = new byte[Constants.SIZEOF_ITEMBOX];

            if (slot == 1)
            {
                string firstSlot = saveFile[0x13].ToString("X2") + saveFile[0x12].ToString("X2") + saveFile[0x11].ToString("X2") + saveFile[0x10].ToString("X2");
                player.SaveOffset = Int32.Parse(firstSlot, System.Globalization.NumberStyles.HexNumber);
            }
            else if (slot == 2)
            {
                string secondSlot = saveFile[0x17].ToString("X2") + saveFile[0x16].ToString("X2") + saveFile[0x15].ToString("X2") + saveFile[0x14].ToString("X2");
                player.SaveOffset = Int32.Parse(secondSlot, System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                string thirdSlot = saveFile[0x21].ToString("X2") + saveFile[0x20].ToString("X2") + saveFile[0x19].ToString("X2") + saveFile[0x18].ToString("X2");
                player.SaveOffset = Int32.Parse(thirdSlot, System.Globalization.NumberStyles.HexNumber);
            }

            // Character Name
            Array.Copy(saveFile, player.SaveOffset, charNameByte, 0, Constants.SIZEOF_NAME); // copies from savefile to buffer
            player.Name = Encoding.UTF8.GetString(charNameByte);

            // Play Time
            Array.Copy(saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET, playTimeByte, 0, 4);
            player.PlayTime = BitConverter.ToInt32(playTimeByte, 0);

            // Funds
            Array.Copy(saveFile, player.SaveOffset + Offsets.FUNDS_OFFSET, fundsByte, 0, 4);
            player.Funds = BitConverter.ToInt32(fundsByte, 0);

            // Hunter Rank
            Array.Copy(saveFile, player.SaveOffset + Offsets.HUNTER_RANK_OFFSET, rankByte, 0, 2);
            player.HunterRank = BitConverter.ToInt16(rankByte, 0);

            // Hunter Rank Points
            Array.Copy(saveFile, player.SaveOffset + Offsets.HR_POINTS_OFFSET, hrPointsByte, 0, 4);
            player.HRPoints = BitConverter.ToInt32(hrPointsByte, 0);

            // Academy Points
            Array.Copy(saveFile, player.SaveOffset + Offsets.ACADEMY_POINTS_OFFSET, acaPointsByte, 0, 4);
            player.AcademyPoints = BitConverter.ToInt32(acaPointsByte, 0);

            // Village Points
            Array.Copy(saveFile, player.SaveOffset + Offsets.BHERNA_POINTS_OFFSET, villagePointsByte, 0, 4);
            player.BhernaPoints = BitConverter.ToInt32(villagePointsByte, 0);
            Array.Copy(saveFile, player.SaveOffset + Offsets.KOKOTO_POINTS_OFFSET, villagePointsByte, 0, 4);
            player.KokotoPoints = BitConverter.ToInt32(villagePointsByte, 0);
            Array.Copy(saveFile, player.SaveOffset + Offsets.POKKE_POINTS_OFFSET, villagePointsByte, 0, 4);
            player.PokkePoints = BitConverter.ToInt32(villagePointsByte, 0);
            Array.Copy(saveFile, player.SaveOffset + Offsets.YUKUMO_POINTS_OFFSET, villagePointsByte, 0, 4);
            player.YukumoPoints = BitConverter.ToInt32(villagePointsByte, 0);

            // Character Info
            player.Voice = saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET];
            player.EyeColor = saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET];
            player.Clothing = saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET];
            player.Gender = saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET];
            player.HairStyle = saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET];
            player.Face = saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET];
            player.Features = saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET];

            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET, player.SkinColorRGBA, 0, 4);
            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET, player.HairColorRGBA, 0, 4);
            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET, player.FeaturesColorRGBA, 0, 4);
            Array.Copy(saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET, player.ClothingColorRGBA, 0, 4);

            // Item Box
            Array.Copy(saveFile, player.SaveOffset + Offsets.ITEM_BOX_OFFSET, itemBytes, 0, Constants.SIZEOF_ITEMBOX);
            Array.Reverse(itemBytes);
            var result = string.Concat(itemBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            result = result.Substring(4,result.Length-4); // To remove the unnecessary/extra '0000'
            for (int a = 2299; a >= 0; a--)
            {
                player.itemCount[a] = Convert.ToInt32(result.Substring(0, 7), 2).ToString();
                player.itemId[a] = Convert.ToInt32(result.Substring(7, 12), 2).ToString();
                result = result.Substring(19, result.Length - 19);
            }

            // Equipment Box
            Array.Copy(saveFile, player.SaveOffset + Offsets.EQUIPMENT_BOX_OFFSET, player.equipmentInfo, 0, Constants.SIZEOF_EQUIPBOX);

            // Palico Equipment Box
            Array.Copy(saveFile, player.SaveOffset + Offsets.PALICO_EQUIPMENT_OFFSET, player.equipmentPalico, 0, Constants.SIZEOF_PALICOEQUIPBOX);

            // Palico
            Array.Copy(saveFile, player.SaveOffset + Offsets.PALICO_OFFSET, player.PalicoData, 0, Constants.SIZEOF_PALICOES);
        }
    }
}