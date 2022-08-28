using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        pos.x -= moveSpeed * Time.deltaTime;
        this.transform.localPosition = pos;
    }
}
