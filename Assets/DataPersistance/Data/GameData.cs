using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;

    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> coinCollected;

    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        coinCollected = new SerializableDictionary<string, bool>();
    }
}
