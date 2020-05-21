using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFix : MonoBehaviour
{
    [SerializeField]
    private RuntimeAnimatorController controller;

    [SerializeField]
    private Animator animator;

    private void Update()
    {
        if (animator.runtimeAnimatorController == null)
            animator.runtimeAnimatorController = controller;
    }
}