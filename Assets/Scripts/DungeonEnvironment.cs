using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnvironment : MonoBehaviour
{
    public static DungeonEnvironment instance;
    public Vector2 dungeonPosition;

    public List<Vector2> spawnItemPosition;
    public GameObject removeBoard;

    public GameObject tree;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        removeBoard.SetActive(false);

        //임시로 숲 맵 아이템 스폰 설정
        dungeonPosition = new Vector2(-1000f, 500f);

        spawnItemPosition.Add(new Vector2(dungeonPosition.x - 6, dungeonPosition.y - 2));
        spawnItemPosition.Add(new Vector2(dungeonPosition.x - 10, dungeonPosition.y - 3));
        spawnItemPosition.Add(new Vector2(dungeonPosition.x - 10, dungeonPosition.y - 6));
        spawnItemPosition.Add(new Vector2(dungeonPosition.x - 2, dungeonPosition.y - 8));
        spawnItemPosition.Add(new Vector2(dungeonPosition.x + 6, dungeonPosition.y - 7));
        spawnItemPosition.Add(new Vector2(dungeonPosition.x + 10, dungeonPosition.y - 4));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clearField(int mapCode)
    {
        for (int i = 0; i < SpawnManager.instance.enemyCount.Length; i++)
        {
            SpawnManager.instance.enemyCount[i] = 0;
        }

        switch(mapCode)
        {
            case 2:
                removeBoard.SetActive(true);
                removeBoard.transform.position = new Vector2(-1000, 500);
                break;
            default:
                break;
        }
    }

    public void spawnItem(int mapCode, int difficulty = 0)
    {
        removeBoard.SetActive(false);

        switch (mapCode)
        {
            case 2:
                SpawnManager.instance.spawnBreakableEntity(0, -1010, 489);

                for (int i = 0; i < spawnItemPosition.Count; i++)
                {
                    if (generateRandom(700))
                    {
                        ItemDatabase.instance.spawnItemByCode(spawnItemPosition[i], ItemDatabase.instance.itemDB[Random.Range(0, 7)].code);
                    }
                }

                ItemDatabase.instance.spawnItemByCode(new Vector2(-1058, 699.5f), ItemDatabase.instance.itemDB[Random.Range(7, 9)].code);
                break;
            default:
                break;
        }

    }

    public bool generateRandom(int num)
    {
        return Random.Range(0, 1000) < num;
    }
}
