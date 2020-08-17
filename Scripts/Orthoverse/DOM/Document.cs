using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

using Orthoverse.DOM.Entity;

namespace Orthoverse.DOM
{
    public delegate void OnClick();
    public delegate void OnOver();
    public delegate void OnOut();
    [MoonSharpUserData]
    public class Document : MonoBehaviour
    {
        // private script
        private EntityBase[] rootEntity;
        public string title;
        private string originalHoml;
        public Uri uri;
        public string script;
        private Script _scriptContext;
        public OnClick event_click;
        public OnOver event_over;
        public OnOut event_out;

        public Bounds bounds;
        public bool boundsGiven = false;

        private Dictionary<string, EntityBase> EntityByID = new Dictionary<string, EntityBase>();

        public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, byte[]> assetItems = new Dictionary<string, byte[]>();

        public DocumentManager dm;

        [MoonSharpHidden]
        public void init(EntityBase[] roots, string original){
            gameObject.name = "Document";
            rootEntity = roots;
            originalHoml = original;
            foreach(EntityBase node in roots){
                node.gameObject.transform.SetParent(this.gameObject.transform,false);
            }
            var bc = gameObject.AddComponent<BoxCollider>();
            if(!boundsGiven){
                bounds = new Bounds(this.transform.position,Vector3.zero);
                recurseBounds(ref bounds, this.transform);
            }
            bc.center = bounds.center;
            bc.size = bounds.size;
            bc.isTrigger = true;
            bc.enabled = true;
        }

        [MoonSharpHidden]
        public void resetBoxCollider(){
            var bc = gameObject.GetComponent<BoxCollider>();
            if(!boundsGiven){
                bounds = new Bounds(this.transform.position,Vector3.zero);
                recurseBounds(ref bounds, this.transform);
            }
            bc.center = bounds.center;
            bc.size = bounds.size;
            bc.isTrigger = true;
            bc.enabled = true;
        }

        [MoonSharpHidden]
        private void recurseBounds(ref Bounds bounds, Transform parent){
            int count = parent.childCount;
            for(int i= 0; i < count; i++){
                recurseBounds(ref bounds, parent.GetChild(i));
            }
            var renderer = parent.gameObject.GetComponent<Renderer>();
            if(renderer != null){
                bounds.Encapsulate(renderer.bounds);
            }
        }

        [MoonSharpHidden]
        public void addScript(string str){
            script += str;
        }

        [MoonSharpHidden]
        public void addIDList(string id, EntityBase entity){
            EntityByID.Add(id,entity);
        }

        public EntityBase GetEntityByID(string id){
            EntityBase e;
            e = EntityByID.TryGetValue(id,out e) ? e : null;
            return e;
        }

        // Start is called before the first frame update
        [MoonSharpHidden]
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

        private bool hasUpdate = true;
        // Update is called once per frame
        [MoonSharpHidden]
        void Update()
        {
            if(hasUpdate){
                try{
                    _scriptContext.Call(_scriptContext.Globals["update"]);
                } catch (Exception e){
                    hasUpdate = false;
                }
            }
        }

        [MoonSharpHidden]
        public void RunScript(string _scr){
            _scriptContext.DoString(_scr);

        }

        [MoonSharpHidden]
        public void Dispose(){
            Destroy(gameObject);
        }
        [MoonSharpHidden]
        public void OnClickHandler(){
            event_click?.Invoke();
        }
        [MoonSharpHidden]
        public void OnOverHandler(){
            event_over?.Invoke();
        }
        [MoonSharpHidden]
        public void OnOutHandler(){
            event_out?.Invoke();
        }

        public void SetActive(bool b){
            gameObject.SetActive(b);
        }
    }
}
