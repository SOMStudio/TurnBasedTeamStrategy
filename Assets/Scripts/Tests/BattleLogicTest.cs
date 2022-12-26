using System.Collections.Generic;
using Controller;
using Model;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class BattleLogicTest
    {
        private readonly PersonageList personageList = LoadManager.GetPersonageList();
        private readonly LevelList levelList = LoadManager.GetLevelList();
        private readonly BattleSetting setting = LoadManager.GetBattleSetting();
        private readonly BattleManager battleManager = new BattleManager();

        public BattleLogicTest()
        {
            battleManager.SetBattleSetting(setting);
        }
        
        [Test]
        public void CheckPlayersCanMove()
        {
            battleManager.ClearPlayers();

            foreach (var playerData in personageList.personage)
            {
                battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            }

            for (int i = 0; i < personageList.personage.Length; i++)
            {
                Assert.AreEqual( battleManager.IsPlayerCanMove(i), true);
            }
        }
        
        [Test]
        public void CheckPlayersCanAttack()
        {
            battleManager.ClearPlayers();

            foreach (var playerData in personageList.personage)
            {
                battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            }

            for (int i = 0; i < personageList.personage.Length; i++)
            {
                Assert.AreEqual( battleManager.IsPlayerCanAttack(i), true);
            }
        }
        
        [Test]
        public void CheckOccupiedPlayerPlace()
        {
            battleManager.ClearPlayers();

            var setPosition = new Vector2Int(0, 0);
            battleManager.AddPlayer(personageList.personage[0], setPosition);
            battleManager.SetLevel(levelList.level[0]);

            Assert.AreEqual( battleManager.IsPositionFree(setPosition), false);
        }
        
        [Test]
        public void CheckFreeLevel4X4Places()
        {
            battleManager.ClearPlayers();

            var setPosition = new Vector2Int(0, 0);
            battleManager.AddPlayer(personageList.personage[0], setPosition);
            battleManager.SetLevel(levelList.level[0]);

            var levelDate = battleManager.GetLevelData();
            for (int i = 0; i < levelDate.x; i++)
            {
                for (int j = 0; j < levelDate.y; j++)
                {
                    var checkPosition = new Vector2Int(i, j);
                    if (checkPosition != setPosition) Assert.AreEqual(battleManager.IsPositionFree(checkPosition), true);
                }
            }
        }
        
        [Test]
        public void CheckMoveLineDirectPlayer()
        {
            battleManager.ClearPlayers();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(3, 0);
            
            battleManager.MovePlayer(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(moveList.Count, 1);
        }
        
        [Test]
        public void CheckMoveBrokenDirectPlayer()
        {
            battleManager.ClearPlayers();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            
            battleManager.MovePlayer(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(moveList.Count, 2);
        }
        
        [Test]
        public void CheckMovePositionPlayer()
        {
            battleManager.ClearPlayers();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            battleManager.MovePlayer(0, newPosition, out List<Vector2Int> _);
            
            Assert.AreEqual(battleManager.GetPlayerPosition(0), newPosition);
        }
        
        [Test]
        public void CheckAttackPlayerEnemy()
        {
            battleManager.ClearPlayers();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(1, 1));
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(2, 1));
            battleManager.SetLevel(levelList.level[0]);

            var healthBeforeAttack = battleManager.GetEnemyDate(0).health;
            battleManager.AttackPlayer(0, 0);
            var healthAfterAttack = battleManager.GetEnemyDate(0).health;
            
            Assert.IsTrue(healthAfterAttack < healthBeforeAttack);
        }

    
        /*[UnityTest]
        public IEnumerator BattleLogicWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }*/
    }
}
