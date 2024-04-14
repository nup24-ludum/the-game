using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameRules : MonoBehaviour {
    private Mobj player;
    [SerializeField] private List<Mobj> monsters;
    [SerializeField] private Mobj.Team currTurn;

    private void Awake() {
        for (int i = 0; i < transform.childCount; ++i) {
            Mobj child = transform.GetChild(i).GetComponent<Mobj>();
            switch (child.team()) {
                case Mobj.Team.PLAYER:
                    player = child;
                    break;
                case Mobj.Team.MONSTER:
                    monsters.Add(child);
                    break;
                case Mobj.Team.ITEM:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        switch (currTurn) {
            case Mobj.Team.PLAYER:
                if (!player.TurnReady()) { return; }
                player.DoTurn();
                currTurn = Mobj.Team.MONSTER;
                break;
            case Mobj.Team.MONSTER:
                if (!monsters.All(x => x.TurnReady())) { return; }
                monsters.ForEach(x => x.DoTurn());
                currTurn = Mobj.Team.PLAYER;
                break;
            case Mobj.Team.ITEM:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
