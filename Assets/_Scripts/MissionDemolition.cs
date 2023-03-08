using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public TextMeshProUGUI uitLevel;  
    public TextMeshProUGUI uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;
 
    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam mode
 
    void Start()
    {
        S = this; // Define the Singleton
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }
        // Destroy old projectiles if they exist (the method is not yet written)
        Projectile.DESTROY_PROJECTILES();
        // This will be underlined in red
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        // Reset the goal
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }
    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of "+ levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    void Update()
    {
        UpdateGUI();
        // Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        { 
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }
// Static method that allows code anywhere to increment shotsTaken
    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }
// Static method that allows code anywhere to get a reference to S.castle
    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}