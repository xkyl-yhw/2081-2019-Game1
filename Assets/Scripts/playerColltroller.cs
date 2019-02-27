using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerColltroller : MonoBehaviour {

    public GameObject bullet;
    private Transform playerTran;
	void Start () {
        playerTran = this.GetComponent<Transform>();
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            
        }
        if (Input.GetKey(KeyCode.W))
        {
            playerTran.Translate(Vector3.forward * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerTran.Translate(-Vector3.forward * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerTran.Translate(Vector3.left * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerTran.Translate(-Vector3.left * Time.deltaTime);
        }
    }
}
