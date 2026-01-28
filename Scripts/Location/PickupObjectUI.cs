using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupObjectUI : MonoBehaviour
{
    [SerializeField] Button addClueButton;
    [SerializeField] TMP_Text objectDescriptionText;

    PickupObject selectedObject;

    public Action OnUIClose;

    private void Start()
    {
        Close();
    }

    public void Close()
    {
        selectedObject?.Drop();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        selectedObject = null;

        GameManager.Instance.Cursor.SetVisible(false);
        GameManager.Instance.Cursor.SetLock(true);

        OnUIClose?.Invoke();
    }

    public void OnAddClueButtonPressed()
    {
        if (selectedObject == null) return;
     
        selectedObject.AddAsClue();
        addClueButton.interactable = false;

        selectedObject.Drop();
        Close();
    }

    public void SetupUI(PickupObject obj)
    {
        if (obj == null) return;
        selectedObject = obj;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null.");
            return;
        }

        if (GameManager.Instance.IsCardDiscovered(obj.assignedCardData)) addClueButton.interactable = false;
        else addClueButton.interactable = true;

        objectDescriptionText.text = obj.objectDescription;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        GameManager.Instance.Cursor.SetVisible(true);
        GameManager.Instance.Cursor.SetLock(false);
    }
}
