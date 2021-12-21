using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KaitoMajima.Radar
{
    public class RadarChart : MonoBehaviour
    {
        [Header("Local Dependencies")]
        [SerializeField] private GradeStats _stats;
        public GradeStats Stats 
        { 
            get => _stats;
            set 
            {
                _stats = value;
                onStatsChanged?.Invoke();
            }
        }
        public Action onStatsChanged;

        [Header("Debug")]
        [SerializeField] private bool _randomizeIncludeNone = true;
        [SerializeField] private bool _randomizeIncludeInfinity = true; 

        [ContextMenu("Debug Randomize Stats")]
        public void RandomizeStats()
        {
            foreach (var stat in _stats)
            {
                var gradeStat = (GradeStat)stat;

                var minRange = _randomizeIncludeNone ? 0 : 1;
                var maxRange = _randomizeIncludeInfinity ? Enum.GetNames(typeof(GradeStat.Grade)).Length
                 : Enum.GetNames(typeof(GradeStat.Grade)).Length - 1;

                gradeStat.value = (GradeStat.Grade)Random.Range(minRange, maxRange);

            }
            onStatsChanged?.Invoke();
        }
    }
}
