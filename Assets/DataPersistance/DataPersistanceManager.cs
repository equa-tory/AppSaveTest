using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistance = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private DataFileHandler dataHandler;

    private string selectedProfileId = "";

    public static DataPersistanceManager instance { get; private set; }


    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found more Data Persistance Manager in the scene.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistance)
        {
            Debug.LogWarning("Data Persistance is currentrly disabled!");
        }

        this.dataHandler = new DataFileHandler(Application.persistentDataPath, fileName, useEncryption);

        InitizlizeSelectedProfileId();

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;

        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);

        InitizlizeSelectedProfileId();

        LoadGame();
    }

    private void InitizlizeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Override id with test id: " + testSelectedProfileId);
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disableDataPersistance)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            return;
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }

    }   

    public void SaveGame()
    {
        if (disableDataPersistance)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("Tried to save the game but game data was null. This may indicate an issue with the order in which SaveGame() is getting called");
            return;
        }

        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(gameData);

        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        Save();
        Debug.Log("Quit");
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public void Save()
    {
        if (this.gameData == null)
        {
            NewGame();
        }
        SaveGame();
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfileGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
