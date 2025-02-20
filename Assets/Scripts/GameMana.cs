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
    public GameObject botones;

    int posjugador = -1;
    int posIA =-1;
    int numdado;
    int ronda = 1;

    bool tirar = false;
    
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
        infoCasillas[12] = 2;
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
        //botones = GameObject.Find("Botones");

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
       
    }

    public void TirarDado()
    {
         StartCoroutine(Jugar());
    }

    private void Comprobarcasilla()
    {
        // comprueba el estado de la casilla si esta ocupada o no
        if (posjugador > 20)
        {
            posjugador = 20;
        }

        int n = vectorCasillas[posjugador];

        if (n == 0)
        {
            vectorCasillas[posjugador] = 1;
        }
        else if (n == 2)
        {
            botones.SetActive(true);
            textoaviso.text = "Elige";
            Time.timeScale = 0;
            
        }
    }

    private void Movercasilla()
    {
        // Comprueba cuanto ha sacado guardando el valor en una variable para moverse a la posición de la casilla
        
        vectorCasillas[posjugador] = 0;

        if (posjugador >= 20)
        {
            posjugador = 20;
        }

        int a = infoCasillas[posjugador];

        if (a == 99)
        {
            textoaviso.text = "VICTORIA";
            posjugador = 20;
            Time.timeScale = 0;
        }

        fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
        

        if (a == 1)
        {
            textoaviso.text = "Teleport";
            posjugador = posjugador + 6;
            fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
        }
        else if (a == -1)
        {
            textoaviso.text = "Retrocede";
            posjugador = posjugador - 3;
            fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
        }
        else if (a == 2)
        {
           textoaviso.text = "Tira de nuevo";
            StopAllCoroutines();
        }
        
        vectorCasillas[posjugador] = 1;
    }

    public void ComprobarIA()
    {
        // comprueba el estado de la casilla si esta ocupada o no
        if (posIA >= 20)
        {
            posIA = 20;
        }

        int n2 = vectorCasillas[posIA];

        if (n2 == 0)
        {
            vectorCasillas[posIA] = 2;
        }
        else if (n2 == 1)
        {
            int c = infoCasillas[posIA + 1];
            int c2 = infoCasillas[posIA - 1];

            // IA Avanza
            if (c == 1 || c == 2 || c2 == -1)
            {
                posIA++;
                fichaIA.transform.position = vectorObjetos[posIA].transform.position;
            }
            // IA Retrocede
            else if (c2 == 1 || c2 == 2 || c == -1)
            {
                posIA--;
                fichaIA.transform.position = vectorObjetos[posIA].transform.position;
            }
            // IA Random
            else
            {
                int b = Random.Range(0, 2);

                if (b == 0)
                {
                    posIA--;
                }
                else posIA++;

                fichaIA.transform.position = vectorObjetos[posIA].transform.position;
            }
            vectorCasillas[posIA] = 2;
        }
    }
    private void MoverIA()
    {
        vectorCasillas[posIA] = 0;

        if (posIA >= 20)
        {
            posIA = 20;
        }

        int a = infoCasillas[posIA];

        if (a == 99)
        {
            textoaviso.text = "DERROTA";
            posIA = 20;
            Time.timeScale = 0;
        }

        fichaIA.transform.position = vectorObjetos[posIA].transform.position;

        if (a == 1)
        {
            textoaviso.text = "IA Teleport";
            posIA = posIA + 6;
            fichaIA.transform.position = vectorObjetos[posIA].transform.position;
        }
        else if (a == -1)
        {
            textoaviso.text = "IA Retrocede";
            posIA = posIA - 3;
            fichaIA.transform.position = vectorObjetos[posIA].transform.position;
        }
        else if (a == 2)
        {
            textoaviso.text = "IA Tira de nuevo";
            tirar = true;
        }
        else ronda++;

        vectorCasillas[posIA] = 2;
    }

    public void Retroceder()
    {
        Time.timeScale = 1;
        posjugador = posjugador -1;
        fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
        Movercasilla();
        botones.SetActive(false);
    }

    public void Avanzar()
    {
        Time.timeScale = 1;
        posjugador = posjugador + 1;
        fichajugador.transform.position = vectorObjetos[posjugador].transform.position;
        Movercasilla();
        botones.SetActive(false);
    }

    IEnumerator Jugar()
    {
        //Turno Jugador
        // Lanzar el dado con un número aleatorio
        textodado.color = Color.red;

        for (int i = 0; i < 12; i++)
        {
            numdado = Random.Range(1, 7);
            textodado.text = "" + numdado;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        textodado.color = Color.white;
        posjugador = posjugador + numdado;

        Comprobarcasilla();
        Movercasilla();
        yield return new WaitForSeconds(1f);
        Comprobarcasilla();

        yield return new WaitForSeconds(1f);
        textoaviso.text = "";

        // Turno IA
        // Lanzar el dado con un número aleatorio
        textoturno.text = "Turno IA";
        textodado.color = Color.red;

        for (int i = 0; i < 12; i++)
        {
            numdado = Random.Range(1, 7);
            textodado.text = "" + numdado;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        textodado.color = Color.white;
        posIA = posIA + numdado;

        ComprobarIA();
        MoverIA();
        ComprobarIA();

        if (tirar == true)
        {
            textodado.color = Color.red;
            for (int i = 0; i < 12; i++)
            {
                numdado = Random.Range(1, 7);
                textodado.text = "" + numdado;
                yield return new WaitForSeconds(0.1f);
            }
            textodado.color = Color.white;
            posIA = posIA + numdado;

            ComprobarIA();
            MoverIA();
            ComprobarIA();
            tirar = false;
        }

        textoronda.text = "Ronda " + ronda;
        textoturno.text = "Turno Jugador";

        if (posIA < 20 && posjugador < 20)
        {
            textoaviso.text = "Lanza el dado";
        }
    }
}
