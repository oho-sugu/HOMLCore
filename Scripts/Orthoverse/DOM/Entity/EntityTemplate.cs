using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Entity
{
    public class EntityTemplate
    {
        private static Dictionary<string, System.Type> entityTemplate = new Dictionary<string, System.Type>();

        public static void addEntityTemplate(string name, System.Type t){
            entityTemplate.Add(name.ToLower(), t);
        }

        public static System.Type getEntity(string name){
            if(entityTemplate.ContainsKey(name.ToLower())){
                var e = entityTemplate[name.ToLower()];
                return e;
            }
            // Do something error handling
            return null;
        }
    }
}
