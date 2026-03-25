using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private bool moveBack = false;
    private bool moveBackStatue = false;

    public GameObject statue;
    private Vector3 originalPosition;
    private Vector3 statueOriginalPosition;

    private Transform deathChild;

   // [SerializeField] private AfterStatue afterStatue;

    private void Start()
    {
        originalPosition = transform.position;
        statueOriginalPosition = statue.transform.position;

        deathChild = statue.transform.Find("DEATH");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Player2"))
        {
            collider.transform.parent = transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Player2"))
        {
            if (transform.position.y >= originalPosition.y - 0.5f)
            {
                transform.Translate(0, -1f * Time.deltaTime, 0);
            }
            if (statue.transform.position.y <= statueOriginalPosition.y + 3)
            {
                statue.transform.Translate(0, 1f * Time.deltaTime, 0);
            }
            moveBack = false;
            moveBackStatue = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Player2"))
        {
            moveBack = true;
            moveBackStatue = true;
            collider.transform.parent = null;
        }
    }


    private void Update()
    {
        deathChild.gameObject.SetActive(false);

        if (moveBack)
        {
            if (transform.position.y < originalPosition.y) 
            {
                transform.Translate(0, 1f * Time.deltaTime, 0);
            }
            else 
            {
                moveBack = false;
            }
        }
        if (moveBackStatue)
        {
            deathChild.gameObject.SetActive(true);
            if (statue.transform.position.y > statueOriginalPosition.y)
            {
                statue.transform.Translate(0, -6f * Time.deltaTime, 0);
            }
            else
            {
                moveBackStatue = false;
            }
        }
    }
}
