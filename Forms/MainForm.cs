﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MHXXSaveEditor.Data;
using MHXXSaveEditor.Util;
using System.Linq;
using MHXXSaveEditor.Forms;

namespace MHXXSaveEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Text = Constants.EDITOR_VERSION; // Changes app title
        }

        public Player player = new Player();
        string filePath;
        byte[] saveFile;
        int currentPlayer, itemSelectedSlot;
        public int equipSelectedSlot;

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItemSaveSlot1.Enabled = false;
            toolStripMenuItemSaveSlot2.Enabled = false;
            toolStripMenuItemSaveSlot3.Enabled = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MHXX System File|system|All files (*.*)|*.*";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                ofd.Dispose();
                return;
            }

            filePath = ofd.FileName;
            this.Text = string.Format("{0} [{1}]", Constants.EDITOR_VERSION, ofd.SafeFileName); // Changes app title
            saveFile = File.ReadAllBytes(ofd.FileName); // Read all bytes from file into memory buffer
            ofd.Dispose();

            if (saveFile.Length != 4726152) // Check if save file is correct or not
            {
                MessageBox.Show("This is not a MHXX save file, program will now exit.", "Error !");
                if (Application.MessageLoop)
                {
                    Application.Exit();
                }
                else
                {
                    Environment.Exit(1);
                }
            }

            // To see which character slots are enabled
            if (saveFile[4] == 1) // First slot
            {
                currentPlayer = 1;
                toolStripMenuItemSaveSlot1.Enabled = true;
                toolStripMenuItemSaveSlot1.Checked = true;
            }
            if (saveFile[5] == 1 ) // Second slot
            {
                if (currentPlayer != 1)
                {
                    currentPlayer = 2;
                }
                toolStripMenuItemSaveSlot2.Enabled = true;
            }
            if (saveFile[6] == 1) // Third slot
            {
                if (currentPlayer != 1 || currentPlayer != 2)
                {
                    currentPlayer = 3;
                }
                toolStripMenuItemSaveSlot3.Enabled = true;
            }

            saveToolStripMenuItemSave.Enabled = true; // Enables the save toolstrip once save is loaded
            editToolStripMenuItem.Enabled = true;
            tabControlMain.Enabled = true;

            // Extract data from save file
            var ext = new DataExtractor();
            ext.getInfo(saveFile, currentPlayer, player);

            loadSave(); // Load save file data into editor
        }

        private void toolStripMenuItemSaveSlot1_Click(object sender, EventArgs e)
        {
            if(currentPlayer != 1)
            {
                currentPlayer = 1;
                toolStripMenuItemSaveSlot2.Checked = false;
                toolStripMenuItemSaveSlot3.Checked = false;

                var ext = new DataExtractor();
                ext.getInfo(saveFile, currentPlayer, player);
                loadSave();
            }
        }

        private void toolStripMenuItemSaveSlot2_Click(object sender, EventArgs e)
        {
            if (currentPlayer != 2)
            {
                currentPlayer = 2;
                toolStripMenuItemSaveSlot1.Checked = false;
                toolStripMenuItemSaveSlot3.Checked = false;

                var ext = new DataExtractor();
                ext.getInfo(saveFile, currentPlayer, player);
                loadSave();
            }
        }

        private void toolStripMenuItemSaveSlot3_Click(object sender, EventArgs e)
        {
            if (currentPlayer != 3)
            {
                currentPlayer = 1;
                toolStripMenuItemSaveSlot1.Checked = false;
                toolStripMenuItemSaveSlot2.Checked = false;

                var ext = new DataExtractor();
                ext.getInfo(saveFile, currentPlayer, player);
                loadSave();
            }
        }

        private void saveToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            packSaveFile();
            File.WriteAllBytes(filePath, saveFile);
            MessageBox.Show("File saved", "Saved !");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Ukee for GBATemp\nBased off APM's MHX/MHGen Save editor\nAlso thanks to Seth VanHeulen", "About");
        }

        public void loadSave()
        {
            // General Info
            charNameBox.Text = player.Name;
            numericUpDownTime.Value = player.PlayTime;
            numericUpDownFunds.Value = player.Funds;
            numericUpDownHR.Value = player.HunterRank;
            numericUpDownHRP.Value = player.HRPoints;
            numericUpDownWyc.Value = player.AcademyPoints;
            numericUpDownBhe.Value = player.BhernaPoints;
            numericUpDownKok.Value = player.KokotoPoints;
            numericUpDownPok.Value = player.PokkePoints;
            numericUpDownYuk.Value = player.YukumoPoints;

            TimeSpan time = TimeSpan.FromSeconds(player.PlayTime);
            labelConvTime.Text = "D.HH:MM:SS - " + time.ToString();

            // Player
            numericUpDownVoice.Value = Convert.ToInt32(player.Voice);
            numericUpDownEyeColor.Value = Convert.ToInt32(player.EyeColor);
            numericUpDownClothing.Value = Convert.ToInt32(player.Clothing);
            comboBoxGender.SelectedIndex = Convert.ToInt32(player.Gender);
            numericUpDownHair.Value = Convert.ToInt32(player.HairStyle);
            numericUpDownFace.Value = Convert.ToInt32(player.Face);
            numericUpDownFeatures.Value = Convert.ToInt32(player.Features);

            // Colors
            numericUpDownSkinR.Value = player.SkinColorRGBA[0];
            numericUpDownSkinG.Value = player.SkinColorRGBA[1];
            numericUpDownSkinB.Value = player.SkinColorRGBA[2];
            numericUpDownSkinA.Value = player.SkinColorRGBA[3];
            numericUpDownHairR.Value = player.HairColorRGBA[0];
            numericUpDownHairG.Value = player.HairColorRGBA[1];
            numericUpDownHairB.Value = player.HairColorRGBA[2];
            numericUpDownHairA.Value = player.HairColorRGBA[3];
            numericUpDownFeaturesR.Value = player.FeaturesColorRGBA[0];
            numericUpDownFeaturesG.Value = player.FeaturesColorRGBA[1];
            numericUpDownFeaturesB.Value = player.FeaturesColorRGBA[2];
            numericUpDownFeaturesA.Value = player.FeaturesColorRGBA[3];
            numericUpDownClothesR.Value = player.ClothingColorRGBA[0];
            numericUpDownClothesG.Value = player.ClothingColorRGBA[1];
            numericUpDownClothesB.Value = player.ClothingColorRGBA[2];
            numericUpDownClothesA.Value = player.ClothingColorRGBA[3];

            // Item Box
            for (int a = 0; a < 2300; a++)
            {
                string itemName = GameConstants.ItemNameList[Array.IndexOf(GameConstants.ItemIDList , Convert.ToInt32(player.itemId[a]))];
                string[] arr = new string[3];
                arr[0] = (a + 1).ToString();
                arr[1] = itemName;
                arr[2] = player.itemCount[a];
                ListViewItem itm = new ListViewItem(arr);
                listViewItem.Items.Add(itm);
            }
            listViewItem.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewItem.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            comboBoxItem.Items.AddRange(GameConstants.ItemNameList);

            // Equipment Box
            for (int a = 0; a < 2000; a++)
            {
                int eqID = Convert.ToInt32(player.equipmentInfo[(a * 36) + 1].ToString() + player.equipmentInfo[(a * 36) + 2].ToString());
                string equipType = GameConstants.EquipmentTypes[Convert.ToInt32(player.equipmentInfo[a * 36])];
                string eqName = "(None)";

                switch (Convert.ToInt32(player.equipmentInfo[a * 36]))
                {       
                    case 0:
                        break;
                    case 1:
                        eqName = GameConstants.EquipHeadNames[eqID];
                        break;
                    case 2:
                        eqName = GameConstants.EquipChestNames[eqID];
                        break;
                    case 3:
                        eqName = GameConstants.EquipArmsNames[eqID];
                        break;
                    case 4:
                        eqName = GameConstants.EquipWaistNames[eqID];
                        break;
                    case 5:
                        eqName = GameConstants.EquipLegsNames[eqID];
                        break;
                    case 6:
                        eqName = GameConstants.EquipTalismanNames[eqID];
                        break;
                    case 7:
                        eqName = GameConstants.EquipGreatSwordNames[eqID];
                        break;
                    case 8:
                        eqName = GameConstants.EquipSwordnShieldNames[eqID];
                        break;
                    case 9:
                        eqName = GameConstants.EquipHammerNames[eqID];
                        break;
                    case 10:
                        eqName = GameConstants.EquipLanceNames[eqID];
                        break;
                    case 11:
                        eqName = GameConstants.EquipHeavyBowgunNames[eqID];
                        break;
                    case 12:
                        break;
                    case 13:
                        eqName = GameConstants.EquipLightBowgunNames[eqID];
                        break;
                    case 14:
                        eqName = GameConstants.EquipLongswordNames[eqID];
                        break;
                    case 15:
                        eqName = GameConstants.EquipSwitchAxeNames[eqID];
                        break;
                    case 16:
                        eqName = GameConstants.EquipGunlanceNames[eqID];
                        break;
                    case 17:
                        eqName = GameConstants.EquipBowNames[eqID];
                        break;
                    case 18:
                        eqName = GameConstants.EquipDualBladesNames[eqID];
                        break;
                    case 19:
                        eqName = GameConstants.EquipHuntingHornNames[eqID];
                        break;
                    case 20:
                        eqName = GameConstants.EquipInsectGlaiveNames[eqID];
                        break;
                    case 21:
                        eqName = GameConstants.EquipChargeBladeNames[eqID];
                        break;
                }

                string[] arr = new string[3];
                arr[0] = (a + 1).ToString();
                arr[1] = equipType;
                arr[2] = eqName;
                ListViewItem itm = new ListViewItem(arr);
                listViewEquipment.Items.Add(itm);
            }
            listViewEquipment.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewEquipment.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            comboBoxEquipType.Items.AddRange(GameConstants.EquipmentTypes);
            comboBoxEquipDeco1.Items.AddRange(GameConstants.JwlNames);
            comboBoxEquipDeco2.Items.AddRange(GameConstants.JwlNames);
            comboBoxEquipDeco3.Items.AddRange(GameConstants.JwlNames);
        }

        public void packSaveFile()
        {
            // Char Name
            byte[] charNameByte = new byte[32];
            byte[] convName = Encoding.UTF8.GetBytes(charNameBox.Text);
            Array.Copy(convName, 0, charNameByte, 0, convName.Length);
            Array.Copy(charNameByte, 0, saveFile, player.SaveOffset, Constants.SIZEOF_NAME);

            // HR Points
            byte[] hrPoints = BitConverter.GetBytes((int)numericUpDownHRP.Value);
            Array.Copy(hrPoints, 0, saveFile, player.SaveOffset + Offsets.HR_POINTS_OFFSET, 4);

            // Funds
            byte[] funds = BitConverter.GetBytes((int)numericUpDownFunds.Value);
            Array.Copy(funds, 0, saveFile, player.SaveOffset + Offsets.FUNDS_OFFSET, 4);
            Array.Copy(funds, 0, saveFile, player.SaveOffset + Offsets.FUNDS_OFFSET2, 4);

            // Academy Points
            byte[] academyPoints = BitConverter.GetBytes((int)numericUpDownWyc.Value);
            Array.Copy(academyPoints, 0, saveFile, player.SaveOffset + Offsets.ACADEMY_POINTS_OFFSET, 4);

            // Village Points
            byte[] villagePoints = BitConverter.GetBytes((int)numericUpDownBhe.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.BHERNA_POINTS_OFFSET, 4);
            villagePoints = BitConverter.GetBytes((int)numericUpDownPok.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.POKKE_POINTS_OFFSET, 4);
            villagePoints = BitConverter.GetBytes((int)numericUpDownYuk.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.YUKUMO_POINTS_OFFSET, 4);
            villagePoints = BitConverter.GetBytes((int)numericUpDownKok.Value);
            Array.Copy(villagePoints, 0, saveFile, player.SaveOffset + Offsets.KOKOTO_POINTS_OFFSET, 4);

            // Play Time
            byte[] playTime = BitConverter.GetBytes((int)numericUpDownTime.Value);
            Array.Copy(playTime, 0, saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET, 4);
            Array.Copy(playTime, 0, saveFile, player.SaveOffset + Offsets.PLAY_TIME_OFFSET2, 4);

            // Character Faatures
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET2] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_GENDER_OFFSET3] = (byte)comboBoxGender.SelectedIndex;
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET] = (byte)numericUpDownVoice.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET2] = (byte)numericUpDownVoice.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_VOICE_OFFSET3] = (byte)numericUpDownVoice.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET] = (byte)numericUpDownEyeColor.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET2] = (byte)numericUpDownEyeColor.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_EYE_COLOR_OFFSET3] = (byte)numericUpDownEyeColor.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET] = (byte)numericUpDownClothing.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET2] = (byte)numericUpDownClothing.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_CLOTHING_OFFSET3] = (byte)numericUpDownClothing.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET] = (byte)numericUpDownHair.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET2] = (byte)numericUpDownHair.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_HAIRSTYLE_OFFSET3] = (byte)numericUpDownHair.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET] = (byte)numericUpDownFace.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET2] = (byte)numericUpDownFace.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FACE_OFFSET3] = (byte)numericUpDownFace.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET] = (byte)numericUpDownFeatures.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET2] = (byte)numericUpDownFeatures.Value;
            saveFile[player.SaveOffset + Offsets.CHARACTER_FEATURES_OFFSET3] = (byte)numericUpDownFeatures.Value;

            // Colors
            player.SkinColorRGBA[0] = (byte)numericUpDownSkinR.Value;
            player.SkinColorRGBA[1] = (byte)numericUpDownSkinG.Value;
            player.SkinColorRGBA[2] = (byte)numericUpDownSkinB.Value;
            player.SkinColorRGBA[3] = (byte)numericUpDownSkinA.Value;
            Array.Copy(player.SkinColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET, 4);
            Array.Copy(player.SkinColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET2, 4);
            Array.Copy(player.SkinColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_SKIN_COLOR_OFFSET3, 4);
            player.HairColorRGBA[0] = (byte)numericUpDownHairR.Value;
            player.HairColorRGBA[1] = (byte)numericUpDownHairG.Value;
            player.HairColorRGBA[2] = (byte)numericUpDownHairB.Value;
            player.HairColorRGBA[3] = (byte)numericUpDownHairA.Value;
            Array.Copy(player.HairColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET, 4);
            Array.Copy(player.HairColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET2, 4);
            Array.Copy(player.HairColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_HAIR_COLOR_OFFSET3, 4);
            player.FeaturesColorRGBA[0] = (byte)numericUpDownFeaturesR.Value;
            player.FeaturesColorRGBA[1] = (byte)numericUpDownFeaturesG.Value;
            player.FeaturesColorRGBA[2] = (byte)numericUpDownFeaturesB.Value;
            player.FeaturesColorRGBA[3] = (byte)numericUpDownFeaturesA.Value;
            Array.Copy(player.FeaturesColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET, 4);
            Array.Copy(player.FeaturesColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET2, 4);
            Array.Copy(player.FeaturesColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_FEATURES_COLOR_OFFSET3, 4);
            player.ClothingColorRGBA[0] = (byte)numericUpDownClothesR.Value;
            player.ClothingColorRGBA[1] = (byte)numericUpDownClothesG.Value;
            player.ClothingColorRGBA[2] = (byte)numericUpDownClothesB.Value;
            player.ClothingColorRGBA[3] = (byte)numericUpDownClothesA.Value;
            Array.Copy(player.ClothingColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET, 4);
            Array.Copy(player.ClothingColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET2, 4);
            Array.Copy(player.ClothingColorRGBA, 0, saveFile, player.SaveOffset + Offsets.CHARACTER_CLOTHING_COLOR_OFFSET3, 4);

            // Item Box
            string itemBinary = "0000"; // Add back the '0000' that was removed in data extraction

            foreach (ListViewItem i in listViewItem.Items)
            {
                int iteration = Convert.ToInt32(i.SubItems[0].Text) - 1;
                
                player.itemId[iteration] = (GameConstants.ItemIDList[Array.IndexOf(GameConstants.ItemNameList, i.SubItems[1].Text)]).ToString();
                player.itemCount[iteration] = i.SubItems[2].Text;
            }
            for (int a = 2299; a >= 0; a--)
            {
                itemBinary += Convert.ToString(Convert.ToInt32(player.itemCount[a]), 2).PadLeft(7, '0');
                itemBinary += Convert.ToString(Convert.ToInt32(player.itemId[a]), 2).PadLeft(12, '0');
            }
            var byteArray = Enumerable.Range(0, int.MaxValue / 8)
                          .Select(i => i * 8)    // get the starting index of which char segment
                          .TakeWhile(i => i < itemBinary.Length)
                          .Select(i => itemBinary.Substring(i, 8)) // get the binary string segments
                          .Select(s => Convert.ToByte(s, 2)) // convert to byte
                          .ToArray();

            Array.Reverse(byteArray);
            Array.Copy(byteArray, 0, saveFile, player.SaveOffset + Offsets.ITEM_BOX_OFFSET, byteArray.Length);
        }

        private void listViewItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                itemSelectedSlot = Convert.ToInt32(listViewItem.SelectedItems[0].SubItems[0].Text) - 1;
                numericUpDownItemID.Value = Convert.ToInt32(player.itemId[itemSelectedSlot]);
                numericUpDownItemAmount.Value = Convert.ToInt32(player.itemCount[itemSelectedSlot]);
                comboBoxItem.SelectedIndex = Array.IndexOf(GameConstants.ItemIDList, Convert.ToInt32(player.itemId[itemSelectedSlot]));
            }
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                int index = GameConstants.ItemIDList[Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text)];
                numericUpDownItemID.Value = index;
                listViewItem.SelectedItems[0].SubItems[1].Text = GameConstants.ItemNameList[Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text)];

                if (listViewItem.SelectedItems[0].SubItems[2].Text == "0" && listViewItem.SelectedItems[0].SubItems[1].Text != "-----")
                {
                    numericUpDownItemAmount.Value = 1;
                    listViewItem.SelectedItems[0].SubItems[2].Text = "1";
                }
                if (comboBoxItem.Text == "-----")
                {
                    numericUpDownItemAmount.Value = 0;
                    numericUpDownItemID.Value = 0;
                    listViewItem.SelectedItems[0].SubItems[2].Text = "0";

                }

                player.itemCount[itemSelectedSlot] = listViewItem.SelectedItems[0].SubItems[2].Text;
                player.itemId[itemSelectedSlot] = GameConstants.ItemIDList[Array.IndexOf(GameConstants.ItemNameList, comboBoxItem.Text)].ToString();
            }
        }

        private void numericUpDownItemAmount_ValueChanged(object sender, EventArgs e)
        {
            if (listViewItem.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                if (numericUpDownItemAmount.Value == 0)
                {
                    listViewItem.SelectedItems[0].SubItems[1].Text = "-----";
                    listViewItem.SelectedItems[0].SubItems[2].Text = "0";
                    numericUpDownItemID.Value = 0;
                    comboBoxItem.SelectedIndex = 0;
                    player.itemCount[itemSelectedSlot] = "0";
                    player.itemId[itemSelectedSlot] = "0";
                }
                else if (listViewItem.SelectedItems[0].SubItems[1].Text == "-----")
                {
                    listViewItem.SelectedItems[0].SubItems[2].Text = "0";
                    player.itemCount[itemSelectedSlot] = "0";
                    numericUpDownItemAmount.Value = 0;
                }
                else
                {
                    listViewItem.SelectedItems[0].SubItems[2].Text = numericUpDownItemAmount.Value.ToString();
                    player.itemCount[itemSelectedSlot] = numericUpDownItemAmount.Value.ToString();
                }
            }
        }

        private void numericUpDownTime_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds((double)numericUpDownTime.Value);
            labelConvTime.Text = "D.HH:MM:SS - " + time.ToString();
        }

        private void comboBoxEquipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                comboBoxEquipName.Items.Clear();
                numericUpDownEquipLevel.Value = 1;
                comboBoxEquipDeco1.SelectedIndex = 0;
                comboBoxEquipDeco2.SelectedIndex = 0;
                comboBoxEquipDeco3.SelectedIndex = 0;

                if (comboBoxEquipType.SelectedIndex == 0 || comboBoxEquipType.SelectedIndex == 12)
                {
                    comboBoxEquipName.Items.Clear();
                    comboBoxEquipName.Items.Add("(None)");
                    comboBoxEquipName.Enabled = false;
                    comboBoxEquipDeco1.Enabled = false;
                    comboBoxEquipDeco2.Enabled = false;
                    comboBoxEquipDeco3.Enabled = false;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                }
                else if (comboBoxEquipType.SelectedIndex == 20)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = true;
                }
                else if (comboBoxEquipType.SelectedIndex == 6)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditTalisman.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                }
                else
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                }

                switch (comboBoxEquipType.SelectedIndex)
                {
                    case 1:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHeadNames);
                        break;
                    case 2:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipChestNames);
                        break;
                    case 3:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipArmsNames);
                        break;
                    case 4:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipWaistNames);
                        break;
                    case 5:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLegsNames);
                        break;
                    case 6:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipTalismanNames);
                        break;
                    case 7:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipGreatSwordNames);
                        break;
                    case 8:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipSwordnShieldNames);
                        break;
                    case 9:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHammerNames);
                        break;
                    case 10:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLanceNames);
                        break;
                    case 11:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHeavyBowgunNames);
                        break;
                    case 13:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLightBowgunNames);
                        break;
                    case 14:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLongswordNames);
                        break;
                    case 15:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipSwitchAxeNames);
                        break;
                    case 16:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipGunlanceNames);
                        break;
                    case 17:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipBowNames);
                        break;
                    case 18:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipDualBladesNames);
                        break;
                    case 19:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHuntingHornNames);
                        break;
                    case 20:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipInsectGlaiveNames);
                        break;
                    case 21:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipChargeBladeNames);
                        break;
                }
                if (comboBoxEquipType.SelectedIndex == 0 || comboBoxEquipType.SelectedIndex == 12)
                    comboBoxEquipName.SelectedIndex = 0;
                else
                    comboBoxEquipName.SelectedIndex = 1;
                listViewEquipment.SelectedItems[0].SubItems[1].Text = comboBoxEquipType.Text;
                listViewEquipment.SelectedItems[0].SubItems[2].Text = comboBoxEquipName.Text;

                player.equipmentInfo[equipSelectedSlot * 36] = (byte) comboBoxEquipType.SelectedIndex;
                player.equipmentInfo[(equipSelectedSlot * 36) + 1] = 0;
                player.equipmentInfo[(equipSelectedSlot * 36) + 2] = 1;
                for (int a = 3; a < 36; a++)
                {
                    player.equipmentInfo[(equipSelectedSlot * 36) + a] = 0;
                }
            }
        }

        private void comboBoxEquipName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                listViewEquipment.SelectedItems[0].SubItems[2].Text = comboBoxEquipName.Text;
                byte[] idBytes = BitConverter.GetBytes(comboBoxEquipName.SelectedIndex);
                player.equipmentInfo[(equipSelectedSlot * 36) + 1] = idBytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 2] = idBytes[0];
            }
        }

        private void comboBoxEquipDeco1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                byte[] idBytes = BitConverter.GetBytes(GameConstants.JwlIDs[comboBoxEquipDeco1.SelectedIndex]);
                player.equipmentInfo[(equipSelectedSlot * 36) + 7] = idBytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 6] = idBytes[0];
            }
        }

        private void comboBoxEquipDeco2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                byte[] idBytes = BitConverter.GetBytes(GameConstants.JwlIDs[comboBoxEquipDeco2.SelectedIndex]);
                player.equipmentInfo[(equipSelectedSlot * 36) + 9] = idBytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 8] = idBytes[0];
            }
        }

        private void comboBoxEquipDeco3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    return;
                }

                byte[] idBytes = BitConverter.GetBytes(GameConstants.JwlIDs[comboBoxEquipDeco3.SelectedIndex]);
                player.equipmentInfo[(equipSelectedSlot * 36) + 11] = idBytes[1];
                player.equipmentInfo[(equipSelectedSlot * 36) + 10] = idBytes[0];
            }
        }

        private void buttonEditKinsect_Click(object sender, EventArgs e)
        {
            EditKinsectDialog editKinsect = new EditKinsectDialog(this, listViewEquipment.SelectedItems[0].SubItems[2].Text);
            MessageBox.Show("Edit at your own risk", "WARNING");
            editKinsect.ShowDialog();
            editKinsect.Dispose();
        }

        private void maxAmountItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listViewItem.Items)
            {
                if (!i.SubItems[1].Text.Contains("-----"))
                {
                    i.SubItems[2].Text = "99";
                }
            }
        }

        private void buttonEditTalisman_Click(object sender, EventArgs e)
        {
            EditTalismanDialog editKinsect = new EditTalismanDialog(this, listViewEquipment.SelectedItems[0].SubItems[2].Text);
            MessageBox.Show("Edit at your own risk", "WARNING");
            editKinsect.ShowDialog();
            editKinsect.Dispose();
        }

        private void listViewEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquipment.SelectedItems.Count == 0) // Check if nothing was selected
                return;
            else
            {
                ListView ls = (ListView)sender;
                if (!ls.Focused)
                {
                    return;
                }

                comboBoxEquipName.Items.Clear();
                comboBoxEquipType.Enabled = true;

                equipSelectedSlot = Convert.ToInt32(listViewEquipment.SelectedItems[0].SubItems[0].Text) - 1;
                comboBoxEquipType.SelectedIndex = Convert.ToInt32(player.equipmentInfo[equipSelectedSlot * 36]);

                int eqID = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 1].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 2].ToString("X2"), 16);
                int eqLevel = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 3]);
                int deco1 = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 7].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 6].ToString("X2"), 16);
                int deco2 = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 9].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 8].ToString("X2"), 16);
                int deco3 = Convert.ToInt32(player.equipmentInfo[(equipSelectedSlot * 36) + 11].ToString("X2") + player.equipmentInfo[(equipSelectedSlot * 36) + 10].ToString("X2"), 16);

                if (comboBoxEquipType.SelectedIndex == 0 || comboBoxEquipType.SelectedIndex == 12)
                {
                    comboBoxEquipName.Items.Clear();
                    comboBoxEquipName.Text = "(None)";
                    comboBoxEquipName.Enabled = false;
                    comboBoxEquipDeco1.Enabled = false;
                    comboBoxEquipDeco2.Enabled = false;
                    comboBoxEquipDeco3.Enabled = false;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                }
                else if (comboBoxEquipType.SelectedIndex == 20)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = true;
                    buttonEditTalisman.Enabled = false;
                }
                else if (comboBoxEquipType.SelectedIndex == 6)
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditTalisman.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                }
                else
                {
                    comboBoxEquipName.Enabled = true;
                    comboBoxEquipDeco1.Enabled = true;
                    comboBoxEquipDeco2.Enabled = true;
                    comboBoxEquipDeco3.Enabled = true;
                    buttonEditKinsect.Enabled = false;
                    buttonEditTalisman.Enabled = false;
                }

                switch (Convert.ToInt32(player.equipmentInfo[equipSelectedSlot * 36]))
                {
                    case 1:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHeadNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 2:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipChestNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 3:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipArmsNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 4:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipWaistNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 5:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLegsNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 6:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipTalismanNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 7:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipGreatSwordNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 8:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipSwordnShieldNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 9:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHammerNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 10:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLanceNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 11:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHeavyBowgunNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 13:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLightBowgunNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 14:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipLongswordNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 15:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipSwitchAxeNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 16:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipGunlanceNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 17:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipBowNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 18:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipDualBladesNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 19:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipHuntingHornNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 20:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipInsectGlaiveNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                    case 21:
                        comboBoxEquipName.Items.AddRange(GameConstants.EquipChargeBladeNames);
                        comboBoxEquipName.SelectedIndex = eqID;
                        break;
                }

                numericUpDownEquipLevel.Value = eqLevel + 1;
                comboBoxEquipDeco1.SelectedIndex = Array.IndexOf(GameConstants.JwlIDs ,deco1);
                comboBoxEquipDeco2.SelectedIndex = Array.IndexOf(GameConstants.JwlIDs, deco2);
                comboBoxEquipDeco3.SelectedIndex = Array.IndexOf(GameConstants.JwlIDs, deco3);
            }
        }
    }
}
