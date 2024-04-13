using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Walker : MonoBehaviour {
    public enum Location {
        CORRIDOR,
        ROOM,
        INTOROOM,
    }

    [System.Serializable]
    public struct WalkTarget {
        public bool pending;
        public Location location;
        public Room room;
    }

    public float sightLength;
    public float walkSpeed;
    [SerializeField] public Vector3 direction;
    [SerializeField] private WalkTarget walkTarget;
    [SerializeField] public Room room;
    [SerializeField] public Location location;

    private void Awake() {
        walkTarget.pending = false;
    }

    public WalkTarget GetWalkTarget() => walkTarget;

    public virtual void OnDoneWalking() { }

    public virtual void OnWalkReject() { }

    public virtual void OnSight(Walker other) { }

    public void StopWalk() {
        walkTarget.pending = false;
    }

    public void WalkIntoRoom() {
        walkTarget.pending = true;
        walkTarget.location = Location.INTOROOM;
    }

    public void WalkToRoom(Room toRoom) {
        room = null;
        walkTarget.pending = true;
        walkTarget.location = Location.ROOM;
        walkTarget.room = toRoom;
    }

    public void WalkToCorridor() {
        room = null;
        walkTarget.pending = true;
        walkTarget.location = Location.CORRIDOR;
    }

    protected virtual void Update() {
        Debug.DrawLine(transform.position,
            transform.position + direction * sightLength,
            Color.magenta);
    }
}
