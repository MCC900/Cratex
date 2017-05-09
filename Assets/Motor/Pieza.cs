using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour {

	public CreadorCeldas creadorCeldas;
	Celda[,,] celdas;
	bool[,,] existencia;
	/*existenciaExt contiene la matriz de existencia (forma de la pieza) rodeada en los 3 ejes por el valor false.
	Una analogía en 2D, si la pieza estuviese contenida en una matriz 4x3,sería: 
	               [[F, F, F, F, F, F],
	                [F, X, X, X, X, F],
	                [F, X, X, X, X, F],
	                [F, X, X, X, X, F],
	                [F, F, F, F, F, F]]
	Donde los valores X representan la matriz de existencia original de la pieza (V o F según la
	pieza exista o no en esa posición). Esto se hace por eficiencia al buscar vecinos inmediatos para
	una locación [x, y, z] concreta (contenidos en una matriz 3x3x3 centrada en x, y, z). */

	TipoPieza tipo;

	// Use this for initialization
	void Start () {
		bool[,,] existencia = { {
				{ true, false, true },
				{ true, true, true },
				{ false, true, true }
			},  {
				{ true, false, true },
				{ true, true, true },
				{ true, false, true }
			}
		};

		byte[,,] metadata = {{{0,0,0},{0,0,0},{0,0,0}},{{0,0,0},{0,0,0},{0,0,0}}};
		crearPieza (TipoPieza.METAL, existencia, metadata);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void crearPieza(TipoPieza tipo, bool[,,] existencia, byte[,,] metadata){
		this.tipo = tipo;
		this.existencia = existencia;
		bool[,,] existenciaExt = rodearMatriz3Bool (existencia);

		for (int x = 0; x < existencia.GetLength (0); x++) {
			for (int y = 0; y < existencia.GetLength (1); y++) {
				for (int z = 0; z < existencia.GetLength (2); z++) {
					if (existencia [x, y, z]) {
						bool[,,] mapaVecinos = getMapaVecinos(existenciaExt, x, y, z);

						Celda celda = creadorCeldas.crearCelda(tipo, mapaVecinos, metadata[x,y,z]);
						celda.transform.parent = this.transform;
						celda.transform.localPosition = new Vector3 (x, y, z);
					}
				}
			}
		}
	}

	/// <summary>
	/// Devuelve una matriz con 2 espacios añadidos en cada dimensión, con valor FALSE en ambos extremos.
	/// Se suele usar para hacer algoritmos dependientes de vecindad en una matriz más eficientes.
	/// </summary>
	/// <returns>La matriz rodeada por FALSE en las 6 direcciones.</returns>
	/// <param name="matriz">Matriz bool de 3 dimensiones</param>
	bool[,,] rodearMatriz3Bool(bool[,,] matriz) {
		int largoExtendidoX = matriz.GetLength (0) + 2;
		int largoExtendidoY = matriz.GetLength (1) + 2;
		int largoExtendidoZ = matriz.GetLength (2) + 2;

		bool[,,] retorno = new bool[largoExtendidoX, largoExtendidoY, largoExtendidoZ];
		for (int x = 0; x < largoExtendidoX-2; x++) {
			for(int y =0; y < largoExtendidoY-2; y++){
				for (int z = 0; z < largoExtendidoZ-2; z++) {
					retorno [x+1, y+1, z+1] = matriz [x, y, z];
				}
			}
		}
		return retorno;
	}

	bool[,,] getMapaVecinos(bool[,,] existenciaExt, int x, int y, int z){
		bool[,,] retorno = new bool[3,3,3];
		for(int xx = 0; xx < 3; xx++){
			for(int yy = 0; yy < 3; yy++){
				for(int zz = 0; zz < 3; zz++){
					retorno [xx, yy, zz] = existenciaExt [x + xx, y + yy, z + zz];
				}
			}
		}
		return retorno;
	}
}