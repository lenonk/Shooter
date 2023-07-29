using Godot;
using Godot.Collections;
using System;

public partial class Player : CharacterBody2D {
	[Signal] public delegate void PlayerLaserEventHandler(Vector2 pos, Vector2 dir); 
	[Signal] public delegate void PlayerGrenadeEventHandler(Vector2 pos, Vector2 dir, float speed); 
	
	[Export] public float MaxSpeed = 500F;
	
	private const float Acceleration = 1700.0f;
	public double CurSpeed = 0.0f;
	
	private bool _canLaser = true;
	private bool _canGrenade = true;
	private bool _immobilized = false;

	private Timer _laserTimer;
	private Timer _grenadeTimer;

	public override void _Ready() {
		_laserTimer = GetNode<Timer>("LaserTimer");
		_grenadeTimer = GetNode<Timer>("GrenadeTimer");
		CurSpeed = MaxSpeed;
	}

	public override void _Process(double delta) {
		if (_immobilized) return;
		
		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		if (direction != Vector2.Zero)
			CurSpeed = Mathf.MoveToward(CurSpeed, MaxSpeed, Acceleration * delta);
		else
			CurSpeed = Mathf.MoveToward(CurSpeed, 0.0f, Acceleration * delta);
				
		Velocity = direction * (float)CurSpeed;
		MoveAndSlide();

		LookAt(GetGlobalMousePosition());
		
		if (Input.IsActionPressed("primary action") && _canLaser && Globals.Get().LaserAmmo > 0)
			FireLaser();
		else if (Input.IsActionPressed("secondary action") && _canGrenade && Globals.Get().GrenadeAmmo > 0)
			FireGrenade();
	}

	private void FireLaser() {
		_canLaser = false;
		Globals.Get().LaserAmmo -= 1;
		_laserTimer.Start();
		
		var rng = new RandomNumberGenerator();
		GetNode<GpuParticles2D>("LaserParticleEmitter").Emitting = true;
		Array<Node> laserStartPositions = GetNode<Node2D>("LaserStartPosition").GetChildren();
		Marker2D selectedLaser = laserStartPositions[rng.RandiRange(0, laserStartPositions.Count - 1)] as Marker2D;
		Vector2 dir = (GetGlobalMousePosition() - Position).Normalized();
		EmitSignal(SignalName.PlayerLaser, selectedLaser.GlobalPosition, dir);
	}
	
	private void FireGrenade() {
		_canGrenade = false;
		Globals.Get().GrenadeAmmo -= 1;
		_grenadeTimer.Start();
		
		var rng = new RandomNumberGenerator();
		Array<Node> grenadeStartPositions = GetNode<Node2D>("LaserStartPosition").GetChildren();
		Marker2D selectedGrenade = grenadeStartPositions[rng.RandiRange(0, grenadeStartPositions.Count - 1)] as Marker2D;
		Vector2 dir = (GetGlobalMousePosition() - Position).Normalized();
		EmitSignal(SignalName.PlayerGrenade, selectedGrenade.GlobalPosition, dir, CurSpeed);
	}

	private void _on_laser_timer_timeout() => _canLaser = true;

	private void _on_grenade_timer_timeout() => _canGrenade = true;

	public Camera2D GetCamera() => GetNode<Camera2D>("Camera2D");

	public void Hit(int damage) => Globals.Get().Health -= damage;

	public void Immobilize() {
		_immobilized = true;
		var t = CreateTween();
		t.TweenProperty(this, "CurSpeed", 0, 0.5);
	}
}
