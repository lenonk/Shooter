using Godot;
using System;

public partial class House : Area2D {
	[Signal] public delegate void PlayerEnteredHouseEventHandler();
	[Signal] public delegate void PlayerExitedHouseEventHandler();
	private void _on_body_entered(Node2D body) {
		EmitSignal(SignalName.PlayerEnteredHouse);
		GD.Print(body + " Fired House Enter");
	}
	
	private void _on_body_exited(Node2D body) {
		EmitSignal(SignalName.PlayerExitedHouse);
	}
}
