using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour
	{
		[SerializeField]
		Faction alignment = Faction.Enemy;
		public Transform targetCenter;
		public Faction Alignment { get { return alignment; } private set { alignment = value; } }
		void Start()
		{

		}
		void Update()
		{

		}
	}
}