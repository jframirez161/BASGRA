using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWrite : MonoBehaviour {

	private List<string[]> rowData = new List<string[]>();
	private GameObject[]   vacas;
	private String[,]   it_hora;
	private String[,]   dmi_hora;

void Start () {

	vacas = GameObject.FindGameObjectsWithTag ("cow");
	it_hora = new String[vacas.Length,24];
	dmi_hora = new String[vacas.Length,24];

	for(int i=0; i<24; i++){

		for(int j=0; j<vacas.Length; j++){
				it_hora[j,i] = vacas[j].GetComponent<AICharacter>().grazingTimeAll[i].ToString();
				dmi_hora[j,i]= vacas[j].GetComponent<AICharacter>().dmi_All[i].ToString();
		}
	}

		SaveIT();
		SaveDMI();
	}

	void SaveIT(){

		// Creating First row of titles manually..
		string[] rowDataTemp = new string[24];
		rowDataTemp[0]  = "06:00_am";
		rowDataTemp[1]  = "07:00_am";
		rowDataTemp[2]  = "08:00_am";
		rowDataTemp[3]  = "09:00_am";
		rowDataTemp[4]  = "10:00_am";
		rowDataTemp[5]  = "11:00_am";
		rowDataTemp[6]  = "12:00_pm";
		rowDataTemp[7]  = "01:00_pm";
		rowDataTemp[8]  = "02:00_pm";
		rowDataTemp[9]  = "03:00_pm";
		rowDataTemp[10] = "04:00_pm";
		rowDataTemp[11] = "05:00_pm";
		rowDataTemp[12] = "06:00_pm";
		rowDataTemp[13] = "07:00_pm";
		rowDataTemp[14] = "08:00_pm";
		rowDataTemp[15] = "09:00_pm";
		rowDataTemp[16] = "10:00_pm";
		rowDataTemp[17] = "11:00_pm";
		rowDataTemp[18] = "12:00_am";
		rowDataTemp[19] = "01:00_am";
		rowDataTemp[20] = "02:00_am";
		rowDataTemp[21] = "03:00_am";
		rowDataTemp[22] = "04:00_am";
		rowDataTemp[23] = "05:00_am";
		rowData.Add(rowDataTemp);


		// You can add up the values in as many cells as you want.
		for(int i = 0; i < vacas.Length; i++){
			
			rowDataTemp = new string[24];

			for(int j = 0; j < 24; j++){
				rowDataTemp[j]  = it_hora[i,j];
			}

			rowData.Add(rowDataTemp);
		}

		string[][] output = new string[rowData.Count][];

		for(int i = 0; i < output.Length; i++){
			output[i] = rowData[i];
		}

		int    length    = output.GetLength(0);
		string delimiter = ";";

		StringBuilder sb = new StringBuilder();

		for (int index = 0; index < length; index++)
			sb.AppendLine(string.Join(delimiter, output[index]));

	}



		void SaveDMI(){

		// Creating First row of titles manually..
		string[] rowDataTemp = new string[24];
		rowDataTemp[0]  = "06:00_am";
		rowDataTemp[1]  = "07:00_am";
		rowDataTemp[2]  = "08:00_am";
		rowDataTemp[3]  = "09:00_am";
		rowDataTemp[4]  = "10:00_am";
		rowDataTemp[5]  = "11:00_am";
		rowDataTemp[6]  = "12:00_pm";
		rowDataTemp[7]  = "01:00_pm";
		rowDataTemp[8]  = "02:00_pm";
		rowDataTemp[9]  = "03:00_pm";
		rowDataTemp[10] = "04:00_pm";
		rowDataTemp[11] = "05:00_pm";
		rowDataTemp[12] = "06:00_pm";
		rowDataTemp[13] = "07:00_pm";
		rowDataTemp[14] = "08:00_pm";
		rowDataTemp[15] = "09:00_pm";
		rowDataTemp[16] = "10:00_pm";
		rowDataTemp[17] = "11:00_pm";
		rowDataTemp[18] = "12:00_am";
		rowDataTemp[19] = "01:00_am";
		rowDataTemp[20] = "02:00_am";
		rowDataTemp[21] = "03:00_am";
		rowDataTemp[22] = "04:00_am";
		rowDataTemp[23] = "05:00_am";
		rowData.Add(rowDataTemp);


		// You can add up the values in as many cells as you want.
		for(int i = 0; i < vacas.Length; i++){

		rowDataTemp = new string[24];

		for(int j = 0; j < 24; j++){
		rowDataTemp[j]  = dmi_hora[i,j];
		}

		rowData.Add(rowDataTemp);
		}

		string[][] output = new string[rowData.Count][];

		for(int i = 0; i < output.Length; i++){
		output[i] = rowData[i];
		}

		int    length    = output.GetLength(0);
		string delimiter = ";";

		StringBuilder sb = new StringBuilder();

		for (int index = 0; index < length; index++)
		sb.AppendLine(string.Join(delimiter, output[index]));

		string filePath_ = getPath_();
		StreamWriter outStream = System.IO.File.CreateText(filePath_);
		outStream.WriteLine(sb);
		outStream.Close();
		}

		// Following method is used to retrive the relative path as device platform
		private string getPath_(){
		#if UNITY_EDITOR
		return Application.dataPath +"/"+"Resultados.txt";
		#elif UNITY_ANDROID
		return Application.persistentDataPath+"Saved_data.csv";
		#elif UNITY_IPHONE
		return Application.persistentDataPath+"/"+"Saved_data.csv";
		#elif UNITY_WEBGL
		return System.IO.Path.Combine(Application.streamingAssetsPath, "Saved_data.csv");
		#else
		return Application.dataPath +"/"+"Saved_data.csv";
		#endif
		}



}
