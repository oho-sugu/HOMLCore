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

        private static List<Document> documents = new List<Document>();

        private async UniTask<Document> open(Uri uri, string homl){
            Document d = await p.parse(uri, homl);
            d.gameObject.layer = DocumentLayer;
            documents.Add(d);
            return d;
        }

        public void open(Document d,Uri uri,OpenMode mode){
            openDoc(d,uri,mode).Forget();
        }

        async UniTaskVoid openDoc(Document d, Uri uri, OpenMode mode){
            // Convert absolute uri if old document given and uri is relative
            if(d != null){
                uri = ParseUtil.absoluteUri(d.uri, uri);
            }
            
            string data =  await DownloadHOML(uri);
            Document newd = await open(uri, data);
            newd.gameObject.transform.parent = this.transform;
            newd.dm = this;

            switch(mode){
                case OpenMode.blank:
                    // TODO Placement Manager
                    newd.transform.localPosition = ((d != null)? d.transform.localPosition : Vector3.zero) + UnityEngine.Random.onUnitSphere * 0.3f;
                    break;
                case OpenMode.self:
                    if(d != null){
                        documents.Remove(d);
                        newd.transform.localPosition = d.transform.localPosition;
                        newd.transform.localRotation = d.transform.localRotation;
                        newd.transform.localScale = d.transform.localScale;
                        // TODO d dispose
                        // TODO Remove SetActive
                        d.Dispose();
                    } else {
                        newd.transform.localPosition = ((d != null)? d.transform.localPosition : Vector3.zero) + UnityEngine.Random.onUnitSphere * 0.3f;
                    }
                    break;
            }
        }

        async UniTask<string> DownloadHOML(Uri uri){
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
            open(target,target.uri,OpenMode.self);
        }

    }
}
