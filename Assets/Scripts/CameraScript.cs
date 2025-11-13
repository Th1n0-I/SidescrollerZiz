using UnityEngine;

public class CamerScript : MonoBehaviour
{

    [SerializeField] Transform playerPosition;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 5;
    [SerializeField] float lookXValue;

    PlayerMovement s_playerMovement;

    void Start()
    {
        offset = transform.position - playerPosition.position;
        s_playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (s_playerMovement.isFacingRight)
        {
            lookXValue = 3;
        }
        else
        {
            lookXValue = -3;
        }

            transform.position = Vector3.Lerp(transform.position, playerPosition.position + offset + new Vector3(lookXValue, 0, 0), smoothSpeed * Time.deltaTime);
    }
}
