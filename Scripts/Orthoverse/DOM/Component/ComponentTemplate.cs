using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public class ComponentTemplate
    {
        private static Dictionary<string, ComponentBase> componentTemplate = new Dictionary<string, ComponentBase>();

        public static void addComponentTemplate(ComponentBase c){
            componentTemplate.Add(c.getName().ToLower(), c);
        }

        public static ComponentBase getComponent(string name){
            if(componentTemplate.ContainsKey(name.ToLower())){
                var c = componentTemplate[name.ToLower()];
                return c.newComponent();
            }
            // Do something error handling
            return null;
        }
    }
}
