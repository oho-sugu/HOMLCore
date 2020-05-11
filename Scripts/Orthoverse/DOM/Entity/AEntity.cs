using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoonSharp.Interpreter;

namespace Orthoverse.DOM.Entity
{
    [MoonSharpUserData]
    public class AEntity : EntityBase
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
            gameObject.name = "A-ENTITY";
        }
    }
}
