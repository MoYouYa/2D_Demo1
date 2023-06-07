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
    private static GameManager instance;
    public static GameManager Instance
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance; 
        }
    }

    private void Start()
    {
        UIManager.Instance.EnterMainFram();
        //AudioManager.Instance.PlayAudio("BGM");
    }

    public void LoadMap(string mapFilePath)
    {
        Debug.Log("Start to load map.");

        FileStream fileStream = FileStream.Null as FileStream;
        try
        {
            fileStream = new FileStream(mapFilePath, System.IO.FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(ObjectInfoNode));
            ObjectInfoNode objectsInfo = xml.Deserialize(fileStream) as ObjectInfoNode;

            //List<GameObject> exitingGameObjects = GetExistingGameObjects();
            for (int i = 0; i < objectsInfo.children.Count; i++)
            {
                ObjectInfoNode child = objectsInfo.children[i];
                CreateObjects(transform, child/*, exitingGameObjects*/);
            }

            //filePath = "";
            Debug.Log("Loading file "+mapFilePath+ " successully!");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            fileStream.Close();
        }
    }

    private void CreateObjects(Transform transform, ObjectInfoNode objectsInfo/*, List<GameObject> exitingGameObjects*/)
    {
        if (GetPrefabName(objectsInfo.prefabPath) == "")
        {
            GameObject _object = new GameObject(objectsInfo.name);
            _object.transform.parent = transform;
            foreach (ObjectInfoNode child in objectsInfo.children)
            {
                CreateObjects(_object.transform, child/*, exitingGameObjects*/);
            }
        }
        else
        {
            //Debug.Log(Resources.Load("Prefabs/" + GetPrefabName(objectsInfo.prefabPath)).GetType());
            Instantiate(Resources.Load("Prefabs/" + GetPrefabName(objectsInfo.prefabPath)), objectsInfo.position, objectsInfo.rotation, transform).name = objectsInfo.name;
        }
    }

    private string GetPrefabName(string s)
    {
        return s.Split('/').Last<string>().Split('.')[0];
    }

    public void DestroyMap()
    {
        Debug.Log("Start to destroy map.");
        Transform[] children= this.GetComponentsInChildren<Transform>();
        foreach(Transform child in children)
        {
            //Debug.Log(child.name);
            if (child != this.transform)
            {
                child.name = child.name + "_toBeDestroyed";//Destroy 不是立即执行（调用之后不会立即销毁，不是这个函数调用了，下一个语句前就销毁）的，如果不改名字就会影响在同一帧调用的 LoadMap(string mapFilePath) 函数，使其无法生成新的物体
                Destroy(child.gameObject);
            }
        }
        Debug.Log("Destroy map successfully!");
    }

    public void SetPlayerPuase()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.enabled = false;
            player.GetComponent<Rigidbody2D>().Sleep();
        }
    }

    public void SetPlayerContinue()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.enabled = true;
            player.GetComponent<Rigidbody2D>().WakeUp();
        }
    }
}
