using Godot;

public partial class Player : CharacterBody2D
{
	[Export]public float Speed { get; set; } = 200.0f;
	[Export] public float SprintMultiplier {get; set; } = 1.5f;
	
	public float CurrentBattery {get; private set; }
	[Export] public float MaxBattery = 100f;
	[Export] public float DrainPerSecond = 1f;
	[Export] public NodePath BatteryBarPath;
	
	public float CurrentStamina {get; private set; }
	[Export] public float MaxStamina = 100f;
	[Export] public float StaminaDrainPerSecond = 20f;
	[Export] public float StaminaRegenPerSecond = 10f;
	[Export] public NodePath StaminaBarPath;
	
	[Signal] public delegate void BatteryUpdateEventHandler();
	[Signal] public delegate void PlayerDeathEventHandler();
	[Signal] public delegate void StaminaUpdateEventHandler();
	
	private TextureProgressBar _staminaBar;
	private TextureProgressBar _batteryBar;
	private bool isDead = false;
	
	public override void _Ready()
	{
		CurrentBattery = MaxBattery;
		
		if (BatteryBarPath != null && !BatteryBarPath.IsEmpty)
		{
			_batteryBar = GetNode<TextureProgressBar>(BatteryBarPath);
			_batteryBar.MaxValue = MaxBattery;
			_batteryBar.Value = CurrentBattery;
		}
		
		CurrentStamina = MaxStamina;
		if (StaminaBarPath != null && !StaminaBarPath.IsEmpty)
		{
			_staminaBar = GetNode<TextureProgressBar>(StaminaBarPath);
			_staminaBar.MaxValue = MaxStamina;
			_staminaBar.Value = CurrentStamina;
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (isDead)
			return;
			
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		bool wantsToSprint = Input.IsActionPressed("sprint");
		bool hasStamina = CurrentStamina > 0.1f;
		bool isMoving = inputDirection != Vector2.Zero;
		
		float moveSpeed = Speed;
		
		if (wantsToSprint && hasStamina)
		{
			moveSpeed *= SprintMultiplier;
			DrainStamina((float)delta);
		}
		else {
			if (!wantsToSprint)
			{
				RegenerateStamina((float)delta);
			}
		}
		
		Velocity = inputDirection * moveSpeed;
		MoveAndSlide();
		
		UpdateBattery((float)delta);
	}
	
	private void UpdateBattery(float delta)
	{
		if (CurrentBattery <= 0)
		{
			CurrentBattery = 0;
			return;
		}
		
		CurrentBattery -= DrainPerSecond * delta;
		CurrentBattery = Mathf.Clamp(CurrentBattery, 0, MaxBattery);
		
		if (_batteryBar != null)
		{
			_batteryBar.Value = CurrentBattery;
		}
		
		EmitSignal(SignalName.BatteryUpdate, CurrentBattery / 100);
	}
	
	public void AddBattery(float amount)
	{
		CurrentBattery = Mathf.Clamp(CurrentBattery + amount, 0, MaxBattery);
		
		if (_batteryBar != null)
		{
			_batteryBar.Value = CurrentBattery;
		}
	}
	
	private void DrainStamina(float delta)
	{
		CurrentStamina -= StaminaDrainPerSecond * delta;
		CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
		
		if (_staminaBar != null)
		{
			_staminaBar.Value = CurrentStamina;
		}
		
		EmitSignal(SignalName.StaminaUpdate, CurrentStamina / MaxStamina);
	}
	
	private void RegenerateStamina(float delta)
	{
		CurrentStamina += StaminaRegenPerSecond * delta;
		CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
		
		if(_staminaBar != null)
		{
			_staminaBar.Value = CurrentStamina;
		}
		
		EmitSignal(SignalName.StaminaUpdate, CurrentStamina / MaxStamina);
	}
	
	public void Die()
	{
		if (isDead)
			return;
			
		isDead = true;
		
		// Stop all movement
		Velocity = Vector2.Zero;
		
		// Emit the death signal
		EmitSignal(SignalName.PlayerDeath);
	}
}
