using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse.DOM.Component;

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
                addComponentMapping("material","color");
            }
            attrAlias = _attrAlias;
            components.Add("material",ComponentTemplate.getComponent("material"));
        }

        public override void Construct(){
            gameObject.name = "A-ENTITY";
        }
    }
}
