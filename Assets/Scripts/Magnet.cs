using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Magnet : MonoBehaviour
{
    public static Magnet Instance;

    [SerializeField] private float magnetForce;
    private List<Rigidbody> affectedRigidbodies = new List<Rigidbody>();
    private Transform magnet;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        magnet = transform;
        affectedRigidbodies.Clear();
    }

    private void FixedUpdate()
    {
        if (!GameManager.isGameOver && GameManager.isMoving)
        {
            foreach (Rigidbody rb in affectedRigidbodies)
            {
                rb.AddForce((magnet.position - rb.position) * magnetForce * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.isGameOver && (other.CompareTag("Obstacle") || other.CompareTag("Object")))
        {
            AddToMagnetField(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!GameManager.isGameOver && (other.CompareTag("Obstacle") || other.CompareTag("Object")))
        {
            RemoveFromMagnetField(other.attachedRigidbody);
        }

    }

    public void AddToMagnetField(Rigidbody rb)
    {
        affectedRigidbodies.Add(rb);
    }

    public void RemoveFromMagnetField(Rigidbody rb)
    {
        affectedRigidbodies.Remove(rb);

    }
}
