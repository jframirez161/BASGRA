using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using System.Threading;
using System.Runtime.InteropServices;

public class Generator : MonoBehaviour {

	public GameObject grass; 
	public GameObject cows ; 
	public GameObject vago ; 
	public GameObject excel; 
	public GameObject milkRoom;

	GameObject[] pastos;
	GameObject[] wandes;

	public Material[] mat01;

	float spawnSpeed;
	public int worldWidth;
	int alto;
	int medio;
	int bajo;
	float Of_alto;
	float Of_medio;
	float Of_bajo;
	int presion;
	int min;
	float MStotal;

	float T_max0;
	float T_min0;
	float L_max0;
	float L_min0;

	float ordeno;
	int Total_vacas; 

	float startime;
	float t;
	float hora;

	bool show;
	bool show2;
	bool show3;

	float elapsed_;
	int count;

	float[,] clima0;

	public GUISkin myGUISkin;
	public float speed;
 
	void Start(){
		
		spawnSpeed = 0.01f;
		speed = 1f;

		show  = true;
		show3 = false;
		show2 = false;

		alto  = 11;
		medio = 62;
		bajo  = 27;  
		Of_alto = 365;
		Of_medio= 203;
		Of_bajo = 121;
		presion  = 50;
		T_max0 = 35f;
		T_min0 = 15f;
		L_max0 = 35f;
		L_min0 = 15f;
		min  = 14;
		hora = 0;

		count = 1;

		Application.runInBackground = true;
		worldWidth = 50;
		StartCoroutine (CreateWorld());

	}



	void area(int area_){
		worldWidth = (int)Mathf.Sqrt(area_);
		StartCoroutine (CreateWorld());
	}


	void Excel(){
		GameObject excel_ = Instantiate(excel);
		excel_.transform.position = new Vector3(0, 0, 0);
	}


	void Update () {

	if(hora <= 25 && show3 == true){
		//Send data every hour
		elapsed_ += Time.deltaTime;
		if (elapsed_ >= 60) {	

			//Send All Intakes to javascript page
			GameObject[] allCows = GameObject.FindGameObjectsWithTag ("cow");
			float[] allIntakes  = new float[allCows.Length];
			float[] allIntakes_ = new float[allCows.Length];
			float[] allgraTime  = new float[allCows.Length];

			for(int x = 0; x<allCows.Length; x++) {
				allIntakes[x] = allCows[x].GetComponent<AICharacter>().dmi;
				allIntakes_[x] = allCows[x].GetComponent<AICharacter>().dmi_;
				allgraTime[x] = allCows[x].GetComponent<AICharacter>().grazingTime;
			}

			Application.ExternalCall ("Simulate", allIntakes, allgraTime, count, allIntakes_);

			for(int x = 0; x<allCows.Length; x++) {
					allCows [x].GetComponent<AICharacter> ().stored ();
				}

			elapsed_ = 0;
			count = count + 1;
			if(count == 9){
				PM_();//Call future PM() start
				}
		}

	}       
     
		//deleted all cows
		if(hora > 25){
			GameObject[] allCows = GameObject.FindGameObjectsWithTag ("cow");
			for(int xx = 0; xx<allCows.Length; xx++) {
				Destroy (allCows [xx]);
			}
		}


	}


	void OnGUI () {
	
		t = Time.time-startime;
		hora = t/60;

		GUI.skin = myGUISkin;

		GUILayout.BeginArea (new Rect (0,0,190, Screen.height));
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

		if(show == true){
		GUI.Box(new Rect(5, 10, 180, 50), "Area (m2)");
		GUI.Label (new Rect (10, 35, 100, 30), "Total: " + (worldWidth*worldWidth).ToString());                      

		GUI.Box(new Rect(5, 60, 180, 130), "Densidades del pasto");
		alto = (int)GUI.HorizontalSlider  (new Rect (30, 95, 100-(medio+bajo), 30), alto, 0, 100-(medio+bajo));
		GUI.Label (new Rect (40, 105, 100, 30), "Alta: " + alto.ToString() + " %" );
		medio = (int)GUI.HorizontalSlider  (new Rect (30, 125, 100-(alto+bajo), 30), medio, 0, 100-(alto+bajo));
		GUI.Label (new Rect (40, 135, 100, 30), "Media: " + medio.ToString() + " %" );
		bajo = (int)GUI.HorizontalSlider  (new Rect (30, 155, 100-(medio+alto), 30), bajo, 0, 100-(medio+alto));
		GUI.Label (new Rect (40, 165, 100, 30), "Baja: " + bajo.ToString() + " %" );
					

		GUI.Box(new Rect(5, 190, 180, 130), "Oferta de forraje");
		Of_alto = (int)GUI.HorizontalSlider  (new Rect (30, 225, 100, 30), Of_alto, 0, 500);
		GUI.Label (new Rect (40, 235, 100, 30), "Alta: " + Of_alto.ToString() + " g/m2" );
		Of_medio = (int)GUI.HorizontalSlider  (new Rect (30, 255, 100, 30), Of_medio, 0, 300);
		GUI.Label (new Rect (40, 265, 100, 30), "Media: " + Of_medio.ToString() + " g/m2" );
		Of_bajo = (int)GUI.HorizontalSlider  (new Rect (30, 285, 100, 30), Of_bajo, 0, 200);
		GUI.Label (new Rect (40, 295, 100, 30), "Baja: " + Of_bajo.ToString() + " g/m2" );
	

		if (GUI.Button(new Rect(5,340,180,70), "DISTRIBUIR PASTO")){
				GameObject[] targ_ = GameObject.FindGameObjectsWithTag ("vago");
				for(int z=0; z< targ_.Length; z++){
					Destroy(targ_[z]);
				}

				StartCoroutine (tags());
				repintar();	
				show = false;
				Application.ExternalCall ("mostrarOcultarTablas1", "tabla2");
			}

		}

		if (show == false && show2 == false) {

			GUI.Box(new Rect(5, 10, 180, 50), "Oferta de pasto");
			GUI.Label (new Rect (40, 35, 100, 30), (MStotal/1000*2).ToString("f1") + " Kilos MS" ); 

			GUI.Box (new Rect (5, 60, 180, 55), "Presión de pastoreo");
			presion = (int)GUI.HorizontalSlider (new Rect (10, 85, 150, 30), presion, 40, 90);
			GUI.Label (new Rect (10, 95, 100, 30), presion.ToString () + " % utilización");

			GUI.Box (new Rect (5, 115, 180, 60), "Consumo de pasto esperado");
			min = (int)GUI.HorizontalSlider (new Rect (10, 140, 150, 30), min, 1, 20);
			GUI.Label (new Rect (10, 150, 100, 30), min.ToString () + " Kg MS/vaca");  

			GUI.Box(new Rect(5, 175, 180, 50), "Numero de vacas");
			Total_vacas = (int)((MStotal * presion / 100) / (min * 1000)) * 2;
			GUI.Label (new Rect (80, 200, 100, 30), Total_vacas.ToString() ); 

			GUI.Box (new Rect (5, 225, 180, 100), "Tasas de consumo de pasto");
			GUI.Label (new Rect (10, 255, 120, 30), "Max: " + T_max0.ToString () + " g MS/min"); 
			T_max0 = (int)GUI.HorizontalSlider (new Rect (10, 275, 150, 30), T_max0, 25, 50);
			GUI.Label (new Rect (10, 290, 120, 30), "Min: " + T_min0.ToString () + " g MS/min"); 
			T_min0 = (int)GUI.HorizontalSlider (new Rect (10, 310, 150, 30), T_min0, 1, 25);

			GUI.Box (new Rect (5, 325, 180, 95), "Producción de leche");
			GUI.Label (new Rect (10, 350, 100, 30), "Max: " + L_max0.ToString () + " L/dia"); 
			L_max0 = (int)GUI.HorizontalSlider (new Rect (10, 365, 150, 30), L_max0, 20, 50);
			GUI.Label (new Rect (10, 380, 100, 30), "Min: " + L_min0.ToString () + " L/dia"); 
			L_min0 = (int)GUI.HorizontalSlider (new Rect (10, 395, 150, 30), L_min0, 5, 20);

			if (GUI.Button(new Rect(5,440,180,70), "GENERAR VACAS")){
				StartCoroutine (CreateCows());
				show2 = true;
				show3 = true;
			}

		}

   
		if(show2==true){
			
			GUI.Box(new Rect(10, 10, 100, 50), "HORA:");
			GUI.Label(new Rect (40, 30, 80, 30), hora.ToString("f1") ); 

			GUI.Box(new Rect(10, 60, 110, 50), "Velocidad");
			speed = GUI.HorizontalSlider(new Rect(10, 90, 100, 20), speed, 1, 10);    
			Time.timeScale = speed;  

			GameObject[] vacas_ = GameObject.FindGameObjectsWithTag("cow");
			float itALL = 0;
			float dmiALL = 0;
			float distALL = 0;
			for(int x =0; x< vacas_.Length; x++) {
				itALL 	= itALL + vacas_[x].GetComponent<AICharacter>().grazingTime_; 
				dmiALL 	= dmiALL + vacas_[x].GetComponent<AICharacter>().dmi_; 
				distALL = distALL + vacas_[x].GetComponent<AICharacter>().distance_; 
			}

			GameObject[] pastos_t  = GameObject.FindGameObjectsWithTag ("grass");
			float total_grass = 0;
			for (int i =0; i < pastos_t.Length; i++){
				total_grass = total_grass + pastos_t[i].gameObject.GetComponent<BIOMASS> ().MS;
			}


			GUI.Box  (new Rect(10,  110, 130, 50), "Tiempo de Consumo");
			GUI.Label(new Rect(40,  130, 130, 30), (itALL/vacas_.Length).ToString("f1") + " minutos"); 
			GUI.Box  (new Rect(10,  160, 130, 50), "Consumo");
			GUI.Label(new Rect(40, 180, 130, 30), (dmiALL/1000/vacas_.Length).ToString("f1") + " Kg MS"); 
			//GUI.Box  (new Rect(50, 130, 130, 50), "Recorrido");
			//GUI.Label(new Rect(80, 155, 130, 30), (distALL/vacas_.Length).ToString("f1") + " metros"); 
			GUI.Box  (new Rect(10, 210, 130, 50), "Numero de Heces");
			GameObject[] heces_t  = GameObject.FindGameObjectsWithTag ("heces");
			GUI.Label(new Rect(40, 230, 130, 30), heces_t.Length.ToString()); 

		} 

		GUILayout.EndArea ();

		//if (GUI.Button(new Rect(300,690,180,70), "RESET")){
		//	SceneManager.LoadScene("01");
		//}
}

	IEnumerator CreateWorld() {
		
	for(int x =0; x<worldWidth; x++) {
			yield return new WaitForSeconds(spawnSpeed);
			Instantiate(grass,new Vector3(this.transform.position.x + x, 0, this.transform.position.z),Quaternion.identity);
		}
	}



	IEnumerator tags() { 
		pastos = GameObject.FindGameObjectsWithTag("grass_");
		for(int x = 0; x<pastos.Length; x++) {
			if(pastos[x].transform.position.x <= worldWidth/2){
				pastos[x].tag="grass";
				pastos[x].layer = 9;
			}
		}

		float vagos = Random.Range(2f,5f);
		for(int y =0; y< vagos; y++) {
			yield return new WaitForSeconds(spawnSpeed);
			Instantiate(vago, new Vector3(Random.Range(0, worldWidth/2), 0, Random.Range(0, worldWidth/2)),Quaternion.identity);
		}
	}

	void tagsPM() { 
		pastos = GameObject.FindGameObjectsWithTag("grass_");
		for(int x = 0; x<pastos.Length; x++){
			pastos[x].tag="grass"; 
			pastos[x].layer = 9;   
		}
	}


	void PM_() { 

			GameObject[] targ_0 = GameObject.FindGameObjectsWithTag ("vago");
			for(int i =0; i<targ_0.Length; i++){
				targ_0 [i].GetComponent<Vagar> ().pre_gr = true;
			}

			GameObject[] targ_1 = GameObject.FindGameObjectsWithTag ("cow");			
			for(int i =0; i<targ_1.Length; i++){
				targ_1 [i].GetComponent<Seek> ().seekTarget = milkRoom;
			}

			StartCoroutine (CowsPM());

	}

	void repintar() {    

		pastos = GameObject.FindGameObjectsWithTag("grass");
		MStotal = 0.0f;

		for(int x = 0; x<pastos.Length; x++) {

			if(pastos[x].GetComponent<BIOMASS> ().PM == true){

				float aleatorio = Random.Range(0.0f, 100.0f);
				byte R;
				byte G;
				byte B;

				//Alto
				if(aleatorio <= alto){  
					R = 50;
					G = 200;
					B = 0;   
					pastos[x].name =  "Alto";  
					pastos[x].GetComponent<BIOMASS>().R =  R;    
					pastos[x].GetComponent<BIOMASS>().G =  G; 
					pastos[x].GetComponent<BIOMASS>().B =  B;     
					pastos[x].GetComponent<Renderer>().material.color = new Color32(R, G, B, 255);
					pastos[x].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
					pastos[x].GetComponent<BIOMASS>().PM = false;             
					pastos[x].GetComponent<BIOMASS>().MS = Random.Range(Of_alto - (Of_alto * 0.2f), Of_alto + (Of_alto * 0.2f));   
					MStotal = MStotal + pastos[x].GetComponent<BIOMASS>().MS;


				//Bajo     
				}else if (aleatorio >= alto+medio){
					R = 150;
					G = 200;
					B = 0;   
					pastos[x].name =  "Bajo";  
					pastos[x].GetComponent<BIOMASS>().R =  R;    
					pastos[x].GetComponent<BIOMASS>().G =  G; 
					pastos[x].GetComponent<BIOMASS>().B =  B; 
					pastos[x].GetComponent<Renderer>().material.color = new Color32(R, G, B, 255);
					pastos[x].transform.localScale = new Vector3(0.5f, 0.25f, 0.5f);
					pastos[x].GetComponent<BIOMASS>().PM = false; 
					pastos[x].GetComponent<BIOMASS>().MS = Random.Range (Of_bajo - (Of_bajo * 0.2f), Of_bajo + (Of_bajo * 0.2f));
					MStotal = MStotal + pastos[x].GetComponent<BIOMASS>().MS;

				//Medio      
				}else{
					R = 100;
					G = 150;
					B = 50;   
					pastos[x].name =  "Medio";  
					pastos[x].GetComponent<BIOMASS>().R =  R;    
					pastos[x].GetComponent<BIOMASS>().G =  G; 
					pastos[x].GetComponent<BIOMASS>().B =  B; 
					pastos[x].GetComponent<Renderer>().material.color = new Color32(R, G, B, 255);
					pastos[x].transform.localScale = new Vector3(0.5f, 0.4f, 0.5f);
					pastos[x].GetComponent<BIOMASS>().PM = false; 
					pastos[x].GetComponent<BIOMASS>().MS = Random.Range(Of_medio - (Of_medio * 0.2f), Of_medio + (Of_medio * 0.2f));
					MStotal = MStotal + pastos[x].GetComponent<BIOMASS>().MS;
				}

			}

		}

	}


	IEnumerator CreateCows() {

		Invoke("Excel", 1445);

		wandes = GameObject.FindGameObjectsWithTag("vago");
		startime = Time.time;
		GameObject[] pastos_mass  = GameObject.FindGameObjectsWithTag ("grass");

		float selecMax = 0;
		float selecMin = 0;
		if (pastos_mass.Length > 0) {
			float ms_ = pastos_mass[0].gameObject.GetComponent<BIOMASS> ().MS;
		
		for (int i = 1; i < pastos_mass.Length; i++) {
				float ms_0 = pastos_mass [i].gameObject.GetComponent<BIOMASS> ().MS;
				if (ms_0 > ms_) {
					ms_ = ms_0;
					selecMax = ms_;
				}
				if (ms_0 < ms_) {
					ms_ = ms_0;
					selecMin = ms_;
				}

			}
		}


		for(int i = 0; i < Total_vacas; i++){

			GameObject block9 = Instantiate(cows);
			block9.transform.position = new Vector3(-10, 0, -10);
			int aleatorio = (int)Random.Range(0, wandes.Length);
			block9.GetComponent<Seek>().seekTarget = wandes[aleatorio];
			block9.GetComponent<AICharacter>().start_g = MStotal;
			block9.GetComponent<AICharacter>().startime = startime;

			block9.GetComponent<AICharacter>().T_max = T_max0;
			block9.GetComponent<AICharacter>().T_min = T_min0;
			block9.GetComponent<AICharacter>().F_max = selecMax;
			block9.GetComponent<AICharacter>().F_min = selecMin;

			ordeno = Random.Range(L_min0,L_max0);
			block9.GetComponent<AICharacter>().leche = ordeno;

			int rand_Text = Random.Range (0, mat01.Length-1);
			block9.GetComponentInChildren<SkinnedMeshRenderer>().material = mat01 [rand_Text];

			//Out from milkroom
			if (i == Total_vacas -1) {	
				GameObject[] vagos_ = GameObject.FindGameObjectsWithTag("vago");
				for(int j = 0; j< vagos_.Length; j++ ){
					vagos_ [j].GetComponent<Vagar>().pre_gr = false;					
				}
			} else {
				yield return new WaitForSeconds ((ordeno * 0.6f) / 4);
			}

			block9.GetComponent<AICharacter> ().dmi_  = 1000*(ordeno * 0.6f)/4; //1 Kg[] /4 Liters 
			block9.GetComponent<AICharacter> ().dmi   = 1000*(ordeno * 0.6f)/4;

			block9.GetComponent<AICharacter> ().grazingTime = (int)(ordeno * 0.6f);
			block9.GetComponent<AICharacter> ().grazingTime_= (int)(ordeno * 0.6f);


		}

	}



	IEnumerator CowsPM() {

		GameObject[] targ_2 = GameObject.FindGameObjectsWithTag ("cow");
		for (int i = 0; i < targ_2.Length; i++) {

			int aleatorio = (int)Random.Range (0, wandes.Length);
			targ_2 [i].GetComponent<Seek> ().seekTarget = wandes [aleatorio];

			ordeno = targ_2 [i].GetComponent<AICharacter>().leche * 0.4f; //PM milking

			if (i == targ_2.Length -1) {	
				tagsPM();
				repintar();
				for(int j =0; j<wandes.Length; j++){
					wandes[j].GetComponent<Vagar> ().pre_gr = false;
				}

			} else {
				yield return new WaitForSeconds (ordeno / 4);
			}

			float dmi_temp  = targ_2 [i].GetComponent<AICharacter> ().dmi_; 
			float dmi_0temp = targ_2 [i].GetComponent<AICharacter> ().dmi;
			targ_2 [i].GetComponent<AICharacter> ().dmi_= dmi_temp +  (1000*(ordeno/4)); //1000 g[] / 4 Liters 
			targ_2 [i].GetComponent<AICharacter> ().dmi = dmi_0temp + (1000*(ordeno/4));

			int grazingtemp = targ_2 [i].GetComponent<AICharacter> ().grazingTime; 
			int grazingtemp_ = targ_2 [i].GetComponent<AICharacter> ().grazingTime_;
			targ_2 [i].GetComponent<AICharacter> ().grazingTime = grazingtemp  + (int)(ordeno);
			targ_2 [i].GetComponent<AICharacter> ().grazingTime_= grazingtemp_ + (int)(ordeno);


		}

		yield return new WaitForSeconds (120);
	}


	public void clima(string clima_){

		string[] res;
		string stringToEdit = "";
		stringToEdit = clima_;

		res = stringToEdit.Split(","[0]);
		clima0 = new float[res.Length/7 - 1,7];

		int count1 = 7;
		for(int i = 0; i < res.Length/7 - 1; i++){        
			for(int j = 0; j < 7; j++){
				clima0[i,j] = float.Parse(res[count1]);
				count1= count1 + 1;

			}
		}

		//Pasto Alto
		GetComponent<BasgraModel>().StartBASGRA (clima0, "Alt");
		Of_alto = this.GetComponent<BasgraModel> ().DM_;
		Debug.Log (Of_alto);

		//Pasto Medio
		GetComponent<BasgraModel>().StartBASGRA (clima0, "Med");
		Of_medio = this.GetComponent<BasgraModel> ().DM_;
		Debug.Log (Of_medio);

		//Pasto Bajo
		GetComponent<BasgraModel>().StartBASGRA (clima0, "Baj");
		Of_bajo = this.GetComponent<BasgraModel> ().DM_;
		Debug.Log (Of_bajo);


}
    

}
