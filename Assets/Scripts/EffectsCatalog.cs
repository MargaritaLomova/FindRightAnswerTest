using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class EffectsCatalog
{
    public static void BounceEffect(GameObject obj)
    {
        obj.transform.DOMoveY(obj.transform.position.y + 3, 0.125f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            obj.transform.DOMoveY(obj.transform.position.y - 3, 0.125f).SetEase(Ease.InQuad);
        });
    }

    public static void FadeInEffect(GameObject obj, float duration)
    {
        FadeEffect(obj, 0, 1, duration);
    }

    public static void FadeOutEffect(GameObject obj, float duration)
    {
        FadeEffect(obj, 1, 0, duration);
    }

    public static void ShakeEffect(GameObject obj)
    {
        obj.transform.DOShakePosition(0.3f, new Vector3(6, 0), 100);
    }

    public static void RotateEffect(GameObject obj)
    {
        obj.transform.DORotate(new Vector3(0, 0, 540),3.0f).OnComplete(() =>
        {
            obj.transform.DORotate(new Vector3(0, 0, 90),3.0f);
        });
    }

    private static void FadeEffect(GameObject obj, float startOpacity, float endOpacity, float duration)
    {
        foreach (var graphic in obj.GetComponentsInChildren<Graphic>())
        {
            var startColor = graphic.color;
            startColor.a = startOpacity;
            graphic.color = startColor;

            var endColor = graphic.color;
            endColor.a = endOpacity;
            graphic.DOColor(endColor, duration);
        }
    }
}