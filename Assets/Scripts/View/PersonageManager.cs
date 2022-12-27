using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace View
{
    public class PersonageManager : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private int _personageNumberDate = -1;

        [Header("Main")]
        [SerializeField] private StepManager _placeStep;
        [SerializeField] private Transform _mainMesh;
        [SerializeField] private float _pointerEnterScale = 1.2f;
        [SerializeField] private float _pointerEnterScaleTime = 0.5f;

        [Header("Move")] 
        [SerializeField] private float _movePointSpeed = 0.5f;
        
        private Sequence sequence;
        
        public event Action<int, int> OnOverPersonageEvent;
        public event Action<int, int> OnClickPersonageEvent;
        
        public int PersonageNumberDate => _personageNumberDate;
        public Vector2Int PlaceStep => _placeStep.PlaceStep;

        public void MoveToPlaceStep(StepManager newStepManager, List<Vector3> moveVector)
        {
            _placeStep = newStepManager;
            if (sequence == null || !sequence.active)
            {
                foreach (var positionMove in moveVector)
                {
                    sequence.Append(transform.DOMove(positionMove, _movePointSpeed).SetEase(Ease.Linear));
                }
                sequence.Play();
            }
        }
        
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
            
            OnOverPersonageEvent?.Invoke(PlaceStep.x, PlaceStep.y);
        }
        
        public void OnPointerExit()
        {
            OnOverPersonageEvent?.Invoke(PlaceStep.x, PlaceStep.y);
        }
        
        public void OnPointerClick()
        {
            OnClickPersonageEvent?.Invoke(PlaceStep.x, PlaceStep.y);
        }
    }
}
