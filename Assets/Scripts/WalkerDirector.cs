using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WalkerDirector : MonoBehaviour {
    private const float targetRange = 0.001f;
    [SerializeField] private Walker[] walkers;

    private void Awake() {
        walkers = new Walker[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            Walker walker = transform.GetChild(i).GetComponent<Walker>();
            walkers[i] = walker;
            Assert.IsNotNull(walker);
        }
    }

    private Vector3 GetTargetPos(Vector3 walkerPos, Walker.Location loc, Room room) {
        Vector3 roomPos = room ? room.transform.position : Vector3.zero;
        return loc switch {
            Walker.Location.CORRIDOR => new Vector3(walkerPos.x, 0, walkerPos.z),
            Walker.Location.ROOM => new Vector3(roomPos.x, 0, roomPos.z),
            Walker.Location.INTOROOM => room.transform.position,
            _ => throw new ArgumentOutOfRangeException(nameof(loc), loc, null)
        };
    }

    private bool VerifyWalkerTarget(Walker.Location loc, Walker.Location targetLoc) {
        return targetLoc switch {
            Walker.Location.ROOM => loc is Walker.Location.CORRIDOR or Walker.Location.ROOM,
            Walker.Location.INTOROOM => loc == Walker.Location.ROOM,
            Walker.Location.CORRIDOR => loc == Walker.Location.INTOROOM,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void TickWalker(Walker walker) {
        if (!walker.GetWalkTarget().pending) {
            return;
        }

        // Debug.Log("Walk: " +
        //           walker.name + " " +
        //           walker.location + " -> " +
        //           walker.GetWalkTarget().location
        //           );

        // Verify the target and reset if it's not good
        if (!VerifyWalkerTarget(walker.location, walker.GetWalkTarget().location)) {
            // Debug.Log("Rejected");
            walker.StopWalk();
            walker.OnWalkReject();
            return;
        }

        // Debug.Log("Process");

        // Walk target is correct
        Vector3 walkerPos = walker.transform.position;
        Vector3 targetPos = GetTargetPos(
            walkerPos,
            walker.GetWalkTarget().location,
            walker.GetWalkTarget().room
        );

        float delta = walker.walkSpeed * Time.deltaTime;
        if (Vector3.Distance(walkerPos, targetPos) > delta) {
            Vector3 direction = (targetPos - walkerPos).normalized;
            walker.direction = direction;
            walker.transform.position += direction * delta;
            return;
        }

        walker.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
        walker.location = walker.GetWalkTarget().location;
        walker.StopWalk();
        walker.OnDoneWalking();

        walker.room = walker.GetWalkTarget().location switch {
            Walker.Location.CORRIDOR => null,
            Walker.Location.ROOM => null,
            Walker.Location.INTOROOM => walker.GetWalkTarget().room,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    // Update is called once per frame
    private void Update() {
        foreach (Walker walker in walkers) {
            TickWalker(walker);
        }

        foreach (Walker walker1 in walkers) {
            foreach (Walker walker2 in walkers) {
                if (walker1 == walker2) {
                    continue;
                }

                // walker1 --> walker2
                Vector3 between = walker2.transform.position - walker1.transform.position;
                Vector3 sight = Vector3.Project(between, walker1.direction);

                if (Vector3.Dot(between.normalized, walker1.direction) <= 0.89f) {
                    continue;
                }

                Debug.DrawLine(walker1.transform.position,
                    walker1.transform.position + 3*sight.normalized, Color.blue);
                float dist = Vector3.Dot(sight, walker1.direction);

                if (dist >= targetRange && dist <= walker1.sightLength) {
                    walker1.OnSight(walker2);
                }
            }
        }
    }
}
