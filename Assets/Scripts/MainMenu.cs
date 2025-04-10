using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _delayOfClick = 0.5f;
    public void PlayGame(int IndexScene)
    {
        _audioSource.PlayOneShot(_audioClip);
        StartCoroutine(Delay(IndexScene));
        
        
    }
    IEnumerator Delay(int IndexScene)
    {
        yield return new WaitForSeconds(0.5f);
        if (IndexScene == 1) SceneManager.LoadSceneAsync(IndexScene);
    }
}
