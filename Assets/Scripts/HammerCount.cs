using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerCount : MonoBehaviour
{
    public int startingHammer = 20;
    private int counter;
    private Text hammerText;
    private GameObject player;
    public Character_Movement chamo;

    // Start is called before the first frame update
    void Start()
    {
        hammerText = GetComponent<Text>();
        counter = chamo.HammerQty;
        player = GameManager.instance.Player;
        chamo = player.GetComponent<Character_Movement>();


    }

    // Update is called once per frame
    void Update()
    {
        counter = Ammo(chamo.HammerQty);
        hammerText.text = "X" + counter;
    }

    public int Ammo(int ammo)
    {
        return counter = ammo;
    }
}
