using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		Mover mover;
		Fighter fighter;
		Ray lastRay;


		void Start()
		{
			mover = GetComponent<Mover>();
			fighter = GetComponent<Fighter>();
		}

		void Update()
		{
			if (InteractWithCombat())
			{

			}
			else
			if (InteractWithMovement())
			{
				fighter.Cancel();
			}
		}

		private bool InteractWithCombat()
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
				foreach (RaycastHit hit in hits)
				{
					CombatTarget target = hit.transform.GetComponent<CombatTarget>();
					if (target == null)
					{
						continue;
					}


					fighter.TryAttack(target);
					return true;
				}
			}
			return false;
		}

		private bool InteractWithMovement()
		{
			if (Input.GetMouseButtonDown(0))
			{
				lastRay = GetMouseRay();
				RaycastHit hit;
				if (Physics.Raycast(lastRay, out hit))
				{
					mover.StartMoveAction(hit.point);
					return true;
				}
			}
			return false;
		}


		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
}