using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] GameObject heart1;
    [SerializeField] GameObject heart2;
    [SerializeField] GameObject heart3;

    public float health = 3f;
	
    void Update() {
        heart3.GetComponent<Image>().fillAmount = health - 2;
        heart2.GetComponent<Image>().fillAmount = health - 1;
        heart1.GetComponent<Image>().fillAmount = health;
    }
}
