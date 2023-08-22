using UnityEngine;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming

namespace AkilliMum.SRP.CarPaint
{
    public class SpinBlur : MonoBehaviour
    {
        private const float DEGREE = 360;
        private const int MAXQUEUE = 100;

        private Mesh _mesh;

        private Quaternion _preRotation;

        //private bool _startCalculation;
        //private float _calculationTime = 0.5f;
        private float _totalAngle;
        private float _startSmoothness;

        private int _index;

        //private int _shutter = 2;
        private int _samples = 8;

        private Material[] _copyMaterials = new Material[MAXQUEUE];

        //readonly Queue<Quaternion> _rotationQueue = new Queue<Quaternion>();
        List<Quaternion> _rotations = new List<Quaternion>();

        //readonly Queue<Vector3> _positionQueue = new Queue<Vector3>();
        List<Vector3> _positions = new List<Vector3>();
        private Material _material;

        [Tooltip(
            "Calculation mode: Buffered uses previous frames to blur meshes so more accurate (more suitable for none static movements like swords etc.), angle uses current rotation only for best visual (perfect for wheels, rotors and pals etc.)")]
        public SpinBlurMode Mode = SpinBlurMode.Buffered;

        [Space(10)]
        [Header("Mode=Angled Properties")]
        [Tooltip("Turn on X axis (if Mode=Angled)")]
        public bool AngledX = true;
        [Tooltip("Turn on Y axis (if Mode=Angled)")]
        public bool AngledY = false;
        [Tooltip("Turn on Z axis (if Mode=Angled)")]
        public bool AngledZ = false;

        [Space(10)]
        [Header("Common Properties")]
        [Tooltip("Color property name on shader (It is '_BaseColor' for URP and '_Color' for Legacy etc.)")]
        public string ColorName = "_BaseColor";

        [Tooltip("Decrease alpha based on blur amount? If not selected, all samples uses same alpha for more transparency.")]
        public bool DecreaseAlpha;

        [Tooltip("Smoothness property name on shader (It is '_Smoothness' for URP and '_Glossiness' for Legacy etc.)")]
        public string SmoothnessName = "_Smoothness";

        [Tooltip("Decrease smoothness as the blur increases for a darker blur (no reflection)?")]
        public bool DecreaseSmoothness;

        [Tooltip("Per second angle changes to shift samples (360 degree based)")]
        public List<float> ShiftIntervals = new List<float>
        {
            0, 0.5f, 1, 2, 5
        };

        [Range(0, 1f)]
        [Tooltip("Motion Blur Opacity")]
        public float Opacity;

        //[Tooltip("Direction of the blur, normally it is 1,0,0 for objects like wheels etc. but should be 0,1,0 for horizontal heli pals :)")]
        //public Vector3 Direction = new Vector3(1, 0, 0);

        [Range(0.05f, 10)]
        [Tooltip("Deflection value to move blur, higher values creates more sharp effect!")]
        public float Deflection = 1f;

        [Tooltip("Enables material's GPU Instancing property")]
        // ReSharper disable once InconsistentNaming
        public bool EnableGPUInstancing;

        [Tooltip("Index for objects with multiple materials")]
        public int SubMaterialIndex = 0;

        // ReSharper disable once UnusedMember.Local
        void Start()
        {
            _mesh = GetComponent<MeshFilter>().mesh;

            _material = GetComponent<Renderer>().materials[SubMaterialIndex];
            _material.enableInstancing = EnableGPUInstancing;
            if (_material.HasProperty(SmoothnessName))
                _startSmoothness = _material.GetFloat(SmoothnessName);

            _preRotation = transform.rotation;
        }

        // ReSharper disable once UnusedMember.Local
        void Update()
        {
            if (Mode == SpinBlurMode.Buffered)
                Buffered();
            else
                Angled();
        }

        void Buffered()
        {
            var color = _material.GetColor(ColorName);

            //add current to queue
            //_rotationQueue.Enqueue(transform.rotation);
            //_positionQueue.Enqueue(transform.position);
            _rotations.Insert(0, transform.rotation);
            _positions.Insert(0, transform.position);

            //so we find how many rotation (degree) occurs in 1 second
            _totalAngle = GetTotalAngle();
            //Debug.Log("fps: " + (1f / Time.deltaTime));

            CalculateIndex();
            _samples = _index * 5;

            if (_samples >= MAXQUEUE)
                _samples = MAXQUEUE - 1;

            //Debug.Log("index: " + _index);
            if (_index > 0)
            {
                DisableMainMesh(color);

                for (int i = 0; i < _samples; i++)
                {
                    Color tempColor = GetTempColor(color, i);
                    //Debug.Log("temp color alpha for wheel blur: " + tempColor.a);
                    //tempColor = new Color(color.r, color.g, color.b,1);

                    PrepareMaterial(i, tempColor);

                    //var increase = ((i % 2 == 1 ? (-i) : (i - 1))) * Deflection;
                    var rotation = _rotations[i];
                    var position = _positions[i] + new Vector3(0.0001f, 0.0001f, 0.0001f); //z-fight :)

                    //var to = (rotation.eulerAngles - transform.rotation.eulerAngles) /(Deflection*10);
                    //rotation.eulerAngles += to;

                    Matrix4x4 matrix = Matrix4x4.TRS(
                        Vector3.Lerp(transform.position, position, (float)1/(_samples * Deflection)),
                        Quaternion.Lerp(transform.rotation, rotation, (float)1 /(_samples * Deflection)),
                        //position,
                        //rotation,
                        transform.lossyScale);
                    Graphics.DrawMesh(_mesh, matrix, _copyMaterials[i], gameObject.layer, null, SubMaterialIndex);
                    //Debug.Log(position);
                    //Debug.Log(rotationToDraw);
                }
            }
            else
            {
                ResetMainMaterial(color);
            }

            _preRotation = transform.rotation;
            _totalAngle = 0;
            if (_rotations.Count > MAXQUEUE)
            {
                _rotations.RemoveAt(MAXQUEUE);
            }

            if (_positions.Count > MAXQUEUE)
            {
                _positions.RemoveAt(MAXQUEUE);
            }
        }

        private void ResetMainMaterial(Color color)
        {
            if (color.a < 1)
            {
                Color tempColor;
                tempColor = new Color(color.r, color.g, color.b, 1);
                _material.SetColor(ColorName, tempColor);
                if (DecreaseSmoothness)
                    _material.SetFloat(SmoothnessName, _startSmoothness);
            }
        }

        private void PrepareMaterial(int i, Color color)
        {
            if (_copyMaterials[i] == null)
            {
                _copyMaterials[i] = new Material(_material);
                _copyMaterials[i].enableInstancing = EnableGPUInstancing;
            }

            _copyMaterials[i].SetColor(ColorName, color);
            if (DecreaseSmoothness)
                _copyMaterials[i].SetFloat(SmoothnessName, _startSmoothness / (_index + i + 3));
        }

        private void DisableMainMesh(Color color)
        {
            //0 is real (current) position and rotation, so do not use it
            Color tempColor = new Color(color.r, color.g, color.b, 0); //disable main mesh
            //System.Math.Clamp(Mathf.Abs(1f / (_index + 1) + Opacity), 0, 1));
            _material.SetColor(ColorName, tempColor);
            //if (DecreaseSmoothness)
            //    _material.SetFloat(SmoothnessName, _startSmoothness / (_index + 1));
        }

        private float GetTotalAngle()
        {
            return Quaternion.Angle(transform.rotation, _preRotation) * (1f / Time.deltaTime);
        }

        void Angled()
        {
            var color = _material.GetColor(ColorName);

            //so we find how many rotation (degree) occurs in 1 second
            _totalAngle = GetTotalAngle();
            //Debug.Log("fps: " + (1f / Time.deltaTime));

            CalculateIndex();
            _samples = _index * 5;

            //Debug.Log("index: " + _index);
            if (_index > 0)
            {
                DisableMainMesh(color);

                for (int i = 0; i <= _samples; i++)
                {
                    //Debug.Log("drawing i: " + i);

                    Color tempColor = GetTempColor(color, i);

                    //Debug.Log("temp color alpha for wheel blur: " + tempColor.a);
                    //tempColor = new Color(color.r, color.g, color.b,1);

                    PrepareMaterial(i, tempColor);

                    //var increase = ((i % 2 == 1 ? (-i) : (i - 1))) * Deflection;
                    var increase = -i * Deflection; //draw from front to back
                    //Debug.Log("increase rim: " + increase);
                    var rotation = transform.rotation;
                    rotation.eulerAngles = transform.rotation.eulerAngles;
                    rotation.eulerAngles = new Vector3(   //1>_ -1   2_>  +1
                        rotation.eulerAngles.x + increase * (AngledX ? 1 : 0),
                        rotation.eulerAngles.y + increase * (AngledY ? 1 : 0),
                        rotation.eulerAngles.z + increase * (AngledZ ? 1 : 0));

                    var position = transform.position + new Vector3(0.0001f, 0.0001f, 0.0001f); //z-fight :)

                    Matrix4x4 matrix = Matrix4x4.TRS(position,
                            rotation,
                            transform.lossyScale); //this transform!!!!! this object!!!!!!!
                    Graphics.DrawMesh(_mesh, matrix, _copyMaterials[i], gameObject.layer, null, SubMaterialIndex);
                }
            }
            else
            {
                ResetMainMaterial(color);
            }

            _preRotation = transform.rotation;
            _totalAngle = 0;
        }

        private Color GetTempColor(Color color, int i)
        {
            if (DecreaseAlpha)
                return new Color(color.r, color.g, color.b, System.Math.Clamp(Mathf.Abs((2f / (_index + i + 3)) + Opacity), 0, 1));

            return new Color(color.r, color.g, color.b, System.Math.Clamp(Mathf.Abs((2f / (_samples)) + Opacity), 0, 1));
        }

        private void CalculateIndex()
        {
            _index = 0;
            //Debug.Log("total angle: " + _totalAngle);

            for (int i = 0; i < ShiftIntervals.Count; i++)
            {
                var first = (ShiftIntervals[i] * DEGREE); //if calculation time < 1, it has to be rolled over to 1 second!
                //last index
                if (i == ShiftIntervals.Count - 1)
                {
                    //Debug.Log("first:" + first + " total:" + _totalAngle);
                    if (_totalAngle >= first)
                    {
                        _index = i;
                        break;
                    }
                }
                else
                {
                    var second = (ShiftIntervals[i + 1] * DEGREE);
                    //Debug.Log("first:" + first + " second:" + second + " total:" + _totalAngle);
                    if (first <= _totalAngle && _totalAngle < second)
                    {
                        _index = i;
                        break;
                    }
                }
            }
        }
    }

    public enum SpinBlurMode
    {
        Buffered = 1,
        Angled = 2
    }
}