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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataPersistanceManager.instance.SaveGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void AddDeath()
    {
        deaths++;
        deathsText.text = "Deaths: " + deaths;
        playerGO.transform.position = Vector3.zero;
        //Instantiate(playerGO, Vector2.zero, Quaternion.identity);
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

    public void SaveData(GameData data)
    {
        data.deathCount = this.deaths;
    }
}
