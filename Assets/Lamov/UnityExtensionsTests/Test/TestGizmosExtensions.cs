using Lamov.UnityExtensions.Runtime.Draw;
using UnityEngine;

namespace Lamov.UnityExtensionsTests.Test
{
    [ExecuteAlways]
    public class TestGizmosExtensions : MonoBehaviour
    {
        private void Update()
        {
            /*DebugDrawExtensions.DrawCircle(transform.position + transform.up * 1f, Vector3.up, 1f, Color.red, 1f);
            DebugDrawExtensions.DrawRect(transform.position + transform.up * 1f, transform.up, transform.forward, 1, 2);
            DebugDrawExtensions.DrawBones(new [] {Vector3.zero + transform.up * 1f, Vector3.up + transform.up * 1f, Vector3.up + Vector3.forward + transform.up * 1f, Vector3.up * 2f + Vector3.forward * 2f + transform.up * 1f});*/
            
            DebugDrawExtensions.DrawSphere(transform.position, transform.forward, 1f, Color.magenta, .2f);
        }

        /*private void OnDrawGizmos()
        {
            GizmosDrawExtensions.DrawCircle(transform.position, Vector3.up, 1f);
            GizmosDrawExtensions.DrawRect(transform.position, transform.up, transform.forward, 1, 2);
            //GizmosDrawExtensions.DrawBone(Vector3.zero, Vector3.up, Vector3.forward);
            GizmosDrawExtensions.DrawBones(new [] {Vector3.zero, Vector3.up, Vector3.up + Vector3.forward, Vector3.up * 2f + Vector3.forward * 2f});
        }*/
    }
}