using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistance
{
    public static GameManager Instance;

    public int deaths;
    public TMP_Text deathsText;

    public int maxCoins;
    public int collectedCoins;
    public TMP_Text coinsText;

    public GameObject playerGO;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        deathsText.text = "Deaths: " + deaths;
        coinsText.text = "Coins: " + collectedCoins + "/" + maxCoins;
    }

    public void AddDeath()
    {
        deaths++;
        deathsText.text = "Deaths: " + deaths;
        Instantiate(playerGO, Vector2.zero, Quaternion.identity);
    }

    public void AddCoin()
    {
        collectedCoins++;
        coinsText.text = "Coins: " + collectedCoins + "/" + maxCoins;
    }

    public void LoadData(GameData data)
    {
        foreach(KeyValuePair<string, bool> pair in data.coinCollected)
        {
            if (pair.Value)
            {
                collectedCoins++;
            }
        }

        this.deaths = data.deathCount;
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = this.deaths;
    }
}
