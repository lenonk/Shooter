using Godot;
using System;
using System.Linq.Expressions;

public partial class ItemPickup : Area2D
{
	private enum PickupType {
		LaserAmmo,
		GrenadeAmmo ,
		Health
	}

	private PickupType _type;
	private Sprite2D _sprite;
	
	public Vector2 SpawnDirection;
	
	public override void _Ready() {
		_sprite = GetNode<Sprite2D>("Sprite2D");
		var rng = new RandomNumberGenerator();
		var t = rng.RandiRange(0, Enum.GetNames(typeof(PickupType)).Length - 1);

		_type = t switch {
			0 => PickupType.LaserAmmo,
			1 => PickupType.GrenadeAmmo,
			2 => PickupType.Health,
			_ => _type
		};

		_sprite.Modulate = _type switch {
			PickupType.LaserAmmo => new Color(0.1F, 0.2F, 0.8F),
			PickupType.GrenadeAmmo => new Color(0.8F, 0.2F, 0.1F),
			PickupType.Health => new Color(0.1F, 0.8F, 0.1F),
			_ => _sprite.Modulate
		};
		
		var targetPos = Position + SpawnDirection * rng.RandiRange(150, 250);
		var tween = CreateTween();
		tween.SetParallel();
		tween.TweenProperty(this, "position", targetPos, 0.4);
		tween.TweenProperty(this, "scale", new Vector2(1, 1), 0.3).From(new Vector2(0, 0));

	}

	public override void _Process(double delta) {
		Rotation += (float)(4.0 * delta);
	}

	private void OnActivate(Node2D body) {
		switch (_type) {
			case PickupType.LaserAmmo:
				Globals.Get().LaserAmmo += 20;
				break;
			case PickupType.GrenadeAmmo:
				Globals.Get().GrenadeAmmo += 5;
				break;
			case PickupType.Health:
				Globals.Get().Health += 25;
				break;
			default:
				GD.Print("Unknown PickupType in OnActivate()");
				break;
		}
		QueueFree();	
	}
}
