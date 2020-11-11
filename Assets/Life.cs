using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    public ComputeShader lifeCompute;
    
    [Range(0.01f, 0.99f)]
    public float seedChance;

    private RenderTexture target;
    private int kernel, threadGroupsX, threadGroupsY;

    private void Awake() {
        threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);     
    }

    private void Seed() {
        kernel = lifeCompute.FindKernel("Seed");
        lifeCompute.SetTexture(kernel, "_Result", target);
        lifeCompute.SetFloat("_RandSeed", Random.Range(2, 1000));
        lifeCompute.SetFloat("_SeedChance", seedChance);
        lifeCompute.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
    }

    private void OnEnable() {
        if (target == null) {
            target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }

        Seed();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        kernel = lifeCompute.FindKernel("NewGeneration");
        lifeCompute.SetTexture(kernel, "_Result", target);
        lifeCompute.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);
    }
}
