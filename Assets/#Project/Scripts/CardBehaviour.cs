using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CardBehaviour : MonoBehaviour
{
    public CardManager manager; // public pour qu'il permette d'être utilisé dans d'autre classe on peut aussi utilisé internal
    [SerializeField] internal Sprite face;
    private Sprite back;
    internal int faceID;
    private SpriteRenderer spriteRenderer;
    private bool faceUp = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        back = spriteRenderer.sprite;  //pour mémorisé la face qui est actuellement affiché
    }

    public void Turn()
    {
        if(faceUp){
            spriteRenderer.sprite = back; // si faceUp met le back
        }
        else
        {
            spriteRenderer.sprite= face; // sinon met le côté faceUp
        }
        faceUp =!faceUp; //si faceUp => pas faceUp | si pas faceUp => faceUp
    }
    private void  TurnAction(){
        animator.SetTrigger("clicked");
        animator.SetBool("over", false);
        manager.CardHasBeenTurned(this);  // callback on lui passe l'instance de la carte qui a été retourné

    }
    public void TurnBackAction(){
        animator.SetTrigger("turnback");
    }
    public void OnMouseDown()
    {
        if(!faceUp){
            TurnAction();
        }
        // Turn();
    }
    // public void OnMouseUp()
    // {
    //     animator.SetBool("clicked", false);
    // }
    public void OnMouseEnter(){
        if(!faceUp){

            animator.SetBool("over", true);
        }
    }
    public void OnMouseExit(){
        if(!faceUp){
            animator.SetBool("over", false);
        }
    }
    public void ResetCard()
{
    faceUp = false;
    spriteRenderer.sprite = back; // remettre la carte face cachée
    if (animator != null)
    {
        animator.ResetTrigger("clicked");
        animator.SetBool("over", false); // remettre l'animation à l'état initial
    }
}

}
