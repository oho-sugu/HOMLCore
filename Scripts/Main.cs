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
        Parser p = Parser.getInstance();

        string text = @"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>Orthoverse Test</title>
<meta name='description' content='Orthoverse Test'>
<script type='text/lua'>

i = 0;

function start()
return
end

function update()
    i=i+1
    document:GetEntityByID('test'):GetComponentBase('rotation'):Set('x',''..i)
return
end

</script>
</head>
<body>
<a-scene>
<a-box color='red' depth='2' height='4' width='0.5'>
<a-entity geometry='primitive: cone; radiusBottom: 1; radiusTop: 0.1; height: 2;'></a-entity>
<a-entity geometry='primitive: cylinder; height: 3; radius: 2;'></a-entity>
<a-entity geometry='primitive: cylinder; openEnded: true;'></a-entity>
<a-entity geometry='primitive: torus; radius: 2; radiusTubular: 0.2; arc: 180;'></a-entity>
<a-box id='test' color='blue' depth='1' height='1' width='1' position='1 1 1' rotation='30 0 30' scale='1.5 1.5 1.5'>
<a-sphere color='yellow' radius='0.5' position='2 1 1'></a-sphere></a-box>
<a-box animation='property: object3D.position.z; to: 2; dur: 1000; loop: true' color='green' depth='0.5' height='0.5' width='0.5' position='-1 -1 -1'></a-box>
</a-box>
</a-scene>
</body>
        ";
        var d4 = p.parse(text);

        text = File.ReadAllText(@"C:\Users\oho_s\aframe-logo.html");
        var d3 = p.parse(text);
    }
}
