using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeScript : MonoBehaviour {
    private Vector3 originPosition;
    private Quaternion originRotation;
    public ParticleSystem aura;
    public ParticleSystem Hit;
    public float shake_decay;
    public float shake_intensity;
    int x = 0;
    private float temp_shake_intensity = 0;
    private float delayTimer = .6f;
    void Update() {
        if (temp_shake_intensity > 0) {
            transform.GetChild(0).position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
            transform.GetChild(0).rotation = new Quaternion(
                originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);
            temp_shake_intensity -= shake_decay;
            if (temp_shake_intensity < 0) {
                x++;
            }
        }
        if (x == 1) {
            Debug.Log("sdasd");
            // StartCoroutine("SpawnCharacter");
            SpawnCharacter();
            x++;
        }
        if (x == 2) delayTimer -= Time.deltaTime;
        if (delayTimer <= 0) {
            goToPos();
            x++;
            delayTimer = 1f;
        }
    }

    private void goToPos() {
        Hit.Play();
        gameObject.transform.position = new Vector3(5.5f, 0f, 5.5f);
    }

    private void text() {
        aura.Play();
        originPosition = transform.GetChild(0).position;
        originRotation = transform.GetChild(0).rotation;
        temp_shake_intensity = shake_intensity;
    }
    private void Start() {
        text();
    }
    void SpawnCharacter() {
        aura.Stop();
        // Hit.Play();
        gameObject.transform.position = new Vector3(5.5f, 15f, 5.5f);
        //        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        // gameObject.transform.GetChild(1).gameObject.SetActive(true);
        //  yield return new WaitForSeconds(1f);
        // gameObject.transform.GetChild(0).gameObject.SetActive(true);


    }
}
