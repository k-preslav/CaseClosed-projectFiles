using UnityEngine;
using UnityEngine.UI;

public enum CursorAccent
{
    Light,
    Dark
}

public class CustomCursor : MonoBehaviour
{
    [SerializeField] Color Color = Color.white;
    [SerializeField] float lerpSpeed = 3f;

    bool isVisible = true;

    private void Start()
    {
        SetVisible(true);

        // set the native cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        transform.position = Vector3.Lerp(transform.position, mousePosition, lerpSpeed * Time.deltaTime);
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
        GetComponent<Image>().color = visible ? Color : new Color(0, 0, 0, 0);
    }
    public void SetLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void SetAccent(CursorAccent accent)
    {
        GetComponent<Image>().color = isVisible ? (accent == CursorAccent.Light ? Color.white : Color.black) : new Color(0, 0, 0, 0);
    }
}