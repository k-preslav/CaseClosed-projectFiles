using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionUI : MonoBehaviour
{
    [SerializeField] Image transitionImage;

    public void FadeIn()
    {
        transitionImage
            .DOFade(1f, 0.06f)
            .SetEase(Ease.OutQuad);
    }
    public void FadeOut()
    {
        transitionImage
            .DOFade(0f, 0.25f)
            .SetEase(Ease.InQuad);

    }
}
