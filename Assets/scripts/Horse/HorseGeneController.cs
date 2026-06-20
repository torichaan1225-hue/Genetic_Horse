using UnityEngine;

public class HorseGeneController : MonoBehaviour
{
    [System.Serializable]
    public class JointGene
    {
        public float amplitude;
        public float frequency;
        public float phase;
    }

    public JointGene[] joints;
    public HingeJoint[] hingeJoints;

    public void ApplyGenome(float[] genome)
    {
        int jointCount = hingeJoints.Length;
        joints = new JointGene[jointCount];

        if (genome == null || genome.Length < jointCount * 3)
        {
            Debug.LogWarning($"genomeの長さがこのモデルの関節数({jointCount})に対して不足しています。不足分は動かないジョイントとして扱います。");
        }

        for (int i = 0; i < jointCount; i++)
        {
            int baseIndex = i * 3;
            if (genome != null && baseIndex + 2 < genome.Length)
            {
                joints[i] = new JointGene
                {
                    amplitude = genome[baseIndex],
                    frequency = genome[baseIndex + 1],
                    phase = genome[baseIndex + 2]
                };
            }
            else
            {
                joints[i] = new JointGene { amplitude = 0f, frequency = 0f, phase = 0f };
            }
        }
    }

    void Update()
    {
        var tracker = GetComponent<HorseRaceTracker>();
        if (tracker == null || !tracker.canMove) return;

        for (int i = 0; i < hingeJoints.Length; i++)
        {
            float targetAngle = joints[i].amplitude *
                Mathf.Sin(Time.time * joints[i].frequency + joints[i].phase);

            var spring = hingeJoints[i].spring;
            spring.targetPosition = targetAngle;
            hingeJoints[i].spring = spring;
        }
    }
}