using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour {
    private Vector3 pointStart;
    private Vector3 pointMid;
    private Vector3 pointEnd;
    private Vector3 pointSM;
    private Vector3 pointME;
    private Vector3 pointSME;
    [SerializeField] int X;
    [SerializeField] int Y;
    [SerializeField] int Height;
    private float interpolateAmount;
    private void Start() {
        pointStart = gameObject.transform.position;

        pointEnd = new Vector3(X + .5f, pointStart.y, Y + .5f);
        Vector3 x = pointStart + ((pointEnd - pointStart) / 2);
        x.y += Height;
        pointMid = x;
    }
    private void FixedUpdate() {
        interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        pointSM = Vector3.Lerp(pointStart, pointMid, interpolateAmount);
        pointME = Vector3.Lerp(pointMid, pointEnd, interpolateAmount);
        gameObject.transform.position = Vector3.Lerp(pointSM, pointME, interpolateAmount);
    }
}