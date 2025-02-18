﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sP
{

    public class MeshInverter : MonoBehaviour
    {
        public bool InvertFaces = true;
        public bool InvertNormals = true;
        public GameObject to_invert;

        void Start()
        {
            MeshFilter mf = to_invert.GetComponent<MeshFilter>();

            if (mf != null)
            {
                var m = Instantiate(mf.mesh);
                Process(m);
                mf.mesh = m;
            }

            var smr = to_invert.GetComponent<SkinnedMeshRenderer>();

            if (smr != null)
            {
                var m = Instantiate(smr.sharedMesh);
                Process(m);
                smr.sharedMesh = m;
            }
        }

        private void Process(Mesh m)
        {
            int subMeshes = m.subMeshCount;

            for (int i = 0; i < subMeshes; i++)
            {
                if (InvertFaces)
                {
                    var type = m.GetTopology(i);
                    var indices = m.GetIndices(i);

                    if (type == MeshTopology.Quads)
                    {
                        for (int n = 0; n < indices.Length; n += 4)
                        {
                            int tmp = indices[n];
                            indices[n] = indices[n + 3];
                            indices[n + 3] = tmp;
                            tmp = indices[n + 1];
                            indices[n + 1] = indices[n + 2];
                            indices[n + 2] = tmp;
                        }
                    }
                    else if (type == MeshTopology.Triangles)
                    {
                        for (int n = 0; n < indices.Length; n += 3)
                        {
                            int tmp = indices[n];
                            indices[n] = indices[n + 1];
                            indices[n + 1] = tmp;
                        }
                    }

                    m.SetIndices(indices, type, i);
                }
            }

            if (InvertNormals)
            {
                var normals = m.normals;

                for (int n = 0; n < normals.Length; n++)
                {
                    normals[n] = -normals[n];
                }

                m.normals = normals;
            }

            to_invert.GetComponent<MeshFilter>().mesh = m;
        }

    }

}
