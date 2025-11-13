using TMPro;
using UnityEditor;
using UnityEngine;

public class Counters : MonoBehaviour
{
    public Inventory inventory;

    [SerializeField] int diamonds;
    [SerializeField] int count;

    [SerializeField] GameObject diamondCounter;
    [SerializeField] GameObject BombCounter;
    [SerializeField] GameObject KeyCounter;

    public void addDiamonds()
    {
        diamonds++;
        return;
    }
   
    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    void Update()
    {
        diamondCounter.GetComponent<TextMeshProUGUI>().text = "X" + diamonds;
        BombCounter.GetComponent<TextMeshProUGUI>().text = "x" + inventory.bombs;
        KeyCounter.GetComponent<TextMeshProUGUI>().text = "X" + inventory.keys;
    }
}
