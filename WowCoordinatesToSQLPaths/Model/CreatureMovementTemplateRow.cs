using System;

namespace WowCoordinatesToSQLPaths.Model
{
    class CreatureMovementTemplateRow : CreateMovementRowBase, ICloneable
    {
        public int Entry { get; set; }

        public int PathId { get; set; }

        public object Clone()
        {
            CreatureMovementTemplateRow creatureMovementTemplateRow = new CreatureMovementTemplateRow();
            creatureMovementTemplateRow.Entry = Entry;
            creatureMovementTemplateRow.PathId = PathId;
            creatureMovementTemplateRow.Point = Point;
            creatureMovementTemplateRow.PositionX = PositionX;
            creatureMovementTemplateRow.PositionY = PositionY;
            creatureMovementTemplateRow.PositionZ = PositionZ;
            creatureMovementTemplateRow.WaitTime = WaitTime;
            creatureMovementTemplateRow.ScriptId = ScriptId;
            creatureMovementTemplateRow.TextId1 = TextId1;
            creatureMovementTemplateRow.TextId2 = TextId2;
            creatureMovementTemplateRow.TextId3 = TextId3;
            creatureMovementTemplateRow.TextId4 = TextId4;
            creatureMovementTemplateRow.TextId5 = TextId5;
            creatureMovementTemplateRow.Emote = Emote;
            creatureMovementTemplateRow.Spell = Spell;
            creatureMovementTemplateRow.Orientation = Orientation;
            creatureMovementTemplateRow.Model1 = Model1;
            creatureMovementTemplateRow.Model2 = Model2;

            return creatureMovementTemplateRow;
        }
    }
}
