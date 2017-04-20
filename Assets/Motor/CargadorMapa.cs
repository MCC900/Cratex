using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargadorMapas : MonoBehaviour  {

	public MapaNivel mapaVacio;
	public MapaNivel mapaActual;
	public bool cargandoMapa = false;
	public string codigoCargaMapa;

	// Update is called once per frame
	void Update(){
		if (cargandoMapa) {
			seguirCargaMapa ();
		}
	}

	//Comienza a cargar un nuevo mapa. Llamado desde el selector de niveles.
	public void cargarNuevoMapa(string codigoCarga){
		GameObject.Destroy(mapaActual);
		mapaActual = GameObject.Instantiate (mapaVacio);
		codigoCargaMapa = codigoCarga;
		cargandoMapa = true;
		seguirCargaMapa();
	}

	//Retorna true mientras siga cargando el mapa
	public void seguirCargaMapa(){
		//TODO
		return;
	}

	public void destruirMapa(){
		GameObject.Destroy (mapaActual);
	}
}
