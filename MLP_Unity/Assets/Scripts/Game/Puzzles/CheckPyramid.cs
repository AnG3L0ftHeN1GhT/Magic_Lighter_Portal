using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CheckPyramid : MonoBehaviour
{
    [Header("Configuração")]
    public float normalTolerance = 0.95f;
    public float positionTolerance = 0.05f;

    private bool puzzleSolved = false;

    void Update()
    {
        if (puzzleSolved)
            return;

        CheckFaces();
    }

    void CheckFaces()
    {
        // Procura todos os Planes dentro da pirâmide
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

        List<Renderer> planes = new List<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            if (renderer.gameObject.name.Contains("Plane"))
            {
                planes.Add(renderer);
            }
        }

        if (planes.Count == 0)
            return;

        // Verifica cada material de Kanji
        string[] kanjis =
        {
            "Kanji-1",
            "Kanji 2",
            "Kanji 3",
            "Kanji  4"
        };

        foreach (string kanji in kanjis)
        {
            if (CheckKanji(planes, kanji))
            {
                puzzleSolved = true;

                Debug.Log("Face completa com " + kanji + "!");

                SceneManager.LoadScene("gluh");

                return;
            }
        }
    }

    bool CheckKanji(List<Renderer> planes, string kanji)
    {
        List<Renderer> kanjiPlanes = new List<Renderer>();

        // Primeiro pega somente os Planes desse Kanji
        foreach (Renderer plane in planes)
        {
            if (plane.sharedMaterial == null)
                continue;

            if (plane.sharedMaterial.name.Contains(kanji))
            {
                kanjiPlanes.Add(plane);
            }
        }

        // Precisamos de pelo menos 2 Planes
        if (kanjiPlanes.Count < 2)
            return false;

        // Tenta encontrar um grupo de Planes alinhados
        for (int i = 0; i < kanjiPlanes.Count; i++)
        {
            Renderer reference = kanjiPlanes[i];

            Vector3 referenceNormal =
                reference.transform.forward.normalized;

            List<Renderer> alignedPlanes = new List<Renderer>();

            foreach (Renderer plane in kanjiPlanes)
            {
                Vector3 normal =
                    plane.transform.forward.normalized;

                // Verifica se as faces estão apontando para
                // a mesma direção.
                float normalSimilarity =
                    Vector3.Dot(referenceNormal, normal);

                if (normalSimilarity > normalTolerance)
                {
                    alignedPlanes.Add(plane);
                }
            }

            // Se todos os Planes daquele Kanji estão
            // voltados para a mesma face, temos uma face completa.
            if (alignedPlanes.Count == kanjiPlanes.Count)
            {
                return true;
            }
        }

        return false;
    }
}