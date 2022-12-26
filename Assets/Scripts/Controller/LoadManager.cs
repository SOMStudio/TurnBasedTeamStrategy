using Model;
using UnityEngine;

namespace Controller
{
    public static class LoadManager
    {
        private static readonly string PlayerJSon = "PlayerList";
        private static readonly string LevelJSon = "LevelList";
        private static readonly string BattleSettingJSon = "BattleSetting";

        public static PlayerList GetPlayerList()
        {
            return JsonUtility.FromJson<PlayerList>(Resources.Load<TextAsset>(PlayerJSon).text);
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
