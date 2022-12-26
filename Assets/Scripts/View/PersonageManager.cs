using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace View
{
    public class PersonageManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [FormerlySerializedAs("_playerNumberDate")]
        [Header("Base")]
        [SerializeField] private int _personageNumberDate = -1;

        [Header("Main")]
        [SerializeField] private StepManager _placeStep;
        
        public event Action<int, int> OnOverPlayerEvent;
        public event Action<int, int> OnClickPlayerEvent;
        
        public int PersonageNumberDate => _personageNumberDate;
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
