using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private const string BufferName = "Render Camera";
    
    private static ShaderTagId _unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

    private CommandBuffer _buffer = new CommandBuffer { name = BufferName };
    
    private ScriptableRenderContext _context;
    private Camera _camera;

    private CullingResults _cullingResults;
    
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        _context = context;
        _camera = camera;

        PrepareForSceneWindow();
        
        if (!Cull())
            return;
        
        Setup();
        DrawVisibleGeometry();
        DrawGizmos();
        Submit();
    }

    private bool Cull()
    {
        if (_camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            _cullingResults = _context.Cull(ref p);
            return true;
        }

        return false;
    }

    private void Setup()
    {
        _context.SetupCameraProperties(_camera);
        _buffer.ClearRenderTarget(true, true, Color.clear);
        _buffer.BeginSample(BufferName);
        ExecuteBuffer();
    }

    private void Submit()
    {
        _buffer.EndSample(BufferName);
        ExecuteBuffer();
        _context.Submit();
    }

    private void DrawVisibleGeometry()
    {
        var sortingSettings = new SortingSettings();
        var drawingSettings = new DrawingSettings(_unlitShaderTagId, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        
        _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        
        _context.DrawSkybox(_camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        
        _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
    }

    private void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
            _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
        }
    }

    private void PrepareForSceneWindow()
    {
        if (_camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
        }
    }
    
    private void ExecuteBuffer()
    {
        _context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }
}
