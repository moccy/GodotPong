using Godot;
using System;

public partial class Ui : CanvasLayer
{
	[Export] 
	public int Player1Score { get; set; } = 0;

	[Export] 
	public int Player2Score { get; set; } = 0;
	
	private Label _player1Label;
	private Label _player2Label;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player1Label = GetNode<Label>("ScoreP1");
		_player2Label = GetNode<Label>("ScoreP2");

		DisplayScores();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetScore(int player, int score)
	{
		switch (player)
		{
			case 0:
				Player1Score = score;
				break;
			case 1:
				Player2Score = score;
				break;
			default:
				GD.PushWarning("Attempted to set score for a player that doesn't exist.");
				break;
		}
		
		DisplayScores();
	}

	private void DisplayScores()
	{
		_player1Label.Text = Player1Score.ToString();
		_player2Label.Text = Player2Score.ToString();
	}

	public void OnBallSideHit(int side)
	{
		int scoreToIncrement = side == 0 ? Player1Score : Player2Score;
		SetScore(side, scoreToIncrement + 1);
	}
}
