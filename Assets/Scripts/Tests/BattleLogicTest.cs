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
        public void CheckCanMovePlayers()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();

            foreach (var personageData in personageList.personage)
            {
                battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            }

            for (int i = 0; i < battleManager.PlayerCount; i++)
            {
                Assert.IsTrue( battleManager.IsPlayerCanMove(i));
            }
        }
        
        [Test]
        public void CheckCanMoveEnemies()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();

            foreach (var personageData in personageList.personage)
            {
                battleManager.AddEnemy(personageList.personage[0], new Vector2Int(0, 0));
            }

            for (int i = 0; i < battleManager.EnemyCount; i++)
            {
                Assert.IsTrue( battleManager.IsEnemyCanMove(i));
            }
        }
        
        [Test]
        public void CheckCanAttackPlayers()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();

            foreach (var personageData in personageList.personage)
            {
                battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            }

            for (int i = 0; i < battleManager.PlayerCount; i++)
            {
                Assert.IsTrue( battleManager.IsPlayerCanAttack(i));
            }
        }
        
        [Test]
        public void CheckCanAttackEnemies()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();

            foreach (var personageData in personageList.personage)
            {
                battleManager.AddEnemy(personageList.personage[0], new Vector2Int(0, 0));
            }

            for (int i = 0; i < battleManager.EnemyCount; i++)
            {
                Assert.IsTrue( battleManager.IsEnemyCanAttack(i));
            }
        }
        
        [Test]
        public void CheckOccupiedLevel4X4Place()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();

            var setPosition = new Vector2Int(0, 0);
            battleManager.AddPlayer(personageList.personage[0], setPosition);
            battleManager.SetLevel(levelList.level[0]);

            Assert.IsFalse( battleManager.IsPositionFree(setPosition));
        }
        
        [Test]
        public void CheckFreeLevel4X4Places()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();

            var setPosition = new Vector2Int(0, 0);
            battleManager.AddPlayer(personageList.personage[0], setPosition);
            battleManager.SetLevel(levelList.level[0]);

            var levelDate = battleManager.GetLevelData();
            for (int i = 0; i < levelDate.x; i++)
            {
                for (int j = 0; j < levelDate.y; j++)
                {
                    var checkPosition = new Vector2Int(i, j);
                    if (checkPosition != setPosition) Assert.IsTrue(battleManager.IsPositionFree(checkPosition));
                }
            }
        }
        
        [Test]
        public void CheckMoveLineDirectPlayer()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(3, 0);
            
            battleManager.MovePlayer(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(moveList.Count, 1);
        }
        
        [Test]
        public void CheckMoveLineDirectEnemy()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(3, 0);
            
            battleManager.MoveEnemy(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(moveList.Count, 1);
        }
        
        [Test]
        public void CheckMoveBrokenDirectPlayer()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            
            battleManager.MovePlayer(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(moveList.Count, 2);
        }
        
        [Test]
        public void CheckMoveBrokenDirectEnemy()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            
            battleManager.MoveEnemy(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(moveList.Count, 2);
        }
        
        [Test]
        public void CheckMovePositionPlayer()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            battleManager.MovePlayer(0, newPosition, out List<Vector2Int> _);
            
            Assert.AreEqual(battleManager.GetPlayerPosition(0), newPosition);
        }
        
        [Test]
        public void CheckMovePositionEnemy()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(0, 0));
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            battleManager.MoveEnemy(0, newPosition, out List<Vector2Int> _);
            
            Assert.AreEqual(battleManager.GetEnemyPosition(0), newPosition);
        }
        
        [Test]
        public void CheckMoveDistancePlayer()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            var startPosition = new Vector2Int(0, 0);
            battleManager.AddPlayer(personageList.personage[0], startPosition);
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            battleManager.MovePlayer(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(battleManager.DistanceForMove(moveList), 4);
            
            battleManager.MovePlayer(0, startPosition, out moveList);
            
            Assert.AreEqual(battleManager.DistanceForMove(moveList), 4);
        }
        
        [Test]
        public void CheckMoveDistanceEnemy()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            var startPosition = new Vector2Int(0, 0);
            battleManager.AddEnemy(personageList.personage[0], startPosition);
            battleManager.SetLevel(levelList.level[0]);
            
            var newPosition = new Vector2Int(2, 2);
            battleManager.MoveEnemy(0, newPosition, out List<Vector2Int> moveList);
            
            Assert.AreEqual(battleManager.DistanceForMove(moveList), 4);
            
            battleManager.MoveEnemy(0, startPosition, out moveList);
            
            Assert.AreEqual(battleManager.DistanceForMove(moveList), 4);
        }
        
        [Test]
        public void CheckAttackPlayerEnemy_WarriorSword()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(1, 1));
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(2, 1));
            battleManager.SetLevel(levelList.level[0]);

            var healthBeforeAttack = battleManager.GetEnemyData(0).health;
            battleManager.AttackPlayer(0, 0);
            var healthAfterAttack = battleManager.GetEnemyData(0).health;
            
            Assert.IsTrue(healthAfterAttack < healthBeforeAttack);
        }
        
        [Test]
        public void CheckCanNotAttackPlayerEnemy_WarriorSword()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[0], new Vector2Int(0, 1));
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(2, 1));
            battleManager.SetLevel(levelList.level[0]);

            var healthBeforeAttack = battleManager.GetEnemyData(0).health;
            battleManager.AttackPlayer(0, 0);
            var healthAfterAttack = battleManager.GetEnemyData(0).health;
            
            Assert.IsTrue(healthAfterAttack == healthBeforeAttack);
        }

        [Test]
        public void CheckAttackPlayerEnemy_WarriorSpear()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[1], new Vector2Int(1, 1));
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(2, 1));
            battleManager.SetLevel(levelList.level[0]);

            var healthBeforeAttack = battleManager.GetEnemyData(0).health;
            battleManager.AttackPlayer(0, 0);
            var healthAfterAttack = battleManager.GetEnemyData(0).health;
            
            Assert.IsTrue(healthAfterAttack < healthBeforeAttack);
        }
        
        [Test]
        public void CheckCanNotAttackPlayerEnemy_WarriorSpear()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[1], new Vector2Int(0, 1));
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(3, 1));
            battleManager.SetLevel(levelList.level[0]);

            var healthBeforeAttack = battleManager.GetEnemyData(0).health;
            battleManager.AttackPlayer(0, 0);
            var healthAfterAttack = battleManager.GetEnemyData(0).health;
            
            Assert.IsTrue(healthAfterAttack == healthBeforeAttack);
        }
        
        [Test]
        public void CheckAttackPlayerEnemy_WarriorBow()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[2], new Vector2Int(1, 1));
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(3, 1));
            battleManager.SetLevel(levelList.level[3]);

            var healthBeforeAttack = battleManager.GetEnemyData(0).health;
            battleManager.AttackPlayer(0, 0);
            var healthAfterAttack = battleManager.GetEnemyData(0).health;
            
            Assert.IsTrue(healthAfterAttack < healthBeforeAttack);
        }
        
        [Test]
        public void CheckCanNotAttackPlayerEnemy_WarriorBow()
        {
            battleManager.ClearPlayers();
            battleManager.ClearEnemies();
            
            battleManager.AddPlayer(personageList.personage[2], new Vector2Int(0, 1));
            battleManager.AddEnemy(personageList.personage[0], new Vector2Int(7, 1));
            battleManager.SetLevel(levelList.level[3]);

            var healthBeforeAttack = battleManager.GetEnemyData(0).health;
            battleManager.AttackPlayer(0, 0);
            var healthAfterAttack = battleManager.GetEnemyData(0).health;
            
            Assert.IsTrue(healthAfterAttack == healthBeforeAttack);
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
