using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using KaitoMajima.Utils;

namespace KaitoMajima.TweenControllers
{
    public class LightFade : TweenController
    {
        [SerializeField] private Light2D _light2D;
        [SerializeField] [Range(0,1)] private float _fadeEndValue;

        protected override Tween TriggerTween()
        {
            _mainTween.Kill(true);

            Tween tween = DOTween.To(() => _light2D.intensity, x => _light2D.intensity = x, _fadeEndValue.Map(0, 1, 0, _light2D.intensity), _tweenSettings.duration);

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = DOTween.To(() => _light2D.intensity, x => _light2D.intensity = x, _fadeEndValue.Map(0, 1, 0, _light2D.intensity), _tweenSettings.duration).From();

            return tween;
        }

        protected override Tween RevertTween()
        {
            _mainTween.Kill(true);

            Tween tween = DOTween.To(() => _light2D.intensity, x => _light2D.intensity = x, _fadeEndValue.Map(0, 1, _light2D.intensity, 0), _tweenSettings.duration);

            if(_tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = DOTween.To(() => _light2D.intensity, x => _light2D.intensity = x, _fadeEndValue.Map(0, 1, _light2D.intensity, 0), _tweenSettings.duration).From();

            return tween;
        }
    }
}