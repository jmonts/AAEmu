﻿using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Items.Actions;
using AAEmu.Game.Models.Game.Quests.Templates;

namespace AAEmu.Game.Models.Game.Quests.Acts
{
    public class QuestActSupplySelectiveItem : QuestActTemplate
    {
        public uint ItemId { get; set; }
        public int Count { get; set; }
        public byte GradeId { get; set; }

        public override bool Use(Character character, Quest quest, int objective)
        {
            _log.Warn("QuestActSupplySelectiveItem");

            character.Inventory.Bag.AcquireDefaultItem(ItemTaskType.QuestSupplyItems, ItemId, Count, GradeId, 0);

            return quest.Template.Score > 0 ? objective * Count >= quest.Template.Score : objective >= Count;
        }
    }
}
