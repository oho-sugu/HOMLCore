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

            // Refer https://easings.net/ja#
            switch(easingStr.ToLower()){
                case "linear":
                    anim.af = (float t) => { return t; };
                    break;

                case "easeinsine":
                    anim.af = (float t) => { return (1f - Mathf.Cos((t * Mathf.PI)/2f)); };
                    break;
                case "easeincubic":
                    anim.af = (float t) => { return t*t*t; };
                    break;
                case "easeinquint":
                    anim.af = (float t) => { return t*t*t*t*t; };
                    break;
                case "easeincirc":
                    anim.af = (float t) => { return (1f - Mathf.Sqrt(1f - t*t)); };
                    break;
                case "easeinelastic":
                    anim.af = (float t) => { float c4 = (2f * Mathf.PI) / 3f; return t == 0f ? 0f : (t == 1f ? 1f : -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * c4)); };
                    break;
                case "easeinquad":
                    anim.af = (float t) => { return t*t; };
                    break;
                case "easeinquart":
                    anim.af = (float t) => { return t*t*t*t; };
                    break;
                case "easeinexpo":
                    anim.af = (float t) => { return t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f); };
                    break;
                case "easeinback":
                    anim.af = (float t) => { float c1 = 1.70158f; float c3 = c1+1f; return c3*t*t*t - c1*t*t; };
                    break;
                case "easeinbounce":
                    anim.af = (float t) => { return 1f - easeOutBounce(1f - t); };
                    break;

                case "easeoutsine":
                    anim.af = (float t) => { return Mathf.Sin((t * Mathf.PI)/2f); };
                    break;
                case "easeoutcubic":
                    anim.af = (float t) => { t = 1f-t; return 1f - t*t*t; };
                    break;
                case "easeoutquint":
                    anim.af = (float t) => { t = 1f-t; return 1f - t*t*t*t*t; };
                    break;
                case "easeoutcirc":
                    anim.af = (float t) => { t = t-1f; return Mathf.Sqrt(1f - t*t); };
                    break;
                case "easeoutelastic":
                    anim.af = (float t) => { float c4 = (2f * Mathf.PI) / 3f; return t == 0f ? 0f : (t == 1f ? 1f : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) +1f); };
                    break;
                case "easeoutquad":
                    anim.af = (float t) => { t = 1f-t; return 1f - t*t; };
                    break;
                case "easeoutquart":
                    anim.af = (float t) => { t = 1f-t; return 1f - t*t*t*t; };
                    break;
                case "easeoutexpo":
                    anim.af = (float t) => { return t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t); };
                    break;
                case "easeoutback":
                    anim.af = (float t) => { float c1 = 1.70158f; float c3 = c1+1f; t = t - 1f; return 1f + c3*t*t*t + c1*t*t; };
                    break;
                case "easeoutbounce":
                    anim.af = (float t) => { return easeOutBounce(t); };
                    break;

                case "easeinoutsine":
                    anim.af = (float t) => { return -(Mathf.Cos(t * Mathf.PI)-1f)/2f; };
                    break;
                case "easeinoutcubic":
                    anim.af = (float t) => { float _t = -2f*t+2f; return t < 0.5f ? 4f*t*t*t : 1f - _t*_t*_t/2f; };
                    break;
                case "easeinoutquint":
                    anim.af = (float t) => { float _t = -2f*t+2f; return t < 0.5f ? 16f*t*t*t*t*t : 1f - _t*_t*_t*_t*_t/2f; };
                    break;
                case "easeinoutcirc":
                    anim.af = (float t) => { return t < 0.5f ? (1f - Mathf.Sqrt(1f - 4f*t*t))/2f : (Mathf.Sqrt(1f - Mathf.Pow(-2f*t+2f,2f))+1f)/2f; };
                    break;
                case "easeinoutelastic":
                    anim.af = (float t) => { float c5 = (2f * Mathf.PI) / 4.5f;
                        return t == 0f ? 0f :
                              (t == 1f ? 1f :
                              (t < 0.5f ?
                                 -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((t * 20f - 11.125f) * c5)) / 2f :
                                 (Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((t * 20f - 11.125f) * c5)) / 2f + 1f)); };
                    break;
                case "easeinoutquad":
                    anim.af = (float t) => { float _t = -2f*t+2f; return t < 0.5f ? 2f*t*t : 1f - _t*_t/2f; };
                    break;
                case "easeinoutquart":
                    anim.af = (float t) => { float _t = -2f*t+2f; return t < 0.5f ? 8f*t*t*t*t : 1f - _t*_t*_t*_t/2f; };
                    break;
                case "easeinoutexpo":
                    anim.af = (float t) => { return t == 0f ? 0f : ( t == 1f ? 1f : (t < 0.5f ? 
                    Mathf.Pow(2f, 20f * t - 10f)/2f :
                    (2f - Mathf.Pow(2f, -20f * t + 10f))/2f)); };
                    break;
                case "easeinoutback":
                    anim.af = (float t) => { float c1 = 1.70158f; float c2 = c1*1.525f; return t < 0.5f 
                        ? 2f*t*t*((c2+1f)*2f*t-c2)
                        : (Mathf.Pow(2f*t-2f,2f)*((c2+1f)*(t*2-2f)+c2)+2f)/2f; };
                    break;
                case "easeinoutbounce":
                    anim.af = (float t) => { return t < 0.5f 
                        ? (1f - easeOutBounce(1f - 2f*t)) / 2f
                        : (1f + easeOutBounce(2f*t - 1f)) / 2f; };
                    break;

                default:
                    anim.af = (float t) => { return t; };
                    break;
            }
        }

        private static float easeOutBounce(float x) {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (x < 1f / d1) {
                return n1 * x * x;
            } else if (x < 2f / d1) {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            } else if (x < 2.5f / d1) {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            } else {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }
    }

}
