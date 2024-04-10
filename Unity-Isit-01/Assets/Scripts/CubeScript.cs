using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private Material _hoverMaterial;
    private Material removeHoverMaterial;
    private Material _defaultMaterial;
    private Material _selectedMaterial;

    private MeshRenderer _renderer;
    private bool _isSelected;
    public bool Selected
    {
        get
        {
            return _isSelected;
        }
    }

    private void OnMouseOver()
    {
        if (!_isSelected)
        {
            CursorMode gmCursor = GameManager.SingleInstance.CurrentCursorMode;
            if (gmCursor == CursorMode.Remove)
            {
                _renderer.material = removeHoverMaterial;

            }
            else if (gmCursor == CursorMode.Select)
            {
                _renderer.material = _hoverMaterial;
            }
        }
    }

    private void OnMouseExit()
    {
        if (!_isSelected)
        {
            _renderer.material = _defaultMaterial;
        }        
    }


    public void Select()
    {
        _isSelected = true;
        _renderer.material = _selectedMaterial;
    }

    public void Unselect()
    {
        _isSelected = false;
        _renderer.material = _defaultMaterial;
    }

    void Awake()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
        _hoverMaterial = Resources.Load<Material>("Materials/HoverCubeMaterial");
        removeHoverMaterial = Resources.Load<Material>("Materials/RemoveHoverCubeMaterial");
        _selectedMaterial = Resources.Load<Material>("Materials/SelectedCubeMaterial");

        _defaultMaterial = _renderer.material;
    }

    void Update()
    {

    }
}
