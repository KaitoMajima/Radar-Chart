using System;
using System.Collections.Generic;
using DG.Tweening;
using KaitoMajima.TweenControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KaitoMajima.Radar
{
    public class RadarChartView : MonoBehaviour
    {
        [SerializeField] private RadarChart _radarChart;

        [Header("Scene Dependencies")]

        [SerializeField] private CanvasRenderer _meshCanvasRenderer;
        [SerializeField] private Material _meshMaterial;

        [SerializeField] private RectTransform _highestRadarPoint;

        [SerializeField] private List<Image> _statsBars;
        [SerializeField] private List<TMP_Text> _statNameTextComponents;
        [SerializeField] private List<TMP_Text> _statValueTextComponents;

        [Header("Visuals")]
        private List<Tween> _scalingTweens = new List<Tween>();
        [SerializeField] private TweenSettings _scalingTweenSettings = TweenControllers.TweenSettings.Default;

        private void OnEnable()
        {
            InitiateVisuals();
            _radarChart.onStatsChanged += ProcessVisuals;
        }
        private void OnDisable()
        {
            _radarChart.onStatsChanged -= ProcessVisuals;
        }
        private void InitiateVisuals()
        {   
            int index = 0;
            foreach (var stat in _radarChart.Stats)
            {
                if(!CheckIndexHasntSurpassedList(index, _statNameTextComponents))
                    return;
                
                var gradeStat = (GradeStat)stat;

                var statNameTextComponent = _statNameTextComponents[index];

                statNameTextComponent.SetText(gradeStat.name);
                index++;
            }

            
        }
        [ContextMenu("Debug Visuals")]
        private void ProcessVisuals()
        {
            _scalingTweens.Clear();

            int index = 0;
            
            foreach (var stat in _radarChart.Stats)
            {
                if(!CheckIndexHasntSurpassedList(index, _statsBars))
                    return;

                var statBarRectTransform = _statsBars[index].rectTransform;

                if(!CheckIndexHasntSurpassedList(index, _statValueTextComponents))
                    return;

                var statValueTextComponent = _statValueTextComponents[index];

                var gradeStat = (GradeStat)stat;

                SetGradeText(gradeStat, statValueTextComponent);
                
                index++;
            }
            CreateTweensForScaling();
        }


        private void CreateTweensForScaling()
        {
            // var scalingTween = (Tween)statBarRectTransform.DOScaleY((float)gradeStat.value / (Enum.GetNames(typeof(GradeStat.Grade)).Length - 1), _scalingTweenSettings.duration);
            // _scalingTweenSettings.ApplyTweenSettings(ref scalingTween);
            // _scalingTweens.Add(scalingTween);

            Mesh mesh;
            Vector3[] verts;
            Vector2[] uvs;
            int[] tris;
            InitializeMeshData(out mesh, out verts, out uvs, out tris);

            DefineVertices(_radarChart.Stats, verts);
            DefineTriangles(tris);
            SetMeshData(mesh, verts, uvs, tris);
        }

        private void InitializeMeshData(out Mesh mesh, out Vector3[] verts, out Vector2[] uvs, out int[] tris)
        {
            var gradeStats = _radarChart.Stats;

            mesh = new Mesh();
            verts = new Vector3[gradeStats.Count + 1];
            uvs = new Vector2[gradeStats.Count + 1];
            tris = new int[gradeStats.Count * 3];
        }
        private void DefineVertices(GradeStats gradeStats, Vector3[] verts)
        {
            verts[0] = Vector3.zero;

            var angleIncrement = 360 / gradeStats.Count;
            var highestRadarPointYAxis = _highestRadarPoint.anchoredPosition.y;
            for (int i = 1; i < verts.Length; i++)
            {
                var statIndex = i - 1;
                var gradeStat = gradeStats.GetStat(statIndex);
                var normalizedGradeStatValue = (float)gradeStat.value / (Enum.GetNames(typeof(GradeStat.Grade)).Length - 1);
                
                if(normalizedGradeStatValue == 0)
                    normalizedGradeStatValue = 0.05f;
                
                verts[i] = Quaternion.Euler(0, 0, statIndex * -angleIncrement) * Vector3.up * (highestRadarPointYAxis * normalizedGradeStatValue);
            }
        }
        private void DefineTriangles(int[] tris)
        {
            for (int i = 0, vertexIndex = 0; i < tris.Length; i++)
            {
                var isTriangleFirstVertex = i % 3 == 0;
                var isTriangleSecondVertex = i % 3 == 1;
                var isLastVertexFromLastTriangle = i == tris.Length - 1;

                if (isTriangleFirstVertex)
                {
                    tris[i] = 0;
                    continue;
                }
                if(isLastVertexFromLastTriangle)
                {
                    tris[i] = 1;
                    break;
                }

                if (vertexIndex == 0 || !isTriangleSecondVertex)
                    vertexIndex++;

                tris[i] = vertexIndex;
            }
        }
        private void SetMeshData(Mesh mesh, Vector3[] verts, Vector2[] uvs, int[] tris)
        {
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;

            _meshCanvasRenderer.SetMesh(mesh);
            _meshCanvasRenderer.SetMaterial(_meshMaterial, null);
        }
        private void SetGradeText(GradeStat gradeStat, TMP_Text statValueTextComponent)
        {
            var gradeStatGrade = gradeStat.value.ToString();

            switch (gradeStatGrade)
            {
                case "None":
                    gradeStatGrade = string.Empty;
                    break;
                case "Infinity":
                    gradeStatGrade = "∞";
                    break;
            }

            statValueTextComponent.SetText(gradeStatGrade);
        }

        private bool CheckIndexHasntSurpassedList<T>(int index, List<T> list)
        {
            if(index > list.Count)
            {
                Debug.LogError("The specified index has surpassed the List's capacity!");
                return false;
            }
            return true;
        }
    }
}
