using System;
using System.Collections.Generic;
using System.Linq;
using Help;
using Model;
using UnityEngine;

namespace Controller
{
    public class BattleManager
    {
        private List<PersonageData> playerListData;
        private List<Vector2Int> playerPositionData;

        private List<PersonageData> enemyListData;
        private List<Vector2Int> enemyPositionData;

        private LevelData levelData;
        private BattleSetting battleSettingData;


        public int PlayerCount => playerListData.Count;
        public PersonageData GetPlayerData(int playerInList) => playerListData[playerInList];
        public Vector2Int GetPlayerPosition(int playerInList) => playerPositionData[playerInList];
        public int NumberPlayerOnPosition(Vector2Int checkPosition)
        {
            if (IsPositionFree(checkPosition)) return -1;
            else
            {
                for (int i = 0; i < playerPositionData.Count; i++)
                    if (playerPositionData[i] == checkPosition) return i;
                
                return -1;
            }
        }

        public int EnemyCount => enemyListData.Count;
        public PersonageData GetEnemyData(int enemyInList) => enemyListData[enemyInList];
        public Vector2Int GetEnemyPosition(int enemyInLIst) => enemyPositionData[enemyInLIst];
        public int NumberEnemyOnPosition(Vector2Int checkPosition)
        {
            if (IsPositionFree(checkPosition)) return -1;
            else
            {
                for (int i = 0; i < enemyPositionData.Count; i++)
                    if (enemyPositionData[i] == checkPosition) return i;
                
                return -1;
            }
        }
        
        public LevelData GetLevelData() => levelData;
        public BattleSetting GetBattleSetting() => battleSettingData;
        
        public BattleManager()
        {
            playerListData = new List<PersonageData>();
            playerPositionData = new List<Vector2Int>();
            
            enemyListData = new List<PersonageData>();
            enemyPositionData = new List<Vector2Int>();
        }

        public void SetLevel(LevelData setLevelData)
        {
            levelData = setLevelData;
        }

        public void SetBattleSetting(BattleSetting setBattleSettingData)
        {
            battleSettingData = setBattleSettingData;
        }

        public void AddPlayer(PersonageData personageData, Vector2Int startPosition)
        {
            playerListData.Add(personageData);
            playerPositionData.Add(startPosition);
        }

        public void ClearPlayers()
        {
            playerListData.Clear();
            playerPositionData.Clear();
        }
        
        public void AddEnemy(PersonageData enemyData, Vector2Int startPosition)
        {
            enemyListData.Add(enemyData);
            enemyPositionData.Add(startPosition);
        }

        public void ClearEnemies()
        {
            enemyListData.Clear();
            enemyPositionData.Clear();
        }

        public bool IsPositionFree(Vector2Int checkPosition)
        {
            if (playerPositionData.Any(position => position == checkPosition))
                return false;
            
            if (enemyPositionData.Any(position => position == checkPosition))
                return false;
            
            return true;
        }

        public int DistanceForMove(List<Vector2Int> moveList)
        {
            return moveList.Sum(moveVector => moveVector.x == 0 ? Math.Abs(moveVector.y) : Math.Abs(moveVector.x));
        }

        public List<Vector2Int> GetMoveList(in Vector2Int moveFrom, in Vector2Int moveTo)
        {
            var moveList = new List<Vector2Int>();
            Vector2Int moveVector = moveTo - moveFrom;
            if (moveVector.x != 0f) moveList.Add(new Vector2Int(moveVector.x, 0));
            if (moveVector.y != 0) moveList.Add(new Vector2Int(0, moveVector.y));
            
            return moveList;
        }

        public bool IsPlayerCanMove(int playerInList)
        {
            return playerListData[playerInList].moveActionList.Length > 0;
        }

        public bool IsEnemyCanMove(int enemyInList)
        {
            return enemyListData[enemyInList].moveActionList.Length > 0;
        }
        
        public int GetPointsForMoveAction(int numberMoveAction)
        {
            return battleSettingData.moveActionPoint[numberMoveAction].point;
        }

        public int GetAttackDistance(in Vector2Int shootFrom, in Vector2Int shootTo)
        {
            return (int)(shootTo - shootFrom).magnitude;
        }

        public bool IsPlayerCanAttack(int playerInList)
        {
            return playerListData[playerInList].attackActionList.Length > 0;
        }

        public bool IsEnemyCanAttack(int enemyInList)
        {
            return enemyListData[enemyInList].attackActionList.Length > 0;
        }

        public int GetPointsForAttackAction(int numberAttackAction)
        {
            return battleSettingData.attackActionPoint[numberAttackAction].point;
        }

        public int GetDistanceForAttackAction(int numberAttackAction)
        {
            return battleSettingData.attackActionPoint[numberAttackAction].distance;
        }

        public bool MovePlayer(int playerInList, Vector2Int newPosition, out List<Vector2Int> moveList)
        {
            if (playerInList < playerListData.Count)
            {
                if (playerPositionData[playerInList] != newPosition && IsPositionFree(newPosition))
                {
                    if (IsPlayerCanMove(playerInList))
                    {
                        moveList = GetMoveList(playerPositionData[playerInList], newPosition);
                        int moveActonNumber = playerListData[playerInList].moveActionList[0];
                        int movePoint = DistanceForMove(moveList) * GetPointsForMoveAction(moveActonNumber);
                        
                        if (movePoint <= playerListData[playerInList].actionPoint)
                        {
                            var newPlayerData = playerListData[playerInList].Clone();
                            newPlayerData.actionPoint -= movePoint;
                            playerListData[playerInList] = newPlayerData;
                            
                            playerPositionData[playerInList] = newPosition;
                            
                            return true;
                        }
                    }
                }
            }

            moveList = null;
            return false;
        }

        public bool AttackPlayer(int playerInList, int enemyInList)
        {
            if (playerInList < playerListData.Count && enemyInList < enemyListData.Count)
            {
                if (IsPlayerCanAttack(playerInList))
                {
                    int attackDistance = GetAttackDistance(enemyPositionData[enemyInList], playerPositionData[playerInList]);
                    int attackActionNumber = playerListData[playerInList].attackActionList[0];
                    
                    if (attackDistance <= GetDistanceForAttackAction(attackActionNumber))
                    {
                        int attackPoint = GetPointsForAttackAction(attackActionNumber);
                        
                        if (attackPoint <= playerListData[playerInList].actionPoint)
                        {
                            if (enemyListData[enemyInList].health > 0)
                            {
                                var newPlayerData = playerListData[playerInList].Clone();
                                newPlayerData.actionPoint -= attackPoint;
                                playerListData[playerInList] = newPlayerData;

                                var newEnemyData = enemyListData[enemyInList].Clone();
                                newEnemyData.health -= newPlayerData.damage <= newEnemyData.health ? newPlayerData.damage : newEnemyData.health;
                                enemyListData[enemyInList] = newEnemyData;

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        
        public bool MoveEnemy(int enemyInList, Vector2Int newPosition, out List<Vector2Int> moveList)
        {
            if (enemyInList < enemyListData.Count)
            {
                if (enemyPositionData[enemyInList] != newPosition && IsPositionFree(newPosition))
                {
                    if (IsEnemyCanMove(enemyInList))
                    {
                        moveList = GetMoveList(enemyPositionData[enemyInList], newPosition);
                        int moveActionNumber = enemyListData[enemyInList].moveActionList[0];
                        int movePoint = DistanceForMove(moveList) * GetPointsForMoveAction(moveActionNumber);

                        if (enemyListData[enemyInList].actionPoint <= movePoint)
                        {
                            var newPlayerData = enemyListData[enemyInList].Clone();
                            newPlayerData.actionPoint -= movePoint;
                            enemyListData[enemyInList] = newPlayerData;
                            
                            enemyPositionData[enemyInList] = newPosition;
                            
                            return true;
                        }
                    }
                }
            }

            moveList = null;
            return false;
        }
        
        public bool AttackEnemy(int enemyInList, int playerInList)
        {
            if (enemyInList < enemyListData.Count && playerInList < playerListData.Count)
            {
                if (IsEnemyCanAttack(enemyInList))
                {
                    int attackDistance = GetAttackDistance(playerPositionData[playerInList],enemyPositionData[enemyInList]);
                    int attackActionNumber = enemyListData[enemyInList].attackActionList[0];
                    
                    if (attackDistance <= GetDistanceForAttackAction(attackActionNumber))
                    {
                        int attackPoint = GetPointsForAttackAction(attackActionNumber);
                        
                        if (attackPoint <= enemyListData[enemyInList].actionPoint)
                        {
                            if (playerListData[playerInList].health > 0)
                            {
                                var newEnemyData = enemyListData[enemyInList].Clone();
                                newEnemyData.actionPoint -= attackPoint;
                                enemyListData[enemyInList] = newEnemyData;

                                var newPlayerData = playerListData[playerInList].Clone();
                                newPlayerData.health -= newEnemyData.damage <= newPlayerData.health ? newEnemyData.damage : newPlayerData.health;
                                playerListData[playerInList] = newPlayerData;

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public int GetPlayerHealth()
        {
            return playerListData.Sum(personageData => personageData.health);
        }

        public int GetEnemyHealth()
        {
            return enemyListData.Sum(personageData => personageData.health);
        }
    }
}
