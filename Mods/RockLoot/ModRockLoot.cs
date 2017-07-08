using FSLoader;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockLoot
{
    [ModInfo("rock_loot", "Rock loot", "Robot9706", 1, 0, "Makes rocks drop random common outfits and weapons when destroyed.")]
    public class ModRockLoot : Mod
    {
        [Hook("RockZone::DestroyRock(System.Boolean)")]
        public void Hook_DestroyRock(CallContext context, bool online)
        {
            RockZone rock = (RockZone)context.This;

			//First do some calculations on where the on-screen loot images will appear
            Vector3 position = MonoSingleton<CameraController>.Instance.GetComponent<Camera>().WorldToViewportPoint(rock.m_center.transform.position);
            position.z = Mathf.Abs((float)(MonoSingleton<CameraController>.Instance.CameraPosition.z - MonoSingleton<CameraController>.Instance.m_RoomBackZPosition));
            Vector3 vector2 = MonoSingleton<VaultGUIManager>.Instance.m_camera2d.ViewportToWorldPoint(position);
            vector2 = new Vector3(vector2.x, vector2.y, 0f);

			//Create a storage for the loot, this will be used by the particle manager to spawn the item on-screen
            Storage loot = new Storage();

			//Get a ResourceParticleMgr instance
			ResourceParticleMgr particles = MonoSingleton<ResourceParticleMgr>.Instance;

			//Get an ItemParameters instance, this contains all the game items
			ItemParameters items = MonoSingleton<GameParameters>.Instance.Items;

			//Get the VaultInventory
            VaultInventory inventory = MonoSingleton<Vault>.Instance.Inventory;

			//Let's see if we want to drop an outfit or a weapon
            if (Random.Range(0, 50) % 2 == 0)
            {
				//Collect all Normal and Common outfits
                List<DwellerOutfitItem> randomOutfits = new List<DwellerOutfitItem>();
                randomOutfits.AddRange(items.GetOutfits(EItemRarity.Normal));
                randomOutfits.AddRange(items.GetOutfits(EItemRarity.Common));

				//Find a random outfit
                DwellerOutfitItem[] outfits = randomOutfits.Distinct().ToArray();
                DwellerOutfitItem selectedOutfit = outfits[Random.Range(0, outfits.Length - 1)];

				//Create the loot particle effect (the item which will go from the rock to the bottom right of the screen)
                particles.SetSpriteName(selectedOutfit.OutfitSprite);
                loot.Resources.CraftedOutfit = 1; //CraftedOutfits seems to work with all outfits (there's no way to represent outfits in Resources otherwise)

				//Add the item to the vault inventory
                inventory.AddItem(new DwellerItem(EItemType.Outfit, selectedOutfit.m_outfitId), false);
            }
            else
            {
				//Collect all Normal and Common weapons
				List<DwellerWeaponItem> randomWeapon = new List<DwellerWeaponItem>();
                randomWeapon.AddRange(items.GetWeapons(EItemRarity.Normal));
                randomWeapon.AddRange(items.GetWeapons(EItemRarity.Common));

				//Find a random weapon
				DwellerWeaponItem[] weapons = randomWeapon.Distinct().ToArray();
                DwellerWeaponItem selectedWeapon = weapons[Random.Range(0, weapons.Length - 1)];

				//Create the loot particle effect
				particles.SetSpriteName(selectedWeapon.WeaponSprite);
                loot.Resources.CraftedWeapon = 1; //Same with CraftedOutfit

				//Add the item to the vault inventory
				inventory.AddItem(new DwellerItem(EItemType.Weapon, selectedWeapon.WeaponId), false);
            }

			//The particle manager will spawn particles based on the storage resources passed to the CollectResourcesGUI function
			//This way we can spawn item particles as crafted items and set the particle sprite by hand using "particles.SetSpriteName(...);"
			particles.CollectResourcesGUI(vector2, loot, true, true, false, false); //Spawn the particles 
        }
    }
}
