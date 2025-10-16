using UnityEngine;

public class Seek:BaseBehavior {
	public Vector3 targetPos;
	public GameObject seekTarget;
	Vector3 seekDirn;
	public bool withArrival;
	public float arrivalFactor;
	float dist;
	float speed;
	
	void Start() {
		Init();
		mCharacter.addBehavior(this);
	}
	
	public override bool UpdateAI() {
		if(seekTarget!=null)
		{
			targetPos = seekTarget.transform.position;	
		}
		
		if(isWithinAOE()) {
			seekDirn = targetPos - transform.position;
			if(withArrival) {
				dist = seekDirn.magnitude;
			}
			seekDirn.Normalize();
			if(withArrival)
			{
				speed = dist / arrivalFactor;
				speed = Mathf.Min(speed, mCharacter.maxSpeed);
				steeringForce = (seekDirn * speed) - mCharacter.vel;
			}
			else
			{
				steeringForce = (seekDirn * mCharacter.maxSpeed) - mCharacter.vel;
			}
			return true;
		}
		
		return false;
	}
	
	override public string getName()
	{
		return "Seek";
	}
	
	override public bool isWithinAOE()
	{
		return checkAOE(transform.position, targetPos);
	}
}
