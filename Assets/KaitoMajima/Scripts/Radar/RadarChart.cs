using System;
using DG.Tweening;
using KaitoMajima.TweenControllers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace KaitoMajima.Radar
{
    public class RadarChart : MonoBehaviour
    {
        [SerializeField] private GradeStats _stats;

        [SerializeField] private Image _statsBar;
        public GradeStats Stats 
        { 
            get => _stats;
            set 
            {
                _stats = value;
                UpdateVisuals();
            }
        }
        [Header("Visuals")]
        private Tween _scalingTween;
        [SerializeField] private TweenSettings _scalingTweenSettings = TweenControllers.TweenSettings.Default;

        [Header("Debug")]
        [SerializeField] private bool _randomizeIncludeNone = true;
        [SerializeField] private bool _randomizeIncludeInfinity = true; 

        [ContextMenu("Debug Visuals")]
        private void UpdateVisuals()
        {
            var statsRectTransform = _statsBar.rectTransform;

            var powerStat = _stats.GetStat("Destructive Power");

            _scalingTween = statsRectTransform.DOScaleY((float)powerStat.value / (Enum.GetNames(typeof(GradeStat.Grade)).Length - 1), _scalingTweenSettings.duration);
            _scalingTweenSettings.ApplyTweenSettings(ref _scalingTween);
        }
        [ContextMenu("Debug Randomize Stats")]
        private void RandomizeStats()
        {
            foreach (var stat in _stats)
            {
                GradeStat gradeStat = (GradeStat)stat;

                var minRange = _randomizeIncludeNone ? 0 : 1;
                var maxRange = _randomizeIncludeInfinity ? Enum.GetNames(typeof(GradeStat.Grade)).Length
                 : Enum.GetNames(typeof(GradeStat.Grade)).Length - 1;

                gradeStat.value = (GradeStat.Grade)Random.Range(minRange, maxRange);

            }
            UpdateVisuals();
        }
    }
}
