using Godot;

public partial class ParentLevel : Node2D {
	protected Player _player;
	
	private PackedScene _laser = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/Laser.tscn");
	private PackedScene _grenade = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/Grenade.tscn");
	private PackedScene _itemPickup = ResourceLoader.Load<PackedScene>("res://scenes/objects/ItemPickup.tscn");

	public override void _Ready() {
		_player = GetNode<Player>("Player");
		_player.PlayerLaser += OnPlayerLaser;
		_player.PlayerGrenade += OnPlayerGrenade;

		foreach (var c in GetTree().GetNodesInGroup("Containers")) {
			((ItemContainer)c).SpawnStatItem += OnSpawnStatItem;
		}

		foreach (var c in GetTree().GetNodesInGroup("Scouts")) {
			((Scout)c).ScoutLaser += OnPlayerLaser;
		}
	}

	private void OnPlayerLaser(Vector2 pos, Vector2 dir) {
		Laser laser = _laser.Instantiate() as Laser;
		if (laser == null) return;
		laser.Position = pos;
		laser.RotationDegrees = Mathf.RadToDeg(dir.Angle()) + 90;
		laser.direction = dir;
		GetNode<Node2D>("Projectiles").AddChild(laser);
	}
	
	private void OnPlayerGrenade(Vector2 pos, Vector2 dir, float speed) {
		Grenade grenade = _grenade.Instantiate() as Grenade;
		if (grenade == null) return;
		grenade.Position = pos;
		grenade.LinearVelocity = dir * (grenade.Speed + speed);
		GetNode<Node2D>("Projectiles").AddChild(grenade);
	}

	private void OnSpawnStatItem(Vector2 pos, Vector2 dir) {
		ItemPickup item = _itemPickup.Instantiate() as ItemPickup;
		if (item == null) return;
		item.Position = pos;
		item.SpawnDirection = dir;
		GetNode<Node2D>("Items").CallDeferred("add_child", item);
	}
}