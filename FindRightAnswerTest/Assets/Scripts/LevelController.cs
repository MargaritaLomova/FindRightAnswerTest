using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("Variables To Level Control"), Range(1, 3), SerializeField]
    private int levelIndex = 1;
    [SerializeField]
    private List<string> pathsToSprites;
    [SerializeField]
    private List<Color> availableColorsOfAnswersBackground;

    [Header("Objects From Scene"), SerializeField]
    private Transform answersVerticalHolder;
    [SerializeField]
    private TMP_Text findText;
    [SerializeField]
    private GameObject restartOverlay;
    [SerializeField]
    List<Image> restartButtonComponents = new List<Image>();
    [SerializeField]
    private GameObject load;

    [Header("Prefabs"), SerializeField]
    private AnswerController answerPrefab;
    [SerializeField]
    private Transform answersHorizontalHolderPrefab;

    private int countOfAnswers;
    private List<Transform> answersHorizontalHolders = new List<Transform>();
    private List<Sprite> sprites = new List<Sprite>();
    private List<Color> tempColors = new List<Color>();
    private List<Sprite> tempSprites = new List<Sprite>();

    private void Start()
    {
        restartOverlay.SetActive(false);

        Clear();
        Draw();
    }

    #region Custom Methods

    #region Public

    public void OnRightAnswerClicked()
    {
        if (levelIndex < 3)
        {
            LoadNextLevel();
        }
        else
        {
            FinishGame();
        }
    }

    public void OnResetClicked()
    {
        EffectsCatalog.FadeInEffect(restartOverlay.gameObject, 1);
        SlowlyHideRestartButton();

        Invoke("ResetLevelToFirst", 1);

        ShowLoadingIconWithRotation();

        Invoke("SlowlyHideOverlay", 2f);
    }

    #endregion

    #region Private

    #region Clear

    private void Clear()
    {
        if (levelIndex == 1)
        {
            ClearAll();
        }
        else
        {
            ClearSpritesInCells();
        }
    }

    private void ClearSpritesInCells()
    {
        foreach(var answer in FindObjectsOfType<AnswerController>())
        {
            answer.RemoveSprite();
        }

        sprites.Clear();
        tempColors.Clear();
        tempSprites.Clear();
    }

    private void ClearAll()
    {
        foreach (var answer in FindObjectsOfType<AnswerController>())
        {
            Destroy(answer.gameObject);
        }

        foreach (var holder in answersHorizontalHolders)
        {
            Destroy(holder.gameObject);
        }
        answersHorizontalHolders.Clear();

        sprites.Clear();
        tempColors.Clear();
        tempSprites.Clear();
    }

    #endregion

    #region Draw

    private void Draw()
    {
        GetRandomListOfSprites();
        if (levelIndex == 1)
        {
            StartCoroutine(DrawAnswersWithAnimation());
        }
        else
        {
            UpdateOldAnswersAndAddNew();
        }
    }

    private void UpdateOldAnswersAndAddNew()
    {
        countOfAnswers = 3 * levelIndex;
        var numberOfRightAnswer = Random.Range(0, countOfAnswers);
        var futureSprites = new List<Sprite>();

        for (int i = 0; i < countOfAnswers; i++)
        {
            futureSprites.Add(GetRandomSprite());
            if (i == numberOfRightAnswer)
            {
                findText.text = $"Find {futureSprites.Last().name}";
            }
        }

        for(int i = 0; i < FindObjectsOfType<AnswerController>().Length; i++)
        {
            FindObjectsOfType<AnswerController>()[i].Set(futureSprites[i], i == numberOfRightAnswer);
        }

        for (int i = FindObjectsOfType<AnswerController>().Length; i < countOfAnswers; i++)
        {
            if (answersHorizontalHolders.Last().childCount % 3 == 0)
            {
                var newAnswerHorizontalHolder = Instantiate(answersHorizontalHolderPrefab, answersVerticalHolder);
                answersHorizontalHolders.Add(newAnswerHorizontalHolder);
            }
            var newAnswer = Instantiate(answerPrefab, answersHorizontalHolders.Last());
            newAnswer.Set(futureSprites[i], GetRandomBackgroundColor(), i == numberOfRightAnswer, false);
        }
    }

    private IEnumerator DrawAnswersWithAnimation()
    {
        countOfAnswers = 3 * levelIndex;
        var numberOfRightAnswer = Random.Range(0, countOfAnswers);
        var futureSprites = new List<Sprite>();

        for (int i = 0; i < countOfAnswers; i++)
        {
            futureSprites.Add(GetRandomSprite());
            if (i == numberOfRightAnswer)
            {
                findText.text = $"Find {futureSprites.Last().name}";
                EffectsCatalog.FadeInEffect(findText.gameObject, 1);
            }
        }

        for (int i = 0; i < countOfAnswers; i++)
        {
            if (i == 0 || i % 3 == 0)
            {
                var newAnswerHorizontalHolder = Instantiate(answersHorizontalHolderPrefab, answersVerticalHolder);
                answersHorizontalHolders.Add(newAnswerHorizontalHolder);
            }
            var newAnswer = Instantiate(answerPrefab, answersHorizontalHolders.Last());
            var backgroundColor = GetRandomBackgroundColor();
            newAnswer.Set(futureSprites[i], backgroundColor, i == numberOfRightAnswer);
            yield return new WaitUntil(() => newAnswer.IsAnimationFinished());
        }
    }

    #endregion

    private void ResetLevelToFirst()
    {
        levelIndex = 1;
        Clear();
        Draw();
    }

    private void FinishGame()
    {
        restartOverlay.SetActive(true);
        load.SetActive(false);
        SlowlyShowRestartButton();
    }

    private void LoadNextLevel()
    {
        levelIndex++;
        Clear();
        Draw();
    }

    #region Show/Hide Objects

    private void ShowLoadingIconWithRotation()
    {
        load.SetActive(true);
        load.GetComponent<Image>().color = new Color(load.GetComponent<Image>().color.r, load.GetComponent<Image>().color.g, load.GetComponent<Image>().color.b, 1);
        EffectsCatalog.RotateEffect(load);
    }

    private void HideOVerlay()
    {
        restartOverlay.SetActive(false);
    }

    #region Slowly Show/Hide Objects

    private void SlowlyHideOverlay()
    {
        EffectsCatalog.FadeOutEffect(restartOverlay.gameObject, 1);
        Invoke("HideOVerlay", 1);
    }

    private void SlowlyHideRestartButton()
    {
        foreach (var component in restartButtonComponents)
        {
            EffectsCatalog.FadeOutEffect(component.gameObject, 0.5f);
            component.gameObject.SetActive(false);
        }
    }

    private void SlowlyShowRestartButton()
    {
        foreach (var component in restartButtonComponents)
        {
            component.gameObject.SetActive(true);
            component.color = new Color(component.color.r, component.color.g, component.color.b, 0);
            EffectsCatalog.FadeInEffect(component.gameObject, 1);
        }
    }

    #endregion

    #endregion

    #region Get Smthng Random

    private Color GetRandomBackgroundColor()
    {
        if (tempColors.Count == 0)
        {
            tempColors.AddRange(availableColorsOfAnswersBackground);
        }
        int randomIndex = Random.Range(0, tempColors.Count);
        var color = tempColors[randomIndex];
        tempColors.Remove(color);
        return color;
    }

    private Sprite GetRandomSprite()
    {
        if (tempSprites.Count == 0)
        {
            tempSprites.AddRange(sprites);
        }
        int randomIndex = Random.Range(0, tempSprites.Count);
        var letter = tempSprites[randomIndex];
        tempSprites.Remove(letter);
        return letter;
    }

    private void GetRandomListOfSprites()
    {
        var objects = Resources.LoadAll(pathsToSprites[Random.Range(0, pathsToSprites.Count)], typeof(Sprite));
        foreach (var obj in objects)
        {
            sprites.Add((Sprite)obj);
        }
    }

    #endregion

    #endregion

    #endregion
}