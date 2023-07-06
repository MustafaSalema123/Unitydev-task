
using UnityEngine;

namespace Raycasting {


public class PositionRelative {
        private Vector3 position;
        private Transform parent;

   

        public PositionRelative(Vector3 m_position, Transform m_parent) {
            parent = m_parent;
            setWorldPosition(m_position);
        }

        public Vector3 getWorldPosition() { return (parent == null) ? position : parent.TransformPoint(position); }
        public Vector3 getLocalPosition() { return position; }
        public Transform getParent() { return parent; }
        public void setWorldPosition(Vector3 m_position) { position = (parent == null) ? m_position : parent.InverseTransformPoint(m_position); }
        public void setLocalPosition(Vector3 m_position) { position = m_position; }
        public void setParent(Transform m_parent) {
            Vector3 old_Position = getWorldPosition();
            parent = m_parent;
            setWorldPosition(old_Position);
        }
    }

 
    public abstract class Cast {

        protected PositionRelative origin;
        protected PositionRelative end;

     



        public Cast(Vector3 m_origin, Vector3 m_end, Transform m_parentOrigin, Transform m_parentEnd) {
            origin = new PositionRelative(m_origin, m_parentOrigin);
            end = new PositionRelative(m_end, m_parentEnd);
        }

        // Returns values in World Space
        public Vector3 getOrigin() { return origin.getWorldPosition(); }
        public Vector3 getDirection() { return (end.getWorldPosition() - origin.getWorldPosition()); }
      
        public Vector3 getEnd() { return end.getWorldPosition(); }

   
        public void setEnd(Vector3 m_end) { end.setWorldPosition(m_end); }
        public void setDistance(float m_distance) { Vector3 p = getOrigin(); end.setWorldPosition(p + (getEnd() - p).normalized * m_distance); }
        public abstract bool castRay(out RaycastHit hitInfo, LayerMask layerMask, QueryTriggerInteraction q = QueryTriggerInteraction.Ignore);

        public abstract RaycastHit[] castRayAll(LayerMask layerMask, QueryTriggerInteraction q = QueryTriggerInteraction.Ignore);

    }


    public class RayCast : Cast {

     
        public RayCast(Vector3 m_origin, Vector3 m_end, Transform m_parentOrigin, Transform m_parentEnd) : base(m_origin, m_end, m_parentOrigin, m_parentEnd) { }


        public override bool castRay(out RaycastHit hitInfo, LayerMask layerMask, QueryTriggerInteraction q = QueryTriggerInteraction.Ignore) {
            Vector3 v = getDirection();
            return Physics.Raycast(getOrigin(), v.normalized, out hitInfo, v.magnitude, layerMask, q);
        }

        public override RaycastHit[] castRayAll(LayerMask layerMask, QueryTriggerInteraction q = QueryTriggerInteraction.Ignore) {
            Vector3 v = getDirection();
            return Physics.RaycastAll(getOrigin(), v, v.magnitude, layerMask, q);
        }

        
    }


    public class SphereCast : Cast {
        protected float radius;

    

      

        public SphereCast(Vector3 m_origin, Vector3 m_end, float m_radius, Transform m_parentOrigin,Transform m_parentEnd) : base(m_origin, m_end, m_parentOrigin, m_parentEnd) {
            setRadius(m_radius);
        }

        // Transform radius with origin Transforms lossyscale.z
        public float getRadius() { return (origin.getParent() == null) ? radius : origin.getParent().lossyScale.z * radius; }

        public void setRadius(float m_radius) { radius = (origin.getParent() == null) ? m_radius : m_radius / origin.getParent().lossyScale.z; }

        public override bool castRay(out RaycastHit hitInfo, LayerMask layerMask, QueryTriggerInteraction q = QueryTriggerInteraction.Ignore) {
            Vector3 v = getDirection();
            return Physics.SphereCast(getOrigin(), getRadius(), v.normalized, out hitInfo, v.magnitude, layerMask, q);
        }

        public override RaycastHit[] castRayAll(LayerMask layerMask, QueryTriggerInteraction q = QueryTriggerInteraction.Ignore) {
            Vector3 v = getDirection();
            return Physics.SphereCastAll(getOrigin(), getRadius(), v.normalized, v.magnitude, layerMask, q);
        }

      
    }
}