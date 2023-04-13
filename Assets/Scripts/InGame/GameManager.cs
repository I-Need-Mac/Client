using UnityEngine;

using System;
using BFM;
using UnityEngine.Tilemaps;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField] private int mapId;
    [SerializeField] private int playerId;
    //private int mapId;
    //private int playerId;

    public PlayerUI playerUi { get; private set; }
    public Player player { get; private set; }
    public GameObject map { get; private set; }
    public Tilemap tileMap { get; private set; }

    private float defaultScale;

    protected override void Awake()
    {
        defaultScale = float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "ImageMultiple", "ConfigValue")));
        playerUi = GameObject.FindWithTag("PlayerUI").GetComponent<PlayerUI>();
        SoundManager.Instance.CreateSoundManager();
        //mapId = UIManager.Instance.selectStageID;
        //playerId = UIManager.Instance.selectCharacterID;
        //playerPoolManager.playerId = playerId;
    }

    private void Start()
    {
        Spawn();
    }

    public int GetPlayerId()
    {
        return playerId;
    }

    #region Player&MapCreate
    private void Spawn()
    {
        MapLoad(mapId);
        PlayerLoad(playerId);
        AssignLayerAndZ();
    }

    private void MapLoad(int mapId)
    {
        string name = LoadMapManager.Instance.SceneNumberToMapName(mapId);
        GameObject mapPrefab = LoadMapManager.Instance.LoadMapNameToMapObject(name);
        map = Instantiate(mapPrefab, transform);
        tileMap = map.transform.Find("Map").transform.Find("Floor").GetComponent<Tilemap>();
        map.transform.SetParent(transform.Find("MapGeneratePos").transform);
        map.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        map.SetActive(true);
    }

    private void PlayerLoad(int playerId)
    {
        player = Instantiate(ResourcesManager.Load<Player>(CSVReader.Read("CharacterTable", playerId.ToString(), "CharacterPrefabPath").ToString()), transform.Find("PlayerSpawnPos").transform);
        player.transform.localScale = Vector3.one * defaultScale;
        player.gameObject.SetActive(true);
    }

    private void AssignLayerAndZ()
    {
        RecursiveChild(player.transform, LayerConstant.SPAWNOBJECT);
        RecursiveChild(map.transform, LayerConstant.MAP);
    }

    private void RecursiveChild(Transform trans, LayerConstant layer)
    {
        if (trans.name.Equals("Character"))
        {
            trans.tag = "Player";
        }
        trans.gameObject.layer = (int)layer;
        trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, (int)layer);

        foreach (Transform child in trans)
        {
            switch (child.name)
            {
                case "Camera":
                    RecursiveChild(child, LayerConstant.POISONFOG);
                    break;
                case "FieldStructure":
                    RecursiveChild(child, LayerConstant.OBSTACLE);
                    break;
                case "Top":
                    RecursiveChild(child, LayerConstant.OBSTACLE - 2);
                    break;
                default:
                    RecursiveChild(child, layer);
                    break;
            }
        }
    }
    #endregion

    #region Time
    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = 1;
        }
    }


    #endregion
}
