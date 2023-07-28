using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	protected Player _target = null;
	private const float MaxSpeed = 700.0f;
	private const float Acceleration = 1400.0f;
	private const float DesiredRange = 750;
	private const float RangeThreshold = 100;
	private const float TurnSpeed = 15;
	
	private double _curSpeed = 0.0f;
	
	protected int _health = 10;

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null) return;

		var dir = (_target.Position - Position).Normalized();

		// Turn toward the player
		Rotation = (float)Mathf.MoveToward(Rotation, dir.Angle(), TurnSpeed * delta);
		
		// Stop moving if too close
		if (Math.Abs(Position.DistanceTo(_target.Position) - DesiredRange) < RangeThreshold) {
			_curSpeed = Mathf.MoveToward(_curSpeed, 0, Acceleration * delta);
		} 
		else if (Position.DistanceTo(_target.Position) < DesiredRange) {
			_curSpeed = Mathf.MoveToward(_curSpeed, -MaxSpeed, Acceleration * delta);
		}
		else {
			_curSpeed = Mathf.MoveToward(_curSpeed, MaxSpeed, Acceleration * delta);
		}
		
		Vector2 velocity = Velocity;
		velocity.X = dir.X * (float)_curSpeed;
		velocity.Y = dir.Y * (float)_curSpeed;
		Velocity = velocity;
		MoveAndSlide();
	}

	public void Hit(int damage) {
		var ap = GetNode<AnimationPlayer>("AnimationPlayer");
		if (!ap.IsPlaying())
			ap.Play("hit_animation");
		
		_health -= damage;
		if (_health <= 0)
			QueueFree();
	}
}
