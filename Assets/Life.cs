using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    public ComputeShader lifeCompute;

    private RenderTexture target;

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (target == null) {
            target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }

        int kernel = lifeCompute.FindKernel("Seed");
        lifeCompute.SetTexture(kernel, "Result", target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        lifeCompute.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);
    }
}
