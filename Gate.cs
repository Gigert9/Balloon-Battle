using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool didScore = false;
    public bool didSpawn = false;
    public bool isBottom;

    GameObject gate;

    // Start is called before the first frame update
    void Start()
    {
        gate = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("player").GetComponent<Player>().isPlaying)
        {
            MoveGate();
        }

        CheckDestroy();
    }

    void MoveGate()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().didSpawnGate)
        {
            Vector2 pos = this.transform.localPosition;
            pos.x -= moveSpeed * Time.deltaTime;
            gate.transform.localPosition = pos;
        }
    }

    void CheckDestroy()
    {
        Vector2 pos = this.transform.localPosition;

        if(pos.x < -14)
        {
            Destroy(gate);
        }

        if (!GameObject.Find("player").GetComponent<Player>().isPlaying)
        {
            Invoke("ForceDestroy", .5f);
        }
    }

    void ForceDestroy()
    {
        Destroy(gate);
    }
}
