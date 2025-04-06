
//Todo 
//1. Add a wall to the environment
//2. Train the agent to avoid the wall and reach the goal
//3. Graph results
//4. Show difference in training wit 1,4,8,16 agents
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class Agent_ : Agent
{
    [SerializeField] private Transform _goal;
    [SerializeField] public GameObject obstacles;
    [SerializeField] private GameObject obstacle_1;
    [SerializeField] private GameObject obstacle_2;
    [SerializeField] private float _moveSpeed = 1.5f;
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorRenderer;


    private Renderer _renderer;

    // public TextMeshProUGUI _currentEpisodeText;
    private int _currentEpisode = 0;
    private float _cumulativeReward = 0;
    private float curriculumValue = 0;

    private List<GameObject> spawnedObstacles = new List<GameObject>();



    // Called when the agent is first initialized
    public override void Initialize()
    {
        _renderer = GetComponent<Renderer>();
        _currentEpisode = 0;
        _cumulativeReward = 0;
    }

    // Called when the agent is reset
    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode Begin");

        //check if the curriculum value is set due to changing configs
        //check if the academt.instance.environmentparameters is not null

        if (Academy.Instance.EnvironmentParameters != null)
        {
            curriculumValue = Academy.Instance.EnvironmentParameters.GetWithDefault("find_goal_param", 0.0f);
            Debug.Log("Current Curriculum Value: " + curriculumValue);
        }
        else
        {
            Debug.Log("Academy Instance is null");
        }

        _currentEpisode++;
        _cumulativeReward = 0f;
        _renderer.material.color = Color.blue;

        SpawnObjects(curriculumValue);
        // updateEpisodeUI();
    }



    // Called when the agent requests a decision
    public override void CollectObservations(VectorSensor sensor)
    {
        //The goal's position
        float goalPositionX = _goal.localPosition.x / 5f;
        float goalPositionZ = _goal.localPosition.z / 5f;

        //The agent's position
        float agentPositionX = transform.localPosition.x / 5f;
        float agentPositionZ = transform.localPosition.z / 5f;

        //The agent's direction
        float agentRotationNormalized = (transform.localRotation.eulerAngles.y / 360f) * 2f - 1f;

        sensor.AddObservation(goalPositionX);
        sensor.AddObservation(goalPositionZ);
        sensor.AddObservation(agentPositionX);
        sensor.AddObservation(agentPositionZ);
        sensor.AddObservation(agentRotationNormalized);


    }

    // Called when the agent requests a decision
    public override void OnActionReceived(ActionBuffers actions)
    {

        //Move thr agent using the action
        MoveAgent(actions.DiscreteActions);

        //Penalise agent for each step to encourage it to reach the goal faster
        AddReward(-2f / MaxStep);

        //Update the cumulative reward
        _cumulativeReward = GetCumulativeReward();
    }



    private void SpawnObjects(float curriculumValue)
    {
        ClearObstacles();

        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0f, 0.15f, 0f);

        Vector3 spawnSize = new Vector3(0.5f, 0.5f, 0.5f); // Size of the goal
        LayerMask obstacleLayer = LayerMask.GetMask("Obstacle");

        // Spawn obstacles for each lesson
        // if (curriculumValue != 0)
        // {
        //     if (curriculumValue >= 1.0f && curriculumValue < 2.0f)
        //     {
        //         GameObject obstacleInstance = Instantiate(obstacle_1, transform.position, Quaternion.identity);
        //         spawnedObstacles.Add(obstacleInstance);
        //         Debug.Log("Obstacle 1");
        //         Physics.SyncTransforms(); // Forces Unity to update physics before goal placement
        //     }
        //     else if (curriculumValue >= 2.0f && curriculumValue < 3.0f)
        //     {
        //         GameObject obstacleInstance = Instantiate(obstacle_2, transform.position, Quaternion.identity);
        //         spawnedObstacles.Add(obstacleInstance);
        //         Debug.Log("Obstacle 2");
        //         Physics.SyncTransforms(); // Forces Unity to update physics before goal placement
        //     }
        //     else if (curriculumValue >= 3.0f)
        //     {

        //         GameObject obstacleInstance = Instantiate(obstacles, transform.position, Quaternion.identity);
        //         spawnedObstacles.Add(obstacleInstance);
        //         Debug.Log("Obstacle 3");
        //         Physics.SyncTransforms(); // Forces Unity to update physics before goal placement
        //     }
        // }
        GameObject obstacleInstance = Instantiate(obstacles, transform.position, Quaternion.identity);
        spawnedObstacles.Add(obstacleInstance);
        Debug.Log("Obstacle 3");
        Physics.SyncTransforms(); // Forces Unity to update physics before goal placement
        Vector3 spawnPosition;

        do
        {
            float randomX = Random.Range(-4f, 4);
            float randomZ = Random.Range(-4f, 4f);
            spawnPosition = new Vector3(randomX, 0.3f, randomZ);
        }
        while (Physics.OverlapBox(spawnPosition, spawnSize, Quaternion.identity, obstacleLayer).Length > 0);

        _goal.localPosition = spawnPosition;
    }

    private void ClearObstacles()
    {
        if (spawnedObstacles.Count == 0) return;
        foreach (GameObject obstacle in spawnedObstacles)
        {
            Destroy(obstacle);
        }
        spawnedObstacles.Clear();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        // Map keyboard inputs to the discrete action space
        if (Input.GetKey(KeyCode.W))
        {
            discreteActions[0] = 1; // Move forward
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActions[0] = 2; // Rotate left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActions[0] = 3; // Rotate right
        }
        else
        {
            discreteActions[0] = 0; // Do nothing
        }
    }
    public void MoveAgent(ActionSegment<int> act)
    {
        var action = act[0];

        switch (action)
        {
            case 1: //Move forward
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;
                break;
            case 2: //Rotate Left
                transform.Rotate(0f, -_rotationSpeed * Time.deltaTime, 0f);
                break;
            case 3: //Rotate Right
                transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            GoalReached();
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Penalise();
        }
    }

    private void GoalReached()
    {
        AddReward(2f); //Large reward for reaching goal
        _cumulativeReward = GetCumulativeReward();
        Debug.Log("Cumulative Reward: " + _cumulativeReward);
        floorRenderer.material = winMaterial;
        EndEpisode();
    }

    private void Penalise()
    {
        AddReward(-1f); //Penalise for hitting the wall
        _cumulativeReward = GetCumulativeReward();
        Debug.Log("Cumulative Reward: " + _cumulativeReward);
        floorRenderer.material = loseMaterial;
        EndEpisode();
    }

    // void updateEpisodeUI()
    // {
    //     currentEpisodeText.text = "Episode: " + _currentEpisode;
    // }

}
