using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orthoverse.DOM.Component
{
    public class ComponentFactory
    {
        public static void init(){
            var position = new Position();
            position.initialize();
            ComponentTemplate.addComponentTemplate(position);

            var rotation = new Rotation();
            rotation.initialize();
            ComponentTemplate.addComponentTemplate(rotation);

            var scale = new Scale();
            scale.initialize();
            ComponentTemplate.addComponentTemplate(scale);
            
            var material = new Material();
            material.initialize();
            ComponentTemplate.addComponentTemplate(material);

            var geometry = new Geometry();
            geometry.initialize();
            ComponentTemplate.addComponentTemplate(geometry);

            var visible = new Visible();
            visible.initialize();
            ComponentTemplate.addComponentTemplate(visible);

            var animation = new Animation();
            animation.initialize();
            ComponentTemplate.addComponentTemplate(animation);

            var link = new Link();
            link.initialize();
            ComponentTemplate.addComponentTemplate(link);
        }
    }
}