using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void OnGameLoadedEvent(SaveData data);

[Serializable]
public struct SaveData
{
    public int GameWon;
}

public class DataManager : MonoBehaviour {

    public static DataManager instance;
    private GameProgress _gameProgress;
    private string saveFileName = "/SavedData.dat";
    private SaveData _gameData;

    public static event OnGameLoadedEvent GameLoadedEvent;

    #region UNITY_API

    void Awake ()
    {
        DataManagerSingleton();
        DontDestroyOnLoad(gameObject);
        _gameData = LoadData();
        SaveData(_gameData);
    }

    private void Start()
    {
        GameLoadedEvent(_gameData);
    }

    private void OnDisable()
    {
        if (_gameProgress) { _gameProgress.GameWonEvent -= SetData; }
        SaveData(_gameData);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (FindObjectOfType<GameProgress>() != null)
        {

            _gameProgress = FindObjectOfType<GameProgress>();
            _gameProgress.GameWonEvent += SetData;
        }
    }

    #endregion //UNITY_API

    #region SAVE_LOAD_SET

    private void SaveData(SaveData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + saveFileName, FileMode.OpenOrCreate);
        bf.Serialize(fs, data);
        fs.Close();
    }

    private SaveData LoadData()
    {
        if (File.Exists(Application.persistentDataPath + saveFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream filestream = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(filestream);
            filestream.Close();
            return saveData;
        }
        else
        {
            SaveData newData = new SaveData();
            newData.GameWon = 0;
            return newData;
        }
    }

    private void SetData(SaveData data)
    {
        _gameData = data;
    }

    #endregion //SAVE_LOAD_SET 

    //Singleton
    private void DataManagerSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
}
