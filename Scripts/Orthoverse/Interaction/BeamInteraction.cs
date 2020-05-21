using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Orthoverse;
using Orthoverse.DOM;
using Orthoverse.DOM.Entity;

namespace Orthoverse.Interaction
{
    public class BeamInteraction : MonoBehaviour
    {
        private bool trigger = false;
        private Transform PrevElementTransform = null;
        private Transform PrevDocumentTransform = null;
        const float MAX_DISTANCE = 200f;

        private RaycastHit hit_infoE = new RaycastHit ();
        private RaycastHit hit_infoD = new RaycastHit ();

        public void hitCheck(Ray ray){
            hitCheck(
                Physics.Raycast (ray, out hit_infoE, MAX_DISTANCE,(1 << DocumentManager.ElementLayer)),
                Physics.Raycast (ray, out hit_infoD, MAX_DISTANCE,(1 << DocumentManager.DocumentLayer))
            );
        }
        public void hitCheck(Vector3 origin, Vector3 direction){
            hitCheck(
                Physics.Raycast (origin, direction, out hit_infoE, MAX_DISTANCE,(1 << DocumentManager.ElementLayer)),
                Physics.Raycast (origin, direction, out hit_infoD, MAX_DISTANCE,(1 << DocumentManager.DocumentLayer))
            );
        }

        // Update is called once per frame
        private void hitCheck(bool is_hitE, bool is_hitD)
        {
            if (is_hitE) {
                if(PrevElementTransform != hit_infoE.transform){
                    if(PrevElementTransform != null){
                        PrevElementTransform.gameObject.GetComponent<EntityBase>().OnOutHandler();
                    }
                    hit_infoE.transform.gameObject.GetComponent<EntityBase>().OnOverHandler();
                }
                if(trigger){
                    hit_infoE.transform.gameObject.GetComponent<EntityBase>().OnClickHandler();
                    trigger = false;
                }
                PrevElementTransform = hit_infoE.transform;
            } else if(PrevElementTransform != null){
                PrevElementTransform.gameObject.GetComponent<EntityBase>().OnOutHandler();
                PrevElementTransform = null;
            }

            if (is_hitD) {
                if(PrevDocumentTransform != hit_infoD.transform){
                    if(PrevDocumentTransform != null){
                        PrevDocumentTransform.gameObject.GetComponent<Document>().OnOutHandler();
                    }
                    hit_infoD.transform.gameObject.GetComponent<Document>().OnOverHandler();
                }
                if(trigger){
                    hit_infoD.transform.gameObject.GetComponent<Document>().OnClickHandler();
                    trigger = false;
                }
                PrevDocumentTransform = hit_infoD.transform;
            } else if(PrevDocumentTransform != null){
                PrevDocumentTransform.gameObject.GetComponent<Document>().OnOutHandler();
                PrevDocumentTransform = null;
            }
        }

        public void SetTrigger(bool _trigger){
            trigger = _trigger;
        }
    }
}
