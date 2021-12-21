using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima.Radar
{
    [Serializable]
    public class GradeStats : IEnumerable
    {
        [SerializeField] private List<GradeStat> _stats;

        public int Count { get => _stats.Count;}
        public void AddStat(string name, GradeStat.Grade value)
        {
            var stat = new GradeStat(name, value);
            _stats.Add(stat);
        }

        public void ChangeStat(string name, GradeStat.Grade value)
        {
            var stat = _stats.Find(x => x.name == name);
            stat.value = value;
        }

        public GradeStat GetStat(string name)
        {
            var stat = _stats.Find(x => x.name == name);
            return stat;
        }
        public GradeStat GetStat(int index)
        {
            return _stats[index];
        }
        public IEnumerator GetEnumerator()
        {
            return _stats.GetEnumerator();
        }
    }

    [Serializable]
    public class ValueStats : IEnumerable
    {
        public int statsMin;
        public int statsMax;
        [SerializeField] private List<ValueStat> _stats;

        public void AddStat(string name, int value)
        {
            var stat = new ValueStat(name, value);
            _stats.Add(stat);
        }

        public void ChangeStat(string name, int value)
        {
            var stat = _stats.Find(x => x.name == name);
            stat.value = value;
        }

        public ValueStat GetStat(string name)
        {
            var stat = _stats.Find(x => x.name == name);
            return stat;
        }
        public ValueStat GetStat(int index)
        {
            return _stats[index];
        }
        public IEnumerator GetEnumerator()
        {
            return _stats.GetEnumerator();
        }
    }
    public abstract class Stat
    {
        public string name;
        public Stat(string name)
        {
            this.name = name;
        }
    }
    [Serializable]
    public class GradeStat : Stat
    {
        public enum Grade
        {
            None = 0,
            E = 1,
            D = 2,
            C = 3,
            B = 4,
            A = 5,
            Infinity = 6
        }

        public Grade value;

        public GradeStat(string name, Grade value) : base(name)
        {
            this.name = name;
            this.value = value;
        }
    }
    [Serializable]
    public class ValueStat : Stat
    {
        public int value;
        
        public ValueStat(string name, int value) : base(name)
        {
            this.name = name;
            this.value = value;
        }
    }
}