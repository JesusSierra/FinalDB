using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{

    [SerializeField] Transform player;
    private NavMeshAgent nav;
    private Animator anim;

    private Enemy01Health enemy01Health;

    void Awake()
    {
        Assert.IsNotNull(player);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator> ();
        nav = GetComponent<NavMeshAgent>();
        enemy01Health = GetComponent <Enemy01Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, this.transform.position)<6)
        {
            if(!GameManager.instance.GameOver && enemy01Health.IsAlive)
            {
            nav.SetDestination(player.position);
            anim.SetBool("isWalking", true);
            anim.SetBool ("isIdle", false);
            }
        } else if (GameManager.instance.GameOver || !enemy01Health.IsAlive)
        {
            anim.SetBool ("isWalking", false);
            anim.SetBool ("isIdle", true);
            nav.enabled = false;
        }
    }
}
