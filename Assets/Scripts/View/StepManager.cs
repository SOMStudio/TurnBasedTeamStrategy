using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class StepManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [Header("Base")]
        [SerializeField] private int _x;
        [SerializeField] private int _y;
        
        [Header("Main")]
        [SerializeField] private Transform _mainMesh;
        [SerializeField] private float _pointerEnterScale = 1.2f; 

        public Vector2Int PlaceStep => new Vector2Int(_x, _y);

        public event Action<int, int> OnOverStepEvent;
        public event Action<int, int> OnClickStepEvent;

        private void SetPointerEnterState()
        {
            
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnOverStepEvent?.Invoke(_x, _y);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickStepEvent?.Invoke(_x, _y);
        }
    }
}
