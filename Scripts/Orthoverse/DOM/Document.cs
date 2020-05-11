using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM
{
    [MoonSharpUserData]
    public class Document : MonoBehaviour
    {
        // private script
        private EntityBase[] rootEntity;
        private string originalHoml;
        public string script;
        private Script _scriptContext;

        private Dictionary<string, EntityBase> EntityByID = new Dictionary<string, EntityBase>();

        public void init(EntityBase[] roots, string original){
            gameObject.name = "Document";
            rootEntity = roots;
            originalHoml = original;
            foreach(EntityBase node in roots){
                node.gameObject.transform.SetParent(this.gameObject.transform,false);
            }
        }

        public void addScript(string str){
            script += str;
        }

        public void addIDList(string id, EntityBase entity){
            EntityByID.Add(id,entity);
        }

        public EntityBase GetEntityByID(string id){
            EntityBase e;
            e = EntityByID.TryGetValue(id,out e) ? e : null;
            return e;
        }

        // Start is called before the first frame update
        void Start()
        {
            _scriptContext = new Script();
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            _scriptContext.Globals["document"] = this;
            if(script != null) _scriptContext.DoString(script);
            
            try{
                _scriptContext.Call(_scriptContext.Globals["start"]);
            } catch (Exception e){
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            try{
                _scriptContext.Call(_scriptContext.Globals["update"]);
            } catch (Exception e){
                
            }
        }
    }
}
