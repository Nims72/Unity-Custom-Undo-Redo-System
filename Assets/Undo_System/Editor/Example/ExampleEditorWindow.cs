using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ExampleEditorWindow : EditorWindow
{

    #region Fields
    /// --------The only necessary Fields:
    private ActionRecorder _actionRecorder;
    // The actions used:
    private RadialSpreadView _radialSpreadAction;
    /// --------The only necessary Fields end here

    // You don't have to have this param it's just for returning results if you 
    // are not using the default Undo/Redo GUI built into the ActionRecorder
    private ResponseObject _actionResult;

    //private List<Type> _actionsList; //A list of actionClasses Types - Not used in this example

    #endregion

    [MenuItem("Tools/UndoRedoTest")]
    static void DoIt()
    {
        var window = GetWindow<ExampleEditorWindow>();
        window.title = "Editor Window";
    }

    void OnEnable() //This called when you open a window and also sometimes if you press the Play button.
    {

        //Debug.Log("Window OnEnable Called");

        //_actionsList = HelperMethods.GetDerivedTypes<ActionLogicBase>();

        if (_actionRecorder == null)
        {
            _actionRecorder = ActionRecorder.GetInstance(5); //Don't use createInstance...use GetInstance(stackSize).

            //Debug.Log("A new actionRecorder initialized");
        }

        //Define the actions after you have made sure you have an instance of actionRecorder.

        // Create instances of the action view's/GUI
        // so you can display it in the OnGUI method of the editor window.
        if (_radialSpreadAction == null)
        {
            //Don't use createInstance use GetInstance instead.
            _radialSpreadAction = RadialSpreadView.GetInstance<RadialSpreadView>(_actionRecorder);
   
            //Debug.Log("A new _radialSpreadAction initialized");
        }

    }


    void OnGUI()
    {

        GUILayout.Space(15);

        // An example of an action class with a GUI attached, we are calling the 
        // Gui component of the action. No need to worry about the logic part as it is already
        // wired up with the view part by definition
        _radialSpreadAction.OnGUI();

        GUILayout.Space(15);

        // Example of action's which don't have GUI/View's attached
        if (GUILayout.Button("Some Action Class", GUILayout.ExpandWidth(true)))
        {
            SomeActionClass someAction = this.GetInstance<SomeActionClass>(_actionRecorder);
            
            _actionResult = _actionRecorder.PerformAction(someAction); //Action's supply back an enumerable result?
        }

        if (GUILayout.Button("Fail Undo Action", GUILayout.ExpandWidth(true)))
        {
            FailUndoAction action = this.GetInstance<FailUndoAction>(_actionRecorder);
           
            _actionResult = _actionRecorder.PerformAction(action);
        }

        if (GUILayout.Button("Fail Redo Action", GUILayout.ExpandWidth(true)))
        {
            FailRedoAction action = this.GetInstance<FailRedoAction>(_actionRecorder);

            _actionResult = _actionRecorder.PerformAction(action);
        }


        

        // -----------------------
        // Display Built in UndoRedoGUI in the Action Recorder
        _actionRecorder.UndoRedoGUI(this.position);

    }

    /// <summary>
    /// Display all actions (which are derived from ActionLogicBase)
    /// Not used in this example, and it doesn't show the GUI of the actions.
    /// </summary>
    void DisplayActions(IEnumerable<Type> actionsList)
    {
        foreach (var actionType in actionsList)
        {
            //GUILayout.Label(actionType.FullName);
            //GUILayout.Label(actionType.Name);
            if (GUILayout.Button(actionType.Name, GUILayout.ExpandWidth(true)))
            {
                if (!actionType.IsSubclassOf(typeof(ActionLogicBase))) continue;
                // Create instance of the class:
                var actionInstance = Activator.CreateInstance(actionType) as ActionLogicBase;

                // Perform action
                _actionRecorder.PerformAction(actionInstance);
            }
        }

    }

    void OnDestroy()
    {
        //Todo persist the actionRecorder/stacks for window closing?!
    }
}
