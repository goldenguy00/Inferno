using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace Inferno.Item
{
    public static class Items
    {
        private const string PrimaryStockItemName = "Inferno: +1 Primary Stock";
        private const string PrimaryStockItemLangTokenName = "INFERNO_PRIMARYSTOCK";
        private const string PrimaryStockItemPickupDesc = "Racecar";
        private const string PrimaryStockItemFullDescription = "15:21";

        private const string SecondaryStockItemName = "Inferno: +1 Secondary Stock";
        private const string SecondaryStockItemLangTokenName = "INFERNO_SECONDARYSTOCK";
        private const string SecondaryStockItemPickupDesc = "Omega";
        private const string SecondaryStockItemFullDescription = "11:44";

        private const string UtilityStockItemName = "Inferno: +1 Utility Stock";
        private const string UtilityStockItemLangTokenName = "INFERNO_UTILITYSTOCK";
        private const string UtilityStockItemPickupDesc = "Reptile";
        private const string UtilityStockItemFullDescription = "16:44";

        private const string SpecialStockItemName = "Inferno: +1 Special Stock";
        private const string SpecialStockItemLangTokenName = "INFERNO_SPECIALSTOCK";
        private const string SpecialStockItemPickupDesc = "Periphery V When";
        private const string SpecialStockItemFullDescription = "??:??";

        private const string AllCooldownItemName = "Inferno: +1% All CDR";
        private const string AllCooldownItemLangTokenName = "INFERNO_ALLCDR";
        private const string AllCooldownItemPickupDesc = "Periphery VI When";
        private const string AllCooldownItemFullDescription = "???:???";

        public static ItemTier Tier;
        public static ItemTag[] ItemTags { get; set; } = new ItemTag[] { };

        public static ItemDef PrimaryStockItemDef;
        public static ItemDef SecondaryStockItemDef;
        public static ItemDef UtilityStockItemDef;
        public static ItemDef SpecialStockItemDef;
        public static ItemDef AllCooldownItemDef;

        public static void Create()
        {
            ItemTags = new List<ItemTag>(ItemTags) { ItemTag.AIBlacklist }.ToArray();
            Tier = ItemTier.NoTier;

            PrimaryStockItemDef = ScriptableObject.CreateInstance<ItemDef>();
            PrimaryStockItemDef.name = "ITEM_" + PrimaryStockItemLangTokenName;
            PrimaryStockItemDef.nameToken = "ITEM_" + PrimaryStockItemLangTokenName + "_NAME";
            PrimaryStockItemDef.pickupToken = "ITEM_" + PrimaryStockItemLangTokenName + "_PICKUP";
            PrimaryStockItemDef.descriptionToken = "ITEM_" + PrimaryStockItemLangTokenName + "_DESCRIPTION";
            PrimaryStockItemDef.hidden = true;
            PrimaryStockItemDef.tags = ItemTags;
            PrimaryStockItemDef.deprecatedTier = Tier;

            LanguageAPI.Add("ITEM_" + PrimaryStockItemLangTokenName + "_NAME", PrimaryStockItemName);
            LanguageAPI.Add("ITEM_" + PrimaryStockItemLangTokenName + "_PICKUP", PrimaryStockItemPickupDesc);
            LanguageAPI.Add("ITEM_" + PrimaryStockItemLangTokenName + "_DESCRIPTION", PrimaryStockItemFullDescription);

            ItemAPI.Add(new CustomItem(PrimaryStockItemDef, CreateItemDisplayRules()));

            SecondaryStockItemDef = ScriptableObject.CreateInstance<ItemDef>();
            SecondaryStockItemDef.name = "ITEM_" + SecondaryStockItemLangTokenName;
            SecondaryStockItemDef.nameToken = "ITEM_" + SecondaryStockItemLangTokenName + "_NAME";
            SecondaryStockItemDef.pickupToken = "ITEM_" + SecondaryStockItemLangTokenName + "_PICKUP";
            SecondaryStockItemDef.descriptionToken = "ITEM_" + SecondaryStockItemLangTokenName + "_DESCRIPTION";
            SecondaryStockItemDef.hidden = true;
            SecondaryStockItemDef.tags = ItemTags;
            SecondaryStockItemDef.deprecatedTier = Tier;

            LanguageAPI.Add("ITEM_" + SecondaryStockItemLangTokenName + "_NAME", SecondaryStockItemName);
            LanguageAPI.Add("ITEM_" + SecondaryStockItemLangTokenName + "_PICKUP", SecondaryStockItemPickupDesc);
            LanguageAPI.Add("ITEM_" + SecondaryStockItemLangTokenName + "_DESCRIPTION", SecondaryStockItemFullDescription);

            ItemAPI.Add(new CustomItem(SecondaryStockItemDef, CreateItemDisplayRules()));

            UtilityStockItemDef = ScriptableObject.CreateInstance<ItemDef>();
            UtilityStockItemDef.name = "ITEM_" + UtilityStockItemLangTokenName;
            UtilityStockItemDef.nameToken = "ITEM_" + UtilityStockItemLangTokenName + "_NAME";
            UtilityStockItemDef.pickupToken = "ITEM_" + UtilityStockItemLangTokenName + "_PICKUP";
            UtilityStockItemDef.descriptionToken = "ITEM_" + UtilityStockItemLangTokenName + "_DESCRIPTION";
            UtilityStockItemDef.hidden = true;
            UtilityStockItemDef.tags = ItemTags;
            UtilityStockItemDef.deprecatedTier = Tier;

            LanguageAPI.Add("ITEM_" + UtilityStockItemLangTokenName + "_NAME", UtilityStockItemName);
            LanguageAPI.Add("ITEM_" + UtilityStockItemLangTokenName + "_PICKUP", UtilityStockItemPickupDesc);
            LanguageAPI.Add("ITEM_" + UtilityStockItemLangTokenName + "_DESCRIPTION", UtilityStockItemFullDescription);

            ItemAPI.Add(new CustomItem(UtilityStockItemDef, CreateItemDisplayRules()));

            SpecialStockItemDef = ScriptableObject.CreateInstance<ItemDef>();
            SpecialStockItemDef.name = "ITEM_" + SpecialStockItemLangTokenName;
            SpecialStockItemDef.nameToken = "ITEM_" + SpecialStockItemLangTokenName + "_NAME";
            SpecialStockItemDef.pickupToken = "ITEM_" + SpecialStockItemLangTokenName + "_PICKUP";
            SpecialStockItemDef.descriptionToken = "ITEM_" + SpecialStockItemLangTokenName + "_DESCRIPTION";
            SpecialStockItemDef.hidden = true;
            SpecialStockItemDef.tags = ItemTags;
            SpecialStockItemDef.deprecatedTier = Tier;

            LanguageAPI.Add("ITEM_" + SpecialStockItemLangTokenName + "_NAME", SpecialStockItemName);
            LanguageAPI.Add("ITEM_" + SpecialStockItemLangTokenName + "_PICKUP", SpecialStockItemPickupDesc);
            LanguageAPI.Add("ITEM_" + SpecialStockItemLangTokenName + "_DESCRIPTION", SpecialStockItemFullDescription);

            ItemAPI.Add(new CustomItem(SpecialStockItemDef, CreateItemDisplayRules()));

            AllCooldownItemDef = ScriptableObject.CreateInstance<ItemDef>();
            AllCooldownItemDef.name = "ITEM_" + AllCooldownItemLangTokenName;
            AllCooldownItemDef.nameToken = "ITEM_" + AllCooldownItemLangTokenName + "_NAME";
            AllCooldownItemDef.pickupToken = "ITEM_" + AllCooldownItemLangTokenName + "_PICKUP";
            AllCooldownItemDef.descriptionToken = "ITEM_" + AllCooldownItemLangTokenName + "_DESCRIPTION";
            AllCooldownItemDef.hidden = true;
            AllCooldownItemDef.tags = ItemTags;
            AllCooldownItemDef.deprecatedTier = Tier;

            LanguageAPI.Add("ITEM_" + AllCooldownItemLangTokenName + "_NAME", AllCooldownItemName);
            LanguageAPI.Add("ITEM_" + AllCooldownItemLangTokenName + "_PICKUP", AllCooldownItemPickupDesc);
            LanguageAPI.Add("ITEM_" + AllCooldownItemLangTokenName + "_DESCRIPTION", AllCooldownItemFullDescription);

            ItemAPI.Add(new CustomItem(AllCooldownItemDef, CreateItemDisplayRules()));

            On.RoR2.CharacterBody.RecalculateStats += AddStock;
            RecalculateStatsAPI.GetStatCoefficients += ChangeCDR;
        }

        public static void ChangeCDR(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                var stack = sender.inventory.GetItemCount(AllCooldownItemDef);
                if (stack > 0)
                {
                    args.cooldownMultAdd -= 0.01f * stack;
                }
            }
        }

        public static void AddStock(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self && self.inventory && self.skillLocator)
            {
                var sl = self.skillLocator;
                if (sl.primary)
                {
                    sl.primary.SetBonusStockFromBody(sl.primary.bonusStockFromBody + self.inventory.GetItemCount(PrimaryStockItemDef));
                }
                if (sl.secondary)
                {
                    sl.secondary.SetBonusStockFromBody(sl.secondary.bonusStockFromBody + self.inventory.GetItemCount(SecondaryStockItemDef));
                }
                if (sl.utility)
                {
                    sl.utility.SetBonusStockFromBody(sl.utility.bonusStockFromBody + self.inventory.GetItemCount(UtilityStockItemDef));
                }
                if (sl.special)
                {
                    sl.special.SetBonusStockFromBody(sl.special.bonusStockFromBody + self.inventory.GetItemCount(SpecialStockItemDef));
                }
            }
        }

        public static ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }
    }
}