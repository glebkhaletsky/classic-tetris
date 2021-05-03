using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetra : MonoBehaviour
{
    public TetraQuad[] Quads;
    public void Move(Vector2Int offset)
    {
        transform.position += new Vector3(offset.x,offset.y,0f);
    }

    public void RotateCW()
    {
        transform.Rotate(0, 0, 90);
    }

    public void RotateCCW()
    {
        transform.Rotate(0, 0, -90);
    }
}
