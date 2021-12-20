using DG.Tweening;
using UnityEngine;

namespace KaitoMajima.TweenControllers
{
    public class Rotate : TweenController
    {
        [SerializeField] private Transform _tweeningTransform;

        private Vector3 _originalRotation;
        [SerializeField] private Vector3 _rotatingTarget;

        protected override Tween TriggerTween()
        {
            _mainTween.Kill(true);

            _originalRotation = _tweeningTransform.localEulerAngles;

            Tween tween = _tweeningTransform.DOLocalRotate(_rotatingTarget, _tweenSettings.duration, RotateMode.FastBeyond360);

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _tweeningTransform.DOLocalRotate(_rotatingTarget, _tweenSettings.duration, RotateMode.FastBeyond360).From();

            return tween;
        }

        protected override Tween RevertTween()
        {
            _mainTween.Kill(true);

            Tween tween = _tweeningTransform.DOLocalRotate(_originalRotation, _tweenSettings.duration, RotateMode.FastBeyond360);

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _tweeningTransform.DOLocalRotate(_originalRotation, _tweenSettings.duration, RotateMode.FastBeyond360).From();

            return tween;
        }

    }
}