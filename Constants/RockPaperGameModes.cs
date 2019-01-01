namespace Constants
{
    using System.Collections.Generic;
    using System.Linq;

    public static class RockPaperGameModes
    {
        public const string HumanvsHuman = "Human vs Human";
        public const string HumanvsComputer = "Human vs Computer";
        public const string HumanvsSuperComputer = "Human vs Super Computer";

        public static List<string> GetAll()
        {
            return (from d in typeof(RockPaperGameModes).GetFields() select d.GetRawConstantValue().ToString()).ToList();
        }
    }
}
