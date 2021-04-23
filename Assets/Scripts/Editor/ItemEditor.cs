using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Linq;

public class ItemEditor : EditorWindow
{
    private enum State
    {
        BLANK,
        EDIT,
        ADD
    }
   
    private State _editorState;
    private int _selectedIndex;
    private ItemDefinition _itemToSave;
   
    private ItemDatabase _database;
    private Vector2 _scrollPos;
    private Vector2 _scrollPosInputs;
   
    [MenuItem("Database/Item Editor")]
    public static void Init ()
    {
        ItemEditor window = EditorWindow.GetWindow<ItemEditor> ();
        window.minSize = new Vector2 (800, 400);
        window.Show ();
        
    }
   
    void OnEnable ()
    {
        LoadDatabase ();
        _itemToSave = new ItemDefinition();
        _editorState = State.BLANK;
    }
   
    void OnGUI ()
    {
        EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));
        DisplayListArea ();
        DisplayMainArea ();
        EditorGUILayout.EndHorizontal ();
        
        Repaint();
    }
   
    void LoadDatabase ()
    {
        _database = Resources.Load<ItemDatabase>("ItemDatabase");
    }

    void DisplayListArea ()
    {
        EditorGUILayout.BeginVertical (GUILayout.Width (250));
        EditorGUILayout.Space ();
       
        _scrollPos = EditorGUILayout.BeginScrollView (_scrollPos, "box", GUILayout.ExpandHeight (true));
       
        for (int i = 0; i < _database.definitions.Count; i++) 
        {
            EditorGUILayout.BeginHorizontal ();
            if (GUILayout.Button ("-", GUILayout.Width (25))) 
            {
                _database.definitions.RemoveAt (i);
                _database.SortByPrice ();
                EditorUtility.SetDirty (_database);
                _editorState = State.BLANK;
                return;
            }
           
            if (GUILayout.Button (_database.definitions[i].itemName, GUILayout.ExpandWidth (true))) 
            {
                _selectedIndex = i;
                _editorState = State.EDIT;
            }
           
            EditorGUILayout.EndHorizontal ();
        }
       
        EditorGUILayout.EndScrollView ();
       
        EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));
        EditorGUILayout.LabelField ("Items: " + _database.definitions.Count, GUILayout.Width (100));
       
        if (GUILayout.Button ("New Item"))
            _editorState = State.ADD;
       
        EditorGUILayout.EndHorizontal ();
        EditorGUILayout.Space ();
        EditorGUILayout.EndVertical ();
    }
   
    void DisplayMainArea ()
    {
        EditorGUILayout.BeginVertical (GUILayout.ExpandWidth (true));
        EditorGUILayout.Space ();
       
        switch (_editorState) {
        case State.ADD:
            DisplayAddMainArea ();
            break;
        case State.EDIT:
            DisplayEditMainArea ();
            break;
        default:
            DisplayBlankMainArea ();
            break;
        }
       
        EditorGUILayout.Space ();
        EditorGUILayout.EndVertical ();
    }
   
    void DisplayBlankMainArea ()
    {
        EditorGUILayout.LabelField (
            "Welcome to Item Editor.", GUILayout.ExpandHeight (true));
    }
   
    void DisplayEditMainArea ()
    {
        var item = _database.definitions[_selectedIndex];
        EditorGUILayout.LabelField ("Id: ", _database.definitions[_selectedIndex].DefId);
        item.itemName        = EditorGUILayout.TextField    ("Name",         item.itemName);
        item.icon            = EditorGUILayout.ObjectField  ("Icon: ",       item.icon, typeof(Sprite), false) as Sprite;
        item.itemType        = (ItemType)EditorGUILayout.EnumPopup("Type: ", item.itemType);
        item.level           = EditorGUILayout.IntField("Level: ", item.level);
        item.description     = EditorGUILayout.TextField("Description: ",  item.description);
        item.price           = EditorGUILayout.IntField("Price: ",  item.price);
        
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            if (GUILayout.Button ("Done", GUILayout.Width (100))) 
            {
                _database.SortByPrice ();
                _editorState = State.BLANK;
                EditorUtility.SetDirty(_database);
            }

        EditorGUILayout.EndHorizontal();
    }
   
    void DisplayAddMainArea ()
    {
        _itemToSave.CreateNew();
        _itemToSave.itemName        = EditorGUILayout.TextField("Name: ", _itemToSave.itemName);
        _itemToSave.icon            = EditorGUILayout.ObjectField("Icon: ", _itemToSave.icon, typeof(Sprite), false) as Sprite;
        _itemToSave.itemType        = (ItemType)EditorGUILayout.EnumPopup("Type: ", _itemToSave.itemType);
        _itemToSave.level           = EditorGUILayout.IntField("Level: ", _itemToSave.level);
        _itemToSave.description     = EditorGUILayout.TextField("Description: ", _itemToSave.description);
        _itemToSave.price           = EditorGUILayout.IntField("Price: ", _itemToSave.price);

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            if (GUILayout.Button("Done", GUILayout.Width(100)))
            {
                _database.definitions.Add(_itemToSave);
                _database.SortByPrice();

                _itemToSave = new ItemDefinition();
                EditorUtility.SetDirty(_database);
                _editorState = State.BLANK;
            }

        EditorGUILayout.EndHorizontal();
    }
}