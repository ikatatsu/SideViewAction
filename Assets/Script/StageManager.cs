using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject initialStage;
    [SerializeField] List<GameObject> stages;
    [SerializeField] float speed = 5.0f;

    private const int stagewidth = 19;
    private const int initialStageCount = 5;
    private List<GameObject> stageListInGame = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        stageListInGame.Add(initialStage);
        for (int i = 0; i < initialStageCount; i++)
        {            
            int index = Random.Range(0, stages.Count);
            GameObject stage = Instantiate(stages[index], new Vector3((i+1)* stagewidth, 0, 0), Quaternion.identity);
            stageListInGame.Add(stage);
        }    
        
    }

    private void Update()
    {
        for (int i = 0; i < stageListInGame.Count; i++)
        {
            stageListInGame[i].transform.position += Vector3.left * speed * Time.deltaTime;

            if (stageListInGame[i].transform.position.x < stagewidth * -2)
            {
                Destroy(stageListInGame[i]);
                stageListInGame.RemoveAt(i);
                int index = Random.Range(0, stages.Count);
                GameObject stage = Instantiate(
                    stages[index], 
                    new Vector3((stageListInGame.Count -1) * stagewidth, 0, 0), 
                    Quaternion.identity);
                stageListInGame.Add(stage);
            }
        }
    }
}
