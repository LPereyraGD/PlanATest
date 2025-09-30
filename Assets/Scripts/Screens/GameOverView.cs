using Game.Services;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private Button replayButton;

    private void Start()
    {
        if (replayButton != null)
        {
            replayButton.onClick.AddListener(OnReplayButtonClicked);
        }
    }

    public void ShowGameOverScreen()
    {
        //TODO implement animations
        gameObject.SetActive(true);
    }

    private void OnReplayButtonClicked()
    {
        //TODO implement animations
        Navigator.Instance.RestartGame();
        Destroy(this.gameObject);
    }
}
