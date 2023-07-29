using Godot;
using System;

public partial class Gate : StaticBody2D {
	[Signal]
	public delegate void GateEnteredEventHandler(Node body);

	private void _on_area_2d_body_entered(Node body) => EmitSignal(SignalName.GateEntered, body);
}
