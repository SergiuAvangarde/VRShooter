var Range : float = 10000;

var HitParticle : GameObject;
var HitSound : GameObject;

var HitParticleSpacing : float = 0.001;

function Update () 
{

var Hit : RaycastHit;

	if (Physics.Raycast(transform.position, transform.forward, Hit , Range))
	{
	
		if (HitParticle)
		{
	      Instantiate(HitParticle, Hit.point + (Hit.normal * HitParticleSpacing), Quaternion.LookRotation(Hit.normal));
		}
	
		if (HitSound)
		{
	      Instantiate(HitSound, Hit.point + (Hit.normal * HitParticleSpacing), Quaternion.LookRotation(Hit.normal));
		}
	
	
	
	}

Destroy(gameObject);

}