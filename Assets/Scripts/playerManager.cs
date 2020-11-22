using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerManager : MonoBehaviour
{
    // Player specific variables
    //private int health;
    //private int score;
    //private List<Collectable> inventory;
    public Text inventoryText;
    public Text descriptionText;
    private int currentIndex;

    // Boolean values
    private bool isGamePaused = false;

    // UI stuff
    public Text healthText;
    public Text scoreText;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;

    public PlayerInfo info;

    // Start is called before the first frame update
    void Start()
    {
        // Makes sure game is "unpaused"
        isGamePaused = false;
        Time.timeScale = 1.0f;

        // Make sure all menus are filled in
        FindAllMenus();

        //Start player with initial health and score
        //health = 100;
        //score = 0;

        info = GameObject.FindWithTag("Info").GetComponent<PlayerInfo>();

        foreach (Colectable item in info.inventory)
        {
            item.player = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + info.health.ToString();
        scoreText.text  = "Score:  " + info.score.ToString();

        if (info.inventory.Count == 0) //if inventory is empty
        {
            inventoryText.text = "Current Selection: None";
            descriptionText.text = "";
        }
        else //if inventory IS NOT empty
        {
            inventoryText.text = "Current Selection: " + info.inventory[currentIndex].collectableName + " " + currentIndex.ToString();
            descriptionText.text = "Press [E] to " + info.inventory[currentIndex].description;
        }

        if (info.inventory.Count > 0)
        {
            //Use currently selected Item
            if (Input.GetKeyDown(KeyCode.E))
            {
                info.inventory[currentIndex].Use();
                info.inventory.RemoveAt(currentIndex);

                if (info.inventory.Count != 0)
                {
                    currentIndex = (currentIndex - 1) % info.inventory.Count;

                }
            }

            // Switch to the next item
            if (Input.GetKeyDown(KeyCode.I))
            {
                currentIndex = (currentIndex + 1) % info.inventory.Count;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (info.health <= 0)
        {
            LoseGame();
        }
    }

   void FindAllMenus()
    {
        if (healthText == null)
        {
            healthText = GameObject.Find("HealthText").GetComponent<Text>();
        }
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }
        if (winMenu == null)
        {
            winMenu = GameObject.Find("WinGameMenu");
            winMenu.SetActive(false);
        }
        if (loseMenu == null)
        {
            loseMenu = GameObject.Find("LoseGameMenu");
            loseMenu.SetActive(false);
        }
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseGameMenu");
            pauseMenu.SetActive(false);
        }
    }

    public void WinGame()
    {
        Time.timeScale = 0.0f;
        winMenu.SetActive(true);
    }

    public void LoseGame()
    {
        Time.timeScale = 0.0f;
        loseMenu.SetActive(true);
    }

    public void PauseGame()
    {
        if (isGamePaused)
        {
            // Unpause game
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
            isGamePaused = false;
        }
        else
        {
            // Pause game
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
            isGamePaused = true;
        }
    }

    public void ChangeHealth(int value)
    {
        info.health += value;
    }

    public void ChangeScore(int value)
    {
        info.score += value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Colectable item = collision.GetComponent<Colectable>();

        if (item != null)
        {
            item.player = this.gameObject;
            item.transform.parent = null;
            info.inventory.Add(item);
            item.gameObject.SetActive(false);
        }
    }
}