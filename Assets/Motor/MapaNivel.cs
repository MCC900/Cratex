using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapaNivel : MonoBehaviour {

	private enum EstadoMapa {CARGANDO, ESPERANDO_JUGADA, EJECUTANDO, ANIM_ENTRADA, ANIM_SALIDA}
	private EstadoMapa estado = EstadoMapa.CARGANDO;

	public static float segundosPorCuadro = 1.0F;

	private List<Pieza> piezas;
	private float deltaCuadro;

	void Start () {
		estado = EstadoMapa.CARGANDO;
	}
	
	// Update is called once per frame
	void Update () {
		switch (estado) {
			
			case EstadoMapa.ESPERANDO_JUGADA:
				break;

			case EstadoMapa.EJECUTANDO:
				deltaCuadro += (float)Time.deltaTime / segundosPorCuadro;
				if (deltaCuadro >= 1.0) {
					deltaCuadro = 0;
					if (ejecutarCuadro()) {
						if (estaNivelResuelto()) {
							estado = EstadoMapa.ANIM_SALIDA;
						} else {
							estado = EstadoMapa.ESPERANDO_JUGADA;
						}
					}
				}

				break;

			case EstadoMapa.CARGANDO:
				//No hacer nada. Esperar a que el CargadorMapa cambie el estado a ANIM_ENTRADA.
			break;

			case EstadoMapa.ANIM_ENTRADA:
				break;

			case EstadoMapa.ANIM_SALIDA:
				break;
		}
	}

	//Retorna true si todas las piezas han parado de moverse.
	bool ejecutarCuadro(){
		//TODO
		return true;
	}

	//Retorna true cuando el nivel ha sido resuelto
	bool estaNivelResuelto(){
		return false;
	}
}
