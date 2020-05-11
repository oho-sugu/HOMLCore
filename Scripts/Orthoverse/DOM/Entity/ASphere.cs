using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse.DOM.Component;

namespace Orthoverse.DOM.Entity
{
    public class ASphere : EntityBase
    {
        /*
        Components List
        geometry
        material
        */

        private static Dictionary<string, ComponentAttribute> _attrAlias;
        public override void initialize(){
            if(_attrAlias == null){
                _attrAlias = new Dictionary<string, ComponentAttribute>();
                attrAlias = _attrAlias;
                // register alias
                addComponentMapping("material","color");
                addComponentMapping("geometry","radius");
            }
            attrAlias = _attrAlias;
            components.Add("material",ComponentTemplate.getComponent("material"));
            ComponentBase g = ComponentTemplate.getComponent("geometry");
            g.addAttribute("primitive","sphere");
            components.Add("geometry",g);
        }

        public override void Construct(){
            gameObject.name = "A-SPHERE";
        }
    }
}
