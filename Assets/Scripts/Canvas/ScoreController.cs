using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("���������� ��������� �����:")]
public class ScoreController : MonoBehaviour
{
    public static int score;
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
    /// ��������� �����
    /// </summary>
    void AddScore()
    {
        score += GameManager.gridWidth * GameManager.gridWidth; ;
        scoreText.text = $"�����: {score}";
    }
}
