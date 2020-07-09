using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System;

using Orthoverse.DOM;
using Cysharp.Threading.Tasks;

namespace Orthoverse
{
    public enum OpenMode{
        blank,
        self
    }
    public class DocumentManager : MonoBehaviour
    {
        private static Parser p = Parser.getInstance();

        public string ElementLayerString;
        public string DocumentLayerString;

        public GameObject Cube404;
        public GameObject CubeError;
        
        public static int ElementLayer;
        public static int DocumentLayer;

        void Start(){
            // No Layer is critical. Make 2 layers at preference and Set 2 layer names Element and Document.
            ElementLayer = LayerMask.NameToLayer(ElementLayerString);
            if(ElementLayerString == "" || ElementLayer == -1){
                Debug.Log("Element Layer Not Found. " + ElementLayerString);
                throw new Exception();
            }
            DocumentLayer = LayerMask.NameToLayer(DocumentLayerString);
            if(DocumentLayerString == "" || DocumentLayer == -1){
                Debug.Log("Document Layer Not Found. " + DocumentLayerString);
                throw new Exception();
            }
        }

        private static List<Container> containers = new List<Container>();

        private async UniTask<Document> open(Uri uri, string homl){
            Document d = await p.parse(uri, homl);
            d.gameObject.layer = DocumentLayer;
            return d;
        }

        public void open(Document d,Uri uri,OpenMode mode){
            openDoc(d,uri,mode).Forget();
        }

        private bool flag404 = false;
        private bool flagError = false;

        async UniTaskVoid openDoc(Document d, Uri uri, OpenMode mode){
            // Convert absolute uri if old document given and uri is relative
            if(d != null){
                uri = ParseUtil.absoluteUri(d.uri, uri);
            }
            
            string data = "";
            try{
                data =  await DownloadHOML(uri);
            }catch(UnityWebRequestException e){
                Debug.Log(e);
                if(e.ResponseCode == 404){
                    flag404 = true;
                } else {
                    flagError = true;
                }
            }

            Document newd = await open(uri, data);
            newd.dm = this;

            if(flag404 || flagError){
                if(flag404){
                    Instantiate(Cube404, Vector3.zero, Quaternion.identity, newd.transform);
                }
                if(flagError){
                    Instantiate(CubeError, Vector3.zero, Quaternion.identity, newd.transform);
                }
                newd.resetBoxCollider();
            }

            switch(mode){
                case OpenMode.blank:
                    var containerGameObject = new GameObject("Container");
                    var container = containerGameObject.AddComponent<Container>();
                    container.transform.parent = this.transform;
                    containers.Add(container);
                    container.Add(newd);
                    container.transform.localPosition = 
                        Placement.PlacementManager.GetNewPosition((d != null)? d.transform.parent.localPosition : Vector3.zero);
                    newd.transform.SetParent(container.transform,false);
                    break;
                case OpenMode.self:
                    if(d != null){
                        var _container = d.transform.parent.GetComponent<Container>();
                        _container.Add(newd);
                        newd.transform.SetParent(_container.transform, false);
                    } else {
                        var containerGameObject2 = new GameObject("Container");
                        var container2 = containerGameObject2.AddComponent<Container>();
                        container2.transform.parent = this.transform;
                        containers.Add(container2);
                        container2.Add(newd);
                        container2.transform.localPosition = 
                            Placement.PlacementManager.GetNewPosition((d != null)? d.transform.parent.localPosition : Vector3.zero);
                        newd.transform.SetParent(container2.transform,false);
                    }
                    break;
            }
        }

        async UniTask<string> DownloadHOML(Uri uri) {
            UnityWebRequest req = UnityWebRequest.Get(uri);
            req.downloadHandler = new DownloadHandlerBuffer();

            await req.SendWebRequest();

            if(req.isNetworkError || req.isHttpError){
                Debug.Log(req.error);
                return null;
            } else {
                string data = req.downloadHandler.text;
                return data;
            }
        }

        public void reload(Document target){
            if(target == null) return;
            open(target,target.uri,OpenMode.self);
        }

    }
}
