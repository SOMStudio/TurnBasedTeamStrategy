using System;
using UnityEngine;

namespace View.UI.Level
{
    public class LevelUiManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private TopPanelManager _topPanel;
        [SerializeField] private ResultWindowManager _resultWindow;
        [SerializeField] private CanvasGroup _resultWindowCanvasGroup;

        private bool isMenuActive = false;

        public event Action ClickNextTurnButtonEvent;
        
        public bool IsMenuActive => isMenuActive;
        
        public void InitInformation(int leftTeamHealth, int rightEnemyHealth)
        {
            _topPanel.InitState(leftTeamHealth, rightEnemyHealth);
        }
        
        public void UpdateInformation(int leftTeamHealth, int rightEnemyHealth)
        {
            _topPanel.UpdateLeftTeamInformation(leftTeamHealth);
            _topPanel.UpdateRightTeamInformation(rightEnemyHealth);
        }

        public void ShowResultWindow(string message)
        {
            _resultWindow.SetTExt(message);
            
            _resultWindowCanvasGroup.alpha = 1;
            _resultWindowCanvasGroup.interactable = true;
            _resultWindowCanvasGroup.blocksRaycasts = true;

            isMenuActive = true;
        }

        public void TurnButtonClickHandler()
        {
            ClickNextTurnButtonEvent?.Invoke();
        }

        public void LockNextTurnButton()
        {
            _topPanel.SetNextTurnButtonInteractiveState(false);
        }
        
        public void UnLockNextTurnButton()
        {
            _topPanel.SetNextTurnButtonInteractiveState(true);
        } 
    }
}
