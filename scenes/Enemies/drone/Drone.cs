using Godot;
using System;

public partial class Drone : CharacterBody2D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		var direction = Vector2.Right;
		Velocity = direction * 100;
		MoveAndSlide();
	}

	public void Hit() {
		GD.Print("Drone damaged");
	}
}
