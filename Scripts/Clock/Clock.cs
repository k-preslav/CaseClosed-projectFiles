using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] TextMeshPro timeText;

    private void Update()
    {
        float time = GameManager.Instance.caseTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}