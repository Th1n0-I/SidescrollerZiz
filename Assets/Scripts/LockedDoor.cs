using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Inventory>().keys > 0)
            {
                collision.gameObject.GetComponent<Inventory>().keys--;
                Destroy(gameObject);
            }
        }
    }
}
