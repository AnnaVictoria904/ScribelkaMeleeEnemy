using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : Enemy
{
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    private bool movingRight = true;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSaw();
    }
    void MoveSaw()
    {
        float movement = movingRight ? speed * Time.deltaTime : -speed * Time.deltaTime;
        float currentDistance = movingRight ? (transform.position.x - startPosition.x) : (startPosition.x - transform.position.x);
        transform.Translate(new Vector3(movement, 0, 0));

        // Agrega aqu� la l�gica para cambiar la direcci�n cuando alcanza la distancia
        if (currentDistance >= distance)
        {
            movingRight = !movingRight;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //Destroy(gameObject);
    }
}
