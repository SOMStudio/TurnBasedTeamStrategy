using Model;
using UnityEngine;

namespace Controller
{
    public static class LoadManager
    {
        private static readonly string PersonageJSon = "PersonageList";
        private static readonly string LevelJSon = "LevelList";
        private static readonly string BattleSettingJSon = "BattleSetting";

        public static PersonageList GetPersonageList()
        {
            return JsonUtility.FromJson<PersonageList>(Resources.Load<TextAsset>(PersonageJSon).text);
        }
        
        public static LevelList GetLevelList()
        {
            return JsonUtility.FromJson<LevelList>(Resources.Load<TextAsset>(LevelJSon).text);
        }
        
        public static BattleSetting GetBattleSetting()
        {
            return JsonUtility.FromJson<BattleSetting>(Resources.Load<TextAsset>(BattleSettingJSon).text);
        }
    }
}
