using UnityEngine;
using UnityEngine.UI;

public class AnswerController : MonoBehaviour
{
    [Header("Answer Components"), SerializeField]
    private Image image;
    [SerializeField]
    private Image background;
    [SerializeField]
    private GameObject stars;

    private bool isAnimationFinished = false;
    private bool isRight;
    private LevelController levelController;

    public void Set(Sprite answerImage, Color backgroundColor, bool isRightValue, bool isNeedToAnimate = true)
    {
        if (levelController == null)
        {
            levelController = FindObjectOfType<LevelController>();
        }

        isRight = isRightValue;
        image.sprite = answerImage;
        image.preserveAspect = true;
        background.color = backgroundColor;

        if (isNeedToAnimate)
        {
            EffectsCatalog.BounceEffect(gameObject);

            Invoke("AnimationIsFinished", 0.25f);
        }
    }

    public void Set(Sprite answerImage, bool isRightValue)
    {
        if (levelController == null)
        {
            levelController = FindObjectOfType<LevelController>();
        }

        isRight = isRightValue;
        image.sprite = answerImage;
    }

    public bool IsAnimationFinished()
    {
        return isAnimationFinished;
    }

    public void OnAnswerClicked()
    {
        if (isRight)
        {
            EffectsCatalog.BounceEffect(image.gameObject);
            stars.SetActive(true);
            stars.GetComponent<ParticleSystem>().Play();
            Invoke("RightAnswer", 0.5f);
        }
        else
        {
            EffectsCatalog.ShakeEffect(gameObject);
        }
    }

    public void RemoveSprite()
    {
        image.sprite = null;
    }

    private void AnimationIsFinished()
    {
        isAnimationFinished = true;
    }

    private void RightAnswer()
    {
        levelController.OnRightAnswerClicked();
    }
}