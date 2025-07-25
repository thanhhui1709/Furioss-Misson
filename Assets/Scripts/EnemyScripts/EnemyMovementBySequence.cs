using UnityEngine;

public class EnemyMovementBySequence : MonoBehaviour
{
    public MovementSequence MovementSequence;
    [HideInInspector]
    public IMovementPattern currentPattern;
    private int currentPatternIndex = 0;

    private float timer = 0f;
    [SerializeField] private float delayTime;

    private enum MovementState { Moving, Waiting }
    private MovementState state;

   
    private void OnEnable()
    {
        if (currentPattern == null)
        {
            currentPatternIndex = 0;
            SetCurrentPattern(currentPatternIndex);
        }
        state=MovementState.Moving;


    }

    void FixedUpdate()
    {
        switch (state)
        {
            case MovementState.Moving:
                currentPattern?.UpdateMovement(transform, Time.fixedDeltaTime);
                if (currentPattern != null && currentPattern.isFinished)
                {
                    state = MovementState.Waiting;
                    timer = 0f;
                    
                }
                break;

            case MovementState.Waiting:
                timer += Time.fixedDeltaTime;
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
