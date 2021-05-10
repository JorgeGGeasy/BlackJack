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
    public Text playerMessage;
    public Text dealerMessage;
    public Text bancaMessage;
    public Text apuestaMessage;
    public bool showFirstCard = false;
    public bool firstHit = false;
    public bool bancarrota = false;

    // Banca = 1000
    public int banca = 1000;
    // Apuesta minima = 10
    public int apuesta = 10;

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
        //Debug.Log(prob1.ToString());

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
        
        //Debug.Log(prob2.ToString());

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
        //Debug.Log(prob3.ToString());


        probMessage.text = "Probabilidad 1 = " + prob1.ToString() + "\r\n" + " Probabilidad 2 = " + prob2.ToString() + "\r\n" + " Probabilidad 3 = " + prob3.ToString();

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
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        bancarrota = comprobarBancarrota();
        if (!bancarrota)
        {
            comprobarFirstHit();
            if (player.GetComponent<CardHand>().points < 21)
            {
                //Repartimos carta al jugador
                PushPlayer();
            }

            if (player.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "BlackJack";
                Stand();
            }

            if (player.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "Perdiste";
                Stand();
            }

            playerMessage.text = player.GetComponent<CardHand>().points.ToString();
        }

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        bancarrota = comprobarBancarrota();
        if (!bancarrota)
        {
            comprobarFirstHit();
            CardHand cardD = dealer.GetComponent<CardHand>();
            if (showFirstCard == false)
            {
                cardD.cards[0].GetComponent<CardModel>().ToggleFace(true);
                showFirstCard = true;
            }

            while (dealer.GetComponent<CardHand>().points <= 16)
            {
                if (player.GetComponent<CardHand>().points < dealer.GetComponent<CardHand>().points)
                {
                    break;
                }

                else if (player.GetComponent<CardHand>().points > 21)
                {
                    break;
                }

                PushDealer();
            }

            if (player.GetComponent<CardHand>().points > 21 && dealer.GetComponent<CardHand>().points <= 21)
            {
                finalMessage.text = "Dealer gana";
            }
            else if (player.GetComponent<CardHand>().points > 21 && dealer.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "Empate";
                banca = banca + apuesta;
                bancaMessage.text = "Banca = " + banca.ToString();
            }
            else if (player.GetComponent<CardHand>().points <= 21 && dealer.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "Jugador gana";
                banca = banca + apuesta*2;
                bancaMessage.text = "Banca = " + banca.ToString();
            }
            else if (player.GetComponent<CardHand>().points <= 21 && dealer.GetComponent<CardHand>().points <= 21)
            {
                if (player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points)
                {
                    finalMessage.text = "Jugador gana";
                    banca = banca + apuesta * 2;
                    bancaMessage.text = "Banca = " + banca.ToString();
                }
                else if (player.GetComponent<CardHand>().points == dealer.GetComponent<CardHand>().points)
                {
                    finalMessage.text = "Empate";
                    banca = banca + apuesta;
                    bancaMessage.text = "Banca = " + banca.ToString();
                }
                else if (player.GetComponent<CardHand>().points < dealer.GetComponent<CardHand>().points)
                {
                    finalMessage.text = "Dealer gana";
                }
            }

            playerMessage.text = player.GetComponent<CardHand>().points.ToString();
            dealerMessage.text = dealer.GetComponent<CardHand>().points.ToString();

            /*
            if (player.GetComponent<CardHand>().points >= 21)
            {
                cardD.cards[0].GetComponent<CardModel>().ToggleFace(true);
            }
            */

            /*TODO:
             * Repartimos cartas al dealer si tiene 16 puntos o menos
             * El dealer se planta al obtener 17 puntos o más
             * Mostramos el mensaje del que ha ganado
             */

        }
    }

    public void Aumentar ()
    {
        if(!((apuesta + 10) > banca))
        {
            apuesta = apuesta + 10;
            apuestaMessage.text = "Apuesta = " + apuesta.ToString();
        }
    }

    public void Reducir()
    {
        if(!((apuesta-10) < 10))
        {
            apuesta = apuesta - 10;
            apuestaMessage.text = "Apuesta = " + apuesta.ToString();
        }
    }

    public void AllIn()
    {
        apuesta = banca;
        apuestaMessage.text = "Apuesta = " + apuesta.ToString();
    }

    public void comprobarFirstHit()
    {
        if(firstHit == false)
        {
            banca = banca - apuesta;
            if(banca < 0)
            {
                banca = 0;
            }
            bancaMessage.text = "Banca = " + banca.ToString();
            firstHit = true;
        }
    }
    public bool comprobarBancarrota()
    {
        if (firstHit == false)
        {
            if (banca == 0)
            {
                finalMessage.text = "Bancarrota";
                return true;
            }
        }
        return false;
    }

    public void PlayAgain()
    {
        bancarrota = comprobarBancarrota();
        comprobarFirstHit();
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        showFirstCard = false;
        firstHit = false;
        playerMessage.text = "";
        dealerMessage.text = "";
        cardIndex = 0;
        apuesta = 10;
        apuestaMessage.text = "Apuesta = " + apuesta.ToString();
        if (!bancarrota) {
            ShuffleCards();
            StartGame();
        }
        else
        {
            finalMessage.text = "Bancarrota";
        }

    }
    
}
