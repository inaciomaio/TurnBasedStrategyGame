using UnityEngine;

public class Unit : MonoBehaviour {

    private const string IS_WALKING = "IsWalking";
    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;
    private GridPosition currentGridPosition;

    private void Awake() {
        targetPosition = transform.position;
    }

    private void Start() {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);
    }

    private void Update() {


        float stoppingDistance = .1f;

        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance) {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;


            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            unitAnimator.SetBool(IS_WALKING, true);
        }
        else {
            unitAnimator.SetBool(IS_WALKING, false);
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != currentGridPosition) {
            LevelGrid.Instance.UnitMovedGridPosition(this, currentGridPosition, newGridPosition);
            currentGridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition) {
        this.targetPosition = targetPosition;

    }
}
