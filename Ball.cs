using Godot;

public partial class Ball : Area2D
{
	[Export] 
	public BinaryDirection InitialDirection;

	[Export] 
	public float Speed = 500f;

	[Export] 
	public float BounceVariation = 5;

	[Export]
	public int MinAngle = 45;
	
	[Signal]
	public delegate void SideHitEventHandler(BinaryDirection side);

	private Vector2 _direction;
	private float _radius;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_direction = InitialDirection == BinaryDirection.Right ? Vector2.Right : Vector2.Left;
		var circle = GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
		_radius = circle?.Radius ?? 0f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += _direction.Normalized() * Speed * (float)delta;
		CheckViewportCollision();
	}

	public void OnAreaEntered(Area2D area)
	{
		if (area.Name.ToString()!.EndsWith("Paddle"))
		{
			_direction = CalculateBounceDirection(area);
		}
	}

	private Vector2 CalculateBounceDirection(Area2D area)
	{
		var paddleShape = area.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
		if (paddleShape == null)
		{
			GD.PushWarning("Failed to find CollisionShape2D attached to paddle.");
			return -_direction;
		}
		
		float relativeY = Position.Y - area.Position.Y;
		float normalizedRelativeY =
			relativeY / (paddleShape.Size.Y / 2);

		float angle = normalizedRelativeY * Mathf.DegToRad(75);

		if (BounceVariation > 0)
		{
			float randomVariance = Mathf.DegToRad(GD.Randf() * (BounceVariation * 2) - BounceVariation);
			angle += randomVariance;
		}
		
		float minAngle = Mathf.DegToRad(MinAngle);
		if (MinAngle > 0 && Mathf.Abs(angle) > minAngle)
		{
			angle = angle < 0 ? -minAngle : minAngle;
		}
		 
		if (area.Name.ToString()!.StartsWith("Right"))
		{
			angle = -angle;
		}

		return new Vector2(-_direction.X, Mathf.Tan(angle)).Normalized();
	}
	
	private void CheckViewportCollision()
	{
		// Get the viewport size
		Vector2 viewportSize = GetViewportRect().Size;
		bool hitTop = Position.Y < 0;
		bool hitBottom = Position.Y > viewportSize.Y;
		bool hitLeft = Position.X < 0;
		bool hitRight = Position.X > viewportSize.X;

		if (hitTop || hitBottom)
		{
			_direction.Y = -_direction.Y;
		}

		if (hitLeft || hitRight)
		{
			_direction.X = -_direction.X;
		}

		if (hitLeft)
		{
			EmitSignal(SignalName.SideHit, Variant.From(BinaryDirection.Left));
		}

		if (hitRight)
		{
			EmitSignal(SignalName.SideHit, Variant.From(BinaryDirection.Right));
		}
	}
}

public enum BinaryDirection
{
	Left,
	Right
}
