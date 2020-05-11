using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Entity
{
    public class EntityFactory
    {
        public static void init(){
            EntityTemplate.addEntityTemplate("a-box",typeof(ABox));
            EntityTemplate.addEntityTemplate("a-sphere",typeof(ASphere));
            EntityTemplate.addEntityTemplate("a-plane",typeof(APlane));
            EntityTemplate.addEntityTemplate("a-entity",typeof(AEntity));
            EntityTemplate.addEntityTemplate("a-scene",typeof(AScene));
        }
    }
}