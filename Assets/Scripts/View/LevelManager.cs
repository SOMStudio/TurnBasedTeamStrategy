using System;
using System.IO;
using Controller;
using Model;
using UnityEngine;

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
                        battleManager.AddPlayer(personageList.personage[playerManager.PersonageNumberDate], playerManager.PlaceStep);
                    else
                        throw new InvalidDataException("Not set player number");

                foreach (var playerManager in _enemyList)
                    if (playerManager.PersonageNumberDate >= 0)
                        battleManager.AddEnemy(personageList.personage[playerManager.PersonageNumberDate], playerManager.PlaceStep);
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
        }

        private void Update()
        {
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

        private void OnOverStepHandler(int x, int y)
        {
            Debug.Log($"Over step: ({x}, {y})");
        }

        private void OnClickStepHandler(int x, int y)
        {
            Debug.Log($"Click step: ({x}, {y})");
            
            if (clickPlayerNumber != -1)
            {
                var clickedPlayer = _playerList[clickPlayerNumber];
                var oldPosition = _playerList[clickPlayerNumber].PlaceStep;
                var newPosition = new Vector2Int(x, y);
                if (battleManager.IsPositionFree(newPosition))
                {
                    if (battleManager.MovePlayer(clickPlayerNumber, newPosition, out var moveStepVector))
                    {
                        var moveVector = new List<Vector3>();
                        foreach (var moveSpline in moveStepVector)
                        {
                            
                        }
                        clickedPlayer.MoveToPlaceStep(_stepList[GetNumberStep(newPosition)], moveVector);
                    }
                }
            }
        }
        
        private void OnOverPlayerHandler(int x, int y)
        {
            Debug.Log($"Over player: ({x}, {y})");
        }

        private void OnClickPlayerHandler(int x, int y)
        {
            Debug.Log($"Click player: ({x}, {y})");
        }
        
        private void OnOverEnemyHandler(int x, int y)
        {
            Debug.Log($"Over enemy: ({x}, {y})");
        }

        private void OnClickEnemyHandler(int x, int y)
        {
            Debug.Log($"Click enemy: ({x}, {y})");
        }
    }
}
