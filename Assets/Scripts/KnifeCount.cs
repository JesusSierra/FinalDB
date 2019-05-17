using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeCount : MonoBehaviour
{
    public int startingKnife = 20;
    private int counter;
    private Text knifeText;
    private GameObject player;
    public Character_Movement chamo;

    // Start is called before the first frame update
    void Start()
    {
        knifeText = GetComponent<Text>();
        counter = chamo.KnifeQty;
        player = GameManager.instance.Player;
        chamo = player.GetComponent<Character_Movement>();


    }

    // Update is called once per frame
    void Update()
    {
        counter = Ammo(chamo.KnifeQty);
        knifeText.text = "X" + counter;
    }

    public int Ammo(int ammo)
    {
        return counter = ammo;
    }
}
