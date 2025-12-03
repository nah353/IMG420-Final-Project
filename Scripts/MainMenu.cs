using Godot;
using System;

public partial class MainMenu : Control
{
	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}
}
