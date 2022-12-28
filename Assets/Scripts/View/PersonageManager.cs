using System;
using System.Collections.Generic;
using DG.Tweening;
using Model;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        [SerializeField] private float _movePositionSpeed = 0.5f;

        [Header("State")]
        [SerializeField] private TMP_Text _personageTypeText;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Slider _pointsSlider;
        [SerializeField] private TMP_Text _pointsText;
        
        private int maxHealth;
        private int currentHealth;

        private int maxPoints;
        private int currentPoints;
        
        private Sequence sequence;
        
        public event Action<int, int> OnOverPersonageEvent;
        public event Action<int, int> OnClickPersonageEvent;
        
        public int PersonageNumberDate => _personageNumberDate;
        public Vector2Int PlaceStep => _placeStep.PlaceStep;

        public void SetStartState(PersonageData personageDate)
        {
            _personageTypeText.text = personageDate.name;
            
            maxHealth = personageDate.health;
            currentHealth = maxHealth;
            UpdateHealthUI();

            maxPoints = personageDate.actionPoint;
            currentPoints = maxPoints;
            _pointsText.text = $"{currentPoints}/{maxPoints}";
            UpdatePointsUI();
        }

        public void UpdateState(PersonageData personageData)
        {
            currentHealth = personageData.health;
            UpdateHealthUI();

            currentPoints = personageData.actionPoint;
            UpdatePointsUI();
        }

        private void UpdateHealthUI()
        {
            _healthSlider.value = (float)currentHealth / maxHealth;
            _healthText.text = $"{currentHealth}/{maxHealth}";
        }
        
        private void UpdatePointsUI()
        {
            _pointsSlider.value = (float)currentPoints / maxPoints;
            _pointsText.text = $"{currentPoints}/{maxPoints}";
        }
        
        public void MoveToPosition(StepManager newStepManager, List<Vector3> moveVector, Action onCompleteMove = null)
        {
            _placeStep = newStepManager;
            if (sequence == null || !sequence.active)
            {
                sequence = DOTween.Sequence();
                foreach (var positionMove in moveVector)
                {
                    sequence.Append(transform.DOMove(positionMove, _movePositionSpeed).SetEase(Ease.Linear));
                }
                if (onCompleteMove != null) sequence.AppendCallback(onCompleteMove.Invoke);
                sequence.Play();
            }
        }
        
        public void Attack(Action onCompleteAttack = null)
        {
            //add attack animation
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
