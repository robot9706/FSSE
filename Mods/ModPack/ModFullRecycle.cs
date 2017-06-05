using FSLoader;
using System;
using System.Collections.Generic;

namespace ModPack
{
    [ModInfo("full_recycle", "100% scrapping rate", "Robot9706", 1, 0)]
    public class ModFullRecycle : Mod
    {
        [Hook("ScrappingParameters::ScrapItems(DwellerItem,System.Int32)")]
        public void Hook_ScrappingParameters(CallingContext context, DwellerItem item, int amountScrapped)
        {
            context.IsHandled = true; //Don't execute the FalloutShelter code

            ScrappingParameters thisObject = (ScrappingParameters)context.This;

            //Get the recipe for the scrapped item
            RecipeList.Recipe scrappingRecipeForItem = MonoSingleton<GameParameters>.Instance.Items.CraftParameters.GetScrappingRecipeForItem(item);
            if (scrappingRecipeForItem == null)
            {
                return;
            }

            //Count the amount of junk the item requires to craft
            Dictionary<string, int> junkCount = new Dictionary<string, int>();
            foreach (RecipeList.IngredientEntry i in scrappingRecipeForItem.Ingredients)
            {
                string id = i.GetIngredient().JunkId;

                if (junkCount.ContainsKey(id))
                {
                    junkCount[id] = junkCount[id] + i.Count * amountScrapped;
                }
                else
                {
                    junkCount.Add(id, i.Count * amountScrapped);
                }
            }

            List<KeyValuePair<DwellerJunkItem, int>> returnList = new List<KeyValuePair<DwellerJunkItem, int>>();
            context.ReturnValue = returnList; //The hooked method should return "returnList"

            if (junkCount.Count > 0)
            {
                //Convert the IDs and counts into actual items
                foreach (string junkID in junkCount.Keys)
                {
                    DwellerJunkItem junk = MonoSingleton<GameParameters>.Instance.Items.GetJunk(junkID);
                    returnList.Add(new KeyValuePair<DwellerJunkItem, int>(junk, junkCount[junkID]));
                }

                returnList.Sort(new Comparison<KeyValuePair<DwellerJunkItem, int>>(thisObject.CompareReward)); //I don't know why this is required, but the game does this too
            }
        }
    }
}
