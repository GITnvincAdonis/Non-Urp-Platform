using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GrassData
{
    public Vector3 Position;
    public Vector3 Rotation;
    public float Scale;
}


public class GPUInstancing : MonoBehaviour
{
  
    // Start is called before the first frame update
    Vector3 postion, rotation, scale;
    [SerializeField] Mesh mesh;
    [SerializeField] Material mat;
    [SerializeField] int Instances;
    [SerializeField] int dimension;
    [SerializeField] int _scale;
    private GrassData[] positionBuffer;
    private Quaternion[] rotationBuffer;
    public ComputeShader shader;
    public Material matShader;
    private GrassData[] posBuffer;
    ComputeBuffer tempbuffer;
    ComputeBuffer Secondtempbuffer;
    private List<Matrix4x4> Batches = new ();

    [SerializeField] private Vector3 _grassScale;
    [SerializeField] private float _Scale;

    Quaternion _rotation;
    Vector3 _myScale;
    [SerializeField] int _maxScale;
    [SerializeField] int _minScale;
    [SerializeField] float _heightNoiseScale; 
    void Start()
    {
        //Matrix4x4 matrix4X4 = Matrix4x4.TRS(pos:postion,q:Quaternion.Euler(rotation),s:scale);
        positionBuffer = new GrassData[Instances];
        rotationBuffer = new Quaternion[Instances];
        posBuffer = new GrassData[Instances];
        PopulateMatrices();
    }
    void PopulateMatrices()
    {

        int size = (sizeof(float) * 3) + (sizeof(float) * 3) + sizeof(float);
        tempbuffer = new ComputeBuffer(Instances,size );
        tempbuffer.SetData(positionBuffer);


        shader.SetBuffer(0, "positionBuffer", tempbuffer);
        shader.SetInt("_Dimension", dimension);
        shader.SetInt("_scale", _scale);
        shader.SetInt("_maxScale",_maxScale);
        shader.SetInt("_minScale", _minScale);
        shader.SetFloat("_heightNoiseScale", _heightNoiseScale);
        shader.Dispatch(0, Mathf.CeilToInt(Instances / 8.0f), Mathf.CeilToInt(Instances / 8.0f),1); 

        tempbuffer.GetData(positionBuffer);


        int PosBuffSize = (sizeof(float) * 3) + (sizeof(float) * 3);
        Secondtempbuffer = new ComputeBuffer(Instances, PosBuffSize);
        

        
        _myScale = new Vector3(_grassScale.x , _grassScale.y , _grassScale.z );
        for (int i = 0; i < Instances; i++) {
            Vector3 rot = positionBuffer[i].Rotation;
            rotationBuffer[i] = Quaternion.Euler(rot);
            posBuffer[i] = positionBuffer[i];
        }


        Secondtempbuffer.SetData(posBuffer);
        matShader.SetBuffer("GrassBuffer", Secondtempbuffer);
        mat.SetFloat("_Scale", _Scale);
    }

    struct MyInstanceData
    {
        public Matrix4x4 objectToWorld;
        public float myOtherData;
        public uint renderingLayerMask;
    };
    void Update()
    {
        RenderParams rp = new RenderParams(mat);
        MyInstanceData[] instData = new MyInstanceData[Instances];


        for (int i = 0; i < Instances; ++i)
        {
            
            instData[i].objectToWorld = Matrix4x4.TRS(positionBuffer[i].Position + transform.position, rotationBuffer[i], new Vector3(_myScale.x, positionBuffer[i].Scale,_myScale.z));
        }
        Graphics.RenderMeshInstanced(rp, mesh, 0, instData);
        
    }
    private void OnDisable()
    {
        tempbuffer.Release();
        Secondtempbuffer.Release();
    }
}
