using Godot;

public partial class Paddle : Area2D
{
	[Export]
	public int Speed { get; set; }
	
	[Export] 
	public string UpAction { get; set; }
	[Export] 
	public string DownAction { get; set; }

	public Vector2 ScreenSize;
	public Vector2 PaddleSize;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var paddleSprite = GetNode<Sprite2D>("Sprite2D");
		ScreenSize = GetViewportRect().Size;
		PaddleSize = paddleSprite.Texture.GetSize() * Scale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;
		
		if (Input.IsActionPressed(UpAction))
		{
			velocity.Y -= 1;
		}

		if (Input.IsActionPressed(DownAction))
		{
			velocity.Y += 1;
		}

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, PaddleSize.X / 2, ScreenSize.X - PaddleSize.X / 2),
			y: Mathf.Clamp(Position.Y, PaddleSize.Y / 2, ScreenSize.Y - PaddleSize.Y / 2)
		);
	}
}
