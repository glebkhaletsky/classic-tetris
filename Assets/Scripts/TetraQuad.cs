using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraQuad : MonoBehaviour
{
    [SerializeField]
    private QuadSettings _quadSettings;

    public void MoveToPosition(Vector3 position)
    {
        StartCoroutine(MoveAnimation(position));
    }

    private IEnumerator MoveAnimation( Vector3 positon)
    {
        Vector3 startPosition = transform.position;
        for (float t = 0; t < 1f; t+= Time.deltaTime * 3)
        {

            float interpolant = _quadSettings.MoveCurve.Evaluate(t);
            transform.position = Vector3.LerpUnclamped(startPosition, positon, interpolant);
            yield return null;
        }

        transform.position = positon;
    }
}
