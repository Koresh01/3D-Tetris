﻿using UnityEngine;

[AddComponentMenu("Custom/InputSettings (Хранит настройки)")]
public class InputSettings : MonoBehaviour
{
    [Header("Основные настройки")]
    public Camera camera;
    public AngleBar angleBar;
    public GameManager gameManager;
    public Transform target;

    [Header("Скорости:")]
    [Range(0.01f, 1f)] public float rotationSpeed;
    [Range(20f, 60f)] public float zoomStep;

    [Header("Расстояния до цели:")]
    public float minDistance = 2f;
    public float maxDistance = 10f;

    [Header("Углы:")]
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
}
