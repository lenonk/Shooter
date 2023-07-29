using Godot;
using System;
using System.Threading.Tasks;

public partial class Enemy : CharacterBody2D
{
	protected Player _target = null;
	
	protected float MaxSpeed = 700.0f;
	protected float Acceleration = 1400.0f;
	protected float DesiredRange = 750;
	protected float RangeThreshold = 100;
	protected float TurnSpeed = 15;
	
	private double _curSpeed = 0.0f;
	
	protected int _health = 10;
	protected bool _dead = false;

	public override void _Process(double delta)
	{
		if (_target == null || _dead) return;

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

	private async Task PlayDeathAnimation() {
		var p = GetNode<GpuParticles2D>("Particles/HitParticles");
		if (p == null) return;

		p.Emitting = true;
		var t = GetTree().CreateTimer(0.5);
		await ToSignal(t, "timeout");
	}
	
	public async virtual void Hit(int damage) {
		var t = CreateTween();
		t.SetLoops(5);
		t.TweenMethod(Callable.From<float>(SetShaderProgress), 0.0f, 0.5f, 0.2);
		t.Finished += ResetShaderProgress;
		_health -= damage;
		GetNode<GpuParticles2D>("Particles/HitParticles").Emitting = true;
		
		if (_health <= 0 && !_dead) {
			_dead = true;
			await PlayDeathAnimation();
			QueueFree();
		}
	}

	protected virtual void SetShaderProgress(float val) {
		var image = GetNode<Sprite2D>("EnemyImage");
		var shader = image.Material as ShaderMaterial;
		shader.SetShaderParameter("progress", val);
	}

	protected virtual void ResetShaderProgress() {
		var shader = GetNode<Sprite2D>("EnemyImage").Material as ShaderMaterial;
		shader.SetShaderParameter("progress", 0);
	}
}
