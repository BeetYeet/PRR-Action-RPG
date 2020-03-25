using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
	public class CameraFollow : MonoBehaviour
	{
		static List<Focusable> focuses;
		static Focusable currentFocus;

		[SerializeField] Transform target;
		Vector3 offset;
		private void Start()
		{
			offset = transform.position - target.position;
		}
		void Update()
		{
			transform.position = target.position + offset;
		}

		/// <summary>
		/// Focuses camera on target object untill returned function is called or a focus of higher priority is requested. Logs a warning if two focus requests of the same priority are requested at the same time.
		/// </summary>
		/// <param name="focusObject">The object to be focused on</param>
		/// <param name="priority">Higher value will try harder to retain focus</param>
		/// <param name="defocusCallback">If a focus of higher priority is requested this is called including the Transform that was just focused</param>
		/// <param name="refocusCallback">If a focus stops and returns focus to the focusObject this function is called</param>
		/// <returns>A callback to stop focusing the focusObject</returns>
		public static System.Action Focus(Transform focusObject, int priority, System.Action<Transform> defocusCallback, System.Action refocusCallback)
		{

			Focusable focus = new Focusable(focusObject, priority, defocusCallback, refocusCallback);

			if(currentFocus == null){
				SetFocus(focus);
			}

			return null;
		}

		static void SetFocus(Focusable Focus){

		}
	}

}