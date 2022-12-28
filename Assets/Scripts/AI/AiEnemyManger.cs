using Controller;
using UnityEngine;

namespace AI
{
    public class AiEnemyManger : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private bool _active;

        private BattleManager battleManager;
        
        public void InitState(BattleManager battleManagerSet)
        {
            battleManager = battleManagerSet;
        }

        public void ActivateAi()
        {
            _active = true;
        }
        
        private void Update()
        {
            if (!_active) return;

            for (int i = 0; i < battleManager.EnemyCount; i++)
            {
                
            }
        }
    }
}
