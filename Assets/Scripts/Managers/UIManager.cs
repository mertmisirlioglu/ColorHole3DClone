using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    [SerializeField] int  sceneOffset;

    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI currentLevelTextShadow;
    [SerializeField] TextMeshProUGUI nextLevelText;
    [SerializeField] TextMeshProUGUI nextLevelTextShadow;

    [SerializeField] Image progressFillImage;

    [SerializeField] private GameObject winPopup;
    [SerializeField] private Image winBackground;
    [SerializeField] private Image continueButton;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject continueText;

    [SerializeField] private Image fadePanel;
    [SerializeField] private GameObject winEffect;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        progressFillImage.fillAmount = 0f;
        SetLevelProgressText();
        FadeAtStart();
    }

    void SetLevelProgressText()
    {
        int level = SceneManager.GetActiveScene().buildIndex + sceneOffset;

        currentLevelText.SetText(level.ToString());
        currentLevelTextShadow.SetText(level.ToString());

        nextLevelText.SetText((level + 1).ToString());
        nextLevelTextShadow.SetText((level + 1).ToString());
    }

    public void UpdateProgressBar()
    {
        float value = (Level.Instance.totalObjects - (float)Level.Instance.objectsInScene) / Level.Instance.totalObjects;
        progressFillImage.DOFillAmount(value, 0.4f);
    }

    public void ShowWinPopup()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings;
        bool isNextLevelAvailable = SceneManager.GetActiveScene().buildIndex + 1 < totalLevels;

        winPopup.SetActive(true);
        winBackground.DOFade(1f, 1f).OnComplete(() =>
        {
            continueButton.DOFade(1f, 1f).OnComplete((() =>
            {
                if (!isNextLevelAvailable)
                {
                    continueText.GetComponent<TextMeshProUGUI>().SetText("REPLAY");
                    continueText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("REPLAY");
                }

                continueText.transform.DOScale(1f, 1f);
            }));
        });

        if (!isNextLevelAvailable)
        {
            winText.GetComponent<TextMeshProUGUI>().SetText("END OF THE GAME!");
            winText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("END OF THE GAME!");
        }

        winText.transform.DOScale(1f, 1f);
    }


    public void FadeAtStart()
    {
        fadePanel.DOFade(0f, 1.5f).From(1f);
    }

    public void PlayWinFx()
    {
        winEffect.SetActive(true);
        Invoke(nameof(ShowWinPopup), 2f);
    }
}
}
