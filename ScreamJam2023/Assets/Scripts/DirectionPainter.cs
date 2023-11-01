using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lighter;

public class DirectionPainter : MonoBehaviour
{
    
    public Transform direction;
    public LayerMask layerMask;
    public float burnDistance =  5.0f;
    public bool isActive = true;
    [Space]
    public bool mouseSingleClick;
    [Space]
    public Color paintColor;
    
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;
    public float radiusCoefficient = 0.5f; // Коэффициент для контроля зависимости
    public float minRadius = 1.0f; // Минимальное значение радиуса
    void Update()
    {
        if (!isActive)
            return;

        Vector3 directionVector = direction.position - transform.position;
        Ray ray = new Ray(transform.position, directionVector);
        RaycastHit[] hits = Physics.RaycastAll(ray, burnDistance, layerMask, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
            // transform.position = hit.point;

            float distanceToHit = Vector3.Distance(transform.position, hit.point);
          // Debug.Log("Distance to Hit: " + distanceToHit);

            Paintable p = hit.collider.GetComponent<Paintable>();
            if (p != null)
            {
                // Изменяем радиус в зависимости от расстояния с учетом коэффициента
                float newRadius = minRadius + distanceToHit * radiusCoefficient;
                PaintManager.instance.paint(p, hit.point, newRadius, hardness, strength, paintColor);
            }
        }
    }
}
