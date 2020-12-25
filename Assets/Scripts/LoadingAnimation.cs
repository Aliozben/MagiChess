using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour {
    [SerializeField] private Sprite[] spriteSheet;
    private int currentFrame = 0;
    private float timer;
    void Update() {
        timer += Time.deltaTime;
        if (timer >= .05f) {
            timer -= .05f;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteSheet[currentFrame];
            currentFrame = currentFrame >= spriteSheet.Length - 1 ? 0 : currentFrame + 1;
        }
    }
}
