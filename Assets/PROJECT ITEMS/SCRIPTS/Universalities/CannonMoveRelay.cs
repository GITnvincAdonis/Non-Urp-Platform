using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="CannonPath",menuName ="CannonPath")]
public class CannonMoveRelay : ScriptableObject
{
    // Start is called before the first frame update
    public UnityEvent<float[]> CannonPathCoords = new UnityEvent<float[]>();
    public float[] Pos = new float[3];
    private void OnEnable()
    {
        if(CannonPathCoords == null)
        {
            CannonPathCoords = new UnityEvent<float[]>();
        }    
    }

    public void TransmitPositionPath(float[] coords)
    {
        Pos[0] = coords[0];
        Pos[1] = coords[1];
        Pos[2] = coords[2];

        CannonPathCoords?.Invoke(Pos);
    }
}
