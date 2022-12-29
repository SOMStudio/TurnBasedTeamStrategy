using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI.Level
{
    public class TopPanelManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Slider _leftTeamSlider;
        [SerializeField] private TMP_Text _leftTeamText;
        [SerializeField] private Slider _rightTeamSlider;
        [SerializeField] private TMP_Text _rightTeamText;
        [SerializeField] private Button _nextTurnButton;

        private int maxHealthLeftTeam;
        private int currentHealthLeftTeam;
        private int maxHealthRightTeam;
        private int currentHealthRightTeam;
        
        public void InitState(int teamLeftHealth, int teamRightHealth)
        {
            maxHealthLeftTeam = teamLeftHealth;
            currentHealthLeftTeam = maxHealthLeftTeam;
            UpdateLeftTeamInformation();

            maxHealthRightTeam = teamRightHealth;
            currentHealthRightTeam = maxHealthRightTeam;
            UpdateRightTeamInformation();
        }
        
        public void UpdateLeftTeamInformation(int teamLeftHealth)
        {
            currentHealthLeftTeam = teamLeftHealth;
            UpdateLeftTeamInformation();
        }
        
        public void UpdateRightTeamInformation(int teamRightHealth)
        {
            currentHealthRightTeam = teamRightHealth;
            UpdateRightTeamInformation();
        }

        private void UpdateLeftTeamInformation()
        {
            _leftTeamSlider.value = (float)currentHealthLeftTeam / maxHealthLeftTeam;
            _leftTeamText.text = $"{currentHealthLeftTeam}/{maxHealthLeftTeam}";
        }
        
        private void UpdateRightTeamInformation()
        {
            _rightTeamSlider.value = (float)currentHealthRightTeam / maxHealthRightTeam;
            _rightTeamText.text = $"{currentHealthRightTeam}/{maxHealthRightTeam}";
        }

        public void SetNextTurnButtonInteractiveState(bool setState)
        {
            _nextTurnButton.interactable = setState;
        }
    }
}
