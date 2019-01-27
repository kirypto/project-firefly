﻿using UnityEngine;

public class SpawnLocationScript : MonoBehaviour
{
    [SerializeField] private int spawnIndex;
    [SerializeField] private int numFireflies;

    public int SpawnIndex => spawnIndex;

    public int NumFireflies => numFireflies;

    public Vector2 Location => transform.position;
}