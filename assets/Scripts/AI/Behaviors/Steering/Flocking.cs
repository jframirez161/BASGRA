using UnityEngine;
using System.Collections.Generic;

public class Flocking:BaseBehavior {
	public float neighborRadius;

	public bool useSeperation;
	public float seperationWeight;
	Vector3 seperationForce;
	
	public bool useAlignment;
	public float alignmentWeight;
	Vector3 alignmentForce;
	
	public bool useCohesion;
	public float cohesionWeight;
	Vector3 cohesionForce;


	Vector3 initPos;
	List<AICharacter> listOfNeighbors;
	Vector3 dirn;

	void Start() {
		Init();
		mCharacter.addBehavior(this);
		initPos = transform.position;
		listOfNeighbors = new List<AICharacter>();
	}



	void Update() {	
		
		seperationWeight = 20.0f;
		alignmentWeight = 0.1f;
		cohesionWeight = 0.1f; 
	}

	
	public override bool UpdateAI() {
		if(!isWithinAOE()) return false;
		
		steeringForce = Vector3.zero;
		listOfNeighbors.Clear();
		
		// Get list of AIChar neighbors
		mCharacter.aiManager.getAICharNeighbors(listOfNeighbors, mCharacter, neighborRadius);
		
		if(listOfNeighbors.Count==0) return false;



		if(useSeperation)
		{
			seperationForce = Vector3.zero;
			foreach(AICharacter neighborChar in listOfNeighbors)
			{
				dirn = transform.position - neighborChar.transform.position;
				seperationForce += dirn.normalized * (1/dirn.magnitude);
			}
			seperationForce *= seperationWeight;
			
			steeringForce += seperationForce;
		}
		
		if(useAlignment)
		{
			// Note in this case alignmentForce represents average heading
			alignmentForce = Vector3.zero;
			foreach(AICharacter neighborChar in listOfNeighbors)
			{
				alignmentForce += neighborChar.Heading();
			}
			alignmentForce /= listOfNeighbors.Count;
			alignmentForce -= mCharacter.Heading();
			alignmentForce *= alignmentWeight;
			
			steeringForce += alignmentForce;
		}
		
		if(useCohesion)
		{
			// Note in this case cohesionForce represents center of mass
			cohesionForce = Vector3.zero;
			foreach(AICharacter neighborChar in listOfNeighbors)
			{
				cohesionForce += neighborChar.transform.position;
			}
			cohesionForce /= listOfNeighbors.Count;
			cohesionForce = seek(cohesionForce);
			cohesionForce *= cohesionWeight;
			
			steeringForce += cohesionForce;
		}
		
		return steeringForce.sqrMagnitude > 0;
	}
	
	override public string getName()
	{
		return "Flocking";
	}
	
	override public bool isWithinAOE()
	{
		return checkAOE(transform.position, initPos);
	}
	
	Vector3 seek(Vector3 targetPos) {
		dirn = targetPos - transform.position;
		dirn.Normalize();
		return((dirn * mCharacter.maxSpeed) - mCharacter.vel);
	}
}
