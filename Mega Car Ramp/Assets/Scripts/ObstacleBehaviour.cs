using UnityEngine;
using DG.Tweening;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private Transform Obstacle;

    #region Rotation References

    [Header("Rotation Settings")]
    [SerializeField] private bool Rotation;
    [SerializeField, ConditionalHide(nameof(Rotation), true)] private bool Rotation_X;
    [SerializeField, ConditionalHide(nameof(Rotation), true)] private bool Rotation_Y;
    [SerializeField, ConditionalHide(nameof(Rotation), true)] private bool Rotation_Z;
    [SerializeField, ConditionalHide(nameof(Rotation), true)] private float Rotation_Speed = 90f;

    #endregion

    #region Movement References

    [Header("Movement Settings")]
    [SerializeField] private bool Movement;
    [SerializeField, ConditionalHide(nameof(Movement), true)] private bool Movement_X;
    [SerializeField, ConditionalHide(nameof(Movement), true)] private bool Movement_Y;
    [SerializeField, ConditionalHide(nameof(Movement), true)] private bool Movement_Z;
    [SerializeField, ConditionalHide(nameof(Movement), true)] private float Movement_Speed = 2f;
    [SerializeField, ConditionalHide(nameof(Movement), true)] private float Move_Min = -5f, Move_Max = 5f;

    private Vector3 initialPosition;

    #endregion

    #region Rotaion from Min To Max

    [Header("Rotation Settings")]
    [SerializeField] private bool Rotation_Front_Back;
    [SerializeField, ConditionalHide(nameof(Rotation_Front_Back), true)] private bool Rotation_X_;
    [SerializeField, ConditionalHide(nameof(Rotation_Front_Back), true)] private bool Rotation_Y_;
    [SerializeField, ConditionalHide(nameof(Rotation_Front_Back), true)] private bool Rotation_Z_;
    [SerializeField, ConditionalHide(nameof(Rotation_Front_Back), true)] private float RotationAmount;
    [SerializeField, ConditionalHide(nameof(Rotation_Front_Back), true)] private float rotationSpeed = 1.0f;  // Time to complete one back-and-forth rotation

    #endregion

    void Start()
    {
        if (Obstacle == null)
        {
            Obstacle = transform;
        }

        initialPosition = Obstacle.localPosition;

        // Start movement on specified axes
        HandleContinuousMovement();

        // Rotate between 0 and -90 on the X-axis continuously
        RotateBackAndForth();
    }

    void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (!Rotation) return;

        float angle = Rotation_Speed * Time.deltaTime;

        // Apply rotation only on selected axes
        if (Rotation_X) Obstacle.localRotation *= Quaternion.Euler(angle, 0, 0);
        if (Rotation_Y) Obstacle.localRotation *= Quaternion.Euler(0, angle, 0);
        if (Rotation_Z) Obstacle.localRotation *= Quaternion.Euler(0, 0, angle);
    }

    private void HandleContinuousMovement()
    {
        if (!Movement) return;

        // Move back and forth between Move_Min and Move_Max along each enabled axis
        if (Movement_X)
        {
            Obstacle.DOLocalMoveX(Move_Max, Movement_Speed)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
        if (Movement_Y)
        {
            Obstacle.DOLocalMoveY(Move_Max, Movement_Speed)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
        if (Movement_Z)
        {
            Obstacle.DOLocalMoveZ(Move_Max, Movement_Speed)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void RotateBackAndForth()
    {
        if (!Rotation_Front_Back) return;

        if (Rotation_X_)
        {
            transform.DOLocalRotate(new Vector3(RotationAmount, 0f, 0f), rotationSpeed)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo);
        }
        if (Rotation_Y_)
        {
            transform.DOLocalRotate(new Vector3(0f, RotationAmount, 0f), rotationSpeed)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo);
        }
        if (Rotation_Z_)
        {
            transform.DOLocalRotate(new Vector3(0f, 0f, RotationAmount), rotationSpeed)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo);
        }
    }

}
