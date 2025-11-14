using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Counters counters;

    private void Start() {
        counters = FindAnyObjectByType<Counters>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            counters.AddDiamonds();
            Destroy(gameObject);
            FindAnyObjectByType<AudioManager>().PlaySound(3);
        }
    }
}
