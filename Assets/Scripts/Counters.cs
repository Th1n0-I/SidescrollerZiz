using TMPro;
using UnityEditor;
using UnityEngine;

public class Counters : MonoBehaviour
{
    public Inventory inventory;

    [SerializeField] int diamonds;
    [SerializeField] int count;

    [SerializeField] GameObject diamondCounter;
    [SerializeField] GameObject bombCounter;
    [SerializeField] GameObject keyCounter;

    public void AddDiamonds() {
        diamonds++;
    }
   
    void Start() {
        inventory = FindAnyObjectByType<Inventory>();
    }

    void Update() {
        diamondCounter.GetComponent<TextMeshProUGUI>().text = "X" + diamonds;
        bombCounter.GetComponent<TextMeshProUGUI>().text = "x" + inventory.bombs;
    }
}
