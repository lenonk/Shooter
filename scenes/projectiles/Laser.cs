using Godot;
using System;

public partial class Laser : Area2D {
	[Export] public int Speed = 1000;
	[Export] public int DamagePerHit = 5;
	
	public Vector2 direction = Vector2.Up;

	public override void _Process(double delta) {
		Position += direction * Speed * (float)delta;
	}

	private void _on_body_entered(Node2D body) {
		if (body is PhysicsBody2D target) {
			if (target.HasMethod("Hit"))
				target.Call("Hit", DamagePerHit);
		}
		QueueFree();
	}

	private void OnTimeout() => QueueFree();
}
