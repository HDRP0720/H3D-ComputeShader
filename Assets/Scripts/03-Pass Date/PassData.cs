using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassData : MonoBehaviour
{
  public ComputeShader shader;
  public int texResolution = 1024;
  public Color clearColor = new Color();
  public Color circleColor = new Color();

  private RenderTexture outputTexture;
  private Renderer rend;
  private int circleHandle;
  private int clearHandle;

  private void Start()
  {
    outputTexture = new RenderTexture(texResolution, texResolution, 0);
    outputTexture.enableRandomWrite = true;
    outputTexture.Create();

    rend = GetComponent<Renderer>();
    rend.enabled = true;

    InitShader();
  }
  private void Update()
  {
    DispatchKernels(10);
  }

  private void InitShader()
  {
    circleHandle = shader.FindKernel("Circles");
    clearHandle = shader.FindKernel("Clear");

    shader.SetInt("texResolution", texResolution);
    shader.SetVector("clearColor", clearColor);
    shader.SetVector("circleColor", circleColor);

    shader.SetTexture(circleHandle, "Result", outputTexture);
    shader.SetTexture(clearHandle, "Result", outputTexture);

    rend.material.SetTexture("_MainTex", outputTexture);
  }

  private void DispatchKernels(int count)
  {
    shader.Dispatch(clearHandle, texResolution/8, texResolution/8, 1);
    shader.SetFloat("time", Time.time);
    shader.Dispatch(circleHandle, count, 1, 1);
  }
}
