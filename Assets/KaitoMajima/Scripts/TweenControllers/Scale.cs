using UnityEngine;
using DG.Tweening;

namespace KaitoMajima.TweenControllers
{
    public class Scale : TweenController
    {
        [SerializeField] private Transform _tweeningTransform;

        private Vector3 _originalScale;
        [SerializeField] private Vector3 _scaleTarget;

        protected override Tween TriggerTween()
        {
            _mainTween.Kill(true);
            
            _originalScale = _tweeningTransform.localScale;

            Tween tween = _tweeningTransform.DOScale(_scaleTarget, _tweenSettings.duration);

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _tweeningTransform.DOScale(_scaleTarget, _tweenSettings.duration).From();

            return tween;
        }

        protected override Tween RevertTween()
        {
            _mainTween.Kill(true);

            Tween tween = _tweeningTransform.DOScale(_originalScale, _tweenSettings.duration);

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _tweeningTransform.DOScale(_originalScale, _tweenSettings.duration).From();

            return tween;
        }

    }
}