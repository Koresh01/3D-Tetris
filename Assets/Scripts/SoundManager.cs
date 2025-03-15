using UnityEngine;

/// <summary>
/// Singleton-класс для управления звуками в игре.
/// Позволяет воспроизводить различные звуки, такие как установка блока, нажатие кнопки и т. д.
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Экземпляр Singleton для доступа к SoundManager из любого места в коде.
    /// </summary>
    public static SoundManager Instance { get; private set; }

    [Header("Sound Clips")]
    [SerializeField] private AudioClip placeBlockSound; // Звук установки кубика
    [SerializeField] private AudioClip buttonClickSound; // Звук нажатия кнопки
    [SerializeField] private AudioClip moveErrorSound; // Звук ошибки перемещения/вращения
    [SerializeField] private AudioClip clearLayerSound; // Звук удаления слоя
    [SerializeField] private AudioClip moveRotateSound; // Звук смещения/вращения детали

    [SerializeField] private AudioSource audioSource; // Источник звука

    private void Awake()
    {
        // Реализация Singleton-паттерна
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать объект при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject); // Удалить дубликат Singleton
            return;
        }

        // Если AudioSource не назначен через инспектор, создаем его программно
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Воспроизводит звук установки кубика.
    /// </summary>
    public void PlayPlaceBlock() => PlaySound(placeBlockSound);

    /// <summary>
    /// Воспроизводит звук нажатия кнопки.
    /// </summary>
    public void PlayButtonClick() => PlaySound(buttonClickSound);

    /// <summary>
    /// Воспроизводит звук ошибки перемещения или вращения детали.
    /// </summary>
    public void PlayMoveError() => PlaySound(moveErrorSound);

    /// <summary>
    /// Воспроизводит звук удаления слоя.
    /// </summary>
    public void PlayClearLayer() => PlaySound(clearLayerSound);

    /// <summary>
    /// Воспроизводит звук смещения или вращения детали.
    /// </summary>
    public void PlayMoveRotate() => PlaySound(moveRotateSound);

    /// <summary>
    /// Воспроизводит переданный звук, если он не null.
    /// </summary>
    /// <param name="clip">Аудиофайл, который нужно воспроизвести.</param>
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // Проигрывание звука без прерывания текущего
        }
    }
}
