using System;
using UnityEngine;

namespace RPG.Core
{
	public class Focusable
	{
		public readonly Transform focusObject;
		public readonly int priority;
		public readonly System.Action<Transform> defocusCallback;
		public readonly System.Action refocusCallback;

		public Focusable(Focusable copy)
		{
			this.focusObject = copy.focusObject ?? throw new ArgumentNullException(nameof(copy.focusObject));
			this.priority = copy.priority;
			this.defocusCallback = copy.defocusCallback;
			this.refocusCallback = copy.refocusCallback;
		}

		public Focusable(Transform focusObject, int priority, Action<Transform> defocusCallback, Action refocusCallback)
		{
			this.focusObject = focusObject ?? throw new ArgumentNullException(nameof(focusObject));
			this.priority = priority;
			this.defocusCallback = defocusCallback;
			this.refocusCallback = refocusCallback;
		}
	}

}