namespace WowCoordinatesToSQLPaths.Model
{
    class CreateMovementRowBase
    {
        public int Point { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        public int WaitTime { get; set; }

        public int ScriptId { get; set; }

        public int TextId1 { get; set; }

        public int TextId2 { get; set; }

        public int TextId3 { get; set; }

        public int TextId4 { get; set; }

        public int TextId5 { get; set; }

        public int Emote { get; set; }

        public int Spell { get; set; }

        public float Orientation { get; set; }

        public int Model1 { get; set; }

        public int Model2 { get; set; }
    }
}