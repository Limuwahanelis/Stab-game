using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public float timePlayed;
    public int sceneToLoadIndex;
    public DateTime date;
    public PlayerData playerData;
    public List<SaveableData> datas = new List<SaveableData>();

    public GameData() 
    {
        date = DateTime.Now;
        timePlayed = 0;
        playerData = new PlayerData();
    }

    public void CreateOrUpdateDatas(List<SaveableData> updatedDatas)
    {
        foreach (SaveableData data in updatedDatas)
        {
            SaveableData savedData = datas.Find(x => x.id == data.id);
            if (savedData!=null)
            {
                savedData.value = data.value;
            }
            else
            {
                datas.Add(data);
            }
        }
    }
}
