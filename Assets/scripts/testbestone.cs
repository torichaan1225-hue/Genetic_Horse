using JetBrains.Annotations;
using UnityEngine;

public class testbestone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float[] genes ={};
    public GameObject objprefab;
    public GameObject agent;
    void Start()
    {
        agent =  Instantiate(objprefab, new Vector3(0,0,0), Quaternion.identity);
        agent.GetComponent<Agent>().assignv(genes);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
