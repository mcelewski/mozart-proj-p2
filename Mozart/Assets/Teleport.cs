using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject Doors;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Teleportation());
        }
    }

    IEnumerator Teleportation()
    {
        yield return new WaitForSeconds(2.5f);
        Player.transform.position = new Vector2(Doors.transform.position.x, Doors.transform.position.y);
    }
}
