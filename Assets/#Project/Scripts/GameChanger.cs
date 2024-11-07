using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameChanger : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    float timer = 5f;
    private string sceneName;

    void Start(){
        timer = delay;
    }
    void Update()
    {
        if(timer <  delay)
        {
            timer += Time.deltaTime; //je rajoute du temps

            if(timer>= delay)
            {
                SceneManager.LoadScene(sceneName);

            }
        }
        
    }
    public void Change(string sceneName){
        sceneName = sceneName.Trim();
        this.sceneName = sceneName;
        timer = 0f;
    }

   


}
