using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ListRessource : MonoBehaviour {

   
    string fileName = "List.txt";


    // Use this for initialization
    void Start () {
       

        Object po = PrefabUtility.GetPrefabParent(transform.GetChild(0));
        string filePath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(po))+"/"+ fileName;
        Debug.Log(filePath);
        StreamWriter sr=File.CreateText(filePath);



        foreach (Transform child in transform)
        {
            Object parentObject = PrefabUtility.GetPrefabParent(child);
            string path = AssetDatabase.GetAssetPath(parentObject);
            path = path.Substring(17);
            path = Path.GetDirectoryName(path)+ "/" +Path.GetFileNameWithoutExtension(path);
            Debug.Log("prefab path:" + path);

            sr.WriteLine(path);

            /*string path = AssetDatabase.GetAssetPath(child);
            Debug.Log(path);*/
        }

        sr.Close();
    }
	
	
}
