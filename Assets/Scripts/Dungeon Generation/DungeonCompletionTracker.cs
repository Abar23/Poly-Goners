using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonCompletionTracker : AbstractSingleton<DungeonCompletionTracker>
{
    [Range(1, 100)]
    public int numberOfCompletionsToMaxDungeonSize;

    private int numberOfCopmletedDungeons;   

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
        this.numberOfCopmletedDungeons = 0;
    }

    void LateUpdate()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 0 || scene.buildIndex == 1 || scene.buildIndex == 2)
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
}
