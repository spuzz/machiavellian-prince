using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BorderController : MonoBehaviour {

    [SerializeField] Material material;
    static int borderPoints = 60;
    private float borderDistance;
    List<SolarSystem> nearbySystems = new List<SolarSystem>();
    List<Vector2> points = new List<Vector2>();
    public BorderController()
    {
        for (int a = 0; a < borderPoints; a++)
        {
            points.Add(Vector2.zero);
        }
    }
    public void SetBorderDistance(float distance)
    {
        borderDistance = distance;
        for (int a = 0; a < borderPoints; a++)
        {
            points[a] =  Vector2FromAngle((360.0f / (float)borderPoints) * (float)a) * distance;
        }
    }


	public List<Vector2> GetBorderPoints()
    {
        return points;
    }

    public void AddNearbySystem(SolarSystem system)
    {
        nearbySystems.Add(system);
    }

    public bool GrowBorder()
    {
        bool moved = false;
        Vector2 startPoint = new Vector2(transform.position.x, transform.position.z);

        for (int a = 0; a < borderPoints; a++)
        {
               
            Vector2 endPoint = new Vector2(transform.position.x + points[a].x, transform.position.z + points[a].y);
            SolarSystem system = FindClosestSystem(endPoint, nearbySystems);
            if (system && Vector2.Distance(endPoint, new Vector2(system.transform.position.x, system.transform.position.z)) < borderDistance)
            {
                Vector2 firstLoc = Vector2.zero;
                Vector2 secondLoc = Vector2.zero;
                int result = FindCircleCircleIntersections(system.transform.position.x, system.transform.position.z, borderDistance, transform.position.x, transform.position.z, borderDistance, out firstLoc, out secondLoc);
                if (result == 2)
                {
                    Vector2 outPoint = Vector2.zero;
                    LineIntersection(startPoint, endPoint, firstLoc, secondLoc, ref outPoint);
                    if(outPoint != Vector2.zero && Vector2.Distance(outPoint - startPoint,points[a]) >= 0.1)
                    {
                        points[a] = outPoint - startPoint;
                        moved = true;
                    }

                }
            }

        }
        return moved;
    }

    private SolarSystem FindClosestSystem(Vector2 point, List<SolarSystem> nearbySystems)
    {
        SolarSystem closest = null;
        float currentDistance = -1;
        foreach (SolarSystem system in nearbySystems)
        {
            float distance = Vector2.Distance(point, new Vector2(system.transform.position.x, system.transform.position.z));
            if (currentDistance == -1 || distance < currentDistance)
            {
                closest = system;
                currentDistance = distance;
            }
        }
        return closest;
    }

    private int FindCircleCircleIntersections(
        float cx0, float cy0, float radius0,
        float cx1, float cy1, float radius1,
        out Vector2 intersection1, out Vector2 intersection2)
    {
        // Find the distance between the centers.
        float dx = cx0 - cx1;
        float dy = cy0 - cy1;
        double dist = Math.Sqrt(dx * dx + dy * dy);

        // See how many solutions there are.
        if (dist > radius0 + radius1)
        {
            // No solutions, the circles are too far apart.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if (dist < Math.Abs(radius0 - radius1))
        {
            // No solutions, one circle contains the other.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if ((dist == 0) && (radius0 == radius1))
        {
            // No solutions, the circles coincide.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else
        {
            // Find a and h.
            double a = (radius0 * radius0 -
                radius1 * radius1 + dist * dist) / (2 * dist);
            double h = Math.Sqrt(radius0 * radius0 - a * a);

            // Find P2.
            double cx2 = cx0 + a * (cx1 - cx0) / dist;
            double cy2 = cy0 + a * (cy1 - cy0) / dist;

            // Get the points P3.
            intersection1 = new Vector2(
                (float)(cx2 + h * (cy1 - cy0) / dist),
                (float)(cy2 - h * (cx1 - cx0) / dist));
            intersection2 = new Vector2(
                (float)(cx2 - h * (cy1 - cy0) / dist),
                (float)(cy2 + h * (cx1 - cx0) / dist));

            // See if we have 1 or 2 solutions.
            if (dist == radius0 + radius1) return 1;
            return 2;
        }
    }
  

    public void CreateBorderMesh()
    {
        
        Triangulator tr = new Triangulator(points);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[points.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(points[i].x, 0, points[i].y);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
        gameObject.AddComponent(typeof(MeshRenderer));
        MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        gameObject.GetComponent<MeshRenderer>().material = material;
        filter.mesh = msh;
        //gameObject.AddComponent<MeshCollider>();
    }

    public Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    public static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 intersection)
    {

        float Ax, Bx, Cx, Ay, By, Cy, d, e, f, num/*,offset*/;

        float x1lo, x1hi, y1lo, y1hi;



        Ax = p2.x - p1.x;

        Bx = p3.x - p4.x;



        // X bound box test/

        if (Ax < 0)
        {

            x1lo = p2.x; x1hi = p1.x;

        }
        else
        {

            x1hi = p2.x; x1lo = p1.x;

        }



        if (Bx > 0)
        {

            if (x1hi < p4.x || p3.x < x1lo) return false;

        }
        else
        {

            if (x1hi < p3.x || p4.x < x1lo) return false;

        }



        Ay = p2.y - p1.y;

        By = p3.y - p4.y;



        // Y bound box test//

        if (Ay < 0)
        {

            y1lo = p2.y; y1hi = p1.y;

        }
        else
        {

            y1hi = p2.y; y1lo = p1.y;

        }



        if (By > 0)
        {

            if (y1hi < p4.y || p3.y < y1lo) return false;

        }
        else
        {

            if (y1hi < p3.y || p4.y < y1lo) return false;

        }



        Cx = p1.x - p3.x;

        Cy = p1.y - p3.y;

        d = By * Cx - Bx * Cy;  // alpha numerator//

        f = Ay * Bx - Ax * By;  // both denominator//



        // alpha tests//

        if (f > 0)
        {

            if (d < 0 || d > f) return false;

        }
        else
        {

            if (d > 0 || d < f) return false;

        }



        e = Ax * Cy - Ay * Cx;  // beta numerator//



        // beta tests //

        if (f > 0)
        {

            if (e < 0 || e > f) return false;

        }
        else
        {

            if (e > 0 || e < f) return false;

        }



        // check if they are parallel

        if (f == 0) return false;

        // compute intersection coordinates //

        num = d * Ax; // numerator //

        //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;   // round direction //

        //    intersection.x = p1.x + (num+offset) / f;
        intersection.x = p1.x + num / f;



        num = d * Ay;

        //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;

        //    intersection.y = p1.y + (num+offset) / f;
        intersection.y = p1.y + num / f;



        return true;

    }


    public void OnDrawGizmos()
    {
        foreach (Vector2 point in points)
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + point.x, 0, transform.position.z + point.y), 0.5f);
        }
    }
}

public class Triangulator
{
    private List<Vector2> m_points = new List<Vector2>();

    public Triangulator(Vector2[] points)
    {
        m_points = new List<Vector2>(points);
    }
    public Triangulator(List<Vector2> points)
    {
        m_points = points;
    }
    public int[] Triangulate()
    {
        List<int> indices = new List<int>();

        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];
        if (Area() > 0)
        {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else
        {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0)
                return indices.ToArray();

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (Snip(u, v, w, nv, V))
            {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        indices.Reverse();
        return indices.ToArray();
    }

    private float Area()
    {
        int n = m_points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    private bool Snip(int u, int v, int w, int n, int[] V)
    {
        int p;
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = m_points[V[p]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }

    private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;

        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;

        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;

        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }
}

