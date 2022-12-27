using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class StepManager : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private int _x;
        [SerializeField] private int _y;
        
        [Header("Main")]
        [SerializeField] private Transform _mainMesh;
        [SerializeField] private float _pointerEnterScale = 1.2f;
        [SerializeField] private float _pointerEnterScaleTime = 0.5f;

        private Sequence sequence;

        private bool playOnPointEnterAnimation = false;
        
        public Vector2Int PlaceStep => new Vector2Int(_x, _y);

        public event Action<int, int> OnOverStepEvent;
        public event Action<int, int> OnClickStepEvent;

        private void PointerEnterAnimation()
        {
            if (sequence == null || !sequence.active)
            {
                sequence = DOTween.Sequence();
                sequence.Append(_mainMesh.DOScale(_pointerEnterScale, _pointerEnterScaleTime)
                    .SetEase(Ease.Linear)
                    .SetLoops(2, LoopType.Yoyo));
                sequence.Play();
            }
        }
        
        public void OnPointerEnter()
        {
            PointerEnterAnimation();
            
            OnOverStepEvent?.Invoke(_x, _y);
        }

        public void OnPointerExit()
        {
            
        }
        
        public void OnPointerClick()
        {
            OnClickStepEvent?.Invoke(_x, _y);
        }
    }
}
