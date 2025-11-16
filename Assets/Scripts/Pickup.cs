using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public                   Counters   counters;
    [SerializeField] private GameObject particles;

    private void Start() {
        counters = FindAnyObjectByType<Counters>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            counters.AddDiamonds();
            GameObject.Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
            FindAnyObjectByType<AudioManager>().PlaySound(3);
        }
    }
}
