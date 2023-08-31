using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NeighborStatus : MonoBehaviour
{
    [Inject] private GridNodeInformation _gridNodeInformation;
    private AIDestinationSetterCustom _aiDestinationSetterCustom;
    [Inject] private SignalBus _signalBus;
    
    private bool isAgentNodesCustomNeighborNodeAvailable;
    private Item _occupiedByGameObject;

    private void Awake()
    {
        _aiDestinationSetterCustom = GetComponent<AIDestinationSetterCustom>();
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<ColorBoxSignals.SendNodeInformationToNeighborStatusSignal>(NeighborNodesOccupancyCheck);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<ColorBoxSignals.SendNodeInformationToNeighborStatusSignal>(NeighborNodesOccupancyCheck);
    }

    private void NeighborNodesOccupancyCheck(ColorBoxSignals.SendNodeInformationToNeighborStatusSignal receivedInformationToNeighborStatusSignal)
    {
        if (receivedInformationToNeighborStatusSignal.GameObject == gameObject)
        {
            var AgentNode = receivedInformationToNeighborStatusSignal.targetNode;
            var CurrentAgentIndexNode = _aiDestinationSetterCustom.TargetNode.NodeIndex;
            if (CurrentAgentIndexNode != null)
            {
                //Debug.Log($"Received Agent node index: {AgentNode.NodeIndex} and current agent {CurrentAgentIndexNode} position");    
            }

            if (AgentNode.NodeIndex == CurrentAgentIndexNode)
            {
                var allNodesCustom = _gridNodeInformation.AllNodesCustom;
                //Debug.Log(allNodesCustom.Count+ " here. Agent node index: " + AgentNode.NodeIndex + "agent information " + allNodesCustom[AgentNode.NodeIndex]);

                var neighborNodes = allNodesCustom[AgentNode.NodeIndex].GetAllNeighbors(); //got neighbor of agent nodes
                //Debug.Log("Neighbour nodes count: " + neighborNodes.Count + " and got all neighbor of AgentNode");

                var AllNeighborNodesOfAgentNodeCustom = new List<NodeWrapper>();
                foreach (var neighborNode in neighborNodes)
                {
                    var nodeWrapper = allNodesCustom[neighborNode.NodeIndex];
                    AllNeighborNodesOfAgentNodeCustom.Add(nodeWrapper);
                }
                
                var neighborCount = 0;
                var custormNeighborNumber = 0;

                foreach (var customNeighborNodeOfAgent in AllNeighborNodesOfAgentNodeCustom)
                {
                    custormNeighborNumber++;
                    if (!customNeighborNodeOfAgent.isOccupied)
                    {
                        neighborCount++;
                        //Debug.Log($"{custormNeighborNumber} Neighbor is empty");
                        isAgentNodesCustomNeighborNodeAvailable = true;
                    }
                    else
                    {
                        //While Neighbor Node is occupied by an Agent. 
                        //Debug.Log($"Number {custormNeighborNumber} Neighbor is Filled by {customNeighborNodeOfAgent.OccupiedBy.name}");
                        if (customNeighborNodeOfAgent.OccupiedBy != null)
                        {
                            _occupiedByGameObject = (customNeighborNodeOfAgent.OccupiedBy.GetComponent<Item>());
                            //Debug.Log($"{customNeighborNodeOfAgent.OccupiedBy.GetComponent<Item>().name} added to Occupied Game Object list");

                            // now check my My agent and see if it has the same color. if same then call Merge method.
                            if ( ((AgentNode.NodeIndex+10) == customNeighborNodeOfAgent.graphNode.NodeIndex) || ((AgentNode.NodeIndex-10) == customNeighborNodeOfAgent.graphNode.NodeIndex) || ((AgentNode.NodeIndex+1) == customNeighborNodeOfAgent.graphNode.NodeIndex) || ((AgentNode.NodeIndex-1) == customNeighborNodeOfAgent.graphNode.NodeIndex))
                            {
                                
                                Debug.Log("The index number of neighbor node: " + customNeighborNodeOfAgent.graphNode.NodeIndex);
                                //calling merge method
                                Debug.Log($"The object that is going to merge now {_occupiedByGameObject.gameObject.name}");
                                CheckAndMergeColorBox(_occupiedByGameObject.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    private void CheckAndMergeColorBox(GameObject NeighborGameobject)
    {
        /*Debug.Log("Neighbor GameObject Name: " +NeighborGameobject.gameObject.name);
        Debug.Log("Agent GameObject Name: " +_aiDestinationSetterCustom.gameObject.name);
        Debug.Log("Neighbor GameObject Color: " +NeighborGameobject.GetComponent<Item>().MaterialColor);
        Debug.Log("Agent GameObject color: " + _aiDestinationSetterCustom.gameObject.GetComponent<Item>().MaterialColor);*/
            if (_aiDestinationSetterCustom.gameObject.GetComponent<Item>().MaterialColor == NeighborGameobject.GetComponent<Item>().MaterialColor)
            {
                /*Debug.Log($"Agent name is {_aiDestinationSetterCustom.gameObject.GetComponent<Item>().name} and it matched with {NeighborGameobject.gameObject.name}");
                Debug.Log($"Agent color is {_aiDestinationSetterCustom.gameObject.GetComponent<Item>().MaterialColor} and color matched with {NeighborGameobject.gameObject.GetComponent<Item>().MaterialColor}");*/

                var NeighbourAgentNodeWrapper = _gridNodeInformation.AllNodesCustom[NeighborGameobject.GetComponent<AIDestinationSetterCustom>().TargetNode.NodeIndex];
                if (NeighbourAgentNodeWrapper != null)
                {
                    Debug.Log("Index Number of Neighbour Node:" +  NeighbourAgentNodeWrapper.OccupiedBy);
                    NeighbourAgentNodeWrapper.ClearingNode();
                    NeighborGameobject.gameObject.SetActive(false);
                    Destroy(NeighborGameobject.gameObject);
                }
                var agentNodeWrapper = _gridNodeInformation.AllNodesCustom[_aiDestinationSetterCustom.TargetNode.NodeIndex];
                if (agentNodeWrapper != null)
                {
                    //Debug.Log("Index Number of Agent Node:" +  agentNodeWrapper);
                    agentNodeWrapper.ClearingNode();
                    gameObject.SetActive(false);
                    Destroy(gameObject);    
                }

                
                //Debug.Log( "Both Destroyed");
            }
            else
            {
                /*Debug.Log($"Agent name is {_aiDestinationSetterCustom.gameObject.GetComponent<Item>().name} and it did not matched with {NeighborGameobject.gameObject.name}"); 
                Debug.Log($"Agent color is {_aiDestinationSetterCustom.gameObject.GetComponent<Item>().MaterialColor} and color did not matched with {NeighborGameobject.gameObject.GetComponent<Item>().MaterialColor}");*/
            }
    }
}
