using Godot;
using System;
using Range = System.Range;

public partial class Box : ItemContainer {
	public override void Hit(int unused) {
		if (_opened) return;
		base.Hit(unused);
		foreach (var i in GD.Range(0, 5))
			SpawnItem();
	}
}
