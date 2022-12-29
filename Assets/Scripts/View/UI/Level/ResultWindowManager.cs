using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View.UI.Level
{
    public class ResultWindowManager : MonoBehaviour
    {
        private const string MenuSceneName = "Menu";
        
        [Header("Main")]
        [SerializeField] private TMP_Text _informationText;
        [SerializeField] private Button _confirmButton;

        private void Start()
        {
            _confirmButton.onClick.AddListener(ClickConfirmButtonHandler);
        }

        public void SetTExt(string message)
        {
            _informationText.text = message;
        }

        private void ClickConfirmButtonHandler()
        {
            SceneManager.LoadScene(MenuSceneName);
        }
    }
}
