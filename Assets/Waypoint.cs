using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, .3f);

        if (transform.parent.childCount != 1)
        {
            DrawToPrevious();
        }

    }

    private void DrawToPrevious()
    {
        int before = transform.GetSiblingIndex() != 0 ? transform.GetSiblingIndex() - 1 : transform.parent.childCount - 1;
        DrawTo(transform.parent.GetChild(before).position);
    }

    void DrawTo(Vector3 pos)
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, pos);
    }
}
