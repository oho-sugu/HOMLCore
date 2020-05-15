using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System;

using Orthoverse.DOM;

namespace Orthoverse
{
    public enum OpenMode{
        blank,
        self
    }
    public class DocumentManager : MonoBehaviour
    {
        private static Parser p = Parser.getInstance();
        
        private static List<Document> documents = new List<Document>();

        private Document open(Uri uri, string homl){
            Document d = p.parse(homl);
            d.uri = uri;
            documents.Add(d);
            return d;
        }

        public void open(Document d,Uri uri,OpenMode mode){
            StartCoroutine(openDoc(d,uri,mode));
        }

        IEnumerator openDoc(Document d, Uri uri, OpenMode mode){
            var cr = DownloadHOML(uri);
            yield return StartCoroutine(cr);

            string data = (string)cr.Current;
            Document newd = open(uri, data);
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

        IEnumerator DownloadHOML(Uri uri){
            UnityWebRequest req = UnityWebRequest.Get(uri);
            req.downloadHandler = new DownloadHandlerBuffer();
            yield return req.SendWebRequest();

            if(req.isNetworkError || req.isHttpError){
                Debug.Log(req.error);
                yield return null;
            } else {
                string data = req.downloadHandler.text;
                yield return data;
            }
        }

        public void reload(Document target){
            open(target,target.uri,OpenMode.self);
        }

    }
}
