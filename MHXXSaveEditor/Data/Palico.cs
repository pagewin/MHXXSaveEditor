using System;
using static MHXXSaveEditor.Data.PalicoOffsets;
using static MHXXSaveEditor.Util.StringUtils;

namespace MHXXSaveEditor.Data
{
    public class Palico
    {

        /* Fill a given array starting at index 0.
         * Offsets and sizes should be taken from Data.PalicoOffsets.
         */
        private void CopyFromOriginalData(byte[] targetArray, int offset) {
            Array.Copy(OriginalData, offset, targetArray, 0, targetArray.Length);
        }

        /* Finalize any changes made by rewriting the original byte array */
        private void OverwriteOriginalData(byte[] sourceArray, int offset) {
            Array.Copy(sourceArray, 0, OriginalData, offset, sourceArray.Length);
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

        private void SaveNewData() {
            OverwriteOriginalData(Name, NAME);
            OverwriteOriginalData(Namegiver, NAMEGIVER);
            OverwriteOriginalData(LastMaster, LAST_MASTER);
            OverwriteOriginalData(Greeting, GREETING);
            OverwriteOriginalData(UniqueId, PALICO_ID);
            OverwriteOriginalData(ScoutId, SCOUT_ID);
            OverwriteOriginalData(OriginalOwner, ORIGINAL_OWNER_ID);

            OverwriteOriginalData(Level, LEVEL);
            OverwriteOriginalData(Exp, EXP);
            OverwriteOriginalData(Enthusiasm, ENTHUSIASM);
            OverwriteOriginalData(BiasBytes, BIAS);
            OverwriteOriginalData(Target, TARGET);
            OverwriteOriginalData(Status, STATUS);
            OverwriteOriginalData(Training, TRAINING);
            OverwriteOriginalData(Job, JOB);
            OverwriteOriginalData(Prowler, PROWLER);

            OverwriteOriginalData(SupportMoveRng, SUPPORT_MOVE_RNG);
            OverwriteOriginalData(SkillRng, SKILL_RNG);
            OverwriteOriginalData(EquippedSupportMoves, EQUIPPED_SUPPORT_MOVES);
            OverwriteOriginalData(EquippedSkills, EQUIPPED_SKILLS);
            OverwriteOriginalData(KnownSupportMoves, KNOWN_SUPPORT_MOVES);
            OverwriteOriginalData(KnownSkills, KNOWN_SKILLS);

            /* editing palico gear is not implemented
            OverwriteOriginalData(EquippedArmor, ARMOR);
            OverwriteOriginalData(EquippedWeapon, WEAPON);
            */

            OverwriteOriginalData(CoatStyle, COAT);
            OverwriteOriginalData(EarStyle, EARS);
            OverwriteOriginalData(TailStyle, TAIL);
            OverwriteOriginalData(ClothingStyle, CLOTHING);
            OverwriteOriginalData(CoatColor, COAT_COLOR);
            OverwriteOriginalData(LeftEyeColor, LEFT_EYE_COLOR);
            OverwriteOriginalData(RightEyeColor, RIGHT_EYE_COLOR);
            OverwriteOriginalData(ClothingColor, CLOTHING_COLOR);
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

            CopyFromOriginalData(Name, NAME);
            CopyFromOriginalData(Namegiver, NAMEGIVER);
            CopyFromOriginalData(LastMaster, LAST_MASTER);
            CopyFromOriginalData(Greeting, GREETING);
        }

        private void CopyIds() {
            OriginalOwner = new byte[SIZEOF_ORIGINAL_OWNER_ID];
            ScoutId = new byte[SIZEOF_SCOUT_ID];
            UniqueId = new byte[SIZEOF_PALICO_ID];

            CopyFromOriginalData(OriginalOwner, ORIGINAL_OWNER_ID);
            CopyFromOriginalData(ScoutId, SCOUT_ID);
            CopyFromOriginalData(UniqueId, PALICO_ID);
        }

        /* Level, exp, enthusiasm, target, bias */
        private void CopyGeneralStats() {
            Level = new byte[SIZEOF_LEVEL];
            Exp = new byte[SIZEOF_EXP];
            Enthusiasm = new byte[SIZEOF_ENTHUSIASM];
            Target = new byte[SIZEOF_TARGET];
            BiasBytes = new byte[SIZEOF_BIAS];

            CopyFromOriginalData(Level, LEVEL);
            CopyFromOriginalData(Exp, EXP);
            CopyFromOriginalData(Enthusiasm, ENTHUSIASM);
            CopyFromOriginalData(Target, TARGET);
            CopyFromOriginalData(BiasBytes, BIAS);
        }

        private void SetBiasType() {
            Bias = (BiasType) Convert.ToByte(BiasBytes[0]);
            Charisma = (Bias == BiasType.Charisma);
        }

        private void CopySkills() {
            SkillRng = new byte[SIZEOF_RNG];
            CopyFromOriginalData(SkillRng, SKILL_RNG);
        }

        private void SetSkillRng() {
            SelectedSkillRng = Array.IndexOf(GameConstants.PalicoSkillRNG, BytesToHexString(SkillRng));
            MaximumSkills = 4 + (GameConstants.PalicoSkillRNGAbbv[SelectedSkillRng].Length);
            SkillRngList = GameConstants.PalicoSkillRNGAbbv;
        }

        private void CopySupportMoves() {
            SupportMoveRng = new byte[SIZEOF_RNG];
            CopyFromOriginalData(SupportMoveRng, SUPPORT_MOVE_RNG);
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

            CopyFromOriginalData(EyeStyle, EYES);
            CopyFromOriginalData(EarStyle, EARS);
            CopyFromOriginalData(CoatStyle, COAT);
            CopyFromOriginalData(TailStyle, TAIL);
            CopyFromOriginalData(Voice, VOICE);
            CopyFromOriginalData(ClothingStyle, CLOTHING);
        }

        private void CopyColors() {
            LeftEyeColor = new byte[SIZEOF_COLOR];
            RightEyeColor = new byte[SIZEOF_COLOR];
            CoatColor = new byte[SIZEOF_COLOR];
            ClothingColor = new byte[SIZEOF_COLOR];

            CopyFromOriginalData(LeftEyeColor, LEFT_EYE_COLOR);
            CopyFromOriginalData(RightEyeColor, RIGHT_EYE_COLOR);
            CopyFromOriginalData(CoatColor, COAT_COLOR);
            CopyFromOriginalData(ClothingColor, CLOTHING_COLOR);
        }

        private void CopyStatus() {
            Status = new byte[SIZEOF_STATUS];
            Training = new byte[SIZEOF_STATUS];
            Job = new byte[SIZEOF_STATUS];
            Prowler = new byte[SIZEOF_STATUS];

            CopyFromOriginalData(Status, STATUS);
            CopyFromOriginalData(Training, TRAINING);
            CopyFromOriginalData(Job, JOB);
            CopyFromOriginalData(Prowler, PROWLER);
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

            byte[] newStatus = { (byte) (Status[0] + DLC_STATUS) };
            Array.Copy(newStatus, Status, SIZEOF_STATUS);

            /* scout id? */
        }

        /* Can't store the real originals anywhere, so just assume the current hunter is the namegiver and orig owner */
        private void UnsetDlc(Player player) { 
            string id = player.ShortGuildId;
            byte[] idBytes = HexStringToBytes(id);
            Array.Copy(idBytes, OriginalOwner, SIZEOF_ORIGINAL_OWNER_ID);
            Array.Copy(idBytes, Namegiver, SIZEOF_ORIGINAL_OWNER_ID);

            byte[] newStatus = { (byte)(Status[0] - DLC_STATUS) };
            Array.Copy(newStatus, Status, SIZEOF_STATUS);

            /* scout id? */
        }
    }
}
