using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{

    public int startingLives = 3;
    private int lifeCounter;
    private Text lifeText;
    private GameObject player;
    public GameObject gameOverScreen;
    public float waitAfterGameOver;

    // Start is called before the first frame update
    void Start()
    {
        lifeText = GetComponent <Text>();
        lifeCounter = 3;
        player = GameManager.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        lifeText.text = "X" + lifeCounter;

        if (lifeCounter <= 0)
        {
            gameOverScreen.SetActive (true);
            player.gameObject.SetActive(false);
        }

        if (gameOverScreen.activeSelf)
        {
            waitAfterGameOver -= Time.deltaTime;
        }
        if (waitAfterGameOver < 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GiveLife()
    {
        lifeCounter ++;
    }

    public void TakeLife()
    {
        lifeCounter --;
    }

}
