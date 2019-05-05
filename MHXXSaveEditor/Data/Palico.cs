using System;
using System.Text;
using static MHXXSaveEditor.Data.PalicoOffsets;

namespace MHXXSaveEditor.Data
{
    public class Palico
    {
        /* Bytes --> hexadecimal string */
        public string BytesToHexString(byte[] bytes) {
            string str = "";
            foreach (var bit in bytes) {
                str += bit.ToString("X2");
            }
            return str;
        }

        /* Bytes --> readable UTF-8 string */
        public string BytesToUtf8String(byte[] bytes) {
            return Encoding.UTF8.GetString(bytes);
        }

        /* Bytes --> RGBA as hexadecimal string */
        public string ColorBytesToString(byte[] rgba) {
            return BitConverter.ToString(rgba).Replace("-", string.Empty);
        }

        /* Fill a given array starting at index 0.
         * Offsets and sizes should be taken from Data.PalicoOffsets.
         */
        private void CopyFromOriginalData(byte[] targetArray, int size, int offset) {
            Array.Copy(OriginalData, offset, targetArray, 0, targetArray.Length);
        }


        public int SelectedPalico { get; set; }
        public byte[] OriginalData { get; set; }

        public bool DLC { get; set; }
        private BiasType Bias { get; set; }
        private bool Charisma { get; set; }

        public byte[] Name { get; set; }
        public byte[] Level { get; set; }
        public byte[] Exp { get; set; }
        public byte[] Enthusiasm { get; set; }
        public byte[] BiasBytes { get; set; }
        public byte[] Target { get; set; }

        public byte[] LastMaster { get; set; }
        public byte[] Namegiver { get; set; }
        public byte[] Greeting { get; set; }

        public byte[] UniqueId { get; set; }
        public byte[] ScoutId { get; set; }
        public byte[] OriginalOwner { get; set; }

        public byte[] SupportMoveRng { get; set; }
        public byte[] SkillRng { get; set; }
        public string[] SupportMoveRngList { get; set; }
        public string[] SkillRngList { get; set; }
        public int SelectedSupportMoveRng { get; set; }
        public int SelectedSkillRng { get; set; }
        private int MaximumSupportMoves { get; set; }
        private int MaximumSkills { get; set; }

        public byte[] CoatStyle { get; set; }
        public byte[] EarStyle { get; set; }
        public byte[] EyeStyle { get; set; }
        public byte[] TailStyle { get; set; }
        public byte[] ClothingStyle { get; set; }
        public byte[] Voice { get; set; }

        public byte[] CoatColor { get; set; }
        public byte[] LeftEyeColor { get; set; }
        public byte[] RightEyeColor { get; set; }
        public byte[] ClothingColor { get; set; }

        public byte[] Status { get; set; }
        public byte[] Training { get; set; }
        public byte[] Job { get; set; }
        public byte[] Prowler { get; set; }

        public byte[] EquippedSupportMoves { get; set; }
        public byte[] EquippedSkills { get; set; }
        public byte[] KnownSupportMoves { get; set; }
        public byte[] KnownSkills { get; set; }

        public byte[] EquippedWeapon { get; set; }
        public byte[] EquippedArmor { get; set; }


        public Palico(byte[] palicoData, int slot) {
            SelectedPalico = slot * Constants.SIZEOF_PALICO;
            CopyOriginalData(palicoData);

            CopyNames();
            CopyIds();
            CopyGeneralStats();
            SetBiasType();

            CopySkills();
            SetSkillRng();

            CopySupportMoves();
            SetSupportMoveRng();

            CopyDesign();
            CopyColors();
            CopyStatus();
        }

        /* Keep a copy of the original data until save time */
        private void CopyOriginalData(byte[] palicoData) {
            OriginalData = new byte[Constants.SIZEOF_PALICO];
            Buffer.BlockCopy(palicoData, SelectedPalico, OriginalData, 0, OriginalData.Length);
        }

        private void CopyNames() {
            Name = new byte[SIZEOF_NAME];
            Namegiver = new byte[SIZEOF_NAME];
            LastMaster = new byte[SIZEOF_NAME];
            Greeting = new byte[SIZEOF_GREETING];

            CopyFromOriginalData(Name, SIZEOF_NAME, NAME);
            CopyFromOriginalData(Namegiver, SIZEOF_NAME, NAMEGIVER);
            CopyFromOriginalData(LastMaster, SIZEOF_NAME, LAST_MASTER);
            CopyFromOriginalData(Greeting, SIZEOF_GREETING, GREETING);
        }

        private void CopyIds() {
            OriginalOwner = new byte[SIZEOF_ORIGINAL_OWNER_ID];
            ScoutId = new byte[SIZEOF_SCOUT_ID];
            UniqueId = new byte[SIZEOF_PALICO_ID];

            CopyFromOriginalData(OriginalOwner, SIZEOF_NAME, ORIGINAL_OWNER_ID);
            CopyFromOriginalData(ScoutId, SIZEOF_SCOUT_ID, SCOUT_ID);
            CopyFromOriginalData(UniqueId, SIZEOF_PALICO_ID, PALICO_ID);
        }

        /* Level, exp, enthusiasm, target, bias */
        private void CopyGeneralStats() {
            Level = new byte[SIZEOF_LEVEL];
            Exp = new byte[SIZEOF_EXP];
            Enthusiasm = new byte[SIZEOF_ENTHUSIASM];
            Target = new byte[SIZEOF_TARGET];
            BiasBytes = new byte[SIZEOF_BIAS];

            CopyFromOriginalData(Level, SIZEOF_LEVEL, LEVEL);
            CopyFromOriginalData(Exp, SIZEOF_EXP, EXP);
            CopyFromOriginalData(Enthusiasm, SIZEOF_ENTHUSIASM, ENTHUSIASM);
            CopyFromOriginalData(Target, SIZEOF_TARGET, TARGET);
            CopyFromOriginalData(BiasBytes, SIZEOF_BIAS, BIAS);
        }

        private void SetBiasType() {
            Bias = (BiasType) Convert.ToByte(BiasBytes[0]);
            Charisma = (Bias == BiasType.Charisma);
        }

        private void CopySkills() {
            SkillRng = new byte[SIZEOF_RNG];
            CopyFromOriginalData(SkillRng, SIZEOF_RNG, SKILL_RNG);
        }

        private void SetSkillRng() {
            SelectedSkillRng = Array.IndexOf(GameConstants.PalicoSkillRNG, BytesToHexString(SkillRng));
            MaximumSkills = 4 + (GameConstants.PalicoSkillRNGAbbv[SelectedSkillRng].Length);
            SkillRngList = GameConstants.PalicoSkillRNGAbbv;
        }

        private void CopySupportMoves() {
            SupportMoveRng = new byte[SIZEOF_RNG];
            CopyFromOriginalData(SupportMoveRng, SIZEOF_RNG, SUPPORT_MOVE_RNG);
        }

        private void SetSupportMoveRng() {
            if (Charisma) {
                SupportMoveRngList = GameConstants.PalicoCharismaSupportMoveRNGAbbv;
                SelectedSupportMoveRng = Array.IndexOf(GameConstants.PalicoCharismaSupportMoveRNG, BytesToHexString(SupportMoveRng));
                MaximumSupportMoves = 6 + GameConstants.PalicoCharismaSupportMoveRNGAbbv[SelectedSupportMoveRng].Length;
            } else {
                SupportMoveRngList = GameConstants.PalicoSupportMoveRNGAbbv;
                SelectedSupportMoveRng = Array.IndexOf(GameConstants.PalicoSupportMoveRNG, BytesToHexString(SupportMoveRng));
                MaximumSupportMoves = 6 + GameConstants.PalicoSupportMoveRNGAbbv[SelectedSupportMoveRng].Length;
            }
        }

        private void CopyDesign() {
            EyeStyle = new byte[SIZEOF_DESIGN];
            EarStyle = new byte[SIZEOF_DESIGN];
            CoatStyle = new byte[SIZEOF_DESIGN];
            TailStyle = new byte[SIZEOF_DESIGN];
            Voice = new byte[SIZEOF_DESIGN];
            ClothingStyle = new byte[SIZEOF_DESIGN];

            CopyFromOriginalData(EyeStyle, SIZEOF_DESIGN, EYES);
            CopyFromOriginalData(EarStyle, SIZEOF_DESIGN, EARS);
            CopyFromOriginalData(CoatStyle, SIZEOF_DESIGN, COAT);
            CopyFromOriginalData(TailStyle, SIZEOF_DESIGN, TAIL);
            CopyFromOriginalData(Voice, SIZEOF_DESIGN, VOICE);
            CopyFromOriginalData(ClothingStyle, SIZEOF_DESIGN, CLOTHING);
        }

        private void CopyColors() {
            LeftEyeColor = new byte[SIZEOF_COLOR];
            RightEyeColor = new byte[SIZEOF_COLOR];
            CoatColor = new byte[SIZEOF_COLOR];
            ClothingColor = new byte[SIZEOF_COLOR];

            CopyFromOriginalData(LeftEyeColor, SIZEOF_COLOR, LEFT_EYE_COLOR);
            CopyFromOriginalData(RightEyeColor, SIZEOF_COLOR, RIGHT_EYE_COLOR);
            CopyFromOriginalData(CoatColor, SIZEOF_COLOR, COAT_COLOR);
            CopyFromOriginalData(ClothingColor, SIZEOF_COLOR, CLOTHING_COLOR);
        }

        private void CopyStatus() {
            Status = new byte[SIZEOF_STATUS];
            Training = new byte[SIZEOF_STATUS];
            Job = new byte[SIZEOF_STATUS];
            Prowler = new byte[SIZEOF_STATUS];

            CopyFromOriginalData(Status, SIZEOF_STATUS, STATUS);
            CopyFromOriginalData(Training, SIZEOF_STATUS, TRAINING);
            CopyFromOriginalData(Job, SIZEOF_STATUS, JOB);
            CopyFromOriginalData(Prowler, SIZEOF_STATUS, PROWLER);
        }

        private bool CheckDlc() {
            return (Status[0] >= 128 // SIZEOF_STATUS is 1, so a single-element array
                    && OriginalOwner == DLC_MASTER
                    && Namegiver == DLC_MASTER);
        }

        /* making assumptions here; maybe this doesn't work */
        /* One-way operation -- warn user before applying in Form */
        private void SetDlc() {
            Array.Copy(DLC_MASTER, OriginalOwner, SIZEOF_ORIGINAL_OWNER_ID);
            Array.Copy(DLC_MASTER, Namegiver, SIZEOF_ORIGINAL_OWNER_ID);
            byte[] newStatus = { (byte) (Status[0] + DLC_STATUS) }; // why does C# cast bytes to int before addition instead of having + operator for bytes?
            Array.Copy(newStatus, Status, SIZEOF_STATUS);
        }

        /* Can't store the real originals anywhere, so just assume the current hunter is the namegiver and orig owner */
        private void UnsetDlc() {
            /* fix hunter stuff, guild card id is hidden in magic numbers in EditGuildCardDialog.cs */
        }
    }
}
