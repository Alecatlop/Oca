using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class GameMana : MonoBehaviour
{
    public int[] vectorCasillas;
    public int[] infoCasillas;
    public GameObject[] vectorObjetos;
    public CanvasMana canvas;
    public TextMeshProUGUI textodado;
    public TextMeshProUGUI textoturno;
    GameObject fichajugador;
    GameObject fichaIA;
    int posjugador;
    int posIA;
    int numdado;
    int turno; // si turno es 0 tira jugador / si es 1 tira IA
    bool seguir = true;

    private void Awake()
    {
        
        vectorCasillas = new int[21];
        infoCasillas = new int[21];

        // RELLENAMOS EL VECTOR DE CASILLAS
        for (int i = 0; i < vectorCasillas.Length; i++)
            vectorCasillas[i] = 0;

        // RELLENAMOS EL VECTOR DE INFO CASILLAS
        for (int i = 0; i < infoCasillas.Length; i++)
            infoCasillas[i] = 0;

        // teleports
        infoCasillas[0] = 1;
        infoCasillas[5] = 1;

        // volver a tirar
        infoCasillas[11] = 2;
        infoCasillas[17] = 2;

        // retroceder 3 casillas
        infoCasillas[4] = -1;
        infoCasillas[9] = -1;
        infoCasillas[13] = -1;
        infoCasillas[18] = -1;
        infoCasillas[19] = -1;

        // victoria
        infoCasillas[20] = 99;

        // RELLENAMOS EL VECTOR DE GAMEOBJECTS
        //vectorObjetos = GameObject.FindGameObjectsWithTag("Casilla");

        // METODO 1: OBTENER LOS HIJOS DE UN PARENT VAC�O
        //vectorObjetos = GameObject.

        // METODO 2: RELLENAR CON UN FOR Y UN FIND
        vectorObjetos = new GameObject[21];

        for (int i = 0; i < vectorObjetos.Length; i++)
            vectorObjetos[i] = GameObject.Find("Casilla" + i);


        // METODO 3: ORDENAR LA LISTA A PARTIR DE LA LISTA DE TAGS
        // LA MAS COMPLICADA PERO LA MAS EFICIENTE

        // 21 CASILLAS DESORDENADAS
        //GameObject[] vectorGOCasillas = GameObject.FindGameObjectsWithTag("Casilla");

        //for (int i = 0; i < vectorGOCasillas.Length; i++)
        //{
        //    GameObject casilla = vectorGOCasillas[i];
        //    // falta terminar ..

        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        fichajugador = GameObject.Find("Jugador");
        fichaIA = GameObject.Find("IA");

        vectorObjetos[1].GetComponent<Renderer>().material.color = Color.yellow;
        vectorObjetos[5].GetComponent<Renderer>().material.color = Color.red;
        vectorObjetos[6].GetComponent<Renderer>().material.color = Color.yellow;
        vectorObjetos[7].GetComponent<Renderer>().material.color = Color.blue;
        vectorObjetos[10].GetComponent<Renderer>().material.color = Color.red;
        vectorObjetos[12].GetComponent<Renderer>().material.color = Color.green;
        vectorObjetos[13].GetComponent<Renderer>().material.color = Color.blue;
        vectorObjetos[14].GetComponent<Renderer>().material.color = Color.red;
        vectorObjetos[18].GetComponent<Renderer>().material.color = Color.green;
        vectorObjetos[19].GetComponent<Renderer>().material.color = Color.red;
        vectorObjetos[20].GetComponent<Renderer>().material.color = Color.red;
        //vectorObjetos[21].GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (turno == 0)
        {
            TurnoJugador();
        }
        //else TurnoIA();
    }

    private void TurnoJugador()
    {
        seguir = true;
        textoturno.text = "Turno Jugador";
    }

    private void TurnoIA()
    {
        seguir = false;
        textoturno.text = "Turno IA";
    }

    public void TirarDado()
    {
        if (seguir == true)
        {
            StartCoroutine(Espera());
        }
    }

    IEnumerator Espera()
    {
        if (turno == 0)
        {
            textodado.material.color = Color.red;
            numdado = Random.Range(1, 7);
            textodado.text = "" + numdado;

            yield return new WaitForSeconds(1f);

            textodado.material.color = Color.white;
            posjugador = posjugador + numdado;

            if (posjugador >= 21)
            {
                print("VICTORIA");
                posjugador = 21;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
            }

            fichajugador.transform.position = vectorObjetos[posjugador].transform.position;

            yield return new WaitForSeconds(1f);

            if (posjugador == 1)
            {
                print("Teleport");
                posjugador = 7;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
            }
            else if (posjugador == 6)
            {
                print("Teleport2");
                posjugador = 13;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
            }
            else if (posjugador == 12 || posjugador == 18)
            {
                print("vuelves a tirar");
            }
            else if (posjugador == 5 || posjugador == 10 || posjugador == 14 || posjugador == 19 || posjugador == 20)
            {
                print("Retrocedes");
                posjugador = posjugador - 3;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
            }
            

            //turno++;
        }
        
        
    }
}
