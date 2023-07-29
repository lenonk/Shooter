using Godot;
using System;
using System.Reflection;
using System.Threading.Tasks;

public partial class TransitionLayer : CanvasLayer {
	private static AnimationPlayer _player;
	private static SceneTree _tree;
	private static TransitionLayer _layer;

	public override void _Ready() {
		_layer = this;
		_player = GetNode<AnimationPlayer>("AnimationPlayer");
		_tree = GetTree();
	}

	private async Task _changeScene(string target) {
		_player.Play("fade_to_black");
		await ToSignal(_player, "animation_finished");
		_tree.ChangeSceneToFile(target);
		_player.PlayBackwards("fade_to_black");
		await ToSignal(_player, "animation_finished");
	}

	public static async void ChangeScene(string target) => 
		await _layer._changeScene(target);
}
