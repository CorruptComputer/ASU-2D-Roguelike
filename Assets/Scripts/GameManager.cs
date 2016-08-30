using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager:MonoBehaviour{

    public float startDelay = .2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

	// Use this for initialization
	void Awake(){
        if (instance == null){
            instance = this;
        }else if(instance != this){
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
	}

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
        level++;
        InitGame();
    }

    void OnEnable(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void InitGame(){
        doingSetup = true;

        levelImage = GameObject.Find("Level Image");
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
        levelText.text = "Day: " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", startDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    void HideLevelImage(){

        levelImage.SetActive(false);
        doingSetup = false;

    }

    public void GameOver(){
        levelText.text = "You survived for " + level + " \ndays before you starved.";
        levelImage.SetActive(true);

        enabled = false;
    }
	
	void Update(){
        if (playersTurn || enemiesMoving || doingSetup){
            return;
        }
        StartCoroutine(MoveEnemies());
	}

    public void AddEnemyToList(Enemy script){
		enemies.Add(script);
    }

    IEnumerator MoveEnemies(){
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0){
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++){
            enemies[i].moveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
