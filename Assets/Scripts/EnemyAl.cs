using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAl : MonoBehaviour {

    private bool enemyPassive = false;
    private bool enemyPositive = false;
    private bool enemyDefensive = false;
    private int enemyHealth = 100;
    public bool playerPosx = false;
    public bool playerPosy = false;
    private bool isblocked = false;
    private bool ishit = false;
    private RaycastHit hit;
    private Transform playerTran;
    private NavMeshAgent m_agent;
    private GameObject[] barrier;
    private GameObject targetBarrier;
    public float speed=5f;
    void Start () {
        barrier = GameObject.FindGameObjectsWithTag("barrier");
        playerTran = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_agent = this.GetComponent<NavMeshAgent>();
        FindNearest();
	}
    void FindNearest()
    {
        float minDis=Vector3.Distance(this.transform.position,barrier[0].GetComponent<Transform>().position);
        foreach(GameObject x in barrier)
        {
            if (Vector3.Distance(this.transform.position, x.GetComponent<Transform>().position) < minDis)
            {
                minDis = Vector3.Distance(this.transform.position, x.GetComponent<Transform>().position);
                targetBarrier = x;
            }
        }
    }
    void backWard()
    {
        if (this.transform.position == targetBarrier.transform.position+new Vector3(0.5f,0.5f,0.5f)) return;
        m_agent.SetDestination(targetBarrier.GetComponent<Transform>().position+new Vector3(0.5f, 0.5f, 0.5f));
        m_agent.Move(this.transform.TransformDirection(new Vector3(0, 0, speed * Time.deltaTime)));
    }
    void shooting()
    {

    }
	void Update () {
        ishit = Physics.Raycast(this.transform.position, (playerTran.position-this.transform.position), out hit);
        if (ishit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                isblocked = true;
            }else isblocked = false;
        }
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
            if (this.transform.position == targetBarrier.transform.position + new Vector3(0.5f, 0.5f, 0.5f)) return;
            backWard();
        }
	}
}
