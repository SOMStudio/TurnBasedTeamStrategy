using System.Collections;
using Controller;
using UnityEngine;
using View;

namespace AI
{
    public class AiEnemyManger : MonoBehaviour
    {
        private const float InitDelay = 0.5f;
        private const float SelectEnemyDelay = 0.3f;
        private const float AttackDelay = 0.2f;
        private const float CompleteDelay = 0.5f;
        
        [Header("Main")]
        [SerializeField] private bool _active;

        private BattleManager battleManager;
        private LevelManager levelManager;

        private AiState aiState = AiState.Init;

        private int activePersonageNumber = 0;
        private int nearestEnemyNumber = 0;

        public event System.Action OnCompleteEvent;
        
        public void InitState(BattleManager battleManagerSet)
        {
            battleManager = battleManagerSet;
        }

        public void ActivateAi()
        {
            aiState = AiState.Init;
            activePersonageNumber = 0;
            
            _active = true;
        }
        
        private void Update()
        {
            if (!_active) return;
            if (aiState == AiState.Wait) return;

            if (aiState == AiState.Init)
            {
                Init();
            }
            else if (aiState == AiState.SelectPersonage)
            {
                SelectPersonage();
            }
            else if (aiState == AiState.MoveToEnemy)
            {
                MoveToEnemy();
            }
            else if (aiState == AiState.AttackEnemy)
            {
                AttackEnemy();
            }
            else if (aiState == AiState.Complete)
            {
                Complete();
            }
        }

        private IEnumerator StartWithDelay(float delay, System.Action runAction)
        {
            yield return new WaitForSeconds(delay);
            
            runAction.Invoke();
        }
        
        private void Init()
        {
            aiState = AiState.Wait;
            
            // init action

            StartCoroutine(StartWithDelay(InitDelay, () => aiState = AiState.SelectPersonage));
        }

        private void SelectPersonage()
        {
            aiState = AiState.Wait;

            var personageDate = battleManager.GetEnemyData(activePersonageNumber);

            if (personageDate.health > 0 && personageDate.actionPoint > 0)
            {
                nearestEnemyNumber = SelectNearestEnemy();

                if (nearestEnemyNumber == -1)
                    StartCoroutine(StartWithDelay(CompleteDelay, () => aiState = AiState.Complete));
                else
                    aiState = AiState.AttackEnemy;
            }
            else
            {
                if (activePersonageNumber < battleManager.EnemyCount)
                {
                    activePersonageNumber++;
                    
                    SelectPersonage();
                }
                else
                {
                    StartCoroutine(StartWithDelay(CompleteDelay, () => aiState = AiState.Complete));
                }
            }
        }

        private void AttackEnemy()
        {
            aiState = AiState.Wait;
            
            var attackingPersonageData = battleManager.GetEnemyData(activePersonageNumber);
            var attackedPersonageData = battleManager.GetPlayerData(nearestEnemyNumber);
            
            if (battleManager.AttackEnemy(activePersonageNumber, nearestEnemyNumber))
            {
                var attackingPersonage = levelManager.GetEnemyPersonageManager(activePersonageNumber);
                var attackedPersonage = levelManager.GetPlayerPersonageManager(nearestEnemyNumber);
                void OnCompleteAction() => aiState = AiState.AttackEnemy;
                levelManager.AttackPersonage(attackingPersonage, attackedPersonage, attackingPersonageData, attackedPersonageData, OnCompleteAction);
            }
            else
            {
                if (attackedPersonageData.health <= 0 || attackingPersonageData.actionPoint <= 0)
                    StartCoroutine(StartWithDelay(SelectEnemyDelay, () => aiState = AiState.SelectPersonage));
                else
                    StartCoroutine(StartWithDelay(SelectEnemyDelay, () => aiState = AiState.MoveToEnemy));
            }
        }
        
        private void MoveToEnemy()
        {
            aiState = AiState.Wait;

            //Vector2Int nearestStep
        }

        private void Complete()
        {
            aiState = AiState.Wait;
            _active = true;
            
            // move back personages
            
            StartCoroutine(StartWithDelay(SelectEnemyDelay, () => OnCompleteEvent?.Invoke()));
        }

        private int SelectNearestEnemy()
        {
            int nearestPlayerNumber = -1;
            int smallerDistanceToEnemy = 100;
            var personagePosition = battleManager.GetEnemyPosition(activePersonageNumber);
            
            for (int i = 0; i < battleManager.PlayerCount; i++)
            {
                var enemyPosition = battleManager.GetPlayerPosition(i);
                var distanceToEnemy = battleManager.DistanceForMove(battleManager.GetMoveList(personagePosition, enemyPosition));
                var enemyDate = battleManager.GetPlayerData(i);

                if (distanceToEnemy < smallerDistanceToEnemy && enemyDate.health > 0)
                {
                    nearestPlayerNumber = i;
                }
            }
            return nearestPlayerNumber;
        }
    }

    public enum AiState
    {
        Init,
        SelectPersonage,
        MoveToEnemy,
        AttackEnemy,
        Complete,
        Wait
    }
}
