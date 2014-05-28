using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Base class for Action's GUI/View to be derived from.
/// </summary>
/// <typeparam name="T">The underlying logic class for this GUI class</typeparam>
[Serializable]
public abstract class ActionViewBase<T> : ScriptableObject
    where T : ActionLogicBase
{

    private T _actionLogic;
    public ActionRecorder ActionRecorder;

    void OnEnable()
    {

        hideFlags = HideFlags.HideAndDontSave;

        //Debug.Log(GetType() + " OnEnable() Called");

        // Safety check - Throw an error we called CreateInstance<T>() On an abstract class!
        if (GetType().IsAbstract)
        {
            throw new Exception("Can't call CreateInstance<" + GetType().Name + "() on an abstract class");
        }
    }

    public static M GetInstance<M>(ActionRecorder actionRecorder)
        where M : ActionViewBase<T>  
    {
        M instance = CreateInstance<M>();
        
        // Safety check - Throw an error we called CreateInstance<T>() On an abstract class!
        if (instance.GetType().IsAbstract)
        {
            throw new Exception("Can't call CreateInstance<" + instance.GetType().Name + "() on an abstract class");
        }

        instance.ActionRecorder = actionRecorder;
        return instance;

    }
    /// <summary>
    /// Performs the action - attach this call to the GUI element responsible for executing the action.
    /// </summary>
    protected void PerformAction()
    {
        OnEventRaised();

    }

    public abstract void OnGUI();

    private void OnEventRaised()
    {
        //Debug.Log("Event raised");

        // First clear the response object
        ActionRecorder.Response.Clear();

        //Check if we are in Edit mode, if we aren't send back a failed response object
        if (!ActionRecorder.InEditMode(ref ActionRecorder.Response)) return;

        // Get new model instance
        _actionLogic = ActionLogicBase.GetInstance<T>(ActionRecorder);

        // Passing Data from View to Model
        ViewToModelParams(_actionLogic);

        // Perform the action
        ActionRecorder.PerformAction(_actionLogic);
    }

    /// <summary>
    /// Programmer to fill this in - Can be left empty if no parameters 
    /// are need from the actionView for the actionLogic execution
    /// </summary>
    protected abstract void ViewToModelParams(T actionLogic);
}