using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerAndCameraPrefab;

    [SerializeField] 
    private string locationToLoad;

    [SerializeField] 
    private string guiScene;
    
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
        SceneManager.LoadScene(guiScene);
        //SceneManager.LoadScene(locatioToLoad, LoadSceneMode.Additive);

        SceneManager.LoadSceneAsync(locationToLoad, LoadSceneMode.Additive).completed += operation =>
        {
            Vector3 startPosition = GameObject.Find("PlayerStart").transform.position;

            Instantiate(playerAndCameraPrefab, startPosition, Quaternion.identity);
        };


    }
}


