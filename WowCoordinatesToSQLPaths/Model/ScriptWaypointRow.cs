namespace WowCoordinatesToSQLPaths.Model
{
    class ScriptWaypointRow
    {
        public int Entry { get; set; }

        public int PointId { get; set; }

        public float LocationX { get; set; }

        public float LocatoinY { get; set; }

        public float LocatoinZ { get; set; }

        public int WaitTime { get; set; }

        public string Comment { get; set; }
    }
}
