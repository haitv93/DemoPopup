using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Popup : MonoBehaviour
{
    [HideInInspector]
    public PopupController ParentScene;

    [HideInInspector]
    public UnityEvent OnOpen;

    [HideInInspector]
    public UnityEvent OnClose;

    private Animator m_animator;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        OnOpen?.Invoke();
    }

    public void Close()
    {
        if (ParentScene != null)
        {
            ParentScene.ClosePopup();
        }

        OnClose?.Invoke();

        if (m_animator != null)
        {
            m_animator.Play("Close");
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