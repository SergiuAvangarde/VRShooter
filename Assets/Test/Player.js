var PlayerState : float;

var PlayerAnimSec : GameObject;
var JumpingAnimationSec : GameObject;

var PlayerMotor : CharacterMotor;
var WalkingSpeed : float = 6;
var AimingInSpeed : float = 2;
var SprintingSpeed : float = 8;
var PlayerBossController : CharacterController;
var CharacterMag;
var PlayerAimingInSpeed : float = 0.2;
var Jumped : boolean;
var ReloadingGun : boolean;
var PlayerTopGun : GameObject;
var AimingIn : boolean = false;
var GunDown : boolean = false;


function Update () 
{

	PlayerStateController();
	PlayerAnims();
	CharacterMag = PlayerBossController.velocity.magnitude;
	ButtonClicker();
}

function ButtonClicker()
{
if (Input.GetButtonDown("Gun Down"))
	{
	if (GunDown == true)
	GunDown = false;
	
	else
	
	if (GunDown == false)
	GunDown = true;
	}
}

function ReloadController()
{
ReloadingGun = PlayerTopGun.GetComponent(GunScript).Reloading;
}



function PlayerStateController()
{
	if ((Input.GetAxis("Vertical") !=0 || Input.GetAxis("Horizontal") !=0))
{
	if (Input.GetButton("Sprint")&&!Input.GetButton("Fire2") && PlayerBossController.isGrounded)
		{
		PlayerState = 2;
		AimingIn = false;
		} 
		
		else if (Input.GetButton("Fire2") && !Input.GetButton("Sprint"))
		{
		PlayerState = 3;
		AimingIn = true;
		}
	else
	{
	PlayerState = 1;
	AimingIn = false;
	}
	
}
else
	if (GunDown == true && !Input.GetButton("Fire2") && !Input.GetButton("Fire1"))
		{
		PlayerState = -1;
		} 
		
else
if (!Input.GetButton("Fire2"))
{
PlayerState = 0;
AimingIn = false;
}

else if (Input.GetButton("Fire2") && !Input.GetButton("Sprint") && ReloadingGun == false)
{
PlayerState = 3;
AimingIn = true;
}
}




function PlayerAnims()
{
	if (Jumped && !PlayerBossController.isGrounded)
	{
	Jumped = false;
	JumpingAnimationSec.animation.Play("Jumpingliftoff Animation");	
	}
	
	if (!Jumped && PlayerBossController.isGrounded)
	{
	Jumped = true;
	JumpingAnimationSec.animation.Play("Jumpingland Animation");	
	}
	
	
if (PlayerState == -1)
	{
	PlayerAnimSec.animation.CrossFade("Gun Down Idle Animation");	
	}	
	
if (PlayerState == 0)
	{
	PlayerAnimSec.animation.CrossFade("Idle Animation");	
	}
	
else if (PlayerState == 1 && PlayerBossController.isGrounded)
	{
	PlayerAnimSec.animation["Walking Animation"].speed = CharacterMag/WalkingSpeed;
    PlayerAnimSec.animation.CrossFade("Walking Animation");
	
	PlayerMotor.movement.maxForwardSpeed = WalkingSpeed;
	PlayerMotor.movement.maxBackwardsSpeed = WalkingSpeed/2;
	PlayerMotor.movement.maxSidewaysSpeed = WalkingSpeed;	
	}	
	
else if (PlayerState == 2 && PlayerBossController.isGrounded)
	{
	PlayerAnimSec.animation["Sprint Animation"].speed = CharacterMag/SprintingSpeed;
	PlayerAnimSec.animation.CrossFade("Sprint Animation");
	
	PlayerMotor.movement.maxForwardSpeed = SprintingSpeed;
	PlayerMotor.movement.maxBackwardsSpeed = SprintingSpeed/2;
	PlayerMotor.movement.maxSidewaysSpeed = SprintingSpeed;
	}
	
else if (PlayerState == 3)
	{
	PlayerAnimSec.animation["Aiming In Idle Animation"].speed = 1+(CharacterMag/WalkingSpeed);
    PlayerAnimSec.animation.CrossFade("Aiming In Idle Animation",PlayerAimingInSpeed);
	
	PlayerMotor.movement.maxForwardSpeed = AimingInSpeed;
	PlayerMotor.movement.maxBackwardsSpeed = AimingInSpeed/2;
	PlayerMotor.movement.maxSidewaysSpeed = AimingInSpeed;	
	}	
}