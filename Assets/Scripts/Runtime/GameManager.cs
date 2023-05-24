using System;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMap(string mapFilePath)
    {
        Func<List<GameObject>> GetExistingGameObjects = delegate ()
        {
            List<GameObject> list = new List<GameObject>();
            GameObject[] gameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.scene.isLoaded)
                {
                    list.Add(gameObject);
                }
            }
            return list;
        };

        FileStream fileStream = FileStream.Null as FileStream;
        try
        {
            fileStream = new FileStream(mapFilePath, System.IO.FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(ObjectInfoNode));
            ObjectInfoNode objectsInfo = xml.Deserialize(fileStream) as ObjectInfoNode;

            List<GameObject> exitingGameObjects = GetExistingGameObjects();
            for (int i = 0; i < objectsInfo.children.Count; i++)
            {
                ObjectInfoNode child = objectsInfo.children[i];
                CreateObjects(transform, child, exitingGameObjects);
            }

            //filePath = "";
            //EditorUtility.DisplayDialog("Load File Message", "Loading file successul!", "OK");
            Debug.Log("Loading file "+mapFilePath+ " successul!");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            //EditorUtility.DisplayDialog("Load File Error Message", "Loading file is failed!\nError message is:\n" + e.Message, "OK");
        }
        finally
        {
            fileStream.Close();
        }
    }

    private void CreateObjects(Transform transform, ObjectInfoNode objectsInfo, List<GameObject> exitingGameObjects)
    {
        GameObject gameObject = IsExistingGameObject(objectsInfo.name, exitingGameObjects);

        if (gameObject == null)
        {
            if (GetPrefabName(objectsInfo.prefabPath) == "")
            {
                GameObject _object = new GameObject(objectsInfo.name);
                _object.transform.parent = transform;
                foreach (ObjectInfoNode child in objectsInfo.children)
                {
                    CreateObjects(_object.transform, child, exitingGameObjects);
                }
            }
            else
            {
                //Debug.Log(Resources.Load("Prefabs/" + GetPrefabName(objectsInfo.prefabPath)).GetType());
                Instantiate(Resources.Load("Prefabs/" + GetPrefabName(objectsInfo.prefabPath)), objectsInfo.position, objectsInfo.rotation, transform).name = objectsInfo.name;         
            }
        }
        else
        {
            foreach (ObjectInfoNode child in objectsInfo.children)
            {
                CreateObjects(gameObject.transform, child, exitingGameObjects);
            }
        }
    }

    private GameObject IsExistingGameObject(string name, List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Equals(name))
            {
                return gameObject;
            }
        }
        return null;
    }

    private string GetPrefabName(string s)
    {
        return s.Split('/').Last<string>().Split('.')[0];
    }

    public void DestroyMap()
    {
        Transform[] children= this.GetComponentsInChildren<Transform>();
        foreach(Transform child in children)
        {
            DestroyChildrenObjects(child);
        }
    }

    private void DestroyChildrenObjects(Transform _transform)
    {
        Transform[] children= _transform.GetComponentsInChildren<Transform>();
        if (children.Length == 0)
        {
            Destroy(_transform.gameObject);
        }
        else
        {
            foreach(Transform child in children)
            {
                DestroyChildrenObjects(child);
            }
        }
    }
}
