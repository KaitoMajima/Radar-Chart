using System;
using System.Collections.Generic;
using DG.Tweening;
using KaitoMajima.TweenControllers;
using TMPro;
using UnityEngine;

namespace KaitoMajima.Radar
{
    public class RadarChartView : MonoBehaviour
    {
        [Header("Local Dependencies")]
        [SerializeField] private RadarChart _radarChart;

        [Header("Scene Dependencies")]
        [SerializeField] private CanvasRenderer _meshCanvasRenderer;
        [SerializeField] private RectTransform _highestRadarPoint;
        [SerializeField] private List<TMP_Text> _statNameTextComponents;
        [SerializeField] private List<TMP_Text> _statValueTextComponents;

        [Header("Project Dependencies")]
        [SerializeField] private Material _meshMaterial;

        [Header("Animations")]
        private List<Tween> _vertexMoveTweens = new List<Tween>();
        [SerializeField] private float _initialTweeningChartDelay = 0.5f;
        [SerializeField] private float _sequentialTweeningChartDelay = 0.25f;
        [SerializeField] private TweenSettings _vertexMoveTweenSettings = TweenControllers.TweenSettings.Default;
        
        [Header("Settings")]
        [SerializeField] private bool _useApproximateLevelsForNone;
        private Mesh _radarMesh;
        private Vector3[] _radarMeshVertices;
        private Vector2[] _radarMeshUVs;
        private int[] _radarMeshTriangles;

        private void OnEnable()
        {
            InitiateVisuals();
            ProcessVisuals();
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
            
            InitializeMeshData(out _radarMesh, out _radarMeshVertices, out _radarMeshUVs, out _radarMeshTriangles);
            
        }
        [ContextMenu("Debug Visuals")]
        private void ProcessVisuals()
        {
            _vertexMoveTweens.Clear();

            int index = 0;
            
            foreach (var stat in _radarChart.Stats)
            {
                if(!CheckIndexHasntSurpassedList(index, _statValueTextComponents))
                    return;

                var statValueTextComponent = _statValueTextComponents[index];

                var gradeStat = (GradeStat)stat;

                SetGradeText(gradeStat, statValueTextComponent);
                
                index++;
            }
            CreateMesh();

            void SetGradeText(GradeStat gradeStat, TMP_Text statValueTextComponent)
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
        }

        private void CreateMesh()
        {
            DefineVertices(_radarChart.Stats, _radarMeshVertices);
            DefineTriangles(_radarMeshTriangles);
            SetMeshData(_radarMesh, _radarMeshVertices, _radarMeshUVs, _radarMeshTriangles);
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

                if (normalizedGradeStatValue == 0 && _useApproximateLevelsForNone)
                    normalizedGradeStatValue = 0.05f;

                var result = Quaternion.Euler(0, 0, statIndex * -angleIncrement) * Vector3.up * (highestRadarPointYAxis * normalizedGradeStatValue);

                var vertex = verts[i];
                var index = i;

                var vertexMoveTween = (Tween)DOTween.To(() => vertex, x => vertex = x, result, _vertexMoveTweenSettings.duration)
                   .OnUpdate(() => SetTweenVertices(_radarMesh, index, vertex));

                _vertexMoveTweenSettings.ApplyTweenSettings(ref vertexMoveTween);
                vertexMoveTween.SetDelay(_initialTweeningChartDelay + statIndex * _sequentialTweeningChartDelay);
                _vertexMoveTweens.Add(vertexMoveTween);

                InitializeVertex(i);
            }
            void SetTweenVertices(Mesh mesh, int index, Vector3 vertex)
            {
                _radarMeshVertices[index] = vertex;
                mesh.vertices = _radarMeshVertices;
                _radarMesh = mesh;
                
                _meshCanvasRenderer.SetMesh(mesh);
                _meshCanvasRenderer.SetMaterial(_meshMaterial, null);
            }
            void InitializeVertex(int index)
            {
                _radarMeshVertices[index] = Vector3.one;
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
