using UnityEngine;

/// <summary>
/// Скрипт устанавливает целевое значение FPS в игре.
/// </summary>
public class FPScontroller : MonoBehaviour
{
    [SerializeField] private int targetFPS = 120;

    private void Awake()
    {
        // Отключаем вертикальную синхронизацию(если включена она сама регулирует FPS)
        QualitySettings.vSyncCount = 0;

        // Устанавливаем желаемый FPS:
        Application.targetFrameRate = targetFPS;
    }


}
