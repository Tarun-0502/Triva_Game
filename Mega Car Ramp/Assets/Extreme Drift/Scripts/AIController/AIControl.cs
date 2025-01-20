using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum VehicleMode { Player = 0, AICar = 1 }
public enum ControlMode { Simple = 1, Mobile = 2 }

[System.Serializable]
public class PlayerDistance
{
    public string playerName;
    public float distance;

    public PlayerDistance(string name, float dist)
    {
        playerName = name;
        distance = dist;
    }

}

public class AIControl : MonoBehaviour
{

    public static AIControl manage;
    public static VehicleControl CurrentVehicle;

    public ControlMode controlMode = ControlMode.Simple;

    public Transform firstAINode;
    public Transform startPoint, startPoint_1, Player_Position;
    public Transform finishPoint;

    [SerializeField, HideInInspector] private GameObject Player_, AI_1, AI_2;

    [SerializeField] List<GameObject> players;

    public GameObject[] CarsPrefabs;

    public bool player;

    [SerializeField] internal GameObject Camera1, Camera2, Camera3;

    void Awake()
    {
        manage = this;
    }

    void Start()
    {

        GameObject AI_Car = Instantiate(CarsPrefabs[PlayerPrefs.GetInt("CurrentVehicle")], startPoint.position, startPoint.rotation) as GameObject;
        GameObject AI_Car_1 = Instantiate(CarsPrefabs[PlayerPrefs.GetInt("CurrentVehicle") + 1], startPoint_1.position, startPoint_1.rotation) as GameObject;
        GameObject Player = Instantiate(CarsPrefabs[PlayerPrefs.GetInt("CurrentVehicle") + 6], Player_Position.position, Player_Position.rotation) as GameObject;
        AI_Car.tag = "AI";
        AI_Car.transform.GetChild(0).GetChild(1).tag = "AI";
        AI_Car_1.tag = "AI";
        AI_Car_1.transform.GetChild(0).GetChild(1).tag = "AI";


        Player_ = Player;
        AI_1 = AI_Car;
        AI_2 = AI_Car_1;

        players.Add(AI_1);
        players.Add(AI_2);
        players.Add(Player_);

        AI_Car.GetComponent<AIVehicle>().nextNode = firstAINode;
        AI_Car.GetComponent<AIVehicle>().lastNode = finishPoint;

        AI_Car_1.GetComponent<AIVehicle>().nextNode = firstAINode;
        AI_Car_1.GetComponent<AIVehicle>().lastNode = finishPoint;

        Player.GetComponent<AIVehicle>().nextNode = firstAINode;
        Player.GetComponent<AIVehicle>().lastNode = finishPoint;

        CurrentVehicle = Player.GetComponent<VehicleControl>();

        VehicleCamera.manage.target = Player.transform;
        VehicleCamera.manage.cameraSwitchView = CurrentVehicle.cameraView.cameraSwitchView;

        VehicleCamera.manage.distance = CurrentVehicle.cameraView.distance;
        VehicleCamera.manage.height = CurrentVehicle.cameraView.height;
        VehicleCamera.manage.angle = CurrentVehicle.cameraView.angle;

        
    }

    private void Update()
    {
        if (GameUI.manage.gameStarted && !GameUI.manage.gameFailed && !GameUI.manage.gameFailed)
        {
            Player_.GetComponent<VehicleControl>().vehicleMode = VehicleMode.Player;
            AI_1.GetComponent<VehicleControl>().vehicleMode = VehicleMode.AICar;
            AI_2.GetComponent<VehicleControl>().vehicleMode = VehicleMode.AICar;
            if (GameUI.manage.Speed_Position_Mode)
            {
                RankPlayersByDistance();
            }
        }
    }

    void RankPlayersByDistance()
    {
        // Create a list to store players and their distances
        List<PlayerDistance> playerDistances = new List<PlayerDistance>();

        // Calculate distance for each player
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, finishPoint.position);
            playerDistances.Add(new PlayerDistance(player.name, distance));
        }

        // Sort players by distance in ascending order
        playerDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Find the rank of Player_
        int playerRank = playerDistances.FindIndex(pd => pd.playerName == Player_.name) + 1;

        GameUI.manage.PlayerPosition.text = playerRank.ToString()+"/"+players.Count;

        //// Display the rankings in the console
        //for (int i = 0; i < playerDistances.Count; i++)
        //{
        //    Debug.Log($"Rank {i + 1}: {playerDistances[i].playerName} (Distance: {playerDistances[i].distance})");
        //}
    }
}

