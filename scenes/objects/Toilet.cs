using Godot;
using System;

public partial class Toilet : ItemContainer {
	public override void Hit(int unused) {
		if (_opened) return;
		base.Hit(unused);
		SpawnItem();
	}
}
