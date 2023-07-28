using Godot;
using System;

public partial class Globals : Node {
	private static Globals _instance;
	
	[Signal] public delegate void PlayerStatsChangedEventHandler();
	
	private int _laserAmmo = 20;
	public int LaserAmmo {
		get => _laserAmmo;
		set  { _laserAmmo = Math.Min(value, 99); EmitSignal(SignalName.PlayerStatsChanged); }
	}
	
	private int _grenadeAmmo = 5;
	public int GrenadeAmmo {
		get => _grenadeAmmo;
		set  { _grenadeAmmo = Math.Min(value, 99); EmitSignal(SignalName.PlayerStatsChanged); }
	}
	
	private int _health = 100;
	public int Health {
		get => _health;
		set  { _health = Math.Min(value, 100); EmitSignal(SignalName.PlayerStatsChanged); }
	}

	public override void _Ready() {
		_instance = this;
	}

	public static Globals Get() => _instance;
	public static int GetLaserAmmo() => _instance.LaserAmmo;
	public static int GetGrenadeAmmo() => _instance.GrenadeAmmo;
	public static int GetHealth() => _instance.Health;
}