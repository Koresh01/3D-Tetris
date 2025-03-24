# 3D-Tetris
Игра 3Д тетрис, для мобильных устройств. Unity3D.
* кросплатформенность
* возможность выбрать размерность поля через параметр
* возможность вращения камеры вокруг поля
* удобное управление

Поиграть в игру можно по ссылке:
https://www.rustore.ru/catalog/app/com.com.mygame.TetrisGame.TetrisGame

![image](https://github.com/user-attachments/assets/e38dbf22-390b-468d-b9cd-12a1bc7582f3)
<img src="https://github.com/user-attachments/assets/7bebf138-e63b-409b-b1f9-bec8d17e10c4" height="550">



# Абстрактные классы
В этой игре я использовал старую систему ввода Untiy, и я решил сделать 2 скрипта:
* ```MobileInput``` - для обработки мобильного ввода (touches)
* ```ComputerInput``` - для обработки компьютерного ввода (смещения позиции мыши и т.д.)

![image](https://github.com/user-attachments/assets/68351dbe-904f-4e1d-8cbc-83fc19454def)

И ещё мне пришла в голову отличная мысль - Прописать одни и те же вспомогательные функции, которые будут использоваться как в ```MobileInput```, так и в ```ComputerInput```. Причем я хочу избежать дублирования кода!

В таких случаях создаётся класс ```CommonFunctions```, который хранит все общие функции и переменные, которые должны повторяться у его наследников:
```C#
/// Абстрактный контроллер камеры, содержащий основные методы для управления камерой,
/// такие как вращение, масштабирование и позиционирование.
/// Этот класс следует расширять для реализации управления на разных устройствах.
public abstract class CommonFunctions : MonoBehaviour
{
    // поля класса ... 

    // Изменяет расстояние между камерой и целевым объектом (зум).
    protected void ZoomCamera(float pinchDelta)
    {
        ...
    }

    // Вращает камеру вокруг целевого объекта с учетом ограничений по вертикальному углу.
    public void RotateCamera(Vector2 rotationDelta)
    {
        ...
    }

    /// Обновляет позицию камеры, сохраняя корректное расстояние до целевого объекта.
    protected void UpdateCameraPosition()
    {
        ...
    }

    // Абстрактный метод для обработки пользовательского ввода.
    // Должен быть реализован в дочерних классах для различных платформ (мобильные устройства, ПК и т. д.).
    protected abstract void HandleInput();    // <----------------------------------------------

    // Вызываем обработку пользовательского ввода на каждом кадре.
    private void Update()
    {
        HandleInput();
    }
}

```

Так что теперь, в его дочерних классах не нужно писать даже ```Update()``` ! Ровно как и реализации вспомогательных функций ```ZoomCamera()``` и ```RotateCamera()```. Просто потому что их реализации уже прописаны в родительском классе.

Вот как выглядит мой скрипт для *комьютерного ввода*:
```C#
public class ComputerInput : CommonFunctions
{
    private Vector2 previousMousePosition;

    protected override void HandleInput()  // <---------------- переопределяем метод Handle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 pixelDeltaX = currentMousePosition - previousMousePosition;

            RotateCamera(pixelDeltaX);  // <---------------- эта функция реализована у класса CommonFunctions
            previousMousePosition = currentMousePosition;
        }

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            ZoomCamera(scroll);  // <---------------- эта функция реализована у класса CommonFunctions
        }
    }
}
```
Вот как выглядит мой скрипт для *мобильного ввода*:
```C#
public class MobileInput : CommonFunctions
{
    private Vector2 previousSingleTouchPos;
    private Vector2 previousTouch1;
    private Vector2 previousTouch2;
    private bool wasUsingTwoFingers = false;

    protected override void HandleInput()  // <---------------- переопределяем метод Handle()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (touchCount == 2)
        {
            HandleTwoFingerTouch(Input.GetTouch(0), Input.GetTouch(1));
        }
    }

    // Обрабатывает перемещение одним пальцем (вращение камеры).
    private void HandleSingleTouch(Touch touch)
    {
        if (wasUsingTwoFingers)
        {
            // Сброс предыдущей позиции, чтобы избежать резких скачков после двух пальцев.
            previousSingleTouchPos = touch.position;
            wasUsingTwoFingers = false;
        }

        if (touch.phase == TouchPhase.Began)
        {
            previousSingleTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 delta = touch.position - previousSingleTouchPos;
            RotateCamera(delta);                   // <---------------- эта функция реализована у класса CommonFunctions
            previousSingleTouchPos = touch.position;
        }
    }

    // Обрабатывает жесты двумя пальцами (зум и вращение).
    private void HandleTwoFingerTouch(Touch touch1, Touch touch2)
    {
        if (!wasUsingTwoFingers)
        {
            // При первом касании двумя пальцами запоминаем позиции.
            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
            wasUsingTwoFingers = true;
        }

        if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
        {
            float prevDistance = Vector2.Distance(previousTouch1, previousTouch2);
            float currDistance = Vector2.Distance(touch1.position, touch2.position);
            float zoomDelta = (currDistance - prevDistance) / (userInputSettings.zoomStep * 1.5f);
            ZoomCamera(zoomDelta);                // <---------------- эта функция реализована у класса CommonFunctions

            Vector2 averageDelta = (touch1.deltaPosition + touch2.deltaPosition) * 0.5f;
            RotateCamera(averageDelta / 2.2f);    // <---------------- эта функция реализована у класса CommonFunctions

            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
        }
    }
}
```
