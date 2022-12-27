using System.Collections.Generic;
using Controller;
using Model;
using UnityEngine;

namespace View
{
    public class DateLoader : MonoBehaviour
    {
        public PersonageList _myPersonageList;
        public LevelList _myLevelList;
        public BattleSetting _myBattleSetting;

        private void Start()
        {
            _myPersonageList = LoadManager.GetPersonageList();
            _myLevelList = LoadManager.GetLevelList();
            _myBattleSetting = LoadManager.GetBattleSetting();

            TestBattleLogic();
        }

        private void TestBattleLogic()
        {
            TestSimpleMovePlayer();
        }

        private void TestSimpleMovePlayer()
        {
            var battleManager = new BattleManager();
            battleManager.AddPlayer(_myPersonageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(_myLevelList.level[0]);
            battleManager.SetBattleSetting(_myBattleSetting);

            battleManager.MovePlayer(0, new Vector2Int(2, 2), out List<Vector2Int> _);
            
            Debug.Log($"New position: {battleManager.GetPlayerPosition(0)}");
        }
    }
}
