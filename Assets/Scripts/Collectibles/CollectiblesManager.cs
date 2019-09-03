using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    void Start()
    {
        if (SaveSystem.shouldLevelBeLoaded)
        {
            LevelData levelData = SaveSystem.loadedLevelData;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if(levelData.activeCollectibles[i] == false)
                {
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
