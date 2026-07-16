using UnityEngine;

/// Tự đọc mesh của object, lặp qua TẤT CẢ các mặt tam giác trên model,
/// vẽ vài đường zic-zac (vết nứt) nằm phẳng lên MỖI mặt bằng LineRenderer.
/// Không cần texture/model riêng.
public static class ProceduralCrack {
    public static GameObject SpawnOn(GameObject target, int linesPerFace, float length, Color color, float surfaceOffset = 0.02f)
    {
        MeshFilter meshFilter = target.GetComponentInChildren<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogWarning($"ProceduralCrack: '{target.name}' không tìm thấy MeshFilter/Mesh, không vẽ được nứt.");
            return null;
        }

        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        GameObject crackRoot = new GameObject("Crack");
        crackRoot.transform.SetParent(meshFilter.transform, false);
        crackRoot.transform.localPosition = Vector3.zero;
        crackRoot.transform.localRotation = Quaternion.identity;
        crackRoot.transform.localScale = Vector3.one;

        // Duyệt qua từng mặt tam giác (mỗi 3 chỉ số = 1 mặt) trên mesh
        for (int t = 0; t < triangles.Length; t += 3)
        {
            Vector3 v0 = vertices[triangles[t]];
            Vector3 v1 = vertices[triangles[t + 1]];
            Vector3 v2 = vertices[triangles[t + 2]];

            Vector3 centroid = (v0 + v1 + v2) / 3f;
            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            GameObject faceRoot = new GameObject("CrackFace");
            faceRoot.transform.SetParent(crackRoot.transform, false);
            faceRoot.transform.localPosition = centroid + normal * surfaceOffset;

            Vector3 tangent = Vector3.Cross(normal, Vector3.up);
            if (tangent.sqrMagnitude < 0.001f)
                tangent = Vector3.Cross(normal, Vector3.right);
            tangent.Normalize();
            Vector3 bitangent = Vector3.Cross(normal, tangent);

            for (int i = 0; i < linesPerFace; i++)
                CreateCrackLine(faceRoot.transform, tangent, bitangent, length, color);
        }

        return crackRoot;
    }

    private static void CreateCrackLine(Transform parent, Vector3 tangent, Vector3 bitangent, float length, Color color)
    {
        GameObject lineObj = new GameObject("CrackLine");
        lineObj.transform.SetParent(parent, false);

        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = color;
        line.endColor = color;
        line.widthMultiplier = 0.015f;
        line.useWorldSpace = false;
        line.numCapVertices = 2;

        int segments = Random.Range(4, 7);
        line.positionCount = segments;

        Vector2 dir2D = Random.insideUnitCircle.normalized;

        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);
            Vector2 jitter = Random.insideUnitCircle * (length * 0.15f);
            Vector2 point2D = dir2D * length * t + jitter;

            Vector3 point3D = tangent * point2D.x + bitangent * point2D.y;
            line.SetPosition(i, point3D);
        }
    }
}   