using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public int deathCount;

    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> coinCollected;

    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        coinCollected = new SerializableDictionary<string, bool>();
    }

    public int GetPercentageComplete()
    {
        int totalCollected = 0;
        foreach(bool collected in coinCollected.Values)
        {
            if (collected) totalCollected++;
        }

        int percentageCompleted = -1;
        if(coinCollected.Count != 0)
        {
            percentageCompleted = (totalCollected * 100 / coinCollected.Count);
        }
        return percentageCompleted;
    }
}
