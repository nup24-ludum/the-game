using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

public class Player : Walker {
    public enum State {
        DEAD,
        ACTIVE,
        HIDDEN,
        RUNNING,
    }

    private Dorm dorm;
    private SpriteRenderer sprite;
    [SerializeField] private int requestedRoom;
    [SerializeField] private Transform character;
    [SerializeField] private State state = State.ACTIVE;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        dorm = FindObjectOfType<Dorm>();
    }

    public void Die() {
        state = State.DEAD;
    }

    public bool isHidden() => state == State.HIDDEN;

    public override void OnDoneWalking() {
        switch (location) {
            case Location.CORRIDOR:
                WalkToRoom(dorm.GetRoom(requestedRoom - 1));
                break;
            case Location.ROOM:
                WalkIntoRoom();
                break;
            case Location.INTOROOM:
                room.player = this;
                state = State.ACTIVE;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        sprite.color = Color.green;
        switch (state) {
            case State.ACTIVE:
                if (Input.GetKey(KeyCode.Q)) {
                    state = State.HIDDEN;
                }

                if (Input.GetKey(KeyCode.Alpha1)) {
                    state = State.RUNNING;
                    room.player = null;
                    WalkToCorridor();
                    requestedRoom = 1;
                }
                if (Input.GetKey(KeyCode.Alpha2)) {
                    state = State.RUNNING;
                    room.player = null;
                    WalkToCorridor();
                    requestedRoom = 2;
                }
                if (Input.GetKey(KeyCode.Alpha3)) {
                    state = State.RUNNING;
                    room.player = null;
                    WalkToCorridor();
                    requestedRoom = 3;
                }
                if (Input.GetKey(KeyCode.Alpha4)) {
                    state = State.RUNNING;
                    room.player = null;
                    WalkToCorridor();
                    requestedRoom = 4;
                }
                if (Input.GetKey(KeyCode.Alpha5)) {
                    state = State.RUNNING;
                    room.player = null;
                    WalkToCorridor();
                    requestedRoom = 5;
                }
                break;
            case State.HIDDEN:
                sprite.color = Color.gray;
                if (!Input.GetKey(KeyCode.Q)) {
                    state = State.ACTIVE;
                }
                break;
            case State.RUNNING:
                break;
            case State.DEAD:
                sprite.color = Color.black;
                StopWalk();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
