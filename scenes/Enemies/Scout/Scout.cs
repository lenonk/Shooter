using Godot;
using Godot.Collections;

public partial class Scout : Enemy {
	[Signal] public delegate void ScoutLaserEventHandler(Vector2 pos, Vector2 dir);
	
	private bool _canLaser = true;
	private int _nextLaser = 0;

	public override void _Ready() {
		_health = 15;
	}
	
	public override void _Process(double delta) {
		base._Process(delta);	
		if (_target == null) return;
		if (_canLaser) FireLaser();
	}

	private void FireLaser() {
		_canLaser = false;
		var laser = GetNode<Node2D>("LaserStartPositions").GetChildren()[_nextLaser];
		Vector2 dir = (_target.GlobalPosition - ((Marker2D)laser).GlobalPosition).Normalized();
		_nextLaser = (_nextLaser == 0) ? 1 : 0;
		GetNode<Timer>("LaserCooldown").Start();
		EmitSignal(SignalName.ScoutLaser, ((Marker2D)laser).GlobalPosition, dir);
	}
	
	private void OnAttackAreaEntered(Node2D body) => _target = body as Player;
	private void OnAttackAreaExited(Node2D body) => _target = null;
	private void OnLaserCooldownExpired() => _canLaser = true;
}