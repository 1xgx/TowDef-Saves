using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _misslePrefab;
    [SerializeField] private float _delay = 3.0f;
    [SerializeField] private GridCell _gridCell;
    public List<string> _missleType;
    public void spawnStart()
    {
        InvokeRepeating(nameof(missleGenerate), _delay, _delay);
    }
    private void missleGenerate()
    {
        int randomIndex = Random.Range(0, _misslePrefab.Length);
        int randomTypeMissle = Random.Range(0, _missleType.Count);
        string typeOfMissle = _misslePrefab[randomIndex].GetComponent<Missle>().missleType;
        if (_missleType[0] == typeOfMissle)
            Instantiate(_misslePrefab[randomIndex], new Vector3(Random.Range(-10.0f, 10.0f), 0, 10.0f), Quaternion.identity);
        if (_missleType[1] == typeOfMissle)
        {
            int randomIndexY = Random.Range(0, 15);
            Vector3 newPosition = _gridCell._hexagonGrid[0, randomIndexY].transform.position;
            //_misslePrefab[randomIndex].GetComponent<MissleCellController>().Spawn();
            GameObject newMissle = Instantiate(_misslePrefab[randomIndex], new Vector3(newPosition.x, .3f, newPosition.z), Quaternion.identity);
            newMissle.GetComponent<MissleCellController>()._MissleCellPosition.Item1 = 0;
            newMissle.GetComponent<MissleCellController>()._MissleCellPosition.Item2 = randomIndexY;
            newMissle.GetComponent<MissleCellController>().Spawn();

        }
    }
    public void missleDebugSpawner()
    {
        for (int i = 0; i < 30; i++) { Instantiate(_misslePrefab[0], new Vector3(Random.Range(-10.0f, 10.0f), 0, 10.0f), Quaternion.identity); }
    }
}
