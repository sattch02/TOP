using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private float speed = 0.01f;
    private Vector2 position;
    private Vector2 scale;

    // Start is called before the first frame update
    void Start()
    {
        this.anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.scale = transform.localScale;
        this.position = transform.position;
        float inputAxis = Mathf.Abs(Input.GetAxis("Horizontal"));

        anim.SetFloat("Speed", inputAxis);

        if (Input.GetKey("right"))
        {
            this.position.x += speed;
        }

        if (Input.GetKey("left"))
        {
            this.position.x -= speed;
        }

        // 向き
        if (Input.GetAxis("Horizontal") >= 0)
        {
            this.scale.x = 1;
        }
        else
        {
            this.scale.x = -1;
        }

        transform.position = this.position;
        transform.localScale = this.scale;
    }
}
