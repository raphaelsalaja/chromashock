using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int last_stage;
    public int last_world;
    void Update()
    {

        DontDestroyOnLoad(this);

    }
}
