using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Controller
{
public class PlayerController : MonoBehaviour
{
    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] MeshCollider _meshCollider;
    [SerializeField] float radius;

    private Mesh mesh;
    private List<int> holeVertices;
    private List<Vector3> offsets;
    public Vector2 moveLimits;
    private int holeVerticesCount;

    private float x, y;
    private Vector3 touch, targetPos;

    public Transform rotatingCircle;

    // Start is called before the first frame update
    void Start()
    {
        holeVertices = new List<int>();
        offsets = new List<Vector3>();
        mesh = _meshFilter.mesh;

        FindHoleVertices();
        RotateCircle();
    }

    void RotateCircle()
    {
        rotatingCircle.DORotate(new Vector3(90f, 0f, -90f), 0.2f)
            .SetEase(Ease.Linear).From(new Vector3(90f, 0f, 0f)).SetLoops(-1, LoopType.Incremental);
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        GameManager.isMoving = Input.GetMouseButton(0);

        if (!GameManager.isGameOver && GameManager.isMoving)
        {
            Move();
            UpdateVertices();
        }
        #else
        GameManager.isMoving = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;

        if (!GameManager.isGameOver && GameManager.isMoving)
        {
            Move();
            UpdateVertices();
        }
        #endif


    }

    void Move()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        Vector3 input = new Vector3(x, 0, y);
        touch = Vector3.Lerp(transform.position, transform.position + input, Constants.HOLE_SPEED * Time.deltaTime);

        Vector3 target = new Vector3(Mathf.Clamp(touch.x, -moveLimits.x, moveLimits.x),
            0,
            Mathf.Clamp(touch.z, -moveLimits.y, moveLimits.y));

        transform.position = target;
    }

    void UpdateVertices()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            vertices[holeVertices[i]] = transform.position + offsets[i];
        }

        mesh.vertices = vertices;
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, mesh.vertices[i]);
            if (distance < radius)
            {
             holeVertices.Add(i);
             offsets.Add(mesh.vertices[i] - transform.position);
            }
        }

        holeVerticesCount = holeVertices.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
}
