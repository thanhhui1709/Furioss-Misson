using UnityEngine;

public class EnemyMovementBySequence : MonoBehaviour
{
    public MovementSequence MovementSequence;
    [HideInInspector]
    public MovementPattern currentPattern;
    private int currentPatternIndex = 0;

    private float timer = 0f;
    [SerializeField] private float delayTime;

    private enum MovementState { Moving, Waiting }
    private MovementState state;

    void Start()
    {
        SetCurrentPattern(0);
        state = MovementState.Moving;
    }

    void Update()
    {
        switch (state)
        {
            case MovementState.Moving:
                currentPattern?.UpdateMovement(transform, Time.deltaTime);
                if (currentPattern != null && currentPattern.isFinished)
                {
                    state = MovementState.Waiting;
                    timer = 0f;
                    
                }
                break;

            case MovementState.Waiting:
                timer += Time.deltaTime;
                if (timer >= delayTime)
                {
                    if (currentPatternIndex < MovementSequence.sequences.Count - 1)
                    {
                        currentPatternIndex++;
                        SetCurrentPattern(currentPatternIndex);
                        state = MovementState.Moving;
                    }
                    else
                    {
                      
                        enabled = false; 
                    }
                }
                break;
        }
    }

    private void SetCurrentPattern(int index)
    {
        currentPattern = Instantiate(MovementSequence.sequences[index]);
        currentPattern.Initialize(transform);
    }
}
