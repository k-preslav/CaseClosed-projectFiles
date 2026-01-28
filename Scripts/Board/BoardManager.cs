using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SceneTransitionUI.FadeOut();
    }
}
