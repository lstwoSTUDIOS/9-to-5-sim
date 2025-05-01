using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Modifiers
{
    public static class ModifierManager
    {
        public static List<BaseModifier> currentModifiers = new();
        public static Dictionary<string, BaseModifier> idToModifierMap = new();
        
        static ModifierManager()
        {
            currentModifiers.Clear();
            idToModifierMap.Clear();
            
            var modifierTypes = typeof(ModifierManager).Assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(BaseModifier)));

            foreach (var modifierType in modifierTypes)
            {
                var modifier = Activator.CreateInstance(modifierType) as BaseModifier;
                idToModifierMap.Add(modifierType.Name, modifier);
            }
        }

        public static void LoadCurrentModifiers()
        {
            var modifierIds = PlayerDataManager.playerData.modifiers;

            foreach (var modifierId in modifierIds)
            {
                currentModifiers.Add(idToModifierMap[modifierId]);
            }
        }
    }
}