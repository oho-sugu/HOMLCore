using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orthoverse;
using System.IO;


public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DocumentManager dm = GetComponent<DocumentManager>();
        dm.open(null,new System.Uri("https://gist.githubusercontent.com/oho-sugu/553c82d5e2ab84eb141721c8e3a19350/raw/cdb0cfe985de7cab39fcc18e88fe3ec7928a693e/Test.homl"), OpenMode.blank);
        dm.open(null,new System.Uri("https://gist.githubusercontent.com/oho-sugu/d883d567ab1571a082713d074150dcb0/raw/a7d167e20186484661853eaf1157f2f4027666fe/gistfile1.txt"), OpenMode.blank);
    }
}
