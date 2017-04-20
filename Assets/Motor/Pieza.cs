using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour {
	
	Celda[,,] celdas;
	TipoPieza tipo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void crearPieza(TipoPieza tipo, bool[,,] existencia, byte[,,] metadata){
		this.tipo = tipo;
		for (int x = 0; x < existencia.GetLength (0); x++) {
			for (int y = 0; y < existencia.GetLength (1); y++) {
				for (int z = 0; z < existencia.GetLength (2); z++) {
					if (existencia [x, y, z]) {
						bool[,,] mapaVecinos = getMapaVecinos(existencia, x, y, z);

						Celda celda = CreadorCeldas.instancia.crearCelda(tipo, mapaVecinos, metadata[x,y,z]);
						celda.transform.parent = this.transform;
						celda.transform.localPosition = new Vector3 (x, y, z);
					}
				}
			}
		}
	}

	bool[,,] getMapaVecinos(bool[,,] existencia, int x, int y, int z){
		bool[,,] retorno = new bool[3,3,3];
		return null;
		//TODO
	}
}