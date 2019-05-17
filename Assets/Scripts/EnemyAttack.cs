using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float range = 3f;
    [SerializeField] private float timeBetweenAttacks = 1f;

    private Animator anim;
    private GameObject player;
    private bool playerInRange;
    private BoxCollider weaponCollider;
    private Enemy01Health enemy01Health;
    // Start is called before the first frame update
    void Start()
    {
        enemy01Health = GetComponent <Enemy01Health>();
       anim = GetComponent<Animator>();
       player = GameManager.instance.Player;
       weaponCollider = GetComponentInChildren<BoxCollider>();
       StartCoroutine (attack());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance (transform.position, player.transform.position)<range && enemy01Health.IsAlive){
            playerInRange = true;
        }else{
            playerInRange = false;
        }
    }

    IEnumerator attack()
    {
        if (playerInRange && !GameManager.instance.GameOver)
        {
            anim.Play ("Attacking");
            yield return new WaitForSeconds (timeBetweenAttacks);
        }

        yield return null;
        StartCoroutine (attack ());
    }
}
