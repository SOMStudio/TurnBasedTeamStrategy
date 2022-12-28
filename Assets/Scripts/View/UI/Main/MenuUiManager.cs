using System.Collections;
using Controller;
using UnityEngine;

namespace View.UI.Main
{
    public class MenuUiManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Transform _scrollContentObject;
        [SerializeField] private LevelItemManager _levelItemPrefab;
        
        private void Start()
        {
            StartCoroutine(UpdateInformation());
        }

        private IEnumerator UpdateInformation()
        {
            var levelList = LoadManager.GetLevelList();

            foreach (var levelData in levelList.level)
            {
                var levelItem = Instantiate(_levelItemPrefab, _scrollContentObject);
                levelItem.InitState(levelData.name, levelData.scene, levelData.sprite);
                
                yield return null;
            }
        }
    }
}
