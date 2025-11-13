using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    public int keys;
    public int bombs;

    InputAction attackAction;
    [SerializeField] GameObject bomb;

    private void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        if (attackAction.WasPerformedThisFrame() && bombs > 0)
        {
            Instantiate(bomb, transform.position, transform.rotation);
            bombs--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            keys++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Bomb"))
        {
            bombs++;
            Destroy(collision.gameObject);
        }
    }
}
