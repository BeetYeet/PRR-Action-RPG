using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDisplay : MonoBehaviour
{
	private void OnDrawGizmosSelected()
	{
		if (transform == null || transform.childCount < 2)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(transform.GetChild(0).position, .3f);
			return;
		}

		Gizmos.color = Color.gray;
		Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(transform.childCount - 1).position);
		for (int i = 0; i < transform.childCount; i++)
		{
			Gizmos.color = Color.gray;
			if (i != transform.childCount - 1)
				Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(transform.GetChild(i).position, .3f);
		}
	}
}
