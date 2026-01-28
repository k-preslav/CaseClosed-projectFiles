using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] Light[] lights;
    [SerializeField] bool state = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                Debug.Log($"Hit: {hitInfo.collider.gameObject.name}, Tag: {hitInfo.collider.gameObject.tag}");
                
                if (hitInfo.collider.gameObject.CompareTag("Switch"))
                {
                    state = !state;
                    foreach (var light in lights)
                    {
                        light.enabled = state;
                    }

                    GetComponent<Animator>().SetBool("IsOn", state);
                }
            }
        }
    }
}
