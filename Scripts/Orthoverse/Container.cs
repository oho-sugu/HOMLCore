using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse.DOM;

namespace Orthoverse
{
    public class Container : MonoBehaviour
    {
        private LinkedList<Document> documents;

        private LinkedListNode<Document> current;
        private const int MAX_COUNT = 10;

        public void Awake(){
            documents = new LinkedList<Document>();
        }

        private void RemoveAfter(LinkedListNode<Document> kokokara){
            if(kokokara.Next != null){
                RemoveAfter(kokokara.Next);
            }

            var d = kokokara.Value;
            kokokara.List.Remove(d);
            d.Dispose();

            return;
        }

        public void Add(Document d){
            if(current != null){
                current.Value.SetActive(false);
                if(current.Next == null){
                    documents.AddLast(d);
                    current = documents.Last;
                    current.Value.SetActive(true);
                } else {
                    documents.AddAfter(current, d);
                    current = current.Next;
                    current.Value.SetActive(true);
                    RemoveAfter(current.Next);
                }
            } else {
                documents.AddLast(d);
                current = documents.Last;
                current.Value.SetActive(true);
            }

            if(documents.Count > MAX_COUNT){
                var f = documents.First.Value;
                documents.RemoveFirst();
                f.Dispose();
            }
        }

        private bool hasNext(){
            if(current != null){
                return (current.Next != null);
            }
            return false;
        }

        private bool hasPrev(){
            if(current != null){
                return (current.Previous != null);
            }
            return false;
        }
        public void next(){
            if(hasNext()){
                current.Value.SetActive(false);
                current = current.Next;
                current.Value.SetActive(true);
            }
        }
        public void prev(){
            if(hasPrev()){
                current.Value.SetActive(false);
                current = current.Previous;
                current.Value.SetActive(true);
            }
        }

        public Document GetCurrent(){
            if(current != null) return current.Value;
            return null;
        }

        public void Dispose(){
            foreach(Document d in documents){
                d.Dispose();
            }
            Destroy(gameObject);
        }
    }
}
