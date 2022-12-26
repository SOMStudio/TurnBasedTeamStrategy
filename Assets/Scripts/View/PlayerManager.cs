using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class PlayerManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [Header("Base")]
        [SerializeField] private int _playerNumberDate = -1;

        [Header("Main")]
        [SerializeField] private StepManager _placeStep;
        
        public event Action<int, int> OnOverPlayerEvent;
        public event Action<int, int> OnClickPlayerEvent;
        
        public int PlayerNumberDate => _playerNumberDate;
        public Vector2Int PlaceStep => _placeStep.PlaceStep;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnOverPlayerEvent?.Invoke(PlaceStep.x, PlaceStep.y);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickPlayerEvent?.Invoke(PlaceStep.x, PlaceStep.y);
        }
    }
}
