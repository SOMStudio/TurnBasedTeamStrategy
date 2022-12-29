using System.Collections.Generic;
using System.IO;
using AI;
using Controller;
using Model;
using UnityEngine;
using View.UI;
using View.UI.Level;

namespace View
{
    public class LevelManager : MonoBehaviour
    {
        private const string StepTag = "Step";
        private const string PlayerTag = "Player";
        private const string EnemyTag = "Enemy";
        
        [Header("Base")]
        [SerializeField] private int _levelNumberDate = -1;
        
        [Header("Main")]
        [SerializeField] private StepManager[] _stepList;
        [SerializeField] private PersonageManager[] _playerList;
        [SerializeField] private PersonageManager[] _enemyList;

        [Header("AiEnemy")]
        [SerializeField] private AiEnemyManger _aiEnemyManager;
        
        [Header("UI")]
        [SerializeField] private LevelUiManager _uiManager;

        private BattleManager battleManager;
        private LevelList levelList;
        private PersonageList personageList;
        private BattleSetting battleSetting;

        private Camera mainCamera;
        private RaycastHit hit;

        private int enterStepNumber = -1;
        private int clickStepNumber = -1;
        private int enterPlayerNumber = -1;
        private int clickPlayerNumber = -1;
        private int enterEnemyNumber = -1;
        private int clickEnemyNumber = -1;

        public PersonageManager GetPlayerPersonageManager(int numberPlayer)
        {
            return _playerList[numberPlayer];
        }

        public PersonageManager GetEnemyPersonageManager(int numberEnemy)
        {
            return _enemyList[numberEnemy];
        }
        
        private void Awake()
        {
            mainCamera = Camera.main;
            
            battleManager = new BattleManager();
            levelList = LoadManager.GetLevelList();
            personageList = LoadManager.GetPersonageList();
            battleSetting = LoadManager.GetBattleSetting();
            
            battleManager.SetBattleSetting(battleSetting);
        }

        private void Start()
        {
            if (_levelNumberDate >= 0)
            {
                battleManager.SetLevel(levelList.level[_levelNumberDate]);

                foreach (var playerManager in _playerList)
                    if (playerManager.PersonageNumberDate >= 0)
                    {
                        battleManager.AddPlayer(personageList.personage[playerManager.PersonageNumberDate], playerManager.PlaceStep);
                        playerManager.SetStartState(personageList.personage[playerManager.PersonageNumberDate]);
                    }
                    else
                        throw new InvalidDataException("Not set player number");

                foreach (var enemyManager in _enemyList)
                    if (enemyManager.PersonageNumberDate >= 0)
                    {
                        battleManager.AddEnemy(personageList.personage[enemyManager.PersonageNumberDate], enemyManager.PlaceStep);
                        enemyManager.SetStartState(personageList.personage[enemyManager.PersonageNumberDate]);
                    }
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
                playerManager.OnOverPersonageEvent += OnOverPlayerHandler;
                playerManager.OnClickPersonageEvent += OnClickPlayerHandler;
            }
            
            foreach (var enemyManager in _enemyList)
            {
                enemyManager.OnOverPersonageEvent += OnOverEnemyHandler;
                enemyManager.OnClickPersonageEvent += OnClickEnemyHandler;
            }
            
            _uiManager.InitInformation(battleManager.GetEnemyTeamHealth(), battleManager.GetPlayerTeamHealth());
            _uiManager.ClickNextTurnButtonEvent += ActivateAiEnemyHandler;
            
            _aiEnemyManager.InitState(battleManager, this);
            _aiEnemyManager.OnCompleteEvent += OnCompleteAiEnemyHandler;
        }

        private void Update()
        {
            if (_uiManager.IsMenuActive) return;
            
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag(StepTag))
                {
                    CheckClearOverPlayerNumber();
                    CheckClearOverEnemyNumber();
                    
                    var enterStepNumberNew = GetNumberStep(hit.transform.GetComponent<StepManager>().PlaceStep);
                    
                    if (enterStepNumber != enterStepNumberNew)
                    {
                        if (enterStepNumber != -1) _stepList[enterStepNumber].OnPointerExit();

                        enterStepNumber = enterStepNumberNew;
                        _stepList[enterStepNumber].OnPointerEnter();
                    }
                }
                else if (hit.collider.CompareTag(PlayerTag))
                {
                    CheckClearOverStepNumber();
                    CheckClearOverEnemyNumber();

                    var enterPlayerNumberNew = GetNumberPlayer(hit.transform.GetComponent<PersonageManager>().PlaceStep);

                    if (enterPlayerNumber != enterPlayerNumberNew)
                    {
                        if (enterPlayerNumber != -1) _playerList[enterPlayerNumber].OnPointerExit();

                        enterPlayerNumber = enterPlayerNumberNew;
                        _playerList[enterPlayerNumber].OnPointerEnter();
                    }
                }
                else if (hit.collider.CompareTag(EnemyTag))
                {
                    CheckClearOverStepNumber();
                    CheckClearOverPlayerNumber();

                    var enterEnemyNumberNew = GetNumberEnemy(hit.transform.GetComponent<PersonageManager>().PlaceStep);

                    if (enterEnemyNumber != enterEnemyNumberNew)
                    {
                        if (enterEnemyNumber != -1) _enemyList[enterEnemyNumber].OnPointerExit();

                        enterEnemyNumber = enterEnemyNumberNew;
                        _enemyList[enterEnemyNumber].OnPointerEnter();
                    }
                }
                else
                {
                    CheckClearOverStepNumber();
                    CheckClearOverPlayerNumber();
                    CheckClearOverEnemyNumber();
                }
            }

            if (_aiEnemyManager.IsAiActive) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                if (enterStepNumber != -1)
                {
                    clickStepNumber = enterStepNumber;
                    _stepList[clickStepNumber].OnPointerClick();
                }
                if (enterPlayerNumber != -1)
                {
                    clickPlayerNumber = enterPlayerNumber;
                    _playerList[clickPlayerNumber].OnPointerClick();
                }
                if (enterEnemyNumber != -1)
                {
                    clickEnemyNumber = enterEnemyNumber;
                    _enemyList[clickEnemyNumber].OnPointerClick();
                }
            }
        }

		private void CheckClearOverStepNumber()
		{
			if (enterStepNumber != -1)
            {
                _stepList[enterStepNumber].OnPointerExit();
                enterStepNumber = -1;
            }
		}

		private void CheckClearOverPlayerNumber(){
			if (enterPlayerNumber != -1)
            {
                _playerList[enterPlayerNumber].OnPointerExit();
                enterPlayerNumber = -1;
            }
		}

		private void CheckClearOverEnemyNumber(){
			if (enterEnemyNumber != -1)
            {
                _enemyList[enterEnemyNumber].OnPointerExit();
                enterEnemyNumber = -1;
            }
		}

        private int GetNumberStep(Vector2Int stepCoordinate)
        {
            return stepCoordinate.y * battleManager.GetLevelData().x + stepCoordinate.x;
        }

        private int GetNumberPlayer(Vector2Int stepCoordinate)
        {
            return battleManager.NumberPlayerOnPosition(stepCoordinate);
        }
        
        private int GetNumberEnemy(Vector2Int stepCoordinate)
        {
            return battleManager.NumberEnemyOnPosition(stepCoordinate);
        }

        public void MovePersonage(PersonageManager personage, Vector2Int newStepPosition, List<Vector2Int> moveStepVector, PersonageData updatedState, System.Action onCompletedMove = null)
        {
            var movePosition = new List<Vector3>();
            var moveStep = personage.PlaceStep;
            foreach (var shiftStep in moveStepVector)
            {
                moveStep += shiftStep;
                movePosition.Add(_stepList[GetNumberStep(moveStep)].transform.position);
            }
            personage.MoveToPosition(_stepList[GetNumberStep(newStepPosition)], movePosition, onCompletedMove);
            personage.UpdateState(updatedState);
        }

        public void AttackPersonage(PersonageManager attacking, PersonageManager attacked, PersonageData attackingState, PersonageData attackedSate, System.Action onCompleteAttack = null)
        {
            attacking.Attack(onCompleteAttack);
            attacked.Attack();
            
            attacking.UpdateState(attackingState);
            attacked.UpdateState(attackedSate);

            CheckCompleteLevel();
        }

        private void CheckCompleteLevel()
        {
            if (battleManager.GetPlayerTeamHealth() == 0)
            {
                _uiManager.ShowResultWindow("You lose this game!");
            }
            if (battleManager.GetEnemyTeamHealth() == 0)
            {
                _uiManager.ShowResultWindow("You win this game!");
            }
            
            _uiManager.UpdateInformation(battleManager.GetEnemyTeamHealth(), battleManager.GetPlayerTeamHealth());
        }

        #region EventHandlers
        private void OnOverStepHandler(int x, int y)
        {
            
        }

        private void OnClickStepHandler(int x, int y)
        {
            if (clickPlayerNumber != -1)
            {
                var clickedPlayer = _playerList[clickPlayerNumber];
                var oldStep = _playerList[clickPlayerNumber].PlaceStep;
                var newStep = new Vector2Int(x, y);
                
                if (battleManager.MovePlayer(clickPlayerNumber, newStep, out var moveStepVector))
                {
                    MovePersonage(clickedPlayer, newStep, moveStepVector, battleManager.GetPlayerData(clickPlayerNumber));
                }
            }
        }
        
        private void OnOverPlayerHandler(int x, int y)
        {
            
        }

        private void OnClickPlayerHandler(int x, int y)
        {
            
        }
        
        private void OnOverEnemyHandler(int x, int y)
        {
            
        }

        private void OnClickEnemyHandler(int x, int y)
        {
            if (clickPlayerNumber != -1)
            {
                var clickedPlayer = _playerList[clickPlayerNumber];
                var clickedEnemy = _enemyList[clickEnemyNumber];
                
                if (battleManager.AttackPlayer(clickPlayerNumber, clickEnemyNumber))
                {
                    var attackingPlayerData = battleManager.GetPlayerData(clickPlayerNumber);
                    var attackedEnemyData = battleManager.GetEnemyData(clickEnemyNumber);
                    AttackPersonage(clickedPlayer, clickedEnemy, attackingPlayerData, attackedEnemyData);
                }
            }
        }

        private void ActivateAiEnemyHandler()
        {
            _uiManager.LockNextTurnButton();
            
            _aiEnemyManager.ActivateAi();
        }
        
        private void OnCompleteAiEnemyHandler()
        {
            _uiManager.UnLockNextTurnButton();
        }
        #endregion
    }
}
