using Godot;
using System;

public partial class player : Node2D
{
	
	private Godot.Vector2 InputDirection;
	private Godot.CharacterBody2D PlayerBody;
	private AnimatedSprite2D Animation;
	private RayCast2D PlayerBodyRay;

	private string PlayerDirection;

	[Export]
	public int Speed { get; set; } = 10;
	private int GameStateOverworld;
	public int Gravity {get; set;} = 100;


	public override void _Ready()
	{	

		PlayerBody = GetChild<CharacterBody2D>(0);
		PlayerBodyRay = PlayerBody.GetChild<RayCast2D>(3);
		Animation = PlayerBody.GetChild<AnimatedSprite2D>(0);
		GameStateOverworld = 0;

	}
	
		public void GetInput()
	{
		InputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		InputDirection.Y = 0;
		PlayerBody.Velocity= InputDirection * Speed;
	}


	public Godot.Vector2 ApplyGravity(double delta)
	{
		var Velocity = PlayerBody.Velocity;



		if (PlayerBodyRay.IsColliding())
		{
			Velocity.Y = 0;
		}

		else
		{
			Velocity.Y += (float)delta * Gravity;
		}
		if (Input.IsActionJustReleased("Jump"))
		{
			Velocity.Y = -30;
			GD.Print("hi");
		}
		//PlayerBody.Velocity = Velocity;

		return Velocity;
	}
	public void PlayPassiveAnimation()
	{
		int PlayerDirectionEnum = 0;
		if(Input.IsActionPressed("Right"))
		{
			if(Input.IsActionPressed("Left")){
				PlayerDirectionEnum = 5; 
			}
			else if(Input.IsActionPressed("Up")){
				PlayerDirectionEnum = 3; 
			}
			else if(Input.IsActionPressed("Down")){
				PlayerDirectionEnum = 9; 
			}
			else {
				PlayerDirectionEnum = 6;
			}
		}
		else if(Input.IsActionPressed("Left"))
		{
			if(Input.IsActionPressed("Right")){
			PlayerDirectionEnum = 5;
			}
			else if(Input.IsActionPressed("Up")){
				PlayerDirectionEnum = 1; 
			}
			else if(Input.IsActionPressed("Down")){
				PlayerDirectionEnum = 7; 
			}
			else {
				PlayerDirectionEnum = 4;
			}
		}
		else if(Input.IsActionPressed("Up"))
		{
			if(Input.IsActionPressed("Right")){
				PlayerDirectionEnum = 3;
			}
			else if(Input.IsActionPressed("Left")){
				PlayerDirectionEnum = 1; 
			}
			else if(Input.IsActionPressed("Down")){
				PlayerDirectionEnum = 5; 
			}
			else {
				PlayerDirectionEnum = 2;
			}
		}
		else if(Input.IsActionPressed("Down"))
		{
			if(Input.IsActionPressed("Right")){
				PlayerDirectionEnum = 9;
			}
			else if(Input.IsActionPressed("Up")){
				PlayerDirectionEnum = 5; 
			}
			else if(Input.IsActionPressed("Left")){
				PlayerDirectionEnum = 7; 
			}
			else {
				PlayerDirectionEnum = 8;
			}
			PlayerDirectionEnum = 8;
		}
		else{
			PlayerDirectionEnum = 5;
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	public override void _PhysicsProcess(double delta)
	{

		switch(GameStateOverworld)
		{
			case(0):
				Godot.Vector2 Motion;
				GetInput();
				Motion = ApplyGravity(delta);
				PlayerBody.MoveAndCollide(Motion);
				PlayPassiveAnimation();
			break;
			case(1):
				
			break;
		}


	}

	

}
