using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] SpawnObject spawnMoney = null;
    
    [SerializeField] Transform spawnPoint = null;

    [SerializeField] Text scoreText = null;

    [SerializeField] Text gameOverScoreText = null;

    [SerializeField] GameObject gameStartUI = null;

    [SerializeField] GameObject gameOverUI = null;

    [SerializeField] GameObject line = null;

    [SerializeField] GameObject bottom = null;


    public bool gameOver;

    public int maxLevel;

    public int currentScore;

    void Awake()
    {
        Application.targetFrameRate = 60;

        gameOver = false;
    }
    public void GameStart()
    {
        line.SetActive(true);

        bottom.SetActive(true);

        scoreText.gameObject.SetActive(true);
        
        gameStartUI.SetActive(false);

        currentScore = 0;

        scoreText.text = "0";

        Invoke("NextMoney", 1f);
    }
    SpawnObject GetMoney()
    {
        GameObject tempMoney = ObjectPool.instance.mint.Dequeue();

        SpawnObject createMoney = tempMoney.GetComponent<SpawnObject>();

        createMoney.transform.position = spawnPoint.transform.position;

        return createMoney;
    }
    void NextMoney()
    {
        if (gameOver) return; 
        
        spawnMoney = GetMoney(); 

        spawnMoney.level = Random.Range(0, maxLevel);

        spawnMoney.gameObject.SetActive(true);

        StartCoroutine(waitCoroutine());
    }
    IEnumerator waitCoroutine()
    {
        while (spawnMoney != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        NextMoney();
    }
    public void GameOver()
    {
        if (gameOver) return;
        
        gameOver = true;

        gameOverScoreText.text = "Á¡¼ö : " + scoreText.text;

        gameOverUI.SetActive(true);

    }
    public void Reset()
    {
        StartCoroutine(ResetCoroutine());
    }
    IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }
    public void TouchDown()
    {
        if (spawnMoney == null) return;

        spawnMoney.objectMove();
    }
    public void TouchUp()
    {
        if (spawnMoney == null) return;

        spawnMoney.objectDrop();

        spawnMoney = null;
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel")) Application.Quit();    
    }
    void LateUpdate()
    {
        scoreText.text = string.Format("{0:#,##0}", currentScore);
    }
}
