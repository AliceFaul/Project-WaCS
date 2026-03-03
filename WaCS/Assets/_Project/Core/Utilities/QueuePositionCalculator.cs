using UnityEngine;

public class QueuePositionCalculator : MonoBehaviour
{
    [SerializeField] private Transform queueStartPoint;
    [SerializeField] private float spacingBetweenPositions = 1.5f;

    public Vector3 GetQueuePosition(int index)
    {
        return queueStartPoint.position - queueStartPoint.forward
            * spacingBetweenPositions * index;
    }
}
