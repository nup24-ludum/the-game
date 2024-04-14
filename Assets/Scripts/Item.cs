using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Mobj {

    public Inventory.ItemTy ty { get; } = Inventory.ItemTy.COOKIE;

    public override Team team() => Team.ITEM;
    public override void DoTurn() { }
    public override bool TurnReady() => true;
}
