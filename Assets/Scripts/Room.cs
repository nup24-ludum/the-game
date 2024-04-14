using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour {
    public RectInt zone;
    public Player player = null;

    public bool WatcherCheck() {
        return player && player.isHidden();
    }
}
