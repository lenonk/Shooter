using Godot;
using System;

public partial class Drone : Enemy
{
	public override void _Ready() {
		MaxSpeed = 1200.0f;
		Acceleration = 2000.0f;
		DesiredRange = 100;
		RangeThreshold = 20;
		TurnSpeed = 25;
	}

	public override void _Process(double delta) {
		base._Process(delta);	
		if (_target == null) return;
	}

	private void OnAttackAreaEntered(Node2D body) => _target = body as Player;
}
