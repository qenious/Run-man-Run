using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class playerMove : MonoBehaviour
{
    [SerializeField] bool cameraBosta = false;
    [SerializeField] GameObject cameraRotate;
    [SerializeField] GameObject cameraBackPos;
    [SerializeField] GameObject oyunBittiText;
    [SerializeField] bool oyunBitti = false;
    Rigidbody rb;
    [SerializeField] float vSpeed;
    [SerializeField] float hSpeed;
    [SerializeField] float speedLock;
    [SerializeField] float brakeSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.x > -7 && !oyunBitti) // Koşma hızı 7f'e sabitlendi
        {
            rb.AddForce(Vector3.left * Time.deltaTime * vSpeed, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Space) && rb.velocity.x < 0 && !oyunBitti) // Koşma hızı -7f'e sabitlendi
        {
            rb.AddForce(Vector3.right * Time.deltaTime * brakeSpeed, ForceMode.Impulse);
        }

        if (oyunBitti)
        {
            Debug.Log("oyun bitti");
            oyunBittiText.SetActive(true);
            transform.LookAt(oyunBittiText.transform.position);
        }

        if (cameraBosta && cameraBackPos.transform.position.x < 25)
        {
            //cameraBackPos.GetComponent<Rigidbody>().AddForce(Vector3.right * Time.deltaTime * 100) ;
            cameraBackPos.transform.Translate(Vector3.back * .5f);
            cameraBackPos.transform.Rotate(Vector3.zero);
        }



    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !oyunBitti)
        {
            transform.Translate(Vector3.left * Time.deltaTime * hSpeed); // Karakterin sol yatay hareketi
        }
        if (Input.GetKey(KeyCode.RightArrow) && !oyunBitti)
        {
            transform.Translate(Vector3.right * Time.deltaTime * hSpeed); // Karakterin sağ yatay hareketi
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("finish"))
        {
            oyunBitti = true;
            cameraRotate.transform.LookAt(oyunBittiText.transform);
            // cameraBackPos.transform.parent = null;
            cameraBackPos.GetComponent<CinemachineBrain>().enabled = false;
            StartCoroutine(cameraSinematik());
        }
    }
    IEnumerator cameraSinematik()
    {
        yield return new WaitForSeconds(4);
        cameraBosta = true;

    }
}
