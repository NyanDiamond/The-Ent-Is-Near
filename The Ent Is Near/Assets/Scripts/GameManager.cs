using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] [Tooltip("(Axemen, Chainsawmen, Cars)")] Vector3[] waves;
    [SerializeField] GameObject[] enemyTypes;
    int currentWave;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] GameObject victoryScreen, loseScreen, pauseScreen;
    private List<Enemy> enemies;
    private List<Tree> trees;
    [SerializeField] float horizontalSpawn, verticalSpawn;
    // Start is called before the first frame update
    private void Awake()
    {
        enemies = new List<Enemy>();
        trees = new List<Tree>();
    }
    void Start()
    {
        Time.timeScale = 1;
        currentWave = 1;
        waveText.text = "Wave: " + currentWave;
        SpawnWave();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(Time.timeScale ==  1)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else if(pauseScreen.activeInHierarchy)
            {
                Time.timeScale = 1;
                pauseScreen.SetActive(false);
            }
        }
    }

    void SpawnWave()
    {
        Vector3 wave = waves[currentWave - 1];
        for (int i = 0; i<Mathf.Max((int)wave.x, (int)wave.y, (int)wave.z); i++)
        {
            if (i < wave.x) Instantiate(enemyTypes[0], SpawnPoint(), Quaternion.identity);
            if (i < wave.y) Instantiate(enemyTypes[1], SpawnPoint(), Quaternion.identity);
            if (i < wave.z) Instantiate(enemyTypes[2], SpawnPoint(), Quaternion.identity);
        }
        
    }

    Vector2 SpawnPoint()
    {
        int edge = Random.Range(0, 4);
        switch(edge)
        {
            case 0:
                return new Vector2(-horizontalSpawn, Random.Range(-verticalSpawn, verticalSpawn));
            case 1:
                return new Vector2(horizontalSpawn, Random.Range(-verticalSpawn, verticalSpawn));
            case 2:
                return new Vector2(Random.Range(-horizontalSpawn, horizontalSpawn), -verticalSpawn);
            case 3:
                return new Vector2(Random.Range(-horizontalSpawn, horizontalSpawn), verticalSpawn);
            default:
                return Vector2.zero;
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        Debug.Log(enemies.Count);
        if (enemies.Count <= 0)
        {
            currentWave++;
            if (currentWave > waves.Length)
            {
                //win
                Time.timeScale = 0;
                victoryScreen.SetActive(true);
            }
            else
            {
                waveText.text = "Wave: " + currentWave;
                SpawnWave();
            }
        }
    }

    public void AddTree(Tree tree)
    {
        trees.Add(tree);
    }

    public void RemoveTree(Tree tree)
    {
        trees.Remove(tree);
        Debug.Log(trees.Count);
        if (trees.Count <= 0)
        {
            Time.timeScale = 0;
            loseScreen.SetActive(true);

        }
    }

}
