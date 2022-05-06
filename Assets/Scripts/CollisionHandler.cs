using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    [SerializeField] float levelLoadDelay = 2f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();          
    }
    
    AudioSource audioSource;

    bool isTransitioning = false;

    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        successParticles.Play();
        audioSource.Stop();
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
        audioSource.PlayOneShot(successSound);
    }

    void StartCrashSequence()
    {
        crashParticles.Play();
        audioSource.Stop();
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
        audioSource.PlayOneShot(crashSound);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);       
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}