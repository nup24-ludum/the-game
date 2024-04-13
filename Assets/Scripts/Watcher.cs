using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Watcher : Walker {
    private Dorm dorm;
    [SerializeField] private bool standing = false;
    [SerializeField] private float standTime = 2f;
    [SerializeField] private float countdown = 0f;
    [SerializeField] private float roomCheckProb = 0.4f;

    private void Awake() {
        dorm = FindObjectOfType<Dorm>();
        transform.position = Vector3.zero;
    }

    private void Start() {
        WalkToRoom(dorm.SelectRoom());
    }

    public override void OnSight(Walker other) {
        if (other.gameObject.TryGetComponent(out Player pl) && !pl.isHidden()) {
            pl.Die();
            Debug.Log("Got you");
        }
    }

    public override void OnWalkReject() { }

    public override void OnDoneWalking() {
        switch (location) {
            case Location.CORRIDOR:
                WalkToRoom(dorm.SelectRoom());
                break;
            case Location.ROOM:
                if (Random.Range(0f, 1f) <= roomCheckProb) {
                    WalkIntoRoom();
                } else {
                    standing = true;
                    countdown = standTime;
                }
                break;
            case Location.INTOROOM:
                if (room.WatcherCheck()) {
                    room.player.Die();
                    Debug.Log("Got you");
                }
                WalkToCorridor();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void Update() {
        base.Update();

        standing = standing && location == Location.ROOM;

        if (countdown <= 0f && standing) {
            standing = false;
            WalkToRoom(dorm.SelectRoom());
            return;
        }

        if (standing) {
            countdown -= Time.deltaTime;
        }
    }
}
