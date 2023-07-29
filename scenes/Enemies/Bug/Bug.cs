using Godot;
using System;
using System.Threading.Tasks;

public partial class Bug : Enemy {
	[Export] public int DamagePerAttack = 10;
	
	private bool _canAttack = true;
	private bool _stopped = false;
	
	public override void _Ready() {
		_health = 40;
		MaxSpeed = 500;
		Acceleration = 1700.0f;
		DesiredRange = 275;
		RangeThreshold = 20;
		TurnSpeed = 5;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (_target == null || _dead) return;

		if (Math.Abs(Position.DistanceTo(_target.Position) - DesiredRange) < RangeThreshold) {
			if (!_stopped) {
				GetNode<AnimatedSprite2D>("AnimatedEnemyImage").Stop();
				_stopped = true;
			}
		}
		else {
			GetNode<AnimatedSprite2D>("AnimatedEnemyImage").Play("walk");
			_stopped = false;
		}

		if (_canAttack && Math.Abs(Position.DistanceTo(_target.Position)) <= (DesiredRange + RangeThreshold)) 
			Attack();
	}

	public override void Hit(int damage) {
		if (_health - damage <= 0)
			GetNode<AnimatedSprite2D>("AnimatedEnemyImage").Stop();
		base.Hit(damage);
	}
	private void Attack() {
		_canAttack = false;
		GetNode<AnimatedSprite2D>("AnimatedEnemyImage").Play("bite");
		_target.Hit(DamagePerAttack);
		GetNode<Timer>("AttackCooldown").Start();
	}

	protected override void SetShaderProgress(float val) {
		var image = GetNode<AnimatedSprite2D>("AnimatedEnemyImage");
		var shader = image.Material as ShaderMaterial;
		shader.SetShaderParameter("progress", val);
	}

	protected override void ResetShaderProgress() {
		var shader = GetNode<AnimatedSprite2D>("AnimatedEnemyImage").Material as ShaderMaterial;
		shader.SetShaderParameter("progress", 0);
	}

	private void OnAttackCooldownExpired() => _canAttack = true;
	private void OnBugAttackAreaEntered(Node2D body) => _target = body as Player;
	private void OnBugAttackAreaExited(Node2D body) => _target = null;
}
