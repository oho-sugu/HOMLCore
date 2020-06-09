using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Orthoverse
{
    public static class ParseUtil
    {
        private static char[] separatorsSpacce = new char[] {' '};
        private static char[] separatorsColon = new char[] {':'};
        private static char[] separatorsSemiColon = new char[] {';'};

        public static Vector3 parseVec3(string text){
            var vals = text.Trim().Split(separatorsSpacce);
            
            // TODO 0: zero 1: x 0 0 2: x x 0
            // length not 3 then return default value
            if(vals.Length != 3) return Vector3.zero;

            var retVec = new Vector3(float.Parse(vals[0]),float.Parse(vals[1]),float.Parse(vals[2]));
            return retVec;
        }

        public static Vector2 parseVec2(string text){
            var vals = text.Trim().Split(separatorsSpacce);
            
            // TODO 0: zero 1: x 0 0 2: x x 0
            // length not 3 then return default value
            if(vals.Length != 2) return Vector2.zero;

            var retVec = new Vector2(float.Parse(vals[0]),float.Parse(vals[1]));
            return retVec;
        }

        private static Dictionary<string, UnityEngine.Color> colorDic;
        private static UnityEngine.Color defaultColor = UnityEngine.Color.gray;
        public static UnityEngine.Color parseColor(string colorString){
            if(colorDic == null){
                colorDic = new Dictionary<string, UnityEngine.Color>(10);
                colorDic["WHITE"] = UnityEngine.Color.white;
                colorDic["RED"] = UnityEngine.Color.red;
                colorDic["BLUE"] = UnityEngine.Color.blue;
                colorDic["GREEN"] = UnityEngine.Color.green;
                colorDic["GRAY"] = UnityEngine.Color.gray;
                colorDic["YELLOW"] = UnityEngine.Color.yellow;
                colorDic["MAGENTA"] = UnityEngine.Color.magenta;
                colorDic["CYAN"] = UnityEngine.Color.cyan;
                colorDic["BLACK"] = UnityEngine.Color.black;
            }

            if(colorString == null || colorString == "") return defaultColor;

            var colorstr_ = colorString.ToUpper();
            if (colorDic.ContainsKey(colorstr_))
            {
                return colorDic[colorstr_];
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(colorstr_, @"^#[0-9A-F]{3}$"))
            {
                float r = int.Parse(colorstr_.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                float g = int.Parse(colorstr_.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                float b = int.Parse(colorstr_.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);

                return new UnityEngine.Color(r / 15.0f, g / 15.0f, b / 15.0f);
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(colorstr_, @"^#[0-9A-F]{6}$"))
            {
                float r = int.Parse(colorstr_.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                float g = int.Parse(colorstr_.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                float b = int.Parse(colorstr_.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                return new UnityEngine.Color(r / 255.0f, g / 255.0f, b / 255.0f);
            }
            // return default color
            return defaultColor;
        }

        public static void parseAttribute(string text, ref Dictionary<string, string> attrDic){
            // parse string "key: value; key: value;" to Dictionary
            var firststep = text.Trim().Split(separatorsSemiColon);
            foreach(string str in firststep){
                var nextstep = str.Trim().Split(separatorsColon);
                if(nextstep.Length == 2){
                    attrDic.Add(nextstep[0].Trim(), nextstep[1].Trim());
                } else if(nextstep.Length > 2){
                    string val = nextstep[1].Trim();
                    for(int i = 2;i < nextstep.Length;i++){
                        val += ":"+nextstep[i].Trim();
                    }
                    attrDic.Add(nextstep[0].Trim(), val);
                }
            }
        }

        public static Uri absoluteUri(Uri baseUri, string relUriStr){
            try{
                var uri = new Uri(baseUri, relUriStr);
                return uri;
            } catch(Exception e){
                Debug.Log(relUriStr);
                Debug.Log(e);
            }
            return null;
        }

        public static Uri absoluteUri(Uri baseUri, Uri relUri){
            try{
                var newuri = new Uri(baseUri, relUri);
                return newuri;
            } catch(Exception e){
                Debug.Log(relUri);
                Debug.Log(e);
            }
            return relUri;
        }
    }
}
