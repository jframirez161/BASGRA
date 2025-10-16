using UnityEngine;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {
	public List<AICharacter> listOfAICharacters;

	float separation;
	float alineacion;
	float cohesion;
	public GUISkin myGUISkin;


	void Awake()
	{
		listOfAICharacters = new List<AICharacter>();

		separation	=10f;
		alineacion	=1f;
		cohesion	=1f;
	}

	/*void OnGUI () {

		GUI.skin = myGUISkin;
		GUI.Box (new Rect (200, 10, 150, 150), "Manada");
		separation = GUI.HorizontalSlider (new Rect (220, 40, 100, 30), separation, 0, 20);
		GUI.Label (new Rect (220, 50, 100, 30), "Separation: " + separation.ToString ("f1"));
		alineacion = GUI.HorizontalSlider (new Rect (220, 80, 100, 30), alineacion, 0, 2);
		GUI.Label (new Rect (220, 90, 100, 30), "Alineacion: " + alineacion.ToString ("f1"));
		cohesion = GUI.HorizontalSlider (new Rect (220, 120, 100, 30), cohesion, 0, 2);
		GUI.Label (new Rect (220, 130, 100, 30), "Cohesion: " + cohesion.ToString ("f1"));
	}*/

	void Update()
	{


		foreach(AICharacter aiChar in listOfAICharacters)
		{
			aiChar.GetComponent<Flocking>().seperationWeight = separation;
			aiChar.GetComponent<Flocking>().alignmentWeight	 = alineacion;
			aiChar.GetComponent<Flocking>().cohesionWeight	 = cohesion;

			aiChar.UpdateAI();	
		}
	}
	
	public void AddCharacter(AICharacter aiChar)
	{
		listOfAICharacters.Add(aiChar);
	}
	
	public void RemoveCharacter(AICharacter aiChar)
	{
		listOfAICharacters.Remove(aiChar);	
	}
	

	public void getAICharNeighbors(List<AICharacter> listOfNeighbors, AICharacter currentChar, float radius)
	{
		Vector3 dist;
		
		foreach(AICharacter aiChar in listOfAICharacters)
		{
			if(currentChar==aiChar) continue;
			
			dist = aiChar.transform.position - currentChar.transform.position;
			if(dist.magnitude<radius)
			{
				listOfNeighbors.Add(aiChar);	
			}
		}
	}
}
