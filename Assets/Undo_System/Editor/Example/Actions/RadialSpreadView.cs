using System;
using UnityEditor;
using UnityEngine;

[Serializable]
class RadialSpreadView : ActionViewBase<RadialSpreadLogic>
{
    private GameObject _originalGameObject;
    private int _numOfCopies = 8;
    private float _radialDistance = 5;
    //private Vector3 CenterPosition = Vector3.zero;
   

    public override void OnGUI()
    {
        _numOfCopies = EditorGUILayout.IntField("Num Of Copies", _numOfCopies);
        _radialDistance = EditorGUILayout.FloatField("Radial Distance", _radialDistance);
        _originalGameObject = EditorGUILayout.ObjectField("Prefab", _originalGameObject, typeof(GameObject), false) as GameObject;

        // Check if entered gameObject is a prefab or not
        if (_originalGameObject != null && !HelperMethods.IsA_Prefab(_originalGameObject))
        {
            _originalGameObject = null;
        }

        //Remember to make this connection in the GUI
        if (GUILayout.Button("Radial Spread", GUILayout.ExpandWidth(true)))
        {
            PerformAction();
        }

        // Note - I am checking inputs from this class by implementing the ValidateBeforeDo() method
        // in the logic portion of this action.
    }

    protected override void ViewToModelParams(RadialSpreadLogic actionLogic)
    {
        actionLogic.Prefab = _originalGameObject;
        actionLogic.NumOfCopies = _numOfCopies;
        actionLogic.RadialDistance = _radialDistance;
    }
}

