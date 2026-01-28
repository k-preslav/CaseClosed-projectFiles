using System.Threading.Tasks;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SceneTransitionUI.FadeOut();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameManager.Instance.GoToBoard();
        }
    }
}
