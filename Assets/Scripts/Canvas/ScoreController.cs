using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Контроллер ссчётчика очков:")]
public class ScoreController : MonoBehaviour
{
    public int score;
    [SerializeField] Text scoreText;

    private void OnEnable()
    {
        StructureController.OnLayerDeleted += AddScore;
    }

    private void OnDisable()
    {
        StructureController.OnLayerDeleted -= AddScore;
    }

    /// <summary>
    /// Добавляет ссчёт
    /// </summary>
    void AddScore()
    {
        score += GameManager.gridWidth * GameManager.gridWidth;
        scoreText.text = $"Ссчёт: {score}";
    }

    /// <summary>
    /// Устанавливает ссчёт.
    /// </summary>
    public void SetScore(int val)
    {
        score = val;
        scoreText.text = $"Ссчёт: {val}";
    }
}
