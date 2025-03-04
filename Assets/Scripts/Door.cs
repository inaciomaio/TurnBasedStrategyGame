using System;
using UnityEditor.Animations;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] private bool isOpen;
    const string IS_OPEN = "IsOpen";
    private Animator animator;
    private GridPosition gridPosition;
    private Action onInteractComplete;
    private bool isActive;
    private float timer;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }

    private void Update() {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f) {
            isActive = false;
            onInteractComplete();
        }
    }

    public void Interact(Action onInteractComplete) {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = .5f;

        if (isOpen) {
            CloseDoor();
        }
        else {
            OpenDoor();
        }
    }

    private void OpenDoor() {
        isOpen = true;
        animator.SetBool(IS_OPEN, true);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor() {
        isOpen = false;
        animator.SetBool(IS_OPEN, false);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
