using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Entity
{
    public class AScene : EntityBase
    {
        private static Dictionary<string, ComponentAttribute> _attrAlias;
        public override void initialize(){
            if(_attrAlias == null){
                _attrAlias = new Dictionary<string, ComponentAttribute>();
                attrAlias = _attrAlias;
            }
            attrAlias = _attrAlias;
        }

        public override void Construct(){
            gameObject.name = "A-SCENE";
        }
    }
}
