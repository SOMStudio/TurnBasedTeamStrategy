using Controller;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View.UI.Main
{
    public class LevelItemManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Image _levelImage;
        [SerializeField] private TMP_Text _levelNameText;

        private string levelSceneName;
        
        public void InitState(string levelName, string sceneName, string spriteName)
        {
            _levelNameText.text = levelName;
            levelSceneName = sceneName;
            _levelImage.sprite = LoadManager.GetSpriteLevel(spriteName);
        }

        public void LoadLevelHandler()
        {
            SceneManager.LoadScene(levelSceneName);
        }
    }
}
