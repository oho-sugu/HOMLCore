using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void ValueChange(float v);
public delegate float ValueGet();

public delegate float AnimFunc(float t);

public class AnimatorScalar : MonoBehaviour
{
    private float time = 0f;
    public float delay,dur,from,to;
    public bool setStart, loopInf;
    public int loop;

    public ValueChange vc;
    public ValueGet vg;
    public AnimFunc af;

    // Start is called before the first frame update
    void Start()
    {
        if(setStart){
            from = vg();
        }
    }

    private float oldt = -1f;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float t;
        if(time < delay){
            return;
        } else if(!loopInf) {
            t = (time - delay) % dur;
            if(t < oldt){
                loop = Mathf.Max(0,loop-1);
                if(loop <= 0) return;
            }
            oldt = t;
        } else {
            t = (time - delay) % dur;
        }
        vc(from + (to - from) * af(t/dur));
    }
}
