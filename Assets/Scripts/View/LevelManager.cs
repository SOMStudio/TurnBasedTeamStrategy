using System.IO;
using Controller;
using Model;
using UnityEngine;

namespace View
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private int _levelNumberDate = -1;
        
        [Header("Main")]
        [SerializeField] private StepManager[] _stepList;
        [SerializeField] private PlayerManager[] _playerList;
        [SerializeField] private PlayerManager[] _enemyList;

        private BattleManager battleManager;
        private LevelList levelList;
        private PlayerList playerList;
        private BattleSetting battleSetting;

        private void Awake()
        {
            battleManager = new BattleManager();
            levelList = LoadManager.GetLevelList();
            playerList = LoadManager.GetPlayerList();
            battleSetting = LoadManager.GetBattleSetting();
            
            battleManager.SetBattleSetting(battleSetting);
        }

        private void Start()
        {
            if (_levelNumberDate >= 0)
            {
                battleManager.SetLevel(levelList.level[_levelNumberDate]);

                foreach (var playerManager in _playerList)
                    if (playerManager.PlayerNumberDate >= 0)
                        battleManager.AddPlayer(playerList.player[playerManager.PlayerNumberDate], playerManager.PlaceStep);
                    else
                        throw new InvalidDataException("Not set player number");

                foreach (var playerManager in _enemyList)
                    if (playerManager.PlayerNumberDate >= 0)
                        battleManager.AddEnemy(playerList.player[playerManager.PlayerNumberDate], playerManager.PlaceStep);
                    else
                        throw new InvalidDataException("Not set enemy number");
            }
            else
            {
                throw new InvalidDataException("Not set level number");
            }

            foreach (var stepManager in _stepList)
            {
                stepManager.OnOverStepEvent += OnOverStepHandler;
                stepManager.OnClickStepEvent += OnClickStepHandler;
            }

            foreach (var playerManager in _playerList)
            {
                playerManager.OnOverPlayerEvent += OnOverPlayerHandler;
                playerManager.OnClickPlayerEvent += OnClickPlayerHandler;
            }
        }

        private void OnOverStepHandler(int x, int y)
        {
            
        }

        private void OnClickStepHandler(int x, int y)
        {
            
        }
        
        private void OnOverPlayerHandler(int x, int y)
        {
            
        }

        private void OnClickPlayerHandler(int x, int y)
        {
            
        }
    }
}