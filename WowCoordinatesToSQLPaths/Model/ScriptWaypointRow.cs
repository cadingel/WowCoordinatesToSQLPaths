namespace WowCoordinatesToSQLPaths.Model
{
    class ScriptWaypointRow
    {
        public int Entry { get; set; }

        public int PointId { get; set; }

        public float LocationX { get; set; }

        public float LocationY { get; set; }

        public float LocationZ { get; set; }

        public int WaitTime { get; set; }

        public string Comment { get; set; } = "";
    }
}
