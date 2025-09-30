using TMPro;
using UnityEngine;

namespace Game.Managers
{
    public class MoveDispalyer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveText;
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

            gameController.OnMovesChanged += OnScoreChanged;
        }
        private void OnScoreChanged(int newScore)
        {
            if (moveText != null)
                moveText.text = $"{newScore}";
        }
    }
}