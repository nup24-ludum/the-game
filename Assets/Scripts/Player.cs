using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Room {
    public enum State {
        ACTIVE,
        HIDDEN,
        DEAD,
    }

    [SerializeField] private Transform character;
    [SerializeField] private State state = State.ACTIVE;

    private void OnGUI() {
       Handles.Label(transform.position, "State: " + state);
    }

    public override bool WatcherCheck() {
        if (state == State.ACTIVE) {
            state = State.DEAD;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case State.ACTIVE:
                if (Input.GetKey(KeyCode.Q)) {
                    state = State.HIDDEN;
                }
                character.localPosition = Vector3.zero;
                break;
            case State.HIDDEN:
                if (!Input.GetKey(KeyCode.Q)) {
                    state = State.ACTIVE;
                }
                character.localPosition = new Vector3(-0.4f, 0, 0);
                break;
            case State.DEAD:
                character.localScale = Vector3.zero;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
