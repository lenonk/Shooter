using Godot;
using System;

public partial class UI : CanvasLayer {
	private Label _laserLabel;
	private Label _grenadeLabel;
	private TextureRect _laserIcon;
	private TextureRect _grenadeIcon;
	private TextureProgressBar _healthBar;

	private Color _green = new("6bbfa3");
	private Color _red = new(0.9F, 0, 0, 1);

	public override void _Ready() {
		_laserLabel = GetNode<Label>("AmmoContainer/GridContainer/LaserLabel");
		_laserIcon = GetNode<TextureRect>("AmmoContainer/GridContainer/BulletTexture");
		
		_grenadeLabel = GetNode<Label>("AmmoContainer/GridContainer/GrenadeLabel");
		_grenadeIcon = GetNode<TextureRect>("AmmoContainer/GridContainer/MarginContainer/GrenadeTexture");

		_healthBar = GetNode<TextureProgressBar>("MarginContainer/TextureProgressBar");

		Globals.Get().PlayerStatsChanged += OnStatsChanged;

		OnStatsChanged(); // For initial setting
	}

	private void OnStatsChanged() {
		UpdateLaserText();	
		UpdateGrenadeText();
		UpdateHealth();
	}
	
	private void UpdateLaserText() {
		_laserLabel.Text = Math.Min(99, Globals.Get().LaserAmmo).ToString("D2");
		UpdateColors(Globals.Get().LaserAmmo, _laserLabel, _laserIcon);
	}
	
	private void UpdateGrenadeText() {
		_grenadeLabel.Text = Math.Min(99, Globals.Get().GrenadeAmmo).ToString("D2");
		UpdateColors(Globals.Get().GrenadeAmmo, _grenadeLabel, _grenadeIcon);
	}

	private void UpdateHealth() => _healthBar.Value = Globals.Get().Health;

	private void UpdateColors(int count, Label label, TextureRect icon) => 
		icon.Modulate = label.Modulate = (count > 0) ? _green : _red;
}
