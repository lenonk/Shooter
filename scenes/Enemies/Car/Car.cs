using Godot;
using Godot.NativeInterop;
using System;

public partial class Car : PathFollow2D {
	[Export] public int Damage = 40;
	
	private float TurnSpeed = 15;
    	
	private Player _target;
	private CharacterBody2D _body;
	private Sprite2D _turret;
	private AnimationPlayer _player;
	private Sprite2D _leftMuzzleFlash;
	private Sprite2D _rightMuzzleFlash;
	private Line2D _leftLine;
	private Line2D _rightLine;
	private RayCast2D _leftRay;
	private RayCast2D _rightRay;
	private Vector2 _leftLineOrigPoint;
	private Vector2 _rightLineOrigPoint;
	
	public override void _Ready() {
		_body = GetNode<CharacterBody2D>("Body");
		_turret = _body.GetNode<Sprite2D>("Turret/TurretImage");
		_player = _body.GetNode<AnimationPlayer>("AnimationPlayer");
		_leftMuzzleFlash = _turret.GetNode<Sprite2D>("LeftMuzzleFlash");
		_rightMuzzleFlash = _turret.GetNode<Sprite2D>("RightMuzzleFlash");
		_leftRay = _turret.GetNode<RayCast2D>("LeftRay");
		_rightRay = _turret.GetNode<RayCast2D>("RightRay");
		_leftLine = _turret.GetNode<Line2D>("LeftLine");
		_rightLine = _turret.GetNode<Line2D>("RightLine");

		_player.AnimationFinished += OnFireAnimationFinished;
		_leftLineOrigPoint = _leftLine.Points[1];
		_rightLineOrigPoint = _rightLine.Points[1];
	}

	public override void _Process(double delta) {
		ProgressRatio += (float)(0.02 * delta);
		
		if (_target != null) {
			var dir = (_target.Position - _turret.GlobalPosition).Normalized();
			var _rotAngle = dir.Angle() + Mathf.DegToRad(90) - Rotation;
			_turret.Rotation = (float)Mathf.MoveToward(_turret.Rotation, _rotAngle, TurnSpeed * delta);

			// This doesn't work the way I want it too.  Not sure why.  It's close, but the 
			// beams sometimes pass through the player, and sometimes stop short.
			_leftRay.ForceRaycastUpdate();
			if (_leftRay.IsColliding()) {
				var colPoint = _turret.ToLocal(_leftRay.GetCollisionPoint());
				var newPoint = new Vector2(_leftLine.Points[0].DistanceTo(colPoint), _leftLine.Points[1].Y);
				_leftLine.RemovePoint(1);
				_leftLine.AddPoint(newPoint);
			}

			_rightRay.ForceRaycastUpdate();
			if (_rightRay.IsColliding()) {
				var colPoint = _turret.ToLocal(_rightRay.GetCollisionPoint());
				var newPoint = new Vector2(_rightLine.Points[0].DistanceTo(colPoint), _rightLine.Points[1].Y);
				_rightLine.RemovePoint(1);
				_rightLine.AddPoint(newPoint);
			}

			if (!_player.IsPlaying())
				_player.Play("fire_weapon");	
		}
	}

	private void OnFireAnimationFinished(StringName name) {
		if (_target == null) {
			_player.Stop();
			return;
		}
		
		var t = CreateTween();
		t.TweenProperty(_leftMuzzleFlash, "modulate:a", 1, 0.05);
		t.Parallel();
		t.TweenProperty(_rightMuzzleFlash, "modulate:a", 1, 0.05);
		
		t.TweenProperty(_leftMuzzleFlash, "modulate:a", 0, 0.5);
		t.Parallel();
		t.TweenProperty(_rightMuzzleFlash, "modulate:a", 0, 0.5);
		
		_target?.Hit(Damage);
		_player.Play("fire_weapon");
	}
	
	private void OnAttackAreaExited(Node2D body) {
		_leftLine.RemovePoint(1);
		_leftLine.AddPoint(_leftLineOrigPoint);
		_rightLine.RemovePoint(1);
		_rightLine.AddPoint(_rightLineOrigPoint);
		
		if (_player.IsPlaying()) {
			_player.Pause();
			_player.PlayBackwards("fire_weapon");
		}
		_target = null;
	}
	
	private void OnAttackAreaEntered(Node2D body) => _target = body as Player;
	
}
