//Todo 
//1. Add a wall to the environment
//2. Train the agent to avoid the wall and reach the goal
//3. Graph results
//4. Show difference in training wit 1,4,8,16 agents
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TurtleAgent : Agent
{
    [SerializeField] private Transform _goal;
    [SerializeField] public GameObject obstacles;

    [SerializeField] private float _moveSpeed = 1.5f;
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorRenderer;


    private Renderer _renderer;
    public Vector3 spawnPosition = Vector3.zero; // World position to spawn the prefab

    private int _currentEpisode = 0;
    private float _cumulativeReward = 0;
    private float curriculumValue;


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
        curriculumValue = Academy.Instance.EnvironmentParameters.GetWithDefault("find_goal_param", 0.0f);
        Debug.Log("Current Curriculum Value: " + curriculumValue);
        _currentEpisode++;
        _cumulativeReward = 0f;
        _renderer.material.color = Color.blue;

        if (curriculumValue >= 1.0f)
        {
            Debug.Log("AvoidObstacle Lesson Active");
        }
        else
        {
            SpawnObjects();
        }

        // Get the current lesson value from the environment parameters
        // float lesson = Academy.Instance.EnvironmentParameters.GetWithDefault("lesson", 0f);

        // // Spawn obstacles based on the current lesson
        // if (lesson >= 1.0f)
        // {
        //     SpawnObstacles();
        // }
    }

    // public override void OnEpisodeBegin()
    // {
    //     // Retrieve curriculum parameter
    //     curriculumValue = Academy.Instance.EnvironmentParameters.GetWithDefault("find_goal_param", 0.0f);
    //     Debug.Log("Current Curriculum Value: " + curriculumValue);

    //     // Adjust difficulty based on curriculum
    //     if (curriculumValue >= 1.0f)
    //     {
    //         // Harder lesson: spawn obstacles differently or reduce rewards
    //         Debug.Log("AvoidObstacle Lesson Active");
    //         SpawnObjects(avoidObstacles: true);
    //     }
    //     else
    //     {
    //         // Easier lesson: standard goal finding
    //         Debug.Log("FindGoal Lesson Active");
    //         SpawnObjects(avoidObstacles: false);
    //     }
    // }



    // Called when the agent requests a decision
    public override void CollectObservations(VectorSensor sensor)
    {
        //The goal's position
        float goalPositionX = _goal.localPosition.x / 5f;
        float goalPositionZ = _goal.localPosition.z / 5f;

        //The turtle's position
        float turtlePositionX = transform.localPosition.x / 5f;
        float turtlePositionZ = transform.localPosition.z / 5f;

        //The turtle's direction
        float turtleRotationNormalized = (transform.localRotation.eulerAngles.y / 360f) * 2f - 1f;

        sensor.AddObservation(goalPositionX);
        sensor.AddObservation(goalPositionZ);
        sensor.AddObservation(turtlePositionX);
        sensor.AddObservation(turtlePositionZ);
        sensor.AddObservation(turtleRotationNormalized);


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



    // private void SpawnObjects()
    // {
    //     transform.localRotation = Quaternion.identity;
    //     transform.localPosition = new Vector3(0f, 0.15f, 0f);

    //     Vector3 spawnSize = new Vector3(0.5f, 0.5f, 0.5f); // Size of the goal
    //     // What does this line do with thr layer?   
    //     LayerMask obstacleLayer = LayerMask.GetMask("Obstacle");

    //     Vector3 spawnPosition;

    //     do
    //     {
    //         float randomX = Random.Range(-2.5f, 2.5f);
    //         float randomZ = Random.Range(-2.5f, 2.5f);
    //         spawnPosition = new Vector3(randomX, 0.3f, randomZ);
    //     }
    //     while (Physics.OverlapBox(spawnPosition, spawnSize, Quaternion.identity, obstacleLayer).Length > 0);

    //     _goal.localPosition = spawnPosition;
    // }

    private void SpawnObjects()
    {

        Instantiate(obstacles, spawnPosition, transform.rotation);
    }




    // private void SpawnObjects()
    // {
    //     transform.localRotation = Quaternion.identity;
    //     transform.localPosition = new Vector3(0f, 0.15f, 0f);

    //     float spawnEdge = Random.Range(0, 4); // Pick one of the four edges
    //     print("Spawn Edge: " + spawnEdge);
    //     float spawnX = 0f;
    //     float spawnZ = 0f;

    //     float edgeOffset = 4f; // Keeps ball near walls but inside the grid (-4 to 4 range)
    //     float randomOffset = Random.Range(-4f, 4f); // Random within boundary

    //     switch ((int)spawnEdge)
    //     {
    //         case 0: // Left Edge
    //             spawnX = -edgeOffset;
    //             spawnZ = randomOffset;
    //             break;
    //         case 1: // Right Edge
    //             spawnX = edgeOffset;
    //             spawnZ = randomOffset;
    //             break;
    //         case 2: // Bottom Edge
    //             spawnX = randomOffset;
    //             spawnZ = -edgeOffset;
    //             break;
    //         case 3: // Top Edge
    //             spawnX = randomOffset;
    //             spawnZ = edgeOffset;
    //             break;
    //     }

    //     _goal.localPosition = new Vector3(spawnX, 0.3f, spawnZ);
    // }



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
        // Debug.Log("Cumulative Reward: " + _cumulativeReward);
        floorRenderer.material = winMaterial;
        EndEpisode();
    }

    private void Penalise()
    {
        AddReward(-1f); //Penalise for hitting the wall
        _cumulativeReward = GetCumulativeReward();
        // Debug.Log("Cumulative Reward: " + _cumulativeReward);
        floorRenderer.material = loseMaterial;
        EndEpisode();
    }

}
