using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAl : MonoBehaviour {

    private bool enemyPassive = false;
    private bool enemyPositive = false;
    private bool enemyDefensive = false;
    private int enemyHealth = 100;
    public bool playerPosx = false;
    public bool playerPosy = false;
    private bool isblocked = false;
    private RaycastHit hit;
    private Transform playerTran;
	void Start () {
        playerTran = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
    void backWard()
    {
        this.transform.Translate((this.transform.position - playerTran.position) * Time.deltaTime);
    }
    void shooting()
    {

    }
	void Update () {
        Debug.DrawLine(transform.position, playerTran.position, Color.red, 1f);
        isblocked =Physics.Linecast(this.transform.position, playerTran.position, 1 << LayerMask.NameToLayer("Player"));
        if (!isblocked)
        {
            isblocked = false;
        }
        else isblocked = true;
        Debug.Log(isblocked);
        if (Vector3.Distance(this.transform.position, playerTran.position) < 5f)
        {
            playerPosx = true;
        }
        else playerPosx = false;
        if (Vector3.Distance(this.transform.position, playerTran.position) < 4f)
        {
            playerPosy = true;
        }
        else playerPosy = false;
        if (enemyHealth == 100 && !playerPosx && !playerPosy)
        {
            enemyPassive = true;
            enemyPassive = false;
            enemyDefensive = false;
        }
        if ((playerPosx == true||playerPosy == true) && isblocked == false)
        {
            enemyPositive = true;
            enemyPassive = false;
            enemyDefensive = false;
            shooting();
        }
        if (playerPosy == true||enemyHealth<=20)
        {
            enemyDefensive = true;
            enemyPassive = false;
            enemyPositive = false;
            backWard();
        }
	}
}
