using UnityEngine;

public class MobileInput : CameraController
{
    // Переменные для хранения предыдущих позиций двух касаний
    private Vector2 previousTouch1;
    private Vector2 previousTouch2;

    // Метод обработки ввода
    protected override void HandleInput()
    {
        // Проверяем, есть ли два активных касания на экране
        if (Input.touchCount == 2)
        {
            // Получаем первое и второе касание
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Проверяем, двигались ли оба пальца по экрану
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                // Вычисляем расстояние между двумя касаниями на предыдущем кадре
                float prevDist = Vector2.Distance(previousTouch1, previousTouch2);

                // Вычисляем расстояние между текущими позициями касаний
                float currDist = Vector2.Distance(touch1.position, touch2.position);

                // Разница между текущим и предыдущим расстоянием (пинч-жест)
                float pinchDelta = currDist - prevDist;

                // Изменяем масштаб камеры на основе разницы расстояний
                ZoomCamera(pinchDelta);
            }

            // Вычисляем среднее значение смещения (движения) двух пальцев
            Vector2 averageDelta = (touch1.deltaPosition + touch2.deltaPosition) / 2;

            // Поворачиваем камеру на основе среднего смещения
            RotateCamera(averageDelta);

            // Сохраняем текущие позиции касаний для использования в следующем кадре
            previousTouch1 = touch1.position;
            previousTouch2 = touch2.position;
        }
    }
}
