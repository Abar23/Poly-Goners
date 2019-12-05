using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonCompletionTracker : AbstractSingleton<DungeonCompletionTracker>
{
    [Range(1, 100)]
    public int numberOfCompletionsToMaxDungeonSize;

    private int numberOfCopmletedDungeons;

    public Text LevelText; 

    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this as DungeonCompletionTracker;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this as DungeonCompletionTracker)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        this.numberOfCopmletedDungeons = 0;
    }

    void LateUpdate()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 0 || scene.buildIndex == 1 || scene.buildIndex == 2 || scene.name == "CastleHub")
        {
            Destroy(this.gameObject);
        }
    }

    public void IncreaseNumberOfCompletedDungeons()
    {
        if(this.numberOfCopmletedDungeons < this.numberOfCompletionsToMaxDungeonSize)
        {
            this.numberOfCopmletedDungeons++;

        }
    }

    public int GetNumberOfCompletedDungeons()
    {
        return this.numberOfCopmletedDungeons;
    }

    public void UpdateLevelText(DungeonGenerator generator)
    {
        if (LevelText != null)
        {
            LevelText.text = "Level " + (numberOfCopmletedDungeons + 1).ToString() + "	 - 	 " + generator.GetNumberOfRooms().ToString() + " Rooms";
        }
        Animator anim = GetComponentInChildren<Animator>();
        anim.Play("AreaName", -1, 0f);
    }
}
