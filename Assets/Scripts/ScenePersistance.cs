using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersistance : MonoBehaviour
{
    private void Awake()
    {
        int numScenePersists = FindObjectsOfType<ScenePersistance>().Length;
        if (numScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetPersistance()
    {
        Destroy(gameObject);
    }
}
