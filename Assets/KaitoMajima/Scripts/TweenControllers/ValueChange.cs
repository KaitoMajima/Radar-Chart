using UnityEngine;
using DG.Tweening;
using System;

namespace KaitoMajima.TweenControllers
{
    public class ValueChange : TweenController
    {
        [SerializeField] private float _endValue;

        public float Value {get; private set;}

        private float _originalValue;
        public Action<float> onValueChange;
        protected override Tween TriggerTween()
        {
            _mainTween.Kill(true);

            _originalValue = Value;

            Tween tween = DOTween.To(() => Value, x => Value = x, _endValue, _tweenSettings.duration).
                OnUpdate(() => onValueChange?.Invoke(Value));

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = DOTween.To(() => Value, x => Value = x, _endValue, _tweenSettings.duration).
                OnUpdate(() => onValueChange?.Invoke(Value)).From();

            return tween;
        }

        protected override Tween RevertTween()
        {
            _mainTween.Kill(true);

            Tween tween = DOTween.To(() => Value, x => Value = x, _originalValue, _tweenSettings.duration).
                OnUpdate(() => onValueChange?.Invoke(Value));

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = DOTween.To(() => Value, x => Value = x, _originalValue, _tweenSettings.duration).
                OnUpdate(() => onValueChange?.Invoke(Value)).From();

            return tween;
        }
    }
}