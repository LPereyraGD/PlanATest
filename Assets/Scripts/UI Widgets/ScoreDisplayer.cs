using TMPro;
using UnityEngine;

namespace Game.Managers
{
    public class ScoreDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameController gameController;

        private void Awake()
        {
            if (gameController == null)
            {
                Debug.LogError("GameController not assigned! Please assign it in the inspector.");
                return;
            }

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (gameController == null) return;

            gameController.OnScoreChanged += OnScoreChanged;
        }
        private void OnScoreChanged(int newScore)
        {
            if (scoreText != null)
                scoreText.text = $"{newScore}";
        }
    }
}
