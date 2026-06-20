using System;
using UnityEngine;
public class Agent: MonoBehaviour
{
    public BoneBehavior[] bones;
    public void assignv(float[] genes)
    {
        for (int i = 0; i < GeneticManager.Legnum; i++)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            bones[i] = transform.GetChild(i).gameObject.GetComponent<BoneBehavior>();
            bones[i].GetComponent<BoneBehavior>().SetVelocity(genes, i * 3);
        }
    }
}
