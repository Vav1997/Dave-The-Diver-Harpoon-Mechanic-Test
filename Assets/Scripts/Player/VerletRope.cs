using System;
using System.Collections.Generic;
using UnityEngine;

public class VerletRope : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.1f;
    private int segmentLength = 20;
    private float lineWidth = 0.12f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint ;

    public void SetStartEndPoints(Transform startPoint, Transform endPoint) {
        this.startPoint = startPoint;
        this.endPoint = endPoint;

        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for(int i = 0; i < segmentLength; i++) {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            //ropeStartPoint.y -= ropeSegLen;
            //ropeStartPoint.x += ropeSegLen;
        }
    }

    public void DestroyRope() {
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update() {
        this.DrawRope();
    }

    private void FixedUpdate() {
        this.Simulate();
    }

    private void Simulate() {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -4.5f);

        for(int i = 1; i < this.segmentLength; i++) {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for(int i = 0; i < 50; i++) {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint() {
        //Constrant to Mouse
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.ropeSegments[0] = firstSegment;

        firstSegment.posNow = startPoint.position;
        ropeSegments[0] = firstSegment;

        RopeSegment endSegment = ropeSegments[segmentLength - 1];
        endSegment.posNow = endPoint.position;
        ropeSegments[segmentLength - 1] = endSegment;

        for(int i = 0; i < this.segmentLength - 1; i++) {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if(dist > ropeSegLen) {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if(dist < ropeSegLen) {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if(i != 0) {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope() {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for(int i = 0; i < this.segmentLength; i++) {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public struct RopeSegment {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos) {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
