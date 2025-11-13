using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D bombRb;
    [SerializeField] float throwForce;
    [SerializeField] float bombTimer;

    [SerializeField] GameObject explosion;
    void Start()
    {
        bombRb = GetComponent<Rigidbody2D>();
        bombRb.AddForce((transform.right + transform.up) * throwForce);

        StartCoroutine(BombTimer());
    }

    IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(bombTimer);
        explosion.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        bombRb.bodyType = RigidbodyType2D.Static;

        FindAnyObjectByType<AudioManager>().PlaySound(0);

        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
