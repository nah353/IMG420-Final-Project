using Godot;

public partial class Arrow : Sprite2D
{
	[Export] public Node2D Pivot;
	[Export] public NodePath SpawnerPath;

	[Export] public float OrbitRadius = 80f;
	[Export] public float HideDistance = 50f;
	[Export] public float SpriteAngleOffsetDegrees = 45f;
	[Export] public float HintDuration = 20f;               // Show hints for this many seconds per battery
	[Export] public float BlinkInterval = 0.5f;       
	
	private ItemSpawnArea _spawner;
	
	private Node2D _trackedItem;
	private float _trackedTime = 0f;
	private float _blinkTimer = 0f;
	
	public override void _Ready()
	{
		if (SpawnerPath != null && !SpawnerPath.IsEmpty)
		{
			_spawner = GetNode<ItemSpawnArea>(SpawnerPath);
		}
	}

	public override void _Process(double delta)
	{
		if (_spawner == null || Pivot == null)
		{
			Visible = false;
			return;
		}

		var target = _spawner.CurrentItem;

		if (target != _trackedItem || (_trackedItem != null && !GodotObject.IsInstanceValid(_trackedItem)))
		{
			_trackedItem = GodotObject.IsInstanceValid(target) ? target : null;
			_trackedTime = 0f;
			_blinkTimer = 0f;
		}
		
		if (_trackedItem == null || !GodotObject.IsInstanceValid(_trackedItem))
		{
			Visible = false;
			return;
		}
		
		_trackedTime += (float)delta;
		
		if (_trackedTime > HintDuration)
		{
			Visible = false;
			return;
		}

		Vector2 pivotPos = Pivot.GlobalPosition;
		Vector2 targetPos = target.GlobalPosition;


		Vector2 dir = targetPos - pivotPos;
		if (dir.LengthSquared() < 0.0001f)
		{
			Visible = false;
			return;
		}

		Vector2 dirNorm = dir.Normalized();

		// Orbit: place arrow on a circle around the pivot
		GlobalPosition = pivotPos + dirNorm * OrbitRadius;

		// Rotate arrow to point at item, correcting for sprite pointing top-right
		float angle = dirNorm.Angle();
		float offset = Mathf.DegToRad(SpriteAngleOffsetDegrees);
		Rotation = angle - offset;

		// Blink logic: visible for BlinkInterval, invisible for BlinkInterval
		_blinkTimer += (float)delta;
		float cycle = BlinkInterval * 2f;
		bool on = (_blinkTimer % cycle) < BlinkInterval;

		Visible = on;
	}
}
