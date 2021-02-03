using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public BoxCollider2D TapCollider;
    public string nextSceneName;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    
    private bool _isGameEnded = false;
    private Bird _shotBird;

    void Start()
    {
        for(int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }
        
        for(int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }
        
        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];

    }
    
    public void ChangeBird()
    {

        if (_isGameEnded) return;

        TapCollider.enabled = false;

        Birds.RemoveAt(0);

        if(Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
        else if (!_isGameEnded)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        // for(int i = 0; i < Enemies.Count; i++)
        // {
        //     if(Enemies[i].gameObject == destroyedEnemy)
        //     {
        //         Enemies.RemoveAt(i);
        //         break;
        //     }
        // }
        
        Enemies.Remove(destroyedEnemy.GetComponent<Enemy>());


        if(Enemies.Count == 0)
        {
            _isGameEnded = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
    
    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }
    
    void OnMouseUp()
    {
        if(_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}