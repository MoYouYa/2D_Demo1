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
    private Player player;
    private List<Vulture> vultures;
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
                child.name = child.name + "_toBeDestroyed";//Destroy ��������ִ�У�����֮�󲻻��������٣�����������������ˣ���һ�����ǰ�����٣��ģ�����������־ͻ�Ӱ����ͬһ֡���õ� LoadMap(string mapFilePath) ������ʹ���޷������µ�����
                Destroy(child.gameObject);
            }
        }
        Debug.Log("Destroy map successfully!");
    }

    public Player GetPlayer()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        if (player != null)
        {
            return player;
        }
        else
        {
            return null;
        }
    }

    public void GamePuase()
    {
        SetPlayerPuase();
        SetVulturesPuase();
    }
    private void SetPlayerPuase()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        if (player != null)
        {
            player.enabled = false;
            player.GetComponent<Rigidbody2D>().Sleep();
        }
    }

    private void SetVulturesPuase()
    {
        if (vultures == null)
        {
            vultures = new List<Vulture>();
        }else if (vultures[0]== null)
        {
            vultures.Clear();
        }
        if(vultures.Count == 0)
        {
            vultures= new List<Vulture>(FindObjectsOfType<Vulture>());
        }
        if (vultures.Count > 0)
        {
            foreach(Vulture vulture in vultures)
            {
                if (vulture != null)
                {
                    Debug.Log($"Set {vulture.name} Puase");
                    vulture.enabled = false;
                }
            }
        }
    }

    public void GameContinue()
    {
        SetPlayerContinue();
        SetVulturesContinue();
    }

    private void SetPlayerContinue()
    {
        if (player != null)
        {
            player.enabled = true;
            player.GetComponent<Rigidbody2D>().WakeUp();
        }
    }

    private void SetVulturesContinue()
    {
        if (vultures.Count > 0)
        {
            foreach (Vulture vulture in vultures)
            {
                if (vulture != null)
                {
                    vulture.enabled = true;
                }
            }
        }
    }
}
