using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonNoise : MonoBehaviour
{
  public ComputeShader shader;
  public int texResolution = 1024;
  public Color fillColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
  public Color clearColor = new Color(0, 0, 0.3f, 1.0f);
  public int sides = 6;

  private RenderTexture outputTexture;
  private Renderer rend;
  private int kernelHandle;

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
    shader.SetFloat("time", Time.time);
    DispatchShader(texResolution / 8, texResolution / 8);
  }

  private void InitShader()
  {
    kernelHandle = shader.FindKernel("CSMain");

    shader.SetVector("fillColor", fillColor);
    shader.SetVector("clearColor", clearColor);

    shader.SetInt("sides", sides);

    shader.SetInt("texResolution", texResolution);
    shader.SetTexture(kernelHandle, "Result", outputTexture);

    rend.material.SetTexture("_MainTex", outputTexture);
  }

  private void DispatchShader(int x, int y)
  {
    shader.SetFloat("time", Time.time);
    shader.Dispatch(kernelHandle, x, y, 1);
  }
}
