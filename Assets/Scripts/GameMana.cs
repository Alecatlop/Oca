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
    public TextMeshProUGUI textodado;
    public TextMeshProUGUI textoturno;
    public TextMeshProUGUI textoronda;
    public TextMeshProUGUI textoaviso;
    GameObject fichajugador;
    GameObject fichaIA;
    GameObject botones;
    int posjugador = -1;
    int posIA =-1;
    int numdado;
    int turno;
    int ronda = 1;
    

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
        infoCasillas[11] = 2; // CAMBIAR A 12
        infoCasillas[17] = 2;

        // retroceder 3 casillas
        infoCasillas[4] = -1;
        infoCasillas[9] = -1;
        infoCasillas[13] = -1;
        infoCasillas[18] = -1;
        infoCasillas[19] = -1;

        // victoria
        infoCasillas[20] = 99;

        // METODO 2: RELLENAR CON UN FOR Y UN FIND
        vectorObjetos = new GameObject[21];

        for (int i = 0; i < vectorObjetos.Length; i++)
            vectorObjetos[i] = GameObject.Find("Casilla" + i);
       
    }

    // Start is called before the first frame update
    void Start()
    {
        fichajugador = GameObject.Find("Jugador");
        fichaIA = GameObject.Find("IA");
        botones = GameObject.Find("Botones");

        botones.SetActive(false);

        for (int i = 0; i < vectorObjetos.Length; i++)
        {
            int a = infoCasillas[i];

            if (a == 1)
            {
                vectorObjetos[i].GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (a == 2)
            {
                vectorObjetos[i].GetComponent<Renderer>().material.color = Color.green;
            }
            else if (a == -1)
            {
                vectorObjetos[i].GetComponent<Renderer>().material.color = Color.red;
            }
            else if (a == 99)
            {
                vectorObjetos[i].GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // comprueba el turno con una variable, Jugador = 0 / IA = 1
        if (turno == 0)
        {
            textoturno.text = "Turno Jugador";
        }
        else if (turno == 1)
        {
            textoturno.text = "Turno IA";
        }
    }

    public void TirarDado()
    {
            StartCoroutine(Espera());
    }

    public void Retroceder()
    {
        posjugador = posjugador--;
        fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
    }

    public void Avanzar()
    {
        posjugador = posjugador++;
        fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
    }

    IEnumerator Espera()
    {
        if (turno == 0)
        {
            // Lanzar el dado con un número aleatorio
            textodado.color = Color.red;
            for (int i = 0; i < 12; i++)
            {
                numdado = Random.Range(1, 7);
                textodado.text = "" + numdado;
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(1f);
            numdado = 3;
            textodado.color = Color.white;
            posjugador = posjugador + numdado;

            // comprueba el estado de la casilla si esta ocupada o no
            int n = vectorCasillas[posjugador];

            if (n == 0)
            {
                vectorCasillas[posjugador] = 1;
            }
            else if (n == 2)
            {
                botones.SetActive(true);
                textoaviso.text = "Elige";
            }

            botones.SetActive(false);

            // Comprueba cuanto ha sacado guardando el valor en una variable para moverse a la posición de la casilla
            if (posjugador >= 20)
            {
                textoaviso.text = "VICTORIA";
                posjugador = 20;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
                turno = 2;
                Time.timeScale = 0;
            }

            fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
            yield return new WaitForSeconds(0.4f);

            if (posjugador == 0)
            {
                vectorCasillas[posjugador] = 0;
                textoaviso.text = "Teleport";
                posjugador = 6;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
                vectorCasillas[posjugador] = 1;
            }
            else if (posjugador == 5)
            {
                vectorCasillas[posjugador] = 0;
                textoaviso.text = "Teleport";
                posjugador = 12;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
                vectorCasillas[posjugador] = 1;
            }
            else if (posjugador == 4 || posjugador == 9 || posjugador == 13 || posjugador == 18 || posjugador == 19)
            {
                vectorCasillas[posjugador] = 0;
                textoaviso.text = "Retrocede";
                posjugador = posjugador - 3;
                fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
                vectorCasillas[posjugador] = 1;
            }

            if (posjugador != 11 && posjugador != 17)
            {
                turno++;
            }
            else if (posjugador == 11 || posjugador == 17)
            {
                textoaviso.text = "Tira de nuevo";
            }
                
            yield return new WaitForSeconds(1f);
            textoaviso.text = "";
        }
        // Turno IA
        else if (turno == 1)
        {
            // Lanzar el dado con un número aleatorio
            textodado.color = Color.red;
            for (int i = 0; i < 12; i++)
            {
                numdado = Random.Range(1, 7);
                textodado.text = "" + numdado;
                yield return new WaitForSeconds(0.1f);
            }

            numdado = 3;
           yield return new WaitForSeconds(1f);

            textodado.color = Color.white;
            posIA = posIA + numdado;
            
            // comprueba el estado de la casilla si esta ocupada o no
            int n2 = vectorCasillas[posIA];

            if (n2 == 0)
            {
                vectorCasillas[posIA] = 2;
            }
            else if (n2 == 1)
            {
                // IA Avanza
                if (posIA + 1 == 5 || posIA + 1 == 11 || posIA + 1 == 17)
                {
                    //print("palante");
                    //posIA++;
                    //fichaIA.transform.position = vectorObjetos[posIA].transform.position;
                    //vectorCasillas[posIA] = 2;
                }
                // IA Retrocede
                else if (posIA - 1 == 0 || posIA - 1 == 5 || posIA - 1 == 11 || posIA - 1 == 17)
                {
                    //print("holaaa");
                    //posIA--;
                    //fichaIA.transform.position = vectorObjetos[posIA].transform.position;
                    //vectorCasillas[posIA] = 2;
                }
                // IA Random
                else
                {
                    int b = Random.Range(0, 2);
                    
                    if (b == 0)
                    {
                        posIA--;
                    }
                    else
                    {
                        posIA++;
                    }

                    fichaIA.transform.position = vectorObjetos[posIA].transform.position;
                    vectorCasillas[posIA] = 2;
                }
            }

            // Comprueba cuanto ha sacado guardadndo el valor en una variable para moverse a la posición de la casilla
            if (posIA >= 20)
            {
                textoaviso.text = "DERROTA";
                posIA = 20;
                fichaIA.transform.position = vectorObjetos[posIA].transform.position;
                Time.timeScale = 0;
                turno = 2;
            }

            fichaIA.transform.position = vectorObjetos[posIA].transform.position;

            yield return new WaitForSeconds(0.4f);

            if (posIA == 0)
            {
                vectorCasillas[posIA] = 0;
                posIA = 6;
                fichaIA.transform.position = vectorObjetos[posIA].transform.position;
                vectorCasillas[posIA] = 2;
            }
            else if (posIA == 5)
            {
                vectorCasillas[posIA] = 0;
                posIA = 12;
                fichaIA.transform.position = vectorObjetos[posIA].transform.position;
                vectorCasillas[posIA] = 2;
            }
            else if (posIA == 4 || posIA == 9 || posIA == 13 || posIA == 18 || posIA == 19)
            {
                vectorCasillas[posIA] = 0;
                posIA = posIA - 3;
                fichaIA.transform.position = vectorObjetos[posIA].transform.position;
                vectorCasillas[posIA] = 2;
            }

            if (posIA != 11 && posIA != 17)
            {
                turno--;
                ronda++;
                textoaviso.text = "Lanza el dado";
                textoronda.text = "Ronda " + ronda;
            }
            else if (posIA == 11 || posIA == 17)
            {
                textoaviso.text = "IA tira de  nuevo";
            }
        }

    }
}
