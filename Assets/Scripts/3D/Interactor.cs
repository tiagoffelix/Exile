using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

interface IInteractable {
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactorSource;
    private float interactRange;

    private void Start()
    {
        interactRange = 5;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}