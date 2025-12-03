using Godot;

public partial class Hud : CanvasLayer
{
	[Export] public Label ScoreLabel;
	[Export] public Timer ScoreTimer;

	private int elapsedSeconds = 0;
	private int minutes = 0;
	private int seconds = 0;
	
	[Export] Control GameOver;
	[Export] Label DescriptionText;

	public override void _Ready()
	{
		UpdateScoreLabel();
		
		GameOver.Visible = false;
	}

	private void OnScoreTimerTimeout()
	{
		elapsedSeconds++;

		UpdateScoreLabel();
	}


	private void UpdateScoreLabel()
	{
		minutes = elapsedSeconds / 60;
		seconds = elapsedSeconds % 60;

		ScoreLabel.Text = $"{minutes:D1}:{seconds:D2}";
	}
	
	private void OnPlayerDeath()
	{
		ScoreLabel.Visible = false;
		GameOver.Visible = true;
		DescriptionText.Text = $"You survived for {minutes:D1}:{seconds:D2}!";
	}
	
	private void OnRestartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}
}
