using System;
using System.Collections;
using System.Globalization;
using TMPro;
using TMPro.Examples;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour {
	[SerializeField] private GameObject  parent;
	[SerializeField] private GameObject  countdown;
	private                  TMP_Text    countdownTMPText;
    private                  Rigidbody2D bombRb;
    [SerializeField]         float       throwForce;
    [SerializeField]         float       bombTimer;
    
    private float countdownTimer = 3.1f;
    
    CinemachineImpulseSource impulseSource;

    [SerializeField] GameObject explosion;
    void Start() {
	    impulseSource = GetComponent<CinemachineImpulseSource>();
        bombRb        = GetComponent<Rigidbody2D>();
        countdownTMPText     = countdown.gameObject.GetComponent<TMP_Text>();
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
		
        impulseSource.GenerateImpulse();
        Destroy(parent);
    }

    // Update is called once per frame
    void Update()
    {
        countdownTimer -= Time.deltaTime;
        countdownTMPText.text =  Math.Ceiling(countdownTimer).ToString();
    }
}
