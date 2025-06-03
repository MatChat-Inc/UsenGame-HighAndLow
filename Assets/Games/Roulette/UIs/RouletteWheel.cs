// Created by LunarEclipse on 2024-6-3 9:33.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Luna.Extensions.Unity;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

namespace USEN.Games.Roulette
{
    public class RouletteWheel : MonoBehaviour
    {
        [SerializeField]
        private RouletteData rouletteData; // Default data
        
        public RouletteData RouletteData
        {
            get => rouletteData;
            set
            {
                rouletteData = value;
                DrawRouletteWheel();
            }
        }
        
        [Header("Wheel Settings")]
        // Radius of the wheel in meters.
        // If the wheel is too small, you should set render mode of the canvas to "Screen Space - Camera" and adjust the scale factor to 9.353078.
        public float radius = 3.2f;
        public int segmentsPerSector = 10;
        public float sectorInterval = 0.05f;
        public float angleOffset = 180f;
        
        public bool clockwise = true;

        public Material material;
        
        [Header("Border Settings")]
        public bool drawBorder = false;             // whether to draw a border around the sectors
        public float borderWidth = 0.2f;           // thickness of the border line
        public Color borderColor = Color.white; // color of the border
        // public Material borderMaterial;             // assign a simple unlit material
        
        [Header("Text Settings")]
        public TMP_FontAsset font;
        public float fontSize = 2.5f;
        public float outlineWidth = 0.3f;
        public float textDistanceFromCenter = 2f;
        
        [Header("Spin Settings")]
        public float spinSpeed = 360f; // Spin speed in degrees per second
        public float spinDuration = 4f; // Duration of the spin in seconds
        
        /// Animation curve to control the spin speed, which only works for `Spin(duration)` method.
        public AnimationCurve spinCurve; 
        
        
        // Event triggered when the spin starts.
        public event Action OnSpinStart;
        
        // Event triggered when the spin ends, passing the index of the winning sector and its content.
        public event Action<int, string> OnSpinEnd;
        

        private Canvas _canvas;

        private bool _isSpinning = false;
        private float _totalAngle;
        private float _startAngle;
        
        private bool _stopSpin = false;
        

        public List<RouletteSector> Sectors => RouletteData?.sectors;
        
        public bool IsSpinning => _isSpinning;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>()?.rootCanvas;
        }

        private void Start()
        {
            if (Sectors != null && Sectors.Count > 0)
                DrawRouletteWheel();
        }
        
        /// Spin the wheel for a certain duration.
        public void Spin(float duration = 0f)
        {
            if (!_isSpinning)
            {
                StartCoroutine(Spining(duration > 0 ? duration : spinDuration));
            }
        }
        
        /// Start spinning and wait for stop signal.
        public void StartSpin()
        {
            if (!_isSpinning)
            {
                StartCoroutine(Spining());
            }
        }
        
        /// Stop spinning the wheel after calling `StartSpin()`.
        public void StopSpin()
        {
            _stopSpin = true;
        }
        
        /// Coroutine to spin the wheel for a certain duration.
        private IEnumerator Spining(float duration)
        {
            _isSpinning = true;
            
            OnSpinStart?.Invoke();
            
            float elapsedTime = 0f;
            float angle = 0f;

            // Randomly determine the target sector
            int targetSectorIndex = Random.Range(0, Sectors.Count);
            float targetAngle = (clockwise ? _totalAngle * targetSectorIndex : 360f - targetSectorIndex * _totalAngle) + angleOffset;

            var startAngle = transform.eulerAngles.z;
            float endAngle = spinSpeed * duration + targetAngle; // Spin multiple times plus target angle

            // Spin the wheel
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                angle = Mathf.Lerp(startAngle, endAngle, spinCurve.Evaluate(t)) % 360;
                transform.eulerAngles = new Vector3(0, 0, angle);

                yield return null;
            }

            // Ensure the wheel stops at the exact target sector
            transform.eulerAngles = new Vector3(0, 0, endAngle % 360);
            _isSpinning = false;

            // Announce the prize
            // Debug.Log("Result: " + Sectors[targetSectorIndex].content);
            OnSpinEnd?.Invoke(targetSectorIndex, GetResult(targetSectorIndex));
        }
        
        /// Coroutine to spin the wheel until stop signal.
        private IEnumerator Spining()
        {
            _isSpinning = true;
            
            OnSpinStart?.Invoke();
            
            /* Constant speed phase */
            
            float elapsedTime = 0f;
            float speed = 0f;

            // Spin the wheel until stop signal
            while (!_stopSpin)
            {
                elapsedTime += Time.deltaTime;
                
                // Accelerate the wheel
                speed = Mathf.Lerp(speed, spinSpeed, Mathf.Sin(elapsedTime / spinDuration * Mathf.PI / 2));
                
                transform.Rotate(0, 0, speed * Time.deltaTime);
                yield return null;
            }
            
            /* Deceleration phase */
            
            elapsedTime = 0f;

            // Randomly determine the target sector
            int targetSectorIndex = Random.Range(0, Sectors.Count);
            float targetAngle = (clockwise ? _totalAngle * targetSectorIndex : 360f - targetSectorIndex * _totalAngle) + angleOffset;
            
            // Calculate the target angle and duration
            var startAngle = transform.eulerAngles.z;
            var endAngle = startAngle - startAngle % 360 + targetAngle + 360f * 9;
            var endDuration = (endAngle - startAngle) / spinSpeed;
            var endTime = elapsedTime + endDuration * Mathf.PI / 2f;

            // Spin to the target sector while slowing down
            while (elapsedTime < endTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / endTime;
                var angle = Mathf.Lerp(startAngle, endAngle, Mathf.Sin(t * Mathf.PI / 2)) % 360;
                transform.eulerAngles = new Vector3(0, 0, angle);
                yield return null;
            }

            // Ensure the wheel stops at the exact target sector
            transform.eulerAngles = new Vector3(0, 0, endAngle % 360);
            _isSpinning = false;

            // Announce the prize
            Debug.Log("Result: " + Sectors[targetSectorIndex].content);
            OnSpinEnd?.Invoke(targetSectorIndex, GetResult(targetSectorIndex));
            
            _stopSpin = false;
        }
        
        public string GetResult(int index)
        {
            return Sectors[index].content;
        }

        public void DrawRouletteWheel()
        {
            Clear();
            
            // Clear existing sectors
            if (Sectors == null || Sectors.Count == 0)
                return;
            
            // Reset the rotation of the wheel
            transform.localRotation = Quaternion.identity;
            _totalAngle = 360f / Sectors.Count;

            for (int i = 0; i < Sectors.Count; i++)
            {
                CreateSector(i, _totalAngle);
            }
            
            // Rotate the wheel to align with the first sector
            transform.localRotation = Quaternion.Euler(0, 0, angleOffset);
        }
        
        public void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        public GameObject GetSector(int index)
        {
            if (index < 0 || index >= Sectors.Count)
                return null;

            return transform.Find("Sector_" + index)?.gameObject;
        }

        private void CreateSector(int index, float sectorAngle)
        {
            float startAngle = (index - 0.5f) * sectorAngle;
            float endAngle = startAngle + sectorAngle;

            if (clockwise)
            {
                startAngle = -startAngle;
                endAngle = -endAngle;
            }

            // Create GameObject for the sector
            GameObject sectorGO = new GameObject("Sector_" + index);
            sectorGO.transform.SetParent(transform, false);
            sectorGO.transform.localPosition = Vector3.zero;
            if (_canvas != null)
            {
                var scaleFactor = _canvas.GetScaleFactor(true);
                sectorGO.transform.localScale = new Vector3(scaleFactor.x, scaleFactor.y, 1f);
            }

            // Create and set up the Mesh
            MeshFilter meshFilter = sectorGO.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = sectorGO.AddComponent<MeshRenderer>();
            meshRenderer.sortingOrder = 1;

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color> colors = new List<Color>();

            vertices.Add(Vector3.zero); // Center point

            // Center color
            Color.RGBToHSV(Sectors[index].color, out float h, out float s, out float v);
            var centerColor = Sectors[index].color;
            centerColor = centerColor.WithSaturation(s * 0.95f).WithAlpha(centerColor.a);
            colors.Add(centerColor);
            
            for (int j = 0; j <= segmentsPerSector; j++)
            {
                float lerpFactor = j * (1 - sectorInterval * 360f / sectorAngle) / segmentsPerSector;
                float angle = Mathf.Lerp(startAngle, endAngle, lerpFactor);
                Vector3 point = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
                    Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
                    0);
                vertices.Add(point);
                
                var color = Sectors[index].color;
                colors.Add(color);

                if (j > 0)
                {
                    triangles.Add(0);
                    triangles.Add(j);
                    triangles.Add(j + 1);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.colors = colors.ToArray();

            // Use a material that supports vertex colors
            if (material == null) 
                material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));
            meshRenderer.material = material;

            // Add border using LineRenderer
            if (drawBorder)
            {
                GameObject borderGO = new GameObject("Border") { active = false } ;
                borderGO.transform.SetParent(sectorGO.transform, false);
                var line = borderGO.AddComponent<LineRenderer>();
                line.useWorldSpace = false;
                line.loop = true;
                line.positionCount = vertices.Count;
                line.startWidth = borderWidth;
                line.endWidth = borderWidth;
                line.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));
                line.startColor = borderColor;
                line.endColor = borderColor;
                line.sortingOrder = 2;
                // line.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.3f));
            
                Vector3[] borderPoints = vertices.Skip(1).ToArray(); // Outer points only
                line.SetPositions(borderPoints);
            }

            // Add text label
            GameObject textGO = new GameObject("Text_" + index);
            textGO.transform.SetParent(sectorGO.transform, false);

            TextMeshPro text = textGO.AddComponent<TextMeshPro>();
            text.text = Sectors[index].content;
            text.horizontalAlignment = HorizontalAlignmentOptions.Left;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;
            text.enableWordWrapping = true;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.sortingOrder = 2;
            text.font = font ? font : Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            // text.material = fontMaterial ? fontMaterial : text.font.material;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.outlineColor = Color.black;
            text.outlineWidth = outlineWidth;

            float midAngle = (startAngle + endAngle) / 2f;
            Vector3 textPos = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * midAngle) * textDistanceFromCenter,
                Mathf.Sin(Mathf.Deg2Rad * midAngle) * textDistanceFromCenter,
                0);
            textGO.transform.localPosition = textPos;

            // Rotate the text to align with the sector
            textGO.transform.localRotation = Quaternion.Euler(0, 0, midAngle - 180f);

            RectTransform rectTransform = textGO.GetComponent<RectTransform>();
            rectTransform.pivot = new Vector2(1f, 0.5f);
            float sectorRadian = Mathf.Deg2Rad * sectorAngle / 2;
            float textHeight = radius * Mathf.Sin(sectorRadian);
            rectTransform.sizeDelta = new Vector2((radius - textDistanceFromCenter) * 0.95f, textHeight);
        }
        
        private IEnumerator AdjustTextSize(TextMeshPro text, float maxWidth)
        {
            yield return null; // Wait for one frame to get the correct bounds
            text.ForceMeshUpdate();

            while (text.bounds.size.x > maxWidth)
            {
                text.fontSize -= 1;
                text.ForceMeshUpdate();
            }
        }
    }
}