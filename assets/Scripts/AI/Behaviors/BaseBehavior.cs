
using UnityEngine;

// TO DO: Wall Avoidance, Interpose, Hide, Offset Pursuit, Leader Following

public abstract class BaseBehavior:MonoBehaviour {
	public float weight;
	public float aoe; // A sphere around the AI object - 0 -> means not taken into consideration
	public int priority;
	
	[HideInInspector]
	public AICharacter mCharacter;
	[HideInInspector]
	public Vector3 steeringForce;
	[HideInInspector]
	public bool paused;
	[HideInInspector]
	public bool ignoreWeightCalculations;
	
	public void Init() 
	{
		mCharacter = GetComponent<AICharacter>();
		ignoreWeightCalculations = false;
	}
	
	abstract public bool UpdateAI();
	
	protected bool checkAOE(Vector3 myPos, Vector3 targetPos) 
	{
		if(aoe==0) return true;
		if((myPos-targetPos).magnitude<=aoe) return true;	
		return false;
	}
	
	abstract public string getName();
	
	abstract public bool isWithinAOE();
}

