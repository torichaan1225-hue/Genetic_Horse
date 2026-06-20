using Unity.VisualScripting;
using UnityEngine;

public class BoneBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float v_x, v_y, v_z, radius, phase;

    public Rigidbody rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        phase = 0;
    }


    public void SetVelocity(float[] genes,int index_num)
    {

        v_x = genes[index_num];
        v_y = genes[index_num+1];
        v_z = genes[index_num+2];//����킴�킴����Ă�̂�agent�̎��s�Ɛ��F�̂�n�����ے��𕪗������邽��
        phase = 0;//�O�p�֐��̈ʑ�����Z�b�g
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (index_num +GeneticManager.Legnum*3 < GeneticManager.Legnum*GeneticManager.Bonenum*3)//���F�̂̏���͑��̐��~���ꂼ��̃{�[�����~4(�O�����{�����j
        {
            BoneBehavior child = transform.GetChild(0).gameObject.GetComponent<BoneBehavior>();
            child.SetVelocity(genes, index_num + 3*GeneticManager.Legnum);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        phase += Time.deltaTime*2;
        rb.transform.eulerAngles = new Vector3(v_x,v_y,0)*(Mathf.Cos(phase));
    }
}
