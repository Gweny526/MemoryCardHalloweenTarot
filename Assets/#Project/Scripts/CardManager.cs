
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // nécessaire pour utilisé UnityEvent

public class CardManager : MonoBehaviour
{
    //si on avait utiliser un vector2
    // [SerializeField] Vector2 gameSize = Vector2.one * 4;
    // Start is called before the first frame update
    [SerializeField] int columns = 4;
    [SerializeField] int rows = 4;
    [SerializeField] GameObject prefab;
    [SerializeField] float gapX = 0.5f;
    [SerializeField] float gapY = 0.5f;
    [SerializeField] Sprite[] possibleFaces;

    private List<CardBehaviour> cards = new();
    private List<CardBehaviour> faceUpCards = new ();
    private List<CardBehaviour> goodCards = new();
    [SerializeField] private UnityEvent whenPlayerWon;
    
    // propriété 
    // private int  countCard{
    //     get {return rows * columns}
    // }

    
    void Start()
    {
        if((rows * columns) % 2 != 0)
        {
            Debug.LogError("You need to have an even number of cards.");
            return;
        }
        if((rows * columns)/2 > possibleFaces.Length)
        {
            Debug.LogError($"You cannot have more cards than {possibleFaces.Length /2}");
            return;
        }
        
        Initialize();
    }
    private void Initialize()
    {
        int nbrFace = (rows*columns) /2;
        List<int> faces = new(); // face qui vont être dans le jeu
        for(int n = 0; n < nbrFace; n++) // _ == n c'est la même chose du coup on met ce qu'on veut
        {
            int face = Random.Range(0, possibleFaces.Length);
            while(faces.Contains(face)) // tant que ma liste de faces contient la face que je viens de tiré, tire une autre face??? (tout le while)
            {
                face = Random.Range(0, possibleFaces.Length);
            }
            faces.Add(face); //sinon ajoute la carte 
        }
        faces.AddRange(faces); // j'ajoute les même face (j'ajoute les faces a lui même) ce qui me crée des double
        // Debug.Log(faces.Count);

        for(int x = 0; x < columns; x++ ){
            for(int y = 0; y < rows; y++){
                int index = Random.Range(0, faces.Count); // List => Count| tableau (array) => Length || je prend un index aléatoire dans la list 
                InstantiateCard(x,y, faces[index]); // x,y => position puis il va mettre l'index de faces déterminé au dessus 
                faces.RemoveAt(index); // si j'ai tiré une face (ex: 3) il va la retiré la faceID qui a l'index 3 de la liste
            }
        }
    }
    private void InstantiateCard(int x, int y, int faceID){
        GameObject card = Instantiate(prefab);
        if(card.TryGetComponent(out CardBehaviour cardBehaviour)){ // on verifie qu'on a bien une card Behaviour
            cards.Add(cardBehaviour); //le manager a une liste de cards et il va aller l'ajouter dans la liste
            cardBehaviour.manager = this; //this c'est celui là (lui même)
            GiveFace(cardBehaviour, faceID);
        }
        else
        {
            Debug.Log($"Prefab {prefab.name} doesn't have a collider");
        }
        // Collider2D collider = card.GetComponent<Collider2D>();
        if (card.TryGetComponent(out Collider2D collider)){
            Vector3 cardSize = collider.bounds.size;
            float cardX = x * (cardSize.x + gapX);
            float cardY = y * (cardSize.y + gapY);

            Vector3 position = new(cardX, cardY);
            // position += transform.position;
            card.transform.SetParent(transform);
            card.transform.localPosition = position;
        }
        else
        {
            Debug.Log($"Prefab {prefab.name} uh uh uuuh doesn't have a collider");
        }
    }

    private void GiveFace(CardBehaviour cardBehaviour, int faceID)
    {
        
        cardBehaviour.faceID = faceID;
        cardBehaviour.face = possibleFaces[faceID];
    }

    public void CardHasBeenTurned(CardBehaviour card){ //n'importe quelle carte qui a été tournée va dire a son manager qu'elle a été tournée
        faceUpCards.Add(card);
        if( faceUpCards.Count > 1) //techniquement veut dire si j'ai 2 carte (pour verifié les ID)
        {
            
            if(faceUpCards[0].faceID != faceUpCards[1].faceID)
            {
                //les faire se retourner.
                faceUpCards[0].TurnBackAction(); // si pas les deux mêmes elle se retournent
                faceUpCards[1].TurnBackAction();
            } else
            {
                goodCards.Add(card);
            }
            faceUpCards.Clear(); //clear la liste
        }
        YouWon();
        // Debug.Log($"Number of card face up : {faceUpCards.Count}");
    }
    public void YouWon(){
        if(goodCards.Count == (rows * columns)/2){
            Debug.Log("You Won!");
            whenPlayerWon.Invoke();
        }
    }
    
}
