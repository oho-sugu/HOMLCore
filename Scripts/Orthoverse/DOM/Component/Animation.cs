using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Animation : ComponentBase
    {
        private static string _name = "animation";

        private static Dictionary<string,int> coordIndex;

        public override void initialize()
        {
            this.name = _name;
            if(coordIndex==null){
                coordIndex = new Dictionary<string, int>(3);
                coordIndex.Add("x",0);
                coordIndex.Add("y",1);
                coordIndex.Add("z",2);
            }
        }

        public override ComponentBase newComponent()
        {
            var c = new Animation();
            c.initialize();
            return c;
        }

        public override void Parse(string node)
        {
            ParseUtil.parseAttribute(node, ref attrDic);
        }

        public override void Construct(EntityBase e)
        {
            string propertyStr = attrDic.TryGetValue("property", out propertyStr) ? propertyStr : "";
            string fromStr = attrDic.TryGetValue("from", out fromStr) ? fromStr : "";
            string toStr = attrDic.TryGetValue("to", out toStr) ? toStr : "";
            string delayStr = attrDic.TryGetValue("delay", out delayStr) ? delayStr : "0";
            string durStr = attrDic.TryGetValue("dur", out durStr) ? durStr : "";
            string loopStr = attrDic.TryGetValue("loop", out loopStr) ? loopStr : "";
            string easingStr = attrDic.TryGetValue("easing", out easingStr) ? easingStr : "linear";

            float duration = (float.TryParse(durStr, out duration) ? duration : 1000f) / 1000f;
            float delay = (float.TryParse(delayStr, out delay) ? delay : 0f) / 1000f;

            var anim = e.gameObject.AddComponent<AnimatorScalar>();
            anim.delay = delay;
            anim.dur = duration;

            float from;
            if (float.TryParse(fromStr, out from))
            {
                anim.from = from;
                anim.setStart = false;
            }
            else
            {
                anim.setStart = true;
            }

            anim.to = float.TryParse(toStr, out anim.to) ? anim.to : 0f;

            if (loopStr == "true")
            {
                anim.loopInf = true;
            }
            else
            {
                anim.loopInf = false;
                int loop = int.TryParse(loopStr, out loop) ? loop : 1;
                anim.loop = loop;
            }

            propertyStr = propertyStr.Trim().ToLower().Replace("object3d.", "");
            string[] propertyStrs = propertyStr.Split(new char[] { '.' });

            switch (propertyStrs[0])
            {
                case "position":
                    anim.vc = (float v) =>
                    {
                        var vec3 = e.gameObject.transform.localPosition;
                        vec3[coordIndex[propertyStrs[1]]] = v;
                        e.gameObject.transform.localPosition = vec3;
                    };
                    anim.vg = () => { return e.gameObject.transform.localPosition[coordIndex[propertyStrs[1]]]; };
                    break;
                case "rotation":
                    anim.vc = (float v) =>
                    {
                        var vec3 = e.gameObject.transform.localEulerAngles;
                        vec3[coordIndex[propertyStrs[1]]] = v;
                        e.gameObject.transform.localEulerAngles = vec3;
                    };
                    anim.vg = () => { return e.gameObject.transform.localEulerAngles[coordIndex[propertyStrs[1]]]; };
                    break;
                case "scale":
                    anim.vc = (float v) =>
                    {
                        var vec3 = e.gameObject.transform.localScale;
                        vec3[coordIndex[propertyStrs[1]]] = v;
                        e.gameObject.transform.localScale = vec3;
                    };
                    anim.vg = () => { return e.gameObject.transform.localScale[coordIndex[propertyStrs[1]]]; };
                    break;
                default:
                    anim.vc = (float v) => {};
                    anim.vg = () => {return 0f;};
                    break;
            }

            switch(easingStr.ToLower()){
                case "linear":
                    anim.af = (float t, float f, float x, float d) => { return f + (x - f) * (t / d); };
                    break;
                case "easeoutcubic":
                    anim.af = (float t, float f, float x, float d) => { float _t = t/d -1f; return f + (x - f) * (_t*_t*_t + 1f); };
                    break;
                default:
                    anim.af = (float t, float f, float x, float d) => { return f + (x - f) * (t / d); };
                    break;
            }
        }
    }

}
