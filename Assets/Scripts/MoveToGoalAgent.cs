using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] public GameObject GoalBall;

    private Vector3 PosicionBola;

    void Update(){
        if (GoalBall.transform.position.y <=0.2f){
            floorMeshRenderer.material = winMaterial;
            PosicionBola = new Vector3(Random.Range(-4f, +4f), 10, Random.Range(-2f, +2f));
            GoalBall.transform.localPosition = PosicionBola;
            GoalBall.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            GoalBall.SetActive(true);
        }
        
    }

    public override void OnEpisodeBegin()
    { 
        transform.localPosition = new Vector3(Random.Range(0f, -4f), 0, Random.Range(-2f, +2f));
        PosicionBola = new Vector3(Random.Range(-4f, +4f), 10, Random.Range(-2f, +2f));
        GoalBall.transform.localPosition = PosicionBola;
        GoalBall.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        GoalBall.SetActive(true);
        
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 5f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions =  actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    
    private void OnTriggerEnter (Collider other){
        if (other.TryGetComponent<Goal>(out Goal goal)){
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
        if(other.TryGetComponent<Wall>(out Wall wall)){
            SetReward(-1f);
            EndEpisode();
        }else{
            SetReward(+1f);
        }
        
    }
}
