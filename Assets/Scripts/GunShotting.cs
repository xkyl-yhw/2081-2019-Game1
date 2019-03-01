using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShotting : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    private RaycastHit lasthit;
    public GameObject bullet;
    private float interFire = 0.25f;
    private float nextFire = 0f;
	void Start () {
		
	}
	void Update () {
        thower();
	}
    void thower()
    {
        if (Input.GetMouseButton(0)&&Time.time>nextFire)
        {
            nextFire = Time.time + interFire;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);//ViewportToWorldPoint
            if(Physics.Raycast(ray,out hit))
            {
                Vector3 dir;
                if (hit.collider.gameObject.CompareTag("bullet"))
                {
                    dir = lasthit.point - this.transform.position;
                }
                else
                {
                    dir = hit.point - this.transform.position;
                    lasthit = hit;
                }
                GameObject go = GameObject.Instantiate(bullet,this.transform.position,Quaternion.identity);
                go.GetComponent<Rigidbody>().AddForce(dir * 200f);
            }
            
        }
    }
}
