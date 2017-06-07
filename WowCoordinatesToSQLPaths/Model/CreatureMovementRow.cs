using System;

namespace WowCoordinatesToSQLPaths.Model
{
    class CreatureMovementRow : CreateMovementRowBase, ICloneable
    {
        public int Id { get; set; }

        public object Clone()
        {
            CreatureMovementRow createMovementRow = new CreatureMovementRow();
            createMovementRow.Id = Id;
            createMovementRow.Point = Point;
            createMovementRow.PositionX = PositionX;
            createMovementRow.PositionY = PositionY;
            createMovementRow.PositionZ = PositionZ;
            createMovementRow.WaitTime = WaitTime;
            createMovementRow.ScriptId = ScriptId;
            createMovementRow.TextId1 = TextId1;
            createMovementRow.TextId2 = TextId2;
            createMovementRow.TextId3 = TextId3;
            createMovementRow.TextId4 = TextId4;
            createMovementRow.TextId5 = TextId5;
            createMovementRow.Emote = Emote;
            createMovementRow.Spell = Spell;
            createMovementRow.Orientation = Orientation;
            createMovementRow.Model1 = Model1;
            createMovementRow.Model2 = Model2;

            return createMovementRow;
        }
    }
}