using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationProfile", menuName = "UI/UITween")]
public class TweenConfig : ScriptableObject
{
    [Header("Pop")]
    public float popScale = 1.15f;
    public float popDuration = 0.12f;

    [Header("Fade")]
    public float fadeDuration = 0.2f;

    [Header("Slide")]
    public float slideDuration = 0.3f;

    [Header("Shake")]
    public float shakeDuration = 0.2f;
    public float shakeStrength = 15f;
}
