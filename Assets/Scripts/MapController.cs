using System.Collections;
using System.Collections.Generic;
using System.Linq;

using IJunior.TypedScenes;

using UnityEngine;
using UnityEngine.UI;

public class MapController : Singletone<MapController>
{
    [SerializeField] List<Level> AllLevel = new List<Level>();
    public List<LevelDate> levels = new List<LevelDate>();

    [SerializeField] Transform target;
    [SerializeField] Button ButtonPrefub;
    
    public void OpenLevel(Level level)
    {
        Game.Load(level);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        levels = PlayerSettings.Instance.levelDatas;
        for (int i = 0; i < AllLevel.Count && i < levels.Count; i++)
        {
            var level = AllLevel[i];
            var btn = Instantiate(ButtonPrefub, target);
            btn.onClick.AddListener(()=>OpenLevel(level));
            btn.GetComponentInChildren<Text>().text = (i + 1).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
[System.Serializable]
public class LevelDate
{
    public int levelID;
    public float MinTime;
    public bool IsOpen;
}