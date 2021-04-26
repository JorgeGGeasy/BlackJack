using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public Sprite[] shuffleFaces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public bool showFirstCard = false;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        int valor = 0;
        for(int i = 0; i < 52; i++)
        {
            if(i == 0 || i == 13 || i == 26 || i == 39)
            {
                values[i] = 11;
                valor = 2;
            }
            else
            {
                if (valor > 10)
                {
                    valor = 10;
                }
                values[i] = valor;
                valor++;
            }
            //Debug.Log(values[i].ToString());
        }


    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        Sprite auxiliar;
        int valorAuxiliar;
        for (int i = 0; i < 100; i++)
        {
            int pos1 = Random.Range(0, 52);
            int pos2 = Random.Range(0, 52);

            if(pos1 != pos2)
            {
                auxiliar = faces[pos1];
                faces[pos1] = faces[pos2];
                faces[pos2] = auxiliar;

                valorAuxiliar = values[pos1];
                values[pos1] = values[pos2];
                values[pos2] = valorAuxiliar;
            }
        }

    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */

        CardHand jugador = player.GetComponent<CardHand>();
        CardHand crupier = dealer.GetComponent<CardHand>();
        float puntosJugador = jugador.points;
        float puntosCrupier = crupier.points;
        float cartasPosibles = 0;
        float puntosNecesarios;

        float prob1 = 0;

        puntosNecesarios = puntosJugador - puntosCrupier;

        if (puntosNecesarios < 0)
        {
            prob1 = 1;
        }
        else
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] >= puntosNecesarios)
                {
                    cartasPosibles++;
                }
            }
        }
    
        prob1 = cartasPosibles / 52;
        Debug.Log(prob1.ToString());

        float prob2 = 0;
        cartasPosibles = 0;
        float cartaMin = 17 - puntosJugador;
        float cartaMax = 21 - puntosJugador;

        if(puntosJugador >= 21)
        {
            prob2 = 0;
        }
        else
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (cartaMin < 0 && cartaMax == 0)
                {
                    if (values[i] == 11)
                    {
                        cartasPosibles++;
                    }
                    if (values[i] <= cartaMax)
                    {
                        cartasPosibles++;
                    }
                }
                else
                {
                    if (values[i] == 11)
                    {
                        if (1 >= cartaMin)
                        {
                            cartasPosibles++;
                        }
                        else if (11 <= cartaMin)
                        {
                            cartasPosibles++;
                        }
                    }
                    else if (values[i] >= cartaMin && values[i] <= cartaMax)
                    {
                        cartasPosibles++;
                    }
                }
            }
            prob2 = cartasPosibles / 52;
        }
        
        Debug.Log(prob2.ToString());

        float prob3 = 0;
        puntosNecesarios = 0;
        cartasPosibles = 0;
        if(puntosJugador <= 9)
        {
            prob3 = 0;
        }
        else
        {
            puntosNecesarios = 21 - puntosJugador;
            for(int i = 0; i < values.Length; i++)
            {
                if(values[i] > puntosNecesarios)
                {
                    cartasPosibles++;
                }
            }
        }
        prob3 = cartasPosibles / 52;
        Debug.Log(prob3.ToString());


        probMessage.text = "Probabilidad 1 = " + prob1.ToString() + " Probabilidad 2 = " + prob2.ToString() + " Probabilidad 3 = " + prob3.ToString();

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        /*
            CardHand cardD = dealer.GetComponent<CardHand>();
            cardD.cards[0].GetComponent<CardModel>().ToggleFace(true);
            showFirstCard = true;
        */

            //Repartimos carta al jugador
            PushPlayer();
        
            /*TODO:
             * Comprobamos si el jugador ya ha perdido y mostramos mensaje
             */

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
