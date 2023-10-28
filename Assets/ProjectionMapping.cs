using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionMapping : MonoBehaviour {
    public Camera personCam;
    public RenderTexture personRT;
    public int textureWidth, textureHeight;
    int at = 0;
    Texture2D texture;
    Texture2D personTex;
    float[,] ii, jj;
    Color[] cols;
    // Use this for initialization
    void Start() {
        texture = new Texture2D(textureHeight, textureWidth);
        personTex = new Texture2D(personRT.width, personRT.height);
        ii = new float[textureHeight, textureWidth];
        jj = new float[textureHeight, textureWidth];

        GetComponent<Renderer>().material.mainTexture = texture;
        for (int i = 0; i < textureWidth; i++)
        { 
            for (int j = 0; j < textureHeight; j++)
            {
                ii[j, i] = i * 1.0f / textureWidth - 0.5f;
                jj[j, i] = j * 1.0f / textureHeight - 0.5f;
            }
        }
        cols = new Color[textureHeight*textureWidth];
    }

    // Update is called once per frame
    void Update() {

        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;
        // Set the supplied RenderTexture as the active one
        RenderTexture.active = personRT;
        personTex.ReadPixels(new Rect(0, 0, personTex.width, personTex.height), 0, 0);
        RenderTexture.active = currentActiveRT;

        for (int i = 0; i < texture.width; i++)
        { 
            for (int j = 0; j < texture.height; j++)
            {
                //Get world point
                Vector3 worldPoint = transform.TransformPoint(ii[j, i], jj[j, i], 0);

                //Get PersonCam pixel location and texture.
                Vector3 screen = personCam.WorldToScreenPoint(worldPoint);
                float sx = screen.x * personTex.width / personCam.pixelWidth;
                float sy = screen.y * personTex.height / personCam.pixelHeight;
                cols[j * textureWidth + i] = personTex.GetPixel((int)sx, (int)sy);
            }
        }

        texture.SetPixels(cols);
        texture.Apply();

    }

}
/* Might be useful
 * function UvTo3D(uv: Vector2): Vector3 {
   var mesh: Mesh = GetComponent(MeshFilter).mesh;
   var tris: int[] = mesh.triangles;
   var uvs: Vector2[] = mesh.uv;
   var verts: Vector3[] = mesh.vertices;
   for (var i: int = 0; i < tris.length; i += 3){
     var u1: Vector2 = uvs[tris[i]]; // get the triangle UVs
     var u2: Vector2 = uvs[tris[i+1]];
     var u3: Vector2 = uvs[tris[i+2]];
     // calculate triangle area - if zero, skip it
     var a: float = Area(u1, u2, u3); if (a == 0) continue;
     // calculate barycentric coordinates of u1, u2 and u3
     // if anyone is negative, point is outside the triangle: skip it
     var a1: float = Area(u2, u3, uv)/a; if (a1 < 0) continue;
     var a2: float = Area(u3, u1, uv)/a; if (a2 < 0) continue;
     var a3: float = Area(u1, u2, uv)/a; if (a3 < 0) continue;
     // point inside the triangle - find mesh position by interpolation...
     var p3D: Vector3 = a1*verts[tris[i]]+a2*verts[tris[i+1]]+a3*verts[tris[i+2]];
     // and return it in world coordinates:
     return transform.TransformPoint(p3D);
   }
   // point outside any uv triangle: return Vector3.zero
   return Vector3.zero;
 }
 
 // calculate signed triangle area using a kind of "2D cross product":
 function Area(p1: Vector2, p2: Vector2, p3: Vector2): float {
   var v1: Vector2 = p1 - p3;
   var v2: Vector2 = p2 - p3;
   return (v1.x * v2.y - v1.y * v2.x)/2;
 }

*/    