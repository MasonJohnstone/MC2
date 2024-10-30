using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public void Init(float width, float height, bool isCapsule, bool hasMomentum, Vector3 friction)
    {
        this.width = width;
        this.height = height;
        this.isCapsule = isCapsule;
        this.hasMomentum = hasMomentum;
        this.friction = friction;
    }

    float collisionOffset = 0.1f;
    float height, width;
    bool isCapsule;

    bool hasMomentum;
    Vector3 velocity;
    Vector3 oldPosition;

    float defaultTraction = 0.01f;
    float defaultContactTraction = 0.1f;
    float traction;

    Vector3 friction;

    public bool InContact;

    void Start()
    {
        oldPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (hasMomentum)
        {
            velocity = Vector3.Normalize(transform.position - oldPosition) * ((Vector3.Distance(transform.position, oldPosition) * (1f / Time.fixedDeltaTime)) - ((Vector3.Distance(transform.position, oldPosition) * (1f / Time.fixedDeltaTime)) * traction));

            oldPosition = transform.position;

            if (hasMomentum)
            {
                ApplyForce(velocity, false, false);
            }
        }

        RemoveContact();
    }

    public void ApplyForce(Vector3 force, bool useTraction, bool useFriction)
    {
        if (useTraction)
        {
            force *= traction;
        }

        force = force.normalized * force.magnitude * Time.fixedDeltaTime;

        int maxIterations = 10;
        while (force.magnitude > 0f && maxIterations > 0)
        {
            maxIterations--;

            RaycastHit hit;

            gameObject.GetComponent<Collider>().enabled = false;
            if (isCapsule)
            {
                Physics.CapsuleCast(transform.position + transform.up * (height / 2f - width / 2f), transform.position - transform.up * (height / 2f - width / 2f), width / 2f, force.normalized, out hit, force.magnitude + collisionOffset);
            }
            else
            {
                Physics.BoxCast(transform.position, new Vector3(width / 2f, height / 2f, width / 2f), force.normalized, out hit, transform.rotation, force.magnitude + collisionOffset);
            }
            gameObject.GetComponent<Collider>().enabled = true;

            if (!hit.collider)
            {
                transform.position += force;
                force = Vector3.zero;
            }
            else
            {
                transform.position += force.normalized * (hit.distance - collisionOffset);
                //change this so that it only sets when sliding on object not just when you hit object because it will not make sense with bouncing
                ApplyContact();

                force = force.normalized * (force.magnitude - (hit.distance - collisionOffset));
                float angle = Mathf.Clamp(Vector3.Angle(hit.normal, friction), 0f, 90f);
                force = Vector3.ProjectOnPlane(force, hit.normal);
                if (useFriction)
                    force = force.normalized * Mathf.Clamp(force.magnitude - (friction.magnitude * ((90f - angle) / 90f)), 0f, Mathf.Infinity);
            }
        }
    }

    void ApplyContact()
    {
        traction = defaultContactTraction;
        InContact = true;
    }

    void RemoveContact()
    {
        traction = defaultTraction;
        InContact = false;
    }
}
