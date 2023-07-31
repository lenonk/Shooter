using Godot;
using System;

public partial class Outside : ParentLevel
{
	public override void _Ready() {
		base._Ready();
		
		GetNode<Gate>("Gate").GateEntered += OnGateEntered;
		GetNode<House>("House").PlayerEnteredHouse += OnHouseEntered;
		GetNode<House>("House").PlayerExitedHouse += OnHouseExited;
	}

	private void OnGateEntered(Node body) {
		_player.Immobilize();
		TransitionLayer.ChangeScene("res://scenes/levels/inside.tscn");
	}
		
	private void OnHouseEntered() {
		var cam = _player.GetCamera();
		var t = CreateTween();
		t.TweenProperty(cam, "zoom", new Vector2(0.6F, 0.6F), 1).SetTrans(Tween.TransitionType.Quad);
	}
	
	private void OnHouseExited() {
		var cam = _player.GetCamera();
		var t = CreateTween();
		t.TweenProperty(cam, "zoom", new Vector2(0.4F, 0.4F), 1).SetTrans(Tween.TransitionType.Quad);
	}
}
