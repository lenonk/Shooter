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

	private async Task WaitAnimation(String anim, bool playBackwards = false) {
		if (playBackwards) _player.PlayBackwards(anim);
		else _player.Play(anim);
		
		await ToSignal(_player, "animation_finished");
	}
	
	public static async void ChangeScene(String target) {
		await _layer.WaitAnimation("fade_to_black");
		_tree.ChangeSceneToFile(target);
		await _layer.WaitAnimation("fade_to_black", true);
	}
}
