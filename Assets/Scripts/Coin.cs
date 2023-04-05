using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;

    bool collected;

    public void LoadData(GameData data)
    {
        data.coinCollected.TryGetValue(id, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.coinCollected.ContainsKey(id))
        {
            data.coinCollected.Remove(id);
        }
        data.coinCollected.Add(id, collected);
    }

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) { collected = true; GameManager.Instance.AddCoin(); gameObject.SetActive(false); }
    }
}
