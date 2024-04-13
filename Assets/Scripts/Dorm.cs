using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class Dorm : MonoBehaviour {
    [SerializeField] private Room[] rooms;

    private void Awake() {
        rooms = new Room[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            Room room = transform.GetChild(i).gameObject.GetComponent<Room>();
            Assert.IsNotNull(room);
            rooms[i] = room;
        }
    }

    public Room SelectRoom(int last) {
        int idx;
        do {
            idx = Random.Range(0, rooms.Length);
        } while (idx == last);

        return rooms[idx];
    }

    public Room SelectRoom() {
        return SelectRoom(-1);
    }
}
