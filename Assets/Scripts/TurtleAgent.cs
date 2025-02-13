using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TurtleAgent : Agent
{
    [SerializeField] private Transform _goal;
    [SerializeField] private float _moveSpeed = 1.5f;
    [SerializeField] private float _rotationSpeed = 180f;

    private Renderer _renderer;

    private int _currentEpisode = 0;
    private float _cumulativeReward = 0;

    // Called when the agent is first initialized
    public override void Initialize()
    {
        Debug.Log("Agent Initialized");
        _renderer = GetComponent<Renderer>();
        _currentEpisode = 0;
        _cumulativeReward = 0;
    }

    // Called when the agent is reset
    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode Begin");

        _currentEpisode++;
        _cumulativeReward = 0f;
        _renderer.material.color = Color.blue;

        SpawnObjects();
    }



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



    private void SpawnObjects()
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0f, .15f, 0f);


        //Randomise the distance within the range [1, 2.5]
        float randomX = Random.Range(-2.5f, 2.5f);
        float randomZ = Random.Range(-2.5f, 2.5f);

        //Apply the calculated position to the goal
        _goal.localPosition = new Vector3(randomX, 0.3f, randomZ);

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
            Debug.Log("Goal Reached");
            GoalReached();
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall Hit");
            Penalise();
        }
    }

    private void GoalReached()
    {
        AddReward(2f); //Large reward for reaching goal
        _cumulativeReward = GetCumulativeReward();
        Debug.Log("Cumulative Reward: " + _cumulativeReward);

        EndEpisode();
    }

    private void Penalise()
    {
        AddReward(-1f); //Penalise for hitting the wall
        _cumulativeReward = GetCumulativeReward();
        Debug.Log("Cumulative Reward: " + _cumulativeReward);
        EndEpisode();
    }
}
