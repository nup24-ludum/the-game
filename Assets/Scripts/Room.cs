using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour {
    public virtual bool WatcherCheck() {
        return false;
    }
}
