using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TMP_Text text;
    Button btn;

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if (btn != null && btn.interactable == false)
        {
            text.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            text.fontStyle = FontStyles.Normal;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btn.interactable == false) return;

        text.color = Color.black;

        GameManager.Instance.Cursor.SetAccent(CursorAccent.Dark);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.white;

        GameManager.Instance.Cursor.SetAccent(CursorAccent.Light);
    }

    private void OnClick()
    {
        text.color = Color.white;

        GameManager.Instance.Cursor.SetAccent(CursorAccent.Light);
    }
}
