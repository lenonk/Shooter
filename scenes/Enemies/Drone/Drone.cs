using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class Drone : Enemy {
	[Export] public int ExplosionDamage = 25;
	[Export] public int BlastRadius = 400;
	
	public override void _Ready() {
		MaxSpeed = 2000.0f;
		Acceleration = 600.0f;
		DesiredRange = 150;
		RangeThreshold = 20;
		TurnSpeed = 20;
	}

	public override void _Process(double delta) {
		if (_target == null) return;

		CalculateMovement(delta);

		if (!_dead && MoveAndCollide(Velocity * (float)delta) != null) {
			Hit(9999);
		}
		
		if (!_dead && Math.Abs(Position.DistanceTo(_target.Position)) <= (DesiredRange + RangeThreshold)) 
			Hit(9999);
	}

	public override void Hit(int damage) {
		if (_health - damage <= 0)
			Explode();
		base.Hit(damage);
	}

	private async void DamageContainers() {
		var p = GetNode<AnimationPlayer>("AnimationPlayer");
		if (p.IsPlaying())
			await ToSignal(p, "animation_finished");
		
		foreach (var node in GetTree().GetNodesInGroup("Containers")) {
			var target = (ItemContainer)node;

			if (Math.Abs(GlobalPosition.DistanceTo(target.GlobalPosition)) <= BlastRadius) {
				target.Hit(ExplosionDamage);
			}
		}
	}
	
	private void Explode() {
		if (_target != null && !_dead &&
		    Math.Abs(GlobalPosition.DistanceTo(_target.GlobalPosition)) <= BlastRadius) 
			_target.Hit(ExplosionDamage);

		DamageContainers();
	}
	
	private void OnAttackAreaEntered(Node2D body) => _target = body as Player;
}
