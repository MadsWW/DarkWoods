using UnityEngine;
using UnityEngine.SceneManagement;

public class KongregateUserInfo
{
    public string[] info;
    public int userID;
    public string userName;
    public string gameAuthToken;
}


public class KongregateAPI : MonoBehaviour {

    public static KongregateAPI instance;
    private GameProgress _gameProgress;
    private DataManager _dataManager;

    public void Awake()
    {
        KongregateAPISingleton();
        DontDestroyOnLoad(gameObject);
        gameObject.name = "KongregateAPI";

        Application.ExternalEval(
          @"if(typeof(kongregateUnitySupport) != 'undefined'){
        kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');
      };"
        );
    }

    private void OnEnable()
    {
        DataManager.GameLoadedEvent += KongregateAPI.instance.SendInfoToKongregateAPI;
    }

    private void OnDisable()
    {
        DataManager.GameLoadedEvent -= KongregateAPI.instance.SendInfoToKongregateAPI;
        if(_gameProgress)_gameProgress.GameWonEvent -= SendInfoToKongregateAPI;
    }

    private void OnLevelWasLoaded(int level)
    {
        if(FindObjectOfType<GameProgress>() != null)
        {
            _gameProgress = FindObjectOfType<GameProgress>();
            _gameProgress.GameWonEvent += SendInfoToKongregateAPI;
        }
    }

    public void OnKongregateAPILoaded(string userInfoString)
    {
        //Call function when API Loads.
    }

    private void SendInfoToKongregateAPI(SaveData data)
    {
        Application.ExternalCall("kongregate.stats.submit", "WonGame", data.GameWon);
    }

    private void KongregateAPISingleton()
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
