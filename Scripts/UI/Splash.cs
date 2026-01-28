using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    private async void Start()
    {
        await Task.Delay(3000);
        await SceneManager.LoadSceneAsync(1);
    }
}