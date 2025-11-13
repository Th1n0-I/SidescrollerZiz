using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Counters Counters;

    private void Start()
    {
        Counters = FindAnyObjectByType<Counters>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Counters.addDiamonds();
            Destroy(gameObject);
            FindAnyObjectByType<AudioManager>().PlaySound(3);
        }
    }
}
