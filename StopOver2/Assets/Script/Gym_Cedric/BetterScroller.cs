using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BetterScroller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    #region Variables
    #region RectTransform
    public GameObject mainRectObject;
    private RectTransform mainRect;

    public GameObject viewportRectObject;
    private RectTransform viewportRect;
    #endregion

    #region Parameters
    public float speedScroll = 1f;
    #endregion

    #region private Variables
    private bool onRect = false;
    #endregion
    #endregion

    #region UI interraction
    public void OnPointerEnter(PointerEventData eventData)
    {
        onRect = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onRect = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
    #endregion

    private void OnEnable()
    {
        mainRect = mainRectObject.GetComponent<RectTransform>();
        viewportRect = viewportRectObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        myScroller();
    }

    private void myScroller()
    {
        if (onRect)
        {

        }
    }
}
