using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreadorCeldas : MonoBehaviour {

	public Celda celdaTerreno1;
	public Celda celdaTerreno2;
	public Celda celdaMadera;
	public Celda celdaMetal;
	public Celda celdaError;
	private enum TipoEsquinaCara {BORDE_ESQUINERO, BORDE_HORIZ, BORDE_VERT, ESQUINA, NADA};
	public Celda crearCelda(TipoPieza tipoPieza, bool[,,] mapaVecinos, byte metadata){
		switch (tipoPieza) {
			case TipoPieza.TERRENO:
				return crearCeldaTerreno (mapaVecinos, metadata);
			case TipoPieza.MADERA:
				return crearCeldaMadera (mapaVecinos, metadata);
			case TipoPieza.METAL:
				return crearCeldaMetal (mapaVecinos, metadata);
			default:
				return Instantiate(celdaError);
		}
	}

	Celda crearCeldaTerreno(bool[,,] mapaVecinos, byte metadata){
		Celda resultado;
		switch (metadata) {
		case 0:
			resultado = getCeldaPartePieza (celdaTerreno1, true, mapaVecinos);
			break;
		case 1:
			resultado = getCeldaPartePieza (celdaTerreno1, true, mapaVecinos);
			break;
		default:
			resultado = null;
			break;
		}
		return resultado;
	}


	Celda crearCeldaMadera(bool[,,] mapaVecinos, byte metadata){
		Celda resultado = getCeldaPartePieza (celdaMadera, false, mapaVecinos);
		return resultado;

	}

	Celda crearCeldaMetal(bool[,,] mapaVecinos, byte metadata){
		Celda resultado = getCeldaPartePieza (celdaMetal, false, mapaVecinos);
		return resultado;

	}

	Celda getCeldaPartePieza (Celda celda, bool esSimple, bool[,,] mapaVecinos)
	{
		Celda resultado = Instantiate (celda);
		Mesh modelo3D = new Mesh ();
		int[] triangulos;
		Vector3[] vertices;
		Vector2[] uvs;
		byte i = 0;
		byte t = 0;

		Vector3 negXnegYnegZ = new Vector3 (-0.5F, -0.5F, -0.5F);
		Vector3 negXnegYposZ = new Vector3 (-0.5F, -0.5F, 0.5F);
		Vector3 negXposYnegZ = new Vector3 (-0.5F, 0.5F, -0.5F);
		Vector3 negXposYposZ = new Vector3 (-0.5F, 0.5F, 0.5F);
		Vector3 posXnegYnegZ = new Vector3 (0.5F, -0.5F, -0.5F);
		Vector3 posXnegYposZ = new Vector3 (0.5F, -0.5F, 0.5F);
		Vector3 posXposYnegZ = new Vector3 (0.5F, 0.5F, -0.5F);
		Vector3 posXposYposZ = new Vector3 (0.5F, 0.5F, 0.5F);

		if (esSimple) {
			triangulos = new int[36];
			vertices = new Vector3[24];
			uvs = new Vector2[24];

			if (!mapaVecinos [2, 1, 1]) { //No existe parte vecina en X positivo
				dibujarCara (posXposYnegZ, posXposYposZ, posXnegYnegZ, posXnegYposZ, ref i, ref t, ref triangulos, ref vertices, ref uvs);
			}
			if (!mapaVecinos [0, 1, 1]) { //No existe parte vecina en X negativo
				dibujarCara (negXposYposZ, negXposYnegZ, negXnegYposZ, negXnegYnegZ, ref i, ref t, ref triangulos, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 2, 1]) { //No existe parte vecina en Y positivo
				dibujarCara (negXposYposZ, posXposYposZ, negXposYnegZ, posXposYnegZ, ref i, ref t, ref triangulos, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 0, 1]) { //No existe parte vecina en Y negativo
				dibujarCara (negXnegYnegZ, posXnegYnegZ, negXnegYposZ, posXnegYposZ, ref i, ref t, ref triangulos, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 1, 2]) { //No existe parte vecina en Z positivo
				dibujarCara (posXposYposZ, negXposYposZ, posXnegYposZ, negXnegYposZ, ref i, ref t, ref triangulos, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 1, 0]) { //No existe parte vecina en Z negativo
				dibujarCara (negXposYnegZ, posXposYnegZ, negXnegYnegZ, posXnegYnegZ, ref i, ref t, ref triangulos, ref vertices, ref uvs);
			}

			int[] trimTriangulos = new int[t * 3];
			Vector3[] trimVertices = new Vector3[i];
			Vector2[] trimUvs = new Vector2[i];
			Array.Copy (triangulos, trimTriangulos, trimTriangulos.Length);
			Array.Copy (vertices, trimVertices, trimVertices.Length);
			Array.Copy (uvs, trimUvs, trimUvs.Length);
			modelo3D.vertices = trimVertices;
			modelo3D.uv = trimUvs;
			modelo3D.triangles = trimTriangulos;

		} else {

			triangulos = new int[144];
			int[] triangulos2 = new int[144];
			int[] triangulos3 = new int[144];
			int[] triangulos4 = new int[144];
			int[] triangulos5 = new int[144];

			vertices = new Vector3[96];
			uvs = new Vector2[96];

			byte t2 = 0;
			byte t3 = 0;
			byte t4 = 0;
			byte t5 = 0;
			if (!mapaVecinos [2, 1, 1]) { //No existe parte vecina en X positivo
				dibujarCaraCompleja (posXposYnegZ, posXposYposZ, posXnegYnegZ, posXnegYposZ,
					mapaVecinos[2,2,0] || !mapaVecinos[1,2,0],
					mapaVecinos[2,2,2] || !mapaVecinos[1,2,2],
					mapaVecinos[2,0,0] || !mapaVecinos[1,0,0],
					mapaVecinos[2,0,2] || !mapaVecinos[1,0,2],
					mapaVecinos[2,2,1] || !mapaVecinos[1,2,1],
					mapaVecinos[2,1,2] || !mapaVecinos[1,1,2],
					mapaVecinos[2,0,1] || !mapaVecinos[1,0,1],
					mapaVecinos[2,1,0] || !mapaVecinos[1,1,0],
					ref i, ref t, ref t2, ref t3, ref t4, ref t5,
					ref triangulos, ref triangulos2, ref triangulos3, ref triangulos4, ref triangulos5, ref vertices, ref uvs);
			}
			if (!mapaVecinos [0, 1, 1]) { //No existe parte vecina en X negativo
				dibujarCaraCompleja (negXposYposZ, negXposYnegZ, negXnegYposZ, negXnegYnegZ,
					mapaVecinos[0,2,2] || !mapaVecinos[1,2,2],
					mapaVecinos[0,2,0] || !mapaVecinos[1,2,0],
					mapaVecinos[0,0,2] || !mapaVecinos[1,0,2],
					mapaVecinos[0,0,0] || !mapaVecinos[1,0,0],
					mapaVecinos[0,2,1] || !mapaVecinos[1,2,1],
					mapaVecinos[0,1,0] || !mapaVecinos[1,1,0],
					mapaVecinos[0,0,1] || !mapaVecinos[1,0,1],
					mapaVecinos[0,1,2] || !mapaVecinos[1,1,2],
					ref i, ref t, ref t2, ref t3, ref t4, ref t5,
					ref triangulos, ref triangulos2, ref triangulos3, ref triangulos4, ref triangulos5, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 2, 1]) { //No existe parte vecina en Y positivo
				dibujarCaraCompleja (negXposYposZ, posXposYposZ, negXposYnegZ, posXposYnegZ,
					mapaVecinos[0,2,2] || !mapaVecinos[0,1,2],
					mapaVecinos[2,2,2] || !mapaVecinos[2,1,2],
					mapaVecinos[0,2,0] || !mapaVecinos[0,1,0],
					mapaVecinos[2,2,0] || !mapaVecinos[2,1,0],
					mapaVecinos[1,2,2] || !mapaVecinos[1,1,2],
					mapaVecinos[2,2,1] || !mapaVecinos[2,1,1],
					mapaVecinos[1,2,0] || !mapaVecinos[1,1,0],
					mapaVecinos[0,2,1] || !mapaVecinos[0,1,1],
					ref i, ref t, ref t2, ref t3, ref t4, ref t5,
					ref triangulos, ref triangulos2, ref triangulos3, ref triangulos4, ref triangulos5, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 0, 1]) { //No existe parte vecina en Y negativo
				dibujarCaraCompleja (negXnegYnegZ, posXnegYnegZ, negXnegYposZ, posXnegYposZ,
					mapaVecinos[0,0,0] || !mapaVecinos[0,1,0],
					mapaVecinos[2,0,0] || !mapaVecinos[2,1,0],
					mapaVecinos[0,0,2] || !mapaVecinos[0,1,2],
					mapaVecinos[2,0,2] || !mapaVecinos[2,1,2],
					mapaVecinos[1,0,0] || !mapaVecinos[1,1,0],
					mapaVecinos[2,0,1] || !mapaVecinos[2,1,1],
					mapaVecinos[1,0,2] || !mapaVecinos[1,1,2],
					mapaVecinos[0,0,1] || !mapaVecinos[0,1,1],
					ref i, ref t, ref t2, ref t3, ref t4, ref t5,
					ref triangulos, ref triangulos2, ref triangulos3, ref triangulos4, ref triangulos5, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 1, 2]) { //No existe parte vecina en Z positivo
				dibujarCaraCompleja (posXposYposZ, negXposYposZ, posXnegYposZ, negXnegYposZ,
					mapaVecinos[2,2,2] || !mapaVecinos[2,2,1],
					mapaVecinos[0,2,2] || !mapaVecinos[0,2,1],
					mapaVecinos[2,0,2] || !mapaVecinos[2,0,1],
					mapaVecinos[0,0,2] || !mapaVecinos[0,0,1],
					mapaVecinos[1,2,2] || !mapaVecinos[1,2,1],
					mapaVecinos[0,1,2] || !mapaVecinos[0,1,1],
					mapaVecinos[1,0,2] || !mapaVecinos[1,0,1],
					mapaVecinos[2,1,2] || !mapaVecinos[2,1,1],
					ref i, ref t, ref t2, ref t3, ref t4, ref t5,
					ref triangulos, ref triangulos2, ref triangulos3, ref triangulos4, ref triangulos5, ref vertices, ref uvs);
			}
			if (!mapaVecinos [1, 1, 0]) { //No existe parte vecina en Z negativo
				dibujarCaraCompleja (negXposYnegZ, posXposYnegZ, negXnegYnegZ, posXnegYnegZ,
					mapaVecinos[0,2,0] || !mapaVecinos[0,2,1],
					mapaVecinos[2,2,0] || !mapaVecinos[2,2,1],
					mapaVecinos[0,0,0] || !mapaVecinos[0,0,1],
					mapaVecinos[2,0,0] || !mapaVecinos[2,0,1],
					mapaVecinos[1,2,0] || !mapaVecinos[1,2,1],
					mapaVecinos[2,1,0] || !mapaVecinos[2,1,1],
					mapaVecinos[1,0,0] || !mapaVecinos[1,0,1],
					mapaVecinos[0,1,0] || !mapaVecinos[0,1,1],
					ref i, ref t, ref t2, ref t3, ref t4, ref t5,
					ref triangulos, ref triangulos2, ref triangulos3, ref triangulos4, ref triangulos5, ref vertices, ref uvs);
			}

			int[] trimTriangulos = new int[t * 3];
			int[] trimTriangulos2 = new int[t2 * 3];
			int[] trimTriangulos3 = new int[t3 * 3];
			int[] trimTriangulos4 = new int[t4 * 3];
			int[] trimTriangulos5 = new int[t5 * 3];

			Vector3[] trimVertices = new Vector3[i];
			Vector2[] trimUvs = new Vector2[i];

			Array.Copy (triangulos, trimTriangulos, trimTriangulos.Length);
			Array.Copy (triangulos2, trimTriangulos2, trimTriangulos2.Length);
			Array.Copy (triangulos3, trimTriangulos3, trimTriangulos3.Length);
			Array.Copy (triangulos4, trimTriangulos4, trimTriangulos4.Length);
			Array.Copy (triangulos5, trimTriangulos5, trimTriangulos5.Length);

			Array.Copy (vertices, trimVertices, trimVertices.Length);
			Array.Copy (uvs, trimUvs, trimUvs.Length);

			modelo3D.vertices = trimVertices;
			modelo3D.uv = trimUvs;

			Material[] matsBase = celda.GetComponent<MeshRenderer> ().materials;
			int[] indiceMatsUsados = new int[5];
			int[][] triangArr = {trimTriangulos, trimTriangulos2, trimTriangulos3, trimTriangulos4, trimTriangulos5};

			int subMeshes = 0;

			for (int j = 0; j < 5; j++) {
				if (triangArr [j].Length > 0) {
					indiceMatsUsados [subMeshes] = j;
					subMeshes++;
				}
			}

			modelo3D.subMeshCount = subMeshes;
			Material[] matsUsados = new Material[subMeshes];
			for (int j = 0; j < subMeshes; j++) {
				matsUsados [j] = matsBase [indiceMatsUsados [j]];
				modelo3D.SetTriangles (triangArr [indiceMatsUsados [j]], j);
			}
			resultado.GetComponent<MeshRenderer> ().materials = matsUsados;
		}

		modelo3D.RecalculateBounds ();
		modelo3D.RecalculateNormals ();
		modelo3D.RecalculateTangents ();
		resultado.GetComponent<MeshFilter> ().mesh = modelo3D;
		return resultado;
	}

	void dibujarCara (Vector3 arrIzq, Vector3 arrDer, Vector3 abaIzq, Vector3 abaDer,
		ref byte i, ref byte t, ref int[] triangulos, ref Vector3[] vertices, ref Vector2[] uvs)
	{
		vertices [i] = arrIzq;
		vertices [i + 1] = arrDer;
		vertices [i + 2] = abaIzq;
		vertices [i + 3] = abaDer;
		uvs [i] = new Vector2 (0, 1);
		uvs [i + 1] = new Vector2 (1, 1);
		uvs [i + 2] = new Vector2 (0, 0);
		uvs [i + 3] = new Vector2 (1, 0);
		triangulos [t * 3] = i;
		triangulos [t * 3 + 1] = i + 1;
		triangulos [t * 3 + 2] = i + 3;
		t++;
		triangulos [t * 3] = i;
		triangulos [t * 3 + 1] = i + 3;
		triangulos [t * 3 + 2] = i + 2;
		t++;
		i += 4;
	}

	void dibujarCaraCompleja(Vector3 arrIzq, Vector3 arrDer, Vector3 abaIzq, Vector3 abaDer, 
		bool esqArrIzq, bool esqArrDer, bool esqAbaIzq, bool esqAbaDer,
		bool bordeArr, bool bordeDer, bool bordeAba, bool bordeIzq,
		ref byte i, ref byte t, ref byte t2, ref byte t3, ref byte t4, ref byte t5,
		ref int[] triangulos, ref int[] triangulos2, ref int[] triangulos3, ref int[] triangulos4, ref int[] triangulos5,
		ref Vector3[] vertices, ref Vector2[] uvs){

		int materialArrIzq = getMaterialEsquina(bordeArr, bordeIzq, esqArrIzq);
		int materialArrDer = getMaterialEsquina(bordeArr, bordeDer, esqArrDer);
		int materialAbaIzq = getMaterialEsquina(bordeAba, bordeIzq, esqAbaIzq);
		int materialAbaDer = getMaterialEsquina(bordeAba, bordeDer, esqAbaDer);

		Vector3 arrCen = (arrIzq + arrDer) / 2;
		Vector3 abaCen = (abaIzq + abaDer) / 2;
		Vector3 cenDer = (arrDer + abaDer) / 2;
		Vector3 cenIzq = (arrIzq + abaIzq) / 2;
		Vector3 cenCen = (arrCen + abaCen) / 2;

		switch (materialArrIzq) {
		case 0:
			dibujarCuartoCara (arrIzq, arrCen, cenIzq, cenCen, ref i, ref t, ref triangulos, ref vertices, ref uvs, 0);
			break;
		case 1:
			dibujarCuartoCara (arrIzq, arrCen, cenIzq, cenCen, ref i, ref t2, ref triangulos2, ref vertices, ref uvs, 0);
			break;
		case 2:
			dibujarCuartoCara (arrIzq, arrCen, cenIzq, cenCen, ref i, ref t3, ref triangulos3, ref vertices, ref uvs, 0);
			break;
		case 3:
			dibujarCuartoCara (arrIzq, arrCen, cenIzq, cenCen, ref i, ref t4, ref triangulos4, ref vertices, ref uvs, 0);
			break;
		case 4:
			dibujarCuartoCara (arrIzq, arrCen, cenIzq, cenCen, ref i, ref t5, ref triangulos5, ref vertices, ref uvs, 0);
			break;
		}

		switch (materialArrDer) {
		case 0:
			dibujarCuartoCara (arrCen, arrDer, cenCen, cenDer, ref i, ref t, ref triangulos, ref vertices, ref uvs, 1);
			break;
		case 1:
			dibujarCuartoCara (arrCen, arrDer, cenCen, cenDer, ref i, ref t2, ref triangulos2, ref vertices, ref uvs, 1);
			break;
		case 2:
			dibujarCuartoCara (arrCen, arrDer, cenCen, cenDer, ref i, ref t3, ref triangulos3, ref vertices, ref uvs, 1);
			break;
		case 3:
			dibujarCuartoCara (arrCen, arrDer, cenCen, cenDer, ref i, ref t4, ref triangulos4, ref vertices, ref uvs, 1);
			break;
		case 4:
			dibujarCuartoCara (arrCen, arrDer, cenCen, cenDer, ref i, ref t5, ref triangulos5, ref vertices, ref uvs, 1);
			break;
		}

		switch (materialAbaIzq) {
		case 0:
			dibujarCuartoCara (cenIzq, cenCen, abaIzq, abaCen, ref i, ref t, ref triangulos, ref vertices, ref uvs, 2);
			break;
		case 1:
			dibujarCuartoCara (cenIzq, cenCen, abaIzq, abaCen, ref i, ref t2, ref triangulos2, ref vertices, ref uvs, 2);
			break;
		case 2:
			dibujarCuartoCara (cenIzq, cenCen, abaIzq, abaCen, ref i, ref t3, ref triangulos3, ref vertices, ref uvs, 2);
			break;
		case 3:
			dibujarCuartoCara (cenIzq, cenCen, abaIzq, abaCen, ref i, ref t4, ref triangulos4, ref vertices, ref uvs, 2);
			break;
		case 4:
			dibujarCuartoCara (cenIzq, cenCen, abaIzq, abaCen, ref i, ref t5, ref triangulos5, ref vertices, ref uvs, 2);
			break;
		}

		switch (materialAbaDer) {
		case 0:
			dibujarCuartoCara (cenCen, cenDer, abaCen, abaDer, ref i, ref t, ref triangulos, ref vertices, ref uvs, 3);
			break;
		case 1:
			dibujarCuartoCara (cenCen, cenDer, abaCen, abaDer, ref i, ref t2, ref triangulos2, ref vertices, ref uvs, 3);
			break;
		case 2:
			dibujarCuartoCara (cenCen, cenDer, abaCen, abaDer, ref i, ref t3, ref triangulos3, ref vertices, ref uvs, 3);
			break;
		case 3:
			dibujarCuartoCara (cenCen, cenDer, abaCen, abaDer, ref i, ref t4, ref triangulos4, ref vertices, ref uvs, 3);
			break;
		case 4:
			dibujarCuartoCara (cenCen, cenDer, abaCen, abaDer, ref i, ref t5, ref triangulos5, ref vertices, ref uvs, 3);
			break;
		}
	}

	void dibujarCuartoCara (Vector3 arrIzq, Vector3 arrDer, Vector3 abaIzq, Vector3 abaDer,
		ref byte i, ref byte t, ref int[] triangulos, ref Vector3[] vertices, ref Vector2[] uvs, byte posicion)
	{
		vertices [i] = arrIzq;
		vertices [i + 1] = arrDer;
		vertices [i + 2] = abaIzq;
		vertices [i + 3] = abaDer;

		switch (posicion) {
		case 0: 
			uvs [i] = new Vector2 (0F, 1F);
			uvs [i + 1] = new Vector2 (0.5F, 1F);
			uvs [i + 2] = new Vector2 (0F, 0.5F);
			uvs [i + 3] = new Vector2 (0.5F, 0.5F);
			break;
		case 1: 
			uvs [i] = new Vector2 (0.5F, 1F);
			uvs [i + 1] = new Vector2 (1F, 1F);
			uvs [i + 2] = new Vector2 (0.5F, 0.5F);
			uvs [i + 3] = new Vector2 (1F, 0.5F);
			break;
		case 2: 
			uvs [i] = new Vector2 (0F, 0.5F);
			uvs [i + 1] = new Vector2 (0.5F, 0.5F);
			uvs [i + 2] = new Vector2 (0F, 0F);
			uvs [i + 3] = new Vector2 (0.5F, 0F);
			break;
		case 3: 
			uvs [i] = new Vector2 (0.5F, 0.5F);
			uvs [i + 1] = new Vector2 (1F, 0.5F);
			uvs [i + 2] = new Vector2 (0.5F, 0F);
			uvs [i + 3] = new Vector2 (1F, 0F);
			break;
		}
		triangulos [t * 3] = i;
		triangulos [t * 3 + 1] = i + 1;
		triangulos [t * 3 + 2] = i + 3;
		t++;
		triangulos [t * 3] = i;
		triangulos [t * 3 + 1] = i + 3;
		triangulos [t * 3 + 2] = i + 2;
		t++;
		i += 4;
	}

	int getMaterialEsquina(bool bordeHoriz, bool bordeVert, bool esquina){
		//Materiales:
		//0 = BORDE ESQUINERO
		//1 = BORDE HORIZONTAL
		//2 = BORDE VERTICAL
		//3 = ESQUINITA
		//4 = SIN BORDES
		if (bordeHoriz) {
			if (bordeVert) {
				return 0;
			} else {
				return 1;
			}
		} else {
			if (bordeVert) {
				return 2;
			} else {
				if (esquina) {
					return 3;
				} else {
					return 4;
				}
			}
		}
	}
}
