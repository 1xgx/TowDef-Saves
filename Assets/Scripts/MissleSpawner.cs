using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MissleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _misslePrefab;
    [SerializeField] private float _delay = 10.0f;
    [SerializeField] private GridCell _gridCell;
    public List<string> _missleType;
    [SerializeField] private GameManager _gameManager;
    public void spawnStart(GameManager gameManager, float speedOfInstantiate)
    {
        float Delay = _delay / speedOfInstantiate;
        int Missle_Count = 120 / System.Convert.ToInt32(Delay);
        _gameManager = gameManager;
        StartCoroutine(QueueOfMissle(Delay, Missle_Count));
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator QueueOfMissle(float delay, int Missle_Count)
    {
        while (Missle_Count > 0)
        {
            yield return new WaitForSeconds(delay);
            missleGenerate();
            Missle_Count--;
        }
        
    }
    private void missleGenerate()
    {
        if(_gameManager.FightIsStarted == false) return;
        int randomIndex = Random.Range(0, _misslePrefab.Length);
        int randomTypeMissle = Random.Range(0, _missleType.Count);
        string typeOfMissle = _misslePrefab[randomIndex].GetComponent<Missle>().missleType;
        if (_missleType[0] == typeOfMissle)
            Instantiate(_misslePrefab[randomIndex], new Vector3(Random.Range(-10.0f, 10.0f), 0, 10.0f), Quaternion.identity);
        if (_missleType[1] == typeOfMissle)
        {
            int randomIndexY = Random.Range(0, 15);
            Vector3 newPosition = _gridCell._hexagonGrid[0, randomIndexY].transform.position;
            GameObject newMissle = Instantiate(_misslePrefab[randomIndex], new Vector3(newPosition.x, .3f, newPosition.z), Quaternion.identity);
            newMissle.GetComponent<MissleCellController>().MissleCellPosition.Item1 = 0;
            newMissle.GetComponent<MissleCellController>().MissleCellPosition.Item2 = randomIndexY;
            newMissle.GetComponent<MissleCellController>().Spawn();

        }
    }
    public void missleDebugSpawner()
    {
        for (int i = 0; i < 30; i++) { Instantiate(_misslePrefab[0], new Vector3(Random.Range(-10.0f, 10.0f), 0, 10.0f), Quaternion.identity); }
    }
}
