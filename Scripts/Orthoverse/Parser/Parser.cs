using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Runtime.CompilerServices;
using System;

using Orthoverse.DOM;
using Orthoverse.DOM.Component;
using Orthoverse.DOM.Entity;

using Cysharp.Threading.Tasks;

using UnityEngine.Networking;

namespace Orthoverse
{
    public class Parser
    {
        private static Parser _instance = new Parser();

        public static Parser getInstance(){
            return _instance;
        }

        private Parser(){
            // Initialize elementsTemplate
            ComponentFactory.init();
            EntityFactory.init();
        }

        async UniTask<Texture2D> downloadTexture(Uri uri){
            var uwr = UnityWebRequestTexture.GetTexture(uri);

            try {
                await uwr.SendWebRequest();
            } catch(UnityWebRequestException e){
                Debug.Log(e);
                Debug.Log(e.ResponseCode);
            }

            if(uwr.isNetworkError || uwr.isHttpError){
                Debug.Log(uwr.error);
                return null;
            } else {
                return DownloadHandlerTexture.GetContent(uwr);
            }
        }

        async UniTask<byte[]> downloadData(Uri uri){
            var uwr = UnityWebRequest.Get(uri);

            try {
                await uwr.SendWebRequest();
            } catch(UnityWebRequestException e){
                Debug.Log(e);
                Debug.Log(e.ResponseCode);
            }

            if(uwr.isNetworkError || uwr.isHttpError){
                Debug.Log(uwr.error);
                return null;
            } else {
                return uwr.downloadHandler.data;
            }
        }
    
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public async UniTask<Document> parse(Uri uri, string homl){
            var htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(homl);

            var docGO = new GameObject();
            var doc = docGO.AddComponent<Document>() as Document;
            doc.uri = uri;

            var assets = htmldoc.DocumentNode.SelectNodes(@"//a-assets/*");
            if(assets != null){
                foreach(HtmlNode n in assets){
                    string id;
                    switch(n.Name){
                        case "img":
                            id = n.GetAttributeValue("id","");
                            if(id != ""){
                                if(!doc.textures.ContainsKey(id)){
                                    string src = n.GetAttributeValue("src","");
                                    if(src!=""){
                                        var srcuri = ParseUtil.absoluteUri(doc.uri, src);
                                        if(srcuri != null) {
                                            var tex = await downloadTexture(srcuri);
                                            if(tex != null) { doc.textures.Add(id, tex); }
                                        }
                                    }
                                } else {
                                    Debug.Log("Duplicate key for textures");
                                }
                            }
                            break;
                        case "a-asset-item":
                            id = n.GetAttributeValue("id","");
                            if(id != ""){
                                if(!doc.assetItems.ContainsKey(id)){
                                    string src = n.GetAttributeValue("src","");
                                    if(src!=""){
                                        var srcuri = ParseUtil.absoluteUri(doc.uri, src);
                                        if(srcuri != null) {
                                            var data = await downloadData(srcuri);
                                            if(data != null) { doc.assetItems.Add(id, data); }
                                        }
                                    }
                                } else {
                                    Debug.Log("Duplicate key for asset-item");
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            var scripts = htmldoc.DocumentNode.SelectNodes(@"//script");
            if(scripts != null){
                foreach(HtmlNode n in scripts){
                    if(n.GetAttributeValue("type","") == "text/lua"){
                        doc.addScript(n.InnerText);
                    }
                }
            }

            doc.init(parseRecurse(htmldoc.DocumentNode.ChildNodes, doc),homl);

            return doc;
        }

        private EntityBase[] parseRecurse(HtmlNodeCollection nodes, Document doc){
            List<EntityBase> entities = new List<EntityBase>();
            foreach(HtmlNode node in nodes){
                System.Type entityType;
                if((entityType = EntityTemplate.getEntity(node.Name)) != null){
                    GameObject g = new GameObject();
                    g.layer = DocumentManager.ElementLayer;
                    EntityBase entity;
                    entity = g.AddComponent(entityType) as EntityBase;
                    entity.initialize();
                    entity.ParseAttributes(node);
                    entity.Construct(doc);

                    var childs = parseRecurse(node.ChildNodes, doc);
                    foreach(EntityBase child in childs){
                        child.gameObject.transform.SetParent(entity.gameObject.transform,false);
                        entity.addChild(child);
                    }

                    if(node.Id != null && node.Id != ""){
                        doc.addIDList(node.Id, entity);
                    }
                    entities.Add(entity);
                } else {
                    if(node.Name == "#comment" || node.Name == "#text"){
                        //Debug.Log(node.OuterHtml);
                    } else if(node.Name == "html") {
                        // Do something for html tag
                    } else if(node.Name == "head") {
                        // Do something for head tag
                    } else if(node.Name == "title") {
                        if(node.InnerHtml != null || node.InnerHtml.Length > 0){
                            doc.title = node.InnerHtml;
                        }
                    } else if(node.Name == "body") {
                        var boundsAttr = node.GetAttributeValue("bounds","");
                        if(boundsAttr != ""){
                            Dictionary<string,string> attrdic = new Dictionary<string, string>();
                            ParseUtil.parseAttribute(boundsAttr, ref attrdic);
                            string center = attrdic.TryGetValue("center", out center) ? center : "";
                            string size = attrdic.TryGetValue("size", out size) ? size : "";
                            if(center != "" && size != ""){
                                doc.bounds.center = ParseUtil.parseVec3(center);
                                doc.bounds.size = ParseUtil.parseVec3(size);
                                doc.boundsGiven = true;
                            }
                        }
                    } else {
                        //Debug.Log(node.Name + " Not found. Do you register EntityFactory?");
                    }
                    entities.AddRange(parseRecurse(node.ChildNodes, doc));
                }
            }
            return entities.ToArray();
        }
    }
}
  