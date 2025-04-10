using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _delayOfClick = 0.5f;
    public void changeLevel(int levelIndex)
    {
        _audioSource.PlayOneShot(_audioClip);
        StartCoroutine(Delay(levelIndex));
    }
    IEnumerator Delay(int IndexScene)
    {
        yield return new WaitForSeconds(_delayOfClick);
        SceneManager.LoadSceneAsync(IndexScene);
    }

}
