using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Level : MonoBehaviour
    {
        public static Level Instance;

        [SerializeField] private Transform objectsParent;

        public int objectsInScene;
        public int totalObjects;

        [Header("Materials")]
        [SerializeField] private Material groundMaterial;
        [SerializeField] private Material objectMaterial;
        [SerializeField] private Material obstacleMaterial;
        [SerializeField] private SpriteRenderer groundBorderSprite;
        [SerializeField] private SpriteRenderer groundSideSprite;
        [SerializeField] private Image progressFillImage;
        [SerializeField] private Image bgFadeSprite;

        [Header("Colors")]
        [SerializeField] private Color groundColor;
        [SerializeField] private Color objectColor;
        [SerializeField] private Color obstacleColor;
        [SerializeField] private Color borderColor;
        [SerializeField] private Color sideColor;
        [SerializeField] private Color progressFillColor;
        [SerializeField] private Color fadeColor;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            UpdateLevelColors();
            CountObjects();
        }

        void CountObjects()
        {
            totalObjects = objectsParent.childCount;
            objectsInScene = totalObjects;
        }

        public void LoadNextLevel()
        {
            int totalLevels = SceneManager.sceneCountInBuildSettings;
            bool isNextLevelAvailable = SceneManager.GetActiveScene().buildIndex + 1 < totalLevels;
            if (!isNextLevelAvailable)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.isGameOver = false;
        }

        public void UpdateLevelColors()
        {
            groundMaterial.color = groundColor;
            groundSideSprite.color = sideColor;
            groundBorderSprite.color = borderColor;

            objectMaterial.color = objectColor;
            obstacleMaterial.color = obstacleColor;

            progressFillImage.color = progressFillColor;
            bgFadeSprite.color = fadeColor;

        }

        private void OnValidate()
        {
            UpdateLevelColors();
        }
    }
}
