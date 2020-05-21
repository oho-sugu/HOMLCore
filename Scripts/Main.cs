using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orthoverse;
using System.IO;


public class Main : MonoBehaviour
{
    public string[] urls = new string[2];
    // Start is called before the first frame update
    void Start()
    {
        DocumentManager dm = GetComponent<DocumentManager>();
        for(int i = 0;i < urls.Length;i++){
            dm.open(null,new System.Uri(urls[i]), OpenMode.blank);
        }
    }
}
