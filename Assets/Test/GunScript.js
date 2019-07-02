var PlayerTransform : Transform;
var PlayerScript : Player;
var RotationSpeed : float;
var PlayerCamera : GameObject;
var IsAiming : boolean = false;
var GunAnimationHolder : GameObject;

//Targets

@HideInInspector
var TargetXrotation : float;

@HideInInspector
var TargetYrotation : float;

@HideInInspector
var TargetXrotationV : float;

@HideInInspector
var TargetYrotationV : float;

// Gun Specs
var MaxClipSize : float = 32;
var AmmoInCurrentClip : float = 32;
var ExtraAmmo : float = 128;
var MaxCarringAmmo : float = 256;

//  Bullets and shiz
var Bullet : GameObject;
var BulletSpawn : GameObject;
var BulletSound : GameObject;
var FireRate : float;
var FireTimer : float;

//Recoil
var Recoil : float = 3;
var RecoilAimingIn : float = 3;
var RecoilAimingOut : float = 6;

//Reloads
var ReloadAnimation : GameObject;
var ReloadSound : AudioSource;
var Reloading : boolean = false;
var ReloadName : String;


function Update()
{
AimingInController();
}

function AimingInController()
{
IsAiming = GunAnimationHolder.GetComponent(Player).AimingIn;
}

function LateUpdate () 
{

if (AmmoInCurrentClip > MaxClipSize)

AmmoInCurrentClip = MaxClipSize;

if (ExtraAmmo > MaxCarringAmmo)

ExtraAmmo = MaxCarringAmmo;

if (FireTimer < -5)
FireTimer = -5;

if (MaxClipSize < 0)

MaxClipSize = 0;

if (AmmoInCurrentClip < 0)

AmmoInCurrentClip = 0;

if (!Reloading && AmmoInCurrentClip < MaxClipSize && ExtraAmmo > 0 && Input.GetButtonDown("Reload")&& IsAiming == false)
	{
		Reloading = true;
		ReloadSound.Play();
		ReloadAnimation.animation.Play(ReloadName);
	}
	
if (!Reloading && AmmoInCurrentClip == 0 && ExtraAmmo > 0 && Input.GetButtonDown("Fire1") && IsAiming == false)
	{
		Reloading = true;
		ReloadSound.Play();
		ReloadAnimation.animation.Play(ReloadName);
	}
	
if (Reloading && !ReloadAnimation.animation.IsPlaying(ReloadName))
{
if (ExtraAmmo >= MaxClipSize - AmmoInCurrentClip)
{
ExtraAmmo -= MaxClipSize - AmmoInCurrentClip;
AmmoInCurrentClip = MaxClipSize;
}

if (ExtraAmmo < MaxClipSize - AmmoInCurrentClip)
{
AmmoInCurrentClip += ExtraAmmo;
ExtraAmmo = 0;
}
Reloading = false;
}

var MyBulletSound : GameObject;

if (Input.GetButton("Fire1") && AmmoInCurrentClip > 0 && !Reloading)
{

if (FireTimer <= 0)
{
AmmoInCurrentClip -= 1;

TargetXrotation += (Random.value - 0.5) *Mathf.Lerp (Recoil, RecoilAimingOut,0);
TargetYrotation += (Random.value - 0.5) *Mathf.Lerp (Recoil, RecoilAimingOut,0);


if (Bullet)

Instantiate(Bullet,BulletSpawn.transform.position,BulletSpawn.transform.rotation);

if (BulletSound)

	MyBulletSound = Instantiate(BulletSound,BulletSpawn.transform.position,BulletSpawn.transform.rotation);

FireTimer = 1;

}


}

if (Input.GetButton("Fire2") && !Reloading )
{
	Recoil = RecoilAimingIn;

}

if (Input.GetButton("Fire2") == false || Reloading )
{
	Recoil = RecoilAimingOut;

}


FireTimer -= Time.deltaTime * FireRate;

transform.position = PlayerCamera.transform.position + (Quaternion.Euler(0,TargetYrotation,0) * Quaternion.Euler(TargetXrotation,TargetYrotation,0) * Vector3(0,0,0));

TargetXrotation = Mathf.SmoothDamp(TargetXrotation, -PlayerCamera.GetComponent(MouseLook).rotationY,TargetXrotationV,RotationSpeed);

TargetYrotation =Mathf.SmoothDamp(TargetYrotation, PlayerCamera.GetComponent(MouseLook).RotationX,TargetYrotationV,RotationSpeed);

transform.rotation = Quaternion.Euler(TargetXrotation,TargetYrotation,0);


}