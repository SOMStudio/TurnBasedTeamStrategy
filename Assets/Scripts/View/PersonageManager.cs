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
        [SerializeField] private float _movePositionTime = 0.5f;

        [Header("Attack")]
        [SerializeField] private float _attackTime = 0.5f;

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

            maxPoints = personageDate.actionPointMax;
            currentPoints = personageDate.actionPoint;
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

            var sequence = DOTween.Sequence();
            foreach (var positionMove in moveVector)
            {
                sequence.Append(transform.DOMove(positionMove, _movePositionTime).SetEase(Ease.Linear));
            }
            if (onCompleteMove != null) sequence.AppendCallback(onCompleteMove.Invoke);
            sequence.Play();
        }

        public void Attack(Action onCompleteAttack = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_mainMesh.DORotate(new Vector3(0, 180, 0), _attackTime, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(2, LoopType.Yoyo));
            if (onCompleteAttack != null) sequence.AppendCallback(onCompleteAttack.Invoke);
            sequence.Play();
        }

        public void Death(Action onCompleteAttack = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_mainMesh.DORotate(new Vector3(90, 0, 0), _attackTime, RotateMode.FastBeyond360).SetEase(Ease.Linear))
                .AppendCallback(() => gameObject.SetActive(false));
            if (onCompleteAttack != null) sequence.AppendCallback(onCompleteAttack.Invoke);
            sequence.Play();
        }

        private void PointerEnterAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_mainMesh.DOScale(_pointerEnterScale, _pointerEnterScaleTime)
                .SetEase(Ease.Linear)
                .SetLoops(2, LoopType.Yoyo));
            sequence.Play();
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
