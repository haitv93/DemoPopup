using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Popup : MonoBehaviour
{
    [HideInInspector]
    public PopupController parentScene;

    [HideInInspector]
    public UnityEvent onOpen;

    [HideInInspector]
    public UnityEvent onClose;

    private Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        onOpen.Invoke();
    }

    public void Close()
    {
        if (parentScene != null)
        {
            parentScene.ClosePopup();
        }
        onClose.Invoke();
        if (animator != null)
        {
            animator.Play("Close");
            StartCoroutine(DestroyPopup());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual IEnumerator DestroyPopup()
    {
        yield return new WaitForSeconds(Constant.TIME_TO_FADE_POPUP);
        Destroy(gameObject);
    }
}