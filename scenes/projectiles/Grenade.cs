using Godot;
using Godot.Collections;

public partial class Grenade : RigidBody2D
{
	[Export] public int Speed = 1700;
	[Export] public int BlastRadius = 400;
	[Export] public int HitsPerSecond = 2;
	[Export] public int DamagePerHit = 5;

	private bool _exploding = false;
	private ulong _lastHitFrame = 0;

	public override void _Process(double delta) {
		if (!_exploding) return;

		var frequency = Engine.GetFramesPerSecond() / HitsPerSecond;
		if (_lastHitFrame != 0 && ((Engine.GetProcessFrames() - _lastHitFrame) < frequency)) return;
		
		// Note: Better to have all of these derive from an Entity class.
		// You could just have one GetNodesInGroup() below, and the foreach
		// would be foreach (Entity target in targets) { //if in range; target.Hit(); }
		// But I'm not going back and doing all that work.  
		Array<Node> targets = GetTree().GetNodesInGroup("Entities");
		targets += GetTree().GetNodesInGroup("Containers");

		foreach (var node in targets) {
			if (node is not PhysicsBody2D target) continue;
			if (GlobalPosition.DistanceTo(target.GlobalPosition) > BlastRadius) continue;
			
			if (target.HasMethod("Hit"))
				target.Call("Hit", DamagePerHit);
		}

		_lastHitFrame = Engine.GetProcessFrames();
	}
	
	public void Explode() {
		AnimationPlayer ap = GetNode<AnimationPlayer>("AnimationPlayer");
		ap.Play("Explosion");
		_exploding = true;
	}
}
