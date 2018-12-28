using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FUCK UNITY

public class PsychedelicShaderDriver : MonoBehaviour {

    [Range(0, 1)] public float insanity = 1;


    [Range(.41f, .42f)] public float cX = .41f;
    [SerializeField] float minX = 0.41f;
    [SerializeField] float maxX = 0.42f;
    [SerializeField] bool changeX = true;
    [SerializeField] bool isIncreasingX = true;
    [Range(0, 1)] public float deltaX = 0.0001f;

    [Range(0.067f, 0.4f)] public float cY = .067f;
    [SerializeField] float minY = 0.067f;
    [SerializeField] float maxY = 0.4f;
    [SerializeField] bool changeY = true;
    [SerializeField] bool isIncreasingY = true;
    [Range(0, 1)] public float deltaY = .0001f;

    [Range(2f, 11f)] public float _M1 = 4f;
    [SerializeField] float minM1 = 2f;
    [SerializeField] float maxM1 = 11f;
    [SerializeField] bool changeM1 = true;
    [SerializeField] bool isIncreasingM1 = true;
    [Range(0, 1)] public float deltaM1 = .25f;

    [Range(2f, 11f)] public float _M2 = 5f;
    [SerializeField] float minM2 = 2f;
    [SerializeField] float maxM2 = 11f;
    [SerializeField] bool changeM2 = true;
    [SerializeField] bool isIncreasingM2 = true;
    [Range(0, 1)] public float deltaM2 = .25f;

    [Range(2f, 11f)] public float _M3 = 7f;
    [SerializeField] float minM3 = 2f;
    [SerializeField] float maxM3 = 11f;
    [SerializeField] bool changeM3 = true;
    [SerializeField] bool isIncreasingM3 = true;
    [Range(0, 1)] public float deltaM3 = .25f;


    [SerializeField]
    float _repeat = 8.0f;

    [SerializeField] public Vector4 _Zoom = new Vector4(.1f, .1f, .1f, .1f);
    [SerializeField] public Vector4 _Pan = new Vector4(0f, 0f, 0f, 0f);
    [Range(0, 6)] public float _R = 5f;
    [Range(0, 5)] public float _UVOffset = 4.98f;
    [Range(0, 1)] public float _Shift = .5f;
    [Range(0, 1)] public float _Roll = .4f;
    [Range(0, 1)] public float _Offset = .5f;
    [Range(0, 1)] public float _Aspect = .8f;
    [Range(0, 1)] public float _Saturation = .55f;

    Vector4 seed;
    Vector4 change;

    // Use this for initialization
    void Start () {
        cX = Random.Range(minX, maxX);
        cY = Random.Range(minY, maxY);
        _M1 = Random.Range(minM1, maxM1);
        _M2 = Random.Range(minM2, maxM2);
        _M3 = Random.Range(minM3, maxM3);
        seed = new Vector4(cX, cY, 1, 1);
    }

    // Update is called once per frame
    void Update ()
    {
        change = new Vector4(deltaX, deltaX, deltaX, deltaX);
        seed = new Vector4(cX, cY, 1, 1);

        var div = Mathf.PI * 2 / Mathf.Max(1, _repeat);

        GetComponent<Renderer>().material.SetVector("_Seed", seed);
        GetComponent<Renderer>().material.SetVector("_Zoom", _Zoom);
        GetComponent<Renderer>().material.SetVector("_Pan", _Pan);

        GetComponent<Renderer>().material.SetFloat("_Divisor", div);
        GetComponent<Renderer>().material.SetFloat("_Insanity", insanity);

        GetComponent<Renderer>().material.SetFloat("_R", _R);
        GetComponent<Renderer>().material.SetFloat("_UVOffset", _UVOffset);
        GetComponent<Renderer>().material.SetFloat("_M1", _M1);
        GetComponent<Renderer>().material.SetFloat("_M2", _M2);
        GetComponent<Renderer>().material.SetFloat("_M3", _M3);
        GetComponent<Renderer>().material.SetFloat("_Shift", _Shift);
        GetComponent<Renderer>().material.SetFloat("_Roll", _Roll);
        GetComponent<Renderer>().material.SetFloat("_Offset", _Offset);
        GetComponent<Renderer>().material.SetFloat("_Aspect", _Aspect);
        GetComponent<Renderer>().material.SetFloat("_Saturation", _Saturation);

        UpdateVariable(ref isIncreasingX, changeX, ref cX, deltaX, maxX, minX);
        UpdateVariable(ref isIncreasingY, changeY, ref cY, deltaY, maxY, minY);
        UpdateVariable(ref isIncreasingM1, changeM1, ref _M1, deltaM1, maxM1, minM1);
        UpdateVariable(ref isIncreasingM2, changeM2, ref _M2, deltaM2, maxM2, minM2);
        UpdateVariable(ref isIncreasingM3, changeM3, ref _M3, deltaM3, maxM3, minM3);
    }

    private void UpdateVariable(ref bool isIncreasing, bool change, ref float c, 
                                float delta, float max, float min)
    {
        if (isIncreasing && change)
        {
            c += delta;
        }
        else if (change)
        {
            c -= delta;
        }

        if (change && c > max)
            isIncreasing = false;
        else if (change && c < min)
            isIncreasing = true;
    }
}

