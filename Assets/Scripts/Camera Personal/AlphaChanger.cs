using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AlphaChanger : MonoBehaviour
{
    private static AlphaChanger instance = null;
    public static AlphaChanger Instance => instance;
    
    
    public Material redSoulShaderOpaque;
    public Material yellowSoulShaderOpaque;
    public Material blueSoulShaderOpaque;
    public Material whiteSoulShaderOpaque;
    public Material darkSoulShaderOpaque;
    public Material kiteWhiteOpaque;
    public Material kiteWoodOpaque;
    [Space(10)]

    public Material redSoulShaderTransparent;
    public Material yellowSoulShaderTransparent;
    public Material blueSoulShaderTransparent;
    public Material whiteSoulShaderTransparent;
    public Material darkSoulShaderTransparent;
    public Material kiteWhiteTransparent;
    public Material kiteWoodTransparent;
    [Space(10)]
    public Material playerOutline;
    public SkinnedMeshRenderer[] soulRenderers;
    [Space(10)]

    public SkinnedMeshRenderer redPartRenderer;
    public SkinnedMeshRenderer yellowPartRenderer;
    public SkinnedMeshRenderer bluePartRenderer;
    public SkinnedMeshRenderer whitePartRenderer;
    public MeshRenderer woodPartRenderer1;
    public MeshRenderer woodPartRenderer2;
    public MeshRenderer woodPartRenderer3;
    public MeshRenderer woodPartRenderer4;
    public MeshRenderer woodPartRenderer5;
    public SkinnedMeshRenderer trailPartRenderer1;
    public SkinnedMeshRenderer trailPartRenderer2;
    public SkinnedMeshRenderer trailPartRenderer3;
    public SkinnedMeshRenderer trailPartRenderer4;
    public SkinnedMeshRenderer trailPartRenderer5;
    public SkinnedMeshRenderer trailPartRenderer6;
    public SkinnedMeshRenderer trailPartRenderer7;
    [Space(10)]

    public Transform cameraTarget;
    public CharacterManager characterManagerScript;
    public CinemachineFreeLook cinemachineFreeLook;

    private float alphaValue;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    void FixedUpdate()
    {
        alphaValue = Mathf.Abs(cameraTarget.position.y - cinemachineFreeLook.transform.position.y);
        //Debug.Log(alphaValue);
        if (alphaValue < 1 && (characterManagerScript.isGrounded | characterManagerScript.isGliding)) ChangePlayerAlpha(alphaValue);
        else MakePlayerMaterialOpaque();
    }

    private void ChangePlayerAlpha(float value)
    {
        foreach (SkinnedMeshRenderer soulRenderer in soulRenderers)
        {
            soulRenderer.material = whiteSoulShaderTransparent;
        }
        
        //soulRenderer.materials[1] = null;

        redPartRenderer.material = redSoulShaderTransparent;
        yellowPartRenderer.material = yellowSoulShaderTransparent;
        bluePartRenderer.material = blueSoulShaderTransparent;
        whitePartRenderer.material = kiteWhiteTransparent;
        woodPartRenderer1.material = kiteWoodTransparent;
        woodPartRenderer2.material = kiteWoodTransparent;
        woodPartRenderer3.material = kiteWoodTransparent;
        woodPartRenderer4.material = kiteWoodTransparent;
        woodPartRenderer5.material = kiteWoodTransparent;
        trailPartRenderer1.material = whiteSoulShaderTransparent;
        trailPartRenderer2.material = yellowSoulShaderTransparent;
        trailPartRenderer3.material = redSoulShaderTransparent;
        trailPartRenderer4.material = blueSoulShaderTransparent;
        trailPartRenderer5.material = yellowSoulShaderTransparent;
        trailPartRenderer6.material = redSoulShaderTransparent;
        trailPartRenderer7.material = blueSoulShaderTransparent;

        redSoulShaderTransparent.SetFloat("_Alpha", value);
        yellowSoulShaderTransparent.SetFloat("_Alpha", value);
        blueSoulShaderTransparent.SetFloat("_Alpha", value);
        whiteSoulShaderTransparent.SetFloat("_Alpha", value);
        darkSoulShaderTransparent.SetFloat("_Alpha", value);

        kiteWhiteTransparent.SetFloat("_Alpha", value);
        kiteWoodTransparent.SetFloat("_Alpha", value);
    }

    public void MakePlayerMaterialOpaque()
    {
        foreach (SkinnedMeshRenderer soulRenderer in soulRenderers)
        {
            soulRenderer.material = whiteSoulShaderOpaque;
        }
        //soulRenderer.materials[1] = playerOutline;

        redPartRenderer.material = redSoulShaderOpaque;
        yellowPartRenderer.material = yellowSoulShaderOpaque;
        bluePartRenderer.material = blueSoulShaderOpaque;
        whitePartRenderer.material = kiteWhiteOpaque;
        woodPartRenderer1.material = kiteWoodOpaque;
        woodPartRenderer2.material = kiteWoodOpaque;
        woodPartRenderer3.material = kiteWoodOpaque;
        woodPartRenderer4.material = kiteWoodOpaque;
        woodPartRenderer5.material = kiteWoodOpaque;
        trailPartRenderer1.material = whiteSoulShaderOpaque;
        trailPartRenderer2.material = yellowSoulShaderOpaque;
        trailPartRenderer3.material = redSoulShaderOpaque;
        trailPartRenderer4.material = blueSoulShaderOpaque;
        trailPartRenderer5.material = yellowSoulShaderOpaque;
        trailPartRenderer6.material = redSoulShaderOpaque;
        trailPartRenderer7.material = blueSoulShaderOpaque;
        redSoulShaderTransparent.SetFloat("_Alpha", 1);
        yellowSoulShaderTransparent.SetFloat("_Alpha", 1);
        blueSoulShaderTransparent.SetFloat("_Alpha", 1);
        whiteSoulShaderTransparent.SetFloat("_Alpha", 1);
        darkSoulShaderTransparent.SetFloat("_Alpha", 1);

        kiteWhiteTransparent.SetFloat("_Alpha", 1);
        kiteWoodTransparent.SetFloat("_Alpha", 1);
    }
}
