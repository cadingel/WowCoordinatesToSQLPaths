using System;

namespace WowCoordinatesToSQLPaths.Model
{
    class CreatureMovementTemplateRow : CreateMovementRowBase, ICloneable
    {
        public int Entry { get; set; }

#if (!SPECIAL)
        public int PathId { get; set; }
#endif

        public object Clone()
        {
            CreatureMovementTemplateRow creatureMovementTemplateRow = new CreatureMovementTemplateRow();
            creatureMovementTemplateRow.Entry = Entry;
#if (!SPECIAL)
            creatureMovementTemplateRow.PathId = PathId;
#endif
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
#if (SPECIAL)
            creatureMovementTemplateRow.WpGuid = WpGuid;
#endif
            creatureMovementTemplateRow.Orientation = Orientation;
            creatureMovementTemplateRow.Model1 = Model1;
            creatureMovementTemplateRow.Model2 = Model2;

            return creatureMovementTemplateRow;
        }
    }
}
