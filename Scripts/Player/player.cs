using Godot;
using System;
using System.Security.AccessControl;

public partial class player : Node2D
{
	
	private Vector2 Motion;
	private Godot.CharacterBody2D PlayerBody;
	private AnimatedSprite2D Animation;
	private RayCast2D PlayerBodyRay;

	private string PlayerDirection;
	private string PlayerAnimation;
	private int PreviousDirectionEnum;

	[Export]
	public int Speed { get; set; } = 5;
	private int GameStateOverworld;

	[Export]
	public float JumpHeight = 100.0f;
	[Export]
	public float JumpTimeToPeak = 0.2f;
	[Export]
	public float JumpTimeToDescent = 0.4f;
	public  float JumpVelocity; 
	public float JumpGravity;
	public float FallGravity; 
	private bool IsOnFloor;
	


	public override void _Ready()
	{	
		JumpVelocity = (2.0f * JumpHeight) / (JumpTimeToPeak) * -1.0f;
		JumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
		FallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;
		PlayerBody = GetChild<CharacterBody2D>(0);
		PlayerBodyRay = PlayerBody.GetChild<RayCast2D>(3);
		Animation = PlayerBody.GetChild<AnimatedSprite2D>(0);
		GameStateOverworld = 0;
		
	}
	
		public void GetInput()
	{
		var XDirection = Input.GetAxis("Left","Right");
		Motion = PlayerBody.Velocity;
		Motion.X = XDirection * Speed;
		PlayerBody.Velocity= Motion;
	}

	public void Print(string str)
	{
		GD.Print(str);
	}

	public float ApplyGravity()
	{
		var Velocity = Motion;

		if (Velocity.Y < 0.0)
		{
			return JumpGravity;
		}
		else{
			return FallGravity;
		}
	}
	public Vector2 Jump()
	{
		var Velocity = Motion;
		Velocity.Y = JumpVelocity;
		return Velocity;
	}

	public void PlayWalkingAnimation(int PlayerDirectionEnum)
	{
		string NewPlayerAnimation = "";
		switch (PlayerDirectionEnum)
		{
			case(0):
				NewPlayerAnimation = "IdleRight";
			break;
			case(1):
			break;
			case(2): //UP
				//NewPlayerAnimation = "Jump";
			break;
			case(3):
			break;
			case(4):
				NewPlayerAnimation = "Left";
			break;
			case(5):
				if(PlayerAnimation == "AttackRight" && GameStateOverworld == 0)
				{
					PreviousDirectionEnum = 6;
				}
				else if (PlayerAnimation == "AttackLeft" && GameStateOverworld == 0)
				{
					PreviousDirectionEnum = 4;
				}
				if(PreviousDirectionEnum == 6 || PreviousDirectionEnum == 3 || PreviousDirectionEnum == 9)
				{
					NewPlayerAnimation = "IdleRight";
				}
				else if (PreviousDirectionEnum == 4 || PreviousDirectionEnum == 1 || PreviousDirectionEnum == 7)
				{
					NewPlayerAnimation = "IdleLeft";
				}
				else
				{
					NewPlayerAnimation = PlayerAnimation;
				}
			break;
			case(6):
				NewPlayerAnimation = "Right";
			break;
			case(7):
			break;
			case(8):
				NewPlayerAnimation = "CrouchNeutral";
				//Crouch  
			break;      
			case(9):

			break;
		}
		if(Input.IsActionJustPressed("Attack") && (NewPlayerAnimation == "IdleLeft" || NewPlayerAnimation == "Left")){
			NewPlayerAnimation = "AttackLeft";
			PlayerDirection = "Left";
			GameStateOverworld = 1;
			GD.Print("Hi");
		}
		else if(Input.IsActionJustPressed("Attack") && (NewPlayerAnimation == "IdleRight" || NewPlayerAnimation == "Right")){
			NewPlayerAnimation = "AttackRight";
			PlayerDirection = "Right";
			GameStateOverworld = 1;
			GD.Print("Hi");
		}
		PreviousDirectionEnum = PlayerDirectionEnum;
		PlayerAnimation = NewPlayerAnimation;
		Animation.Play(PlayerAnimation); 
		
	}

	public void AttackKeepLocationAndTrackAnimation()
	{
		var CurrFrame = Animation.Frame;
		SetProcessInput(false);
		switch(CurrFrame)
		{
			case(0):
				break;
			case(1):
			//PERMANENT CHANGE PLAYERBODY TO VAR
				PlayerBody.GetChild<CollisionShape2D>(5).Disabled = false;
				break;
			case(2):
				PlayerBody.GetChild<CollisionShape2D>(5).Disabled = true;
				break;
			case(3):
				GameStateOverworld = 0;
				//CHANGE TO HAVE LOGIC BETWEEN RIGHT AND LEFT
				Print(GetInputDirection().ToString());
				PlayWalkingAnimation(GetInputDirection());
				SetProcessInput(true);
				break;
			default:
			break;
		}
	}
	public int GetInputDirection()
	{
		int PlayerDirectionEnum;
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
		}
		else{
			PlayerDirectionEnum = 5;
		}
		return PlayerDirectionEnum;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	public override void _PhysicsProcess(double delta)
	{


		switch(GameStateOverworld)
		{
			case(0):
				GetInput();
				if (!PlayerBodyRay.IsColliding())
				{
					Motion.Y += ApplyGravity() * (float)delta;
					IsOnFloor = false;
				}
				else{
					Motion.Y = 0;
					IsOnFloor = true;
				}
				if (Input.IsActionJustPressed("Jump") && PlayerBodyRay.IsColliding())
				{
					Motion.Y = JumpVelocity;
					IsOnFloor = false;
				}
				PlayerBody.Velocity = Motion;
				PlayerBody.MoveAndCollide(PlayerBody.Velocity);
				PlayWalkingAnimation(GetInputDirection());

				
			break;
			// Attack
			case(1):
				AttackKeepLocationAndTrackAnimation();
			break;
		}


	}

	

}
