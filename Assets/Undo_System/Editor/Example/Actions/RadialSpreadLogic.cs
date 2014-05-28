
using UnityEngine;
using System;
using Object = UnityEngine.Object;

[Serializable]
public class RadialSpreadLogic : ActionLogicBase
{
    #region Instance Vars
    public GameObject Prefab;
    public int NumOfCopies;
    public float RadialDistance;
    public Vector3 CenterPosition;

    private GameObject[] _gameObjects;

    // To be used in the future...?
    //public string GoName;
    //public Transform OriginalTransformValues;
    //public Vector3 OriginalPosition;
    #endregion

    public enum Rotation //Not Used Yet
    {
        Inwards, Outwards
    }

    public override void DoAction()
    {
        //float degreeInterval = 360 / NumOfCopies;
        double radInterval = Math.PI * 2/ NumOfCopies;
        double degreeInterval = Mathf.Rad2Deg*radInterval;

        // Create a list of positions:
        _gameObjects = new GameObject[NumOfCopies];

        //Register the gameObjects name:
        //GoName = Prefab.name;

        for (int i = 0; i < NumOfCopies; i++)
        {
            // Calculate direction vector * RadialDistance
            Quaternion rotation = Quaternion.AngleAxis((float)(i * degreeInterval), Vector3.up);
            Vector3 newPosition = CenterPosition + (rotation * Vector3.back) * RadialDistance;

            // Place object at newPosition:
            GameObject gameObject =
                Object.Instantiate(Prefab, newPosition, Quaternion.identity) as GameObject;

            _gameObjects[i] = gameObject;

            gameObject.name = Prefab.name; //+ "_0" + (i + 1);
        }
    }

    public override void UndoAction()
    {
        //Delete all the objects in the list:
        foreach (var item in _gameObjects)
        {
            Object.DestroyImmediate(item);
        }
    }

    public override void RedoAction()
    {
        DoAction();
    }

    protected override string SetActionDesc()
    {
        return "Radial Spread";
    }

    public override void ValidateBeforeDo(ref ResponseObject response)
    {
        if (Prefab == null)
        {
            response.StringResponse = "Need an object to copy";
            response.Result=ResponseEnums.Failed;
            return;
        } 

        if (NumOfCopies <= 0)
        {
            response.StringResponse = "Num of copies can't be equal/less than 0";
            response.Result = ResponseEnums.Failed;
            return;
        }

        if (RadialDistance < 0)
        {
            response.StringResponse = "Radial distance can't be less than 0";
            response.Result = ResponseEnums.Failed;
            return;
        }
    }

    public override void ValidateBeforeRedo(ref ResponseObject response)
    {
       
        if (Prefab == null)
        {
            response.StringResponse = "Original Prefab is missing!";
            response.Result=ResponseEnums.Failed;
           
        }
    }

}