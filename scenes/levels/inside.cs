using Godot;
using System;

public partial class inside : ParentLevel
{
	private void OnExitGateEntered(Node2D body) {
		_player.Immobilize();
		TransitionLayer.ChangeScene("res://scenes/levels/outside.tscn");
	}
}
