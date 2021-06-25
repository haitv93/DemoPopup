using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField]
    protected Canvas canvas;

    private static PopupController S;

    private void Awake()
    {
        S = this;
    }

    private static bool PopupActive;
    protected static Stack<GameObject> currentPopups = new Stack<GameObject>();
    protected static Stack<GameObject> currentPanels = new Stack<GameObject>();

    public static void OpenPopup<T>(string popupName, Action<T> onOpened = null, bool isDarkBG = true) where T : Popup
    {
        if (PopupActive) return;
        PopupActive = true;
        S.StartCoroutine(OpenPopupAsync(popupName, onOpened, isDarkBG));
    }

    public void CloseCurrentPopup()
    {
        if (currentPopups.Count > 0)
        {
            var currentPopup = currentPopups.Peek();
            if (currentPopup != null)
            {
                currentPopup.GetComponent<Popup>().Close();
            }
        }
    }

    public void ClosePopup()
    {
        PopupActive = false;

        var topmostPopup = currentPopups.Pop();
        if (topmostPopup == null)
        {
            return;
        }

        var topmostPanel = currentPanels.Pop();
        if (topmostPanel != null)
        {
            StartCoroutine(FadeOut(topmostPanel.GetComponent<Image>(), Constant.TIME_TO_FADE_POPUP, () => Destroy(topmostPanel)));
        }
    }

    protected static IEnumerator OpenPopupAsync<T>(string popupName, Action<T> onOpened, bool isDarkBG = true) where T : Popup
    {
        GameObject panel = null;
        var request = Resources.LoadAsync<GameObject>(popupName);
        while (!request.isDone)
        {
            yield return null;
        }

        if (isDarkBG)
        {
            panel = new GameObject("Panel");
            var panelImage = panel.AddComponent<Image>();
            var color = Color.black;
            color.a = 0;
            panelImage.color = color;
        }
        var panelTransform = panel.GetComponent<RectTransform>();
        panelTransform.anchorMin = new Vector2(0, 0);
        panelTransform.anchorMax = new Vector2(1, 1);
        panelTransform.pivot = new Vector2(0.5f, 0.5f);
        panel.transform.SetParent(S.canvas.transform, false);
        currentPanels.Push(panel);
        S.StartCoroutine(FadeIn(panel.GetComponent<Image>(), Constant.TIME_TO_FADE_POPUP));
        yield return new WaitUntil(() => request.asset != null);
        var popup = Instantiate(request.asset) as GameObject;
        popup.transform.SetParent(S.canvas.transform, false);
        popup.GetComponent<Popup>().parentScene = S;

        if (onOpened != null)
        {
            onOpened(popup.GetComponent<T>());
        }
        currentPopups.Push(popup);
    }

    protected static IEnumerator FadeIn(Image image, float time)
    {
        var alpha = image.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            var color = image.color;
            color.a = Mathf.Lerp(alpha, 220 / 256.0f, t);
            image.color = color;
            yield return null;
        }
    }

    protected static IEnumerator FadeOut(Image image, float time, Action onComplete)
    {
        var alpha = image.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            var color = image.color;
            color.a = Mathf.Lerp(alpha, 0, t);
            image.color = color;
            yield return null;
        }
        if (onComplete != null)
        {
            onComplete();
        }
    }
}