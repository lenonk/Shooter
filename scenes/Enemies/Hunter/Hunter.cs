using Godot;
using System;

public partial class Hunter : Enemy {
	private AnimationPlayer _anim;

	public override void _Ready() {
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");

		MaxSpeed = 200f;
		DesiredRange = 200;
		RangeThreshold = 50;

		_health = 50;
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}
	private void OnAttackAreaEntered(Node2D body) {
		_target = body as Player;
		_anim.Play("walk");
	}

	private void OnAttackAreaExited(Node2D body) {
		_target = null;

		if (_anim.IsPlaying() && _anim.CurrentAnimation == "walk")
			_anim.Stop();
	}
	
}
