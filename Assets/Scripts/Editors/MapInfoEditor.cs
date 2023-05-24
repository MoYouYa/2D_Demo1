using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

using System.Xml.Serialization;

[Serializable]
public class ObjectInfoNode
{
    public string name;
    public string prefabPath;
    public Vector3 position;
    public Quaternion rotation;
    public List<ObjectInfoNode> children;

    public ObjectInfoNode()
    {
        name= string.Empty;
        prefabPath= string.Empty;
        position= Vector3.zero;
        rotation= Quaternion.identity;
        children = new List<ObjectInfoNode>();
    }
    public ObjectInfoNode(string _name,string _prefabPath,Vector3 _posistion,Quaternion _rotation)
    {
        name=_name;
        prefabPath=_prefabPath;
        position=_posistion;
        rotation=_rotation; 
        children=new List<ObjectInfoNode>();
    }
}


public class MapInfoEditor : EditorWindow
{
    private string filePath;
    private ObjectInfoNode rootObjectInfoNode;
    private Vector2 selectedObjectsListScrollPosition;
    //private Vector2 fileInfoGoodsListScrollPosition;

    // Start is called before the first frame update
    [MenuItem("CustomTools/MapInfoEditor")]
    public static void ShowWindow()
    {
        //使用官方提供的实例化窗口方法调用
        MapInfoEditor.CreateInstance<MapInfoEditor>().Show();

        //浮动型的窗口，跟点击Building Setting出现的窗口效果一样
        //MapInfoEditor.CreateInstance<MapInfoEditor>().ShowUtility();

        //弹出窗口时的效果
        //MapInfoEditor.CreateInstance<MapInfoEditor>().ShowPopup();//此方Y法和下面的OnGUI配合着使用，否则会出现页面关不掉的情况
    }

    public void OnGUI()
    {
        rootObjectInfoNode = GetObjectsList();

        GUILayout.Label("MapInfoEditor", "WhiteLargeCenterLabel",GUILayout.MaxHeight(30));
        
        GUILayout.Space(20);

        GUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal();
            {
                //GUILayout.Label("File Path", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
                filePath = EditorGUILayout.TextField("File Path",filePath, "BoldTextField");
                if (GUILayout.Button("选取", GUILayout.MaxWidth(40)))
                {
                    //filePath = EditorUtility.SaveFilePanel("SaveMap", filePath, "newMap", "xml");
                    filePath = EditorUtility.OpenFilePanel("SaveMap", filePath, "xml");
                }
                if (GUILayout.Button("新建", GUILayout.MaxWidth(40)))
                {
                    filePath = EditorUtility.SaveFilePanel("SaveMap", filePath, "newMap", "xml");
                    //filePath = EditorUtility.OpenFilePanel("SaveMap", filePath, "xml");
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            //GUILayout.Box("", "HelpBox");
            //GUILayout.BeginHorizontal();

            selectedObjectsListScrollPosition = GUILayout.BeginScrollView(selectedObjectsListScrollPosition, "HelpBox");
            DrawSelectedObjectsList(rootObjectInfoNode,1, 0);
            GUILayout.EndScrollView();

            //fileInfoGoodsListScrollPosition = GUILayout.BeginScrollView(fileInfoGoodsListScrollPosition, "HelpBox");
            //DrawGoodsList(rootObjectInfoNode, 1, 0);
            //GUILayout.EndScrollView();
            //GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save", "LargeButton"))
                {
                    SaveFile();
                }
                if (GUILayout.Button("Load", "LargeButton"))
                {
                    LoadFile();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
        }
        GUILayout.EndVertical();

    }

    private void DrawSelectedObjectsList(ObjectInfoNode node,int index, int deep)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(deep * 30+10);
        if (deep == 0)
        {
            EditorGUILayout.SelectableLabel(node.name, "WhiteLargeLabel", GUILayout.MaxHeight(20));
        }
        else
        {
            EditorGUILayout.SelectableLabel(index.ToString() + ". " + node.name, GUILayout.MaxHeight(15));
            //EditorGUILayout.SelectableLabel(index.ToString()+". "+ node.name+"\t\t"+node.prefabPath+"\t\t"+node.position+"\t\t"+node.rotation,GUILayout.MaxHeight(15));
        }
        GUILayout.EndHorizontal();

        for(int i=0;i<node.children.Count;i++)
        {
            DrawSelectedObjectsList(node.children[i],i+1, deep+1);
        }
    }

    //private void DrawFileInfoGoodsList(ObjectInfoNode node, int index, int deep)
    //{

    //}

    private ObjectInfoNode GetObjectsList()
    {
        GameObject[] selectedGameObjects= Selection.gameObjects;
        ObjectInfoNode root=new ObjectInfoNode("Selected Objects List:","",Vector3.zero,Quaternion.identity);
        if(selectedGameObjects.Length>0 )
        {
            for(int i=0;i< selectedGameObjects.Length; i++)
            {
                root.children.Add(GetObjectsInfo(selectedGameObjects[i].transform));
            }
        }
        return  root;
    }

    private ObjectInfoNode GetObjectsInfo(Transform transform)
    {
        ObjectInfoNode root = new ObjectInfoNode(transform.name,GetPrefabName(transform.gameObject),transform.position,transform.rotation);
        for (int i = 0; i < transform.childCount; i++)
        {
            root.children.Add(GetObjectsInfo(transform.GetChild(i)));
        }
        return root;
    }

    private string GetPrefabName(GameObject gameObject)
    {
        PrefabAssetType prefabAssetType= UnityEditor.PrefabUtility.GetPrefabAssetType(gameObject);
        if(prefabAssetType== UnityEditor.PrefabAssetType.NotAPrefab)
        {
            return "";
        }
        else
        {
            return UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            //return UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(gameObject).ToString().Replace(" (UnityEngine.GameObject)","");
            //return prefabAssetType.ToString();
        }
    }

    private void SaveFile()
    {
        FileStream fileStream=FileStream.Null as FileStream;
        try
        {
            fileStream = new FileStream(filePath,System.IO.FileMode.OpenOrCreate);
            XmlSerializer xml=new XmlSerializer(typeof(ObjectInfoNode));
            xml.Serialize(fileStream, rootObjectInfoNode);
            EditorUtility.DisplayDialog("Save File Message", "Saving file successul!","OK");
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Save File Error Message", "Saving file is failed!\nError message is:\n"+e.Message, "OK");
        }
        finally { 
            fileStream.Close(); 
        }
    }

    private void LoadFile()
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
            fileStream = new FileStream(filePath, System.IO.FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(ObjectInfoNode));
            ObjectInfoNode objectsInfo = xml.Deserialize(fileStream) as ObjectInfoNode;

            List<GameObject> exitingGameObjects=GetExistingGameObjects();
            for(int i = 0;i< objectsInfo.children.Count;i++)
            {
                ObjectInfoNode child = objectsInfo.children[i];
                CreateObjects(null,child, exitingGameObjects);
            }

            //filePath = "";
            EditorUtility.DisplayDialog("Load File Message", "Loading file successul!", "OK");
        }
        catch(Exception e)
        {
            EditorUtility.DisplayDialog("Load File Error Message", "Loading file is failed!\nError message is:\n" + e.Message, "OK");
        }
        finally { 
            fileStream.Close(); 
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

    //根据读取到的地图信息生成物体
    private void CreateObjects(Transform transform,ObjectInfoNode objectsInfo, List<GameObject> exitingGameObjects)
    {
        GameObject gameObject = IsExistingGameObject(objectsInfo.name, exitingGameObjects);
        if (gameObject==null)
        {
            if (objectsInfo.prefabPath != "")
            {
                Instantiate(AssetDatabase.LoadAllAssetsAtPath(objectsInfo.prefabPath)[0], objectsInfo.position, objectsInfo.rotation, transform).name = objectsInfo.name;
            }
            else
            {
                GameObject _object = new GameObject(objectsInfo.name);
                _object.transform.parent = transform;
                foreach(ObjectInfoNode child in objectsInfo.children)
                {
                    CreateObjects(_object.transform,child, exitingGameObjects);
                }
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
}

