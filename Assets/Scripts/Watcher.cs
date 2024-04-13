using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Watcher : MonoBehaviour {
    public enum State {
        GOTOCENTER,
        GOTOROOM,
        DONE,
        STANDING,
    }

    private const float targetRadius = 0.1f;
    private Dorm dorm;
    private Room targetRoom;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float countDown = 0f;
    [SerializeField] private float peekChance = 0.4f;
    [SerializeField] private float walkSpeed = 0.2f;
    [SerializeField] private State state = State.GOTOCENTER;

    private void Awake() {
        dorm = FindObjectOfType<Dorm>();
        transform.position = GetTargetPos();
    }

    private Vector3 GetTargetPos() {
        return state switch {
            State.GOTOCENTER => Vector3.zero,
            State.GOTOROOM or State.DONE => targetRoom.transform.position,
            State.STANDING => Vector3.zero,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    // Update is called once per frame
    private void Update() {
        if (state == State.DONE) {
            return;
        }

        Vector3 targetPos = GetTargetPos();
        targetPos.z = -1f;
        switch (state) {
            case State.GOTOCENTER:
                if (MathF.Abs(targetPos.y - transform.position.y) > targetRadius) {
                    Vector3 tagetDir = Vector3.Project(
                            targetPos - transform.position,
                            Vector3.up
                            ).normalized;
                    transform.position += tagetDir * (walkSpeed * Time.deltaTime);
                } else {
                    transform.position = new Vector3(transform.position.x, 0, -1);
                    targetRoom = dorm.SelectRoom();
                    state = State.GOTOROOM;
                }
                break;
            case State.GOTOROOM:
                if (MathF.Abs(targetPos.x - transform.position.x) > targetRadius) {
                    Vector3 tagetDir = Vector3.Project(
                            targetPos - transform.position,
                            Vector3.right
                            ).normalized;
                    transform.position += tagetDir * (walkSpeed * Time.deltaTime);
                    if (MathF.Abs(targetPos.x - transform.position.x) <= targetRadius &&
                        Random.Range(0, 1f) <= peekChance) {
                        transform.position = new Vector3(targetRoom.transform.position.x, 0, -1);
                        state = State.STANDING;
                        countDown = waitTime;
                    }
                } else if (MathF.Abs(targetPos.y - transform.position.y) > targetRadius) {
                    Vector3 tagetDir = Vector3.Project(
                            targetPos - transform.position,
                            Vector3.up
                            ).normalized;
                    transform.position += tagetDir * (walkSpeed * Time.deltaTime);
                } else {
                    transform.position = targetRoom.transform.position + new Vector3(0, 0, -1);
                    state = targetRoom.WatcherCheck() ? State.DONE : State.GOTOCENTER;
                }
                break;
            case State.STANDING:
                countDown -= Time.deltaTime;
                if (countDown <= 0f) {
                    targetRoom = dorm.SelectRoom();
                    state = State.GOTOROOM;
                }
                break;
            case State.DONE:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
