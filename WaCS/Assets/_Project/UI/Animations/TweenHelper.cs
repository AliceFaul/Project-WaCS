using UnityEngine;

namespace _Project.UI.Animations
{
    public class TweenHelper : MonoBehaviour
    {
        public static TweenHelper Instance { get; private set; }
        [SerializeField] private TweenConfig config;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Pop animation: scales up and then back to normal
        public void Pop(RectTransform target)
        {
            LeanTween.cancel(target.gameObject);
            target.localScale = Vector3.one;
            // First, scale up to the popScale, then scale back to normal
            LeanTween.scale(target, Vector3.one * config.popScale, config.popDuration).
                setEaseOutBack().setOnComplete(() =>
                {
                    LeanTween.scale(target, Vector3.one, config.popDuration * 0.7f);
                });
        }

        // Fade in animation: fades the canvas group to opaque
        public void FadeIn(CanvasGroup group)
        {
            group.alpha = 0f;
            LeanTween.alphaCanvas(group, 1f, config.fadeDuration)
                .setEaseInQuad();
        }

        // Fade out animation: fades the canvas group to transparent
        public void FadeOut(CanvasGroup group)
        {
            LeanTween.alphaCanvas(group, 0f, config.fadeDuration)
                .setEaseOutQuad();
        }

        // Slide in from left: moves the panel from a specified X position to its anchored position
        public void SlideInFromLeft(RectTransform panel, float fromX)
        {
            panel.anchoredPosition = new Vector2(fromX, panel.anchoredPosition.y);
            LeanTween.moveX(panel, 0f, config.slideDuration)
                .setEaseOutCubic();
        }

        // Shake animation: shakes the panel horizontally
        public void Shake(RectTransform target)
        {
            Vector2 original = target.anchoredPosition;
            LeanTween.moveX(target, original.x + config.shakeStrength,
                config.shakeDuration / 4f)
                .setLoopPingPong(2)
                .setOnComplete(() =>
                {
                    target.anchoredPosition = original;
                });
        }

        public void FloatUp(RectTransform target, float distance)
        {
            Vector3 start = target.anchoredPosition;
            LeanTween.moveY(target, start.y + distance, 0.6f)
                .setEaseOutCubic();
            CanvasGroup group = target.GetComponent<CanvasGroup>();
            if (group != null)
            {
                LeanTween.alphaCanvas(group, 0f, 0.6f);
            }
        }

        // Hover animation: scales up slightly on hover enter, and scales back on hover exit
        public void HoverEnter(RectTransform target)
        {
            LeanTween.cancel(target.gameObject);
            LeanTween.scale(target, Vector3.one * 1.08f, 0.12f)
                .setEaseOutQuad();
        }

        // Hover exit animation: scales back to normal
        public void HoverExit(RectTransform target)
        {
            LeanTween.cancel(target.gameObject);
            LeanTween.scale(target, Vector3.one, 0.12f)
                .setEaseOutQuad();
        }
    }
}