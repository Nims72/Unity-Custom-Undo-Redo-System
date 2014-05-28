

// Actions are made of 2 units the logic and the view.
// Each action/command class has to implement 3 Methods: DoAction() ,UndoAction() and RedoAction().
// Action params should be set as class fields/properties.
// Override the SetActionDesc() method - to set a custom action description.
//
// Action GUI is implemented in a class derived from ActionViewBase.
// Set all the params to be used as fields/properties.
// (These must be PUBLIC if you are setting them from the view/Gui class)
// You can use these params for the Do, Undo, Redo and Validate actions.
// Calling an action to be executed is by using PerformAction(action class instance)

// The Action Class is derived from scriptable object to enable serialization of 
// the DropOutStack array of DERIVED actions!
// Do not use events, delegates, non encapsulated generics, static members and System.Type
// As these are not serialized by Unity.

using System;
using UnityEngine;

/// <summary>
/// Base class for Action's Logic to be derived from.
/// </summary>

[Serializable]
public abstract class ActionLogicBase : ScriptableObject
{

    /// <summary>
    /// The _action description
    /// </summary>
    private string _actionDescription;
    /// <summary>
    /// The _action recorder
    /// </summary>
    public ActionRecorder _actionRecorder;

    /// <summary>
    /// Called when initializing scriptable object or after de-serialization.
    /// </summary>
    void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;

        // Safety check - Throw an error we called CreateInstance<T>() On an abstract class!
        if (GetType().IsAbstract)
        {
            throw new Exception("Can't call CreateInstance<" + GetType().Name + "() on an abstract class");
        }


        if (_actionDescription == null)
        {
            _actionDescription = SetActionDesc(); //Setting action description
        }
    }

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actionRecorder">The action recorder.</param>
    /// <returns>T.</returns>
    public static T GetInstance<T>(ActionRecorder actionRecorder) where T : ActionLogicBase
    {
        // Safety check - Throw an error we called GetInstance<T>() On an abstract class!
        Type type = typeof(T);
        if (type.IsAbstract)
        {
            throw new Exception("Can't use GetInstance<" + type.Name + ">() on an abstract class");

        }
        T instance = CreateInstance<T>();
        instance._actionRecorder = actionRecorder;
        return CreateInstance<T>();
    }

    #region Get/Set for action description

    /// <summary>
    /// Gets the action desc.
    /// </summary>
    /// <returns>System.String.</returns>
    public string GetActionDesc()
    {
        return _actionDescription;
    }

    protected virtual string SetActionDesc()
    {
        return GetType().Name; // returning default action description as class name
    }

    #endregion

    #region Abstract Methods

    /// <summary>
    /// Does the action.
    /// </summary>
    public abstract void DoAction();
    /// <summary>
    /// Undoes the action.
    /// </summary>
    public abstract void UndoAction();
    /// <summary>
    /// Redoes the action.
    /// </summary>
    public abstract void RedoAction();
    #endregion

    #region virtual Methods
    /// <summary>
    /// Validates the before do.
    /// </summary>
    /// <param name="response">The response.</param>
    public virtual void ValidateBeforeDo(ref ResponseObject response)
    {
        //In the implementation you can set response to what ever you want.
    }

    /// <summary>
    /// Validates the before undo.
    /// </summary>
    /// <param name="response">The response.</param>
    public virtual void ValidateBeforeUndo(ref ResponseObject response)
    {
        //In the implementation you can set response to what ever you want.
    }

    /// <summary>
    /// Validates the before redo.
    /// </summary>
    /// <param name="response">The response.</param>
    public virtual void ValidateBeforeRedo(ref ResponseObject response)
    {
        //In the implementation you can set response to what ever you want.
    }

    #endregion

}



