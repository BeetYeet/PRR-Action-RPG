using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSlider : MonoBehaviour
{
    public static TransitionSlider instance;

    public float transitionTime = 1f;

    public event System.Action OnTransitionedOut;

    public event System.Action OnTransitionedIn;

    public Animator anim;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("Somehow multiple transition scripts!");
            enabled = false;
            return;
        }
    }

    public void TansitionedIn()
    {
        OnTransitionedIn?.Invoke();
    }

    public void TansitionedOut()
    {
        instance = null;
        OnTransitionedOut?.Invoke();
    }

    public void TransitionOut()
    {
        anim.SetTrigger("Transition");
    }
}