using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM.Component
{
    public class Text : ComponentBase
    {
        public TextMeshPro tmp;
        private float width,height;
        
        // The Magic Value for font sizing. TODO Find collect calculation method for font size.
        private const float magic = 3.4f;
        private static string _name = "text";

        public override void initialize(){
            this.name = _name;
        }

        public override ComponentBase newComponent(){
            var c = new Text();
            c.initialize();
            return c;
        }
        public override void Parse(string value){
            ParseUtil.parseAttribute(value, ref attrDic);
        }

        public override void Construct(EntityBase e){
            string value;
            string textValue = attrDic.TryGetValue("value", out textValue) ? textValue : "Hello World";
            width = attrDic.TryGetValue("width", out value) ? float.Parse(value) : 1f;
            height = attrDic.TryGetValue("height", out value) ? float.Parse(value) : 1f;
            float wrapCount = attrDic.TryGetValue("wrapCount", out value) ? float.Parse(value) : 40f;
            string stralign = attrDic.TryGetValue("align", out stralign) ? stralign : "left";
            string strvalign = attrDic.TryGetValue("valign", out strvalign) ? strvalign : "top";
            string strcolor = attrDic.TryGetValue("color", out strcolor) ? strcolor : "#FFFFFF";

            tmp = e.gameObject.AddComponent<TextMeshPro>();
            tmp.text = textValue;

            float textSize = tmp.font.faceInfo.lineHeight * width / wrapCount / magic;
            tmp.fontSize = textSize;

            var rect = parent.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(width,height);

            int align = 0;
            switch(stralign){
                case "left":
                    align |= (int)HorizontalAlignmentOptions.Left;
                    break;
                case "right":
                    align |= (int)HorizontalAlignmentOptions.Right;
                    break;
                case "center":
                    align |= (int)HorizontalAlignmentOptions.Center;
                    break;
                case "justified":
                    align |= (int)HorizontalAlignmentOptions.Justified;
                    break;
                case "flush":
                    align |= (int)HorizontalAlignmentOptions.Flush;
                    break;
                case "geometry":
                    align |= (int)HorizontalAlignmentOptions.Geometry;
                    break;
                default:
                    align |= (int)HorizontalAlignmentOptions.Left;
                    break;
            }
            switch(strvalign){
                case "top":
                    align |= (int)VerticalAlignmentOptions.Top;
                    break;
                case "middle":
                    align |= (int)VerticalAlignmentOptions.Middle;
                    break;
                case "bottom":
                    align |= (int)VerticalAlignmentOptions.Bottom;
                    break;
                case "baseline":
                    align |= (int)VerticalAlignmentOptions.Baseline;
                    break;
                case "midline":
                    align |= (int)VerticalAlignmentOptions.Geometry;
                    break;
                case "capline":
                    align |= (int)VerticalAlignmentOptions.Capline;
                    break;
                default:
                    align |= (int)VerticalAlignmentOptions.Top;
                    break;
            }
            tmp.alignment = (TextAlignmentOptions)align;

            tmp.color = ParseUtil.parseColor(strcolor);
        }
    }
}
