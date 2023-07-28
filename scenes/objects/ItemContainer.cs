using Godot;
using System;

public partial class ItemContainer : StaticBody2D {
	[Signal] public delegate void SpawnStatItemEventHandler(Vector2 pos, Vector2 dir);
	
	protected Vector2 CurrentDirection;
	protected bool _opened = false;
	
	public override void _Ready() {
		CurrentDirection = Vector2.Down.Rotated(Rotation);
	}
	
	public virtual void Hit(int unused) {
		_opened = true;
		GetNode<Sprite2D>("LidSprite").Hide();
	}

	protected void SpawnItem() {
		var rng = new RandomNumberGenerator();
		var positions = GetNode<Node2D>("SpawnPositions").GetChildren();
		
		if (positions.Count <= 0)
			return;
		
		var pos = positions[rng.RandiRange(0, positions.Count - 1)] as Marker2D;
		EmitSignal(SignalName.SpawnStatItem, pos.GlobalPosition, CurrentDirection);
	}
}
