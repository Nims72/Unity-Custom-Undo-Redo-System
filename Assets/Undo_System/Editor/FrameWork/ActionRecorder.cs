using System;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Any Editor window which needs to record actions
/// must have an instance of this class.
/// 
/// If the default GUI implementation isn't used then:
/// The button's Undo and Redo in the window editor must be linked
/// to the methods Undo() and Redo() of this class instance.
/// Undo and Redo return a response object.
/// 
/// To perform an action which has no GUI from the EditorWindow call _actionRecorder.PerformAction(action);
/// where _actionRecorder is the instance of this class.
/// _actionRecorder.PerformAction(action) method returns a PerformActionResult.
/// 
/// </summary>

[Serializable]
public class ActionRecorder : ScriptableObject
{

    private ActionDropOutStack _undoActionStack;

    private ActionDropOutStack _redoActionStack;

    public ResponseObject Response; //Only 1 instance of this is used, I am using the clear function before it's reused each time.


    void OnEnable() // Initialize ScriptabelObjects here
    {
        hideFlags = HideFlags.HideAndDontSave;
        if (Response == null)
        {
            Response = new ResponseObject();

            //Debug.Log("New stacks initialized");
        }
    }

    public static ActionRecorder GetInstance(int stackSize)
    {
        ActionRecorder instance = CreateInstance<ActionRecorder>();
        
        // Initializing stacks
        instance._undoActionStack = new ActionDropOutStack(stackSize);
        instance._redoActionStack = new ActionDropOutStack(stackSize);

        return instance;
    }

    /// <summary>
    /// Undo Command to be linked to the
    /// Undo button in the editor window
    /// </summary>
    public ResponseObject Undo()
    {
        // Clear the Response Object
        Response.Clear();

        //Check if we are in Edit mode, if we aren't send back a failed response object
        if (!InEditMode(ref Response)) return Response;

        // Check stuck size
        if (_undoActionStack.Count() == 0)
        {
            Response.Result = ResponseEnums.Failed;
            Response.StringResponse = "UndoStack is empty!";
            return Response;
        }

        // Peek ActionClass from UndoActionStack (We aren't removing it from the stack yet!)
        ActionLogicBase action = _undoActionStack.Peek();

        // Validate Undo Command feasibility
        action.ValidateBeforeUndo(ref Response);

        //Check if response object was set to null by mistake?
        NullResponselCheck(ref Response);

        //Unable to perform Undo action
        if (Response.Result == ResponseEnums.Failed)
        {
            // Set response.StringResponse if it's null or empty.
            if (string.IsNullOrEmpty(Response.StringResponse))
            {
                Response.StringResponse = "Couldn't perform Undo Action!";
            }
        }
        else
        {
            try
            {
                // Perform Undo Action
                action.UndoAction();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Response.Result = ResponseEnums.Failed;
                Response.StringResponse = "Couldn't perform Undo Action!";
                return Response;
            }

            // Pop this action off the undo stack
            _undoActionStack.Pop();

            // Push The Action to the RedoActionStack
            _redoActionStack.Push(action);

        }

        return Response;
    }


    /// <summary>
    /// Redo Command to be linked to the
    /// Redo button in the editor window
    /// </summary>
    public ResponseObject Redo()
    {
        // Clear the Response Object
        Response.Clear();

        //Check if we are in Edit mode, if we aren't send back a failed response object
        if (!InEditMode(ref Response)) return Response;

        // Check redo stack size
        if (_redoActionStack.Count() == 0)
        {
            //Debug.Log("RedoStack is empty!");
            Response.Result = ResponseEnums.Failed;
            Response.StringResponse = "RedoStack is empty!";
            return Response;
        }

        // Peek ActionClass from RedoActionStack (We aren't removing it from the stack yet!)
        ActionLogicBase action = _redoActionStack.Peek();

        // Validate Undo Command feasibility
        action.ValidateBeforeRedo(ref Response);

        //Check if response object was set to null by mistake?
        NullResponselCheck(ref Response);

        //Unable to perform Undo action
        if (Response.Result == ResponseEnums.Failed)
        {
            // Set response.StringResponse if it's null or empty.
            if (string.IsNullOrEmpty(Response.StringResponse))
            {
                Response.StringResponse = "Couldn't perform Redo Action!";
            }
        }
        else
        {
            try
            {
                // Perform Redo Action
                action.RedoAction();
            }
            catch (Exception e)
            {
                Response.Result = ResponseEnums.Failed;
                Response.StringResponse = "Couldn't perform Redo Action!";
                return Response;
            }

            // Pop this action off the redo stack
            _redoActionStack.Pop();

            // Push the Action to the RedoActionStack
            _undoActionStack.Push(action);

        }

        return Response;
    }

    public ResponseObject PerformAction<T>(T actionInstance)
        where T : ActionLogicBase
    {
        // Clear the Response Object
        Response.Clear();

        //Check if we are in Edit mode, if we aren't send back a failed response object
        if (!InEditMode(ref Response)) return Response;

        // Validate Undo Command feasibility
        actionInstance.ValidateBeforeDo(ref Response);

        //Check if response object was set to null by mistake?
        NullResponselCheck(ref Response);

        if (Response.Result == ResponseEnums.Failed)
        {
            // Set response.StringResponse if it's null or empty.
            if (string.IsNullOrEmpty(Response.StringResponse))
            {
                Response.StringResponse = "Couldn't perform Action!";
            }
        }
        else
        {
            // Invoke/call action action
            try
            {
                actionInstance.DoAction();
            }
            catch (Exception e)
            {
                Response.Result = ResponseEnums.Failed;
                return Response;
            }

            // Add received actionInstance to UndoActionStack
            _undoActionStack.Push(actionInstance as ActionLogicBase);

            // Clear the Redo Stack:
            _redoActionStack.Clear();

            Response.Result = ResponseEnums.Success;
        }

        return Response;
    }


    private void NullResponselCheck(ref ResponseObject response)
    {
        // What to do if user sets ResponseObject Object to null?
        // This is before the Undo is performed so 
        // my preference is to not enable the action
        // make a new response with response.Valid = false
        if (response == null)
        {
            response = new ResponseObject
            {
                Result = ResponseEnums.Failed,
                StringResponse = "User set ResponseObject Object to null?"
            };
        }
    }


    /// <summary>
    /// Return description for next Undo Action.
    /// </summary>
    public string NxtUndoDsc()
    {
        string nxtUndoDsc = null;

        //Check if there is anything in the Undo Stack:
        if (_undoActionStack != null && _undoActionStack.Count() > 0)
        {
            ActionLogicBase action = _undoActionStack.Peek();
            nxtUndoDsc = action.GetActionDesc();
        }
        else
        {
            nxtUndoDsc = "UndoStack is empty!";
        }


        return nxtUndoDsc;
    }


    /// <summary>
    /// Return description for next Redo Action.
    /// </summary>
    public string NxtRedoDsc()
    {
        string nxtRedoDsc = null;

        //Check if there is anything in the Redo Stack:
        if (_redoActionStack != null && _redoActionStack.Count() > 0)
        {
            ActionLogicBase action = _redoActionStack.Peek();
            nxtRedoDsc = action.GetActionDesc();
        }
        else
        {
            nxtRedoDsc = "redoStack is empty!";
        }

        return nxtRedoDsc;
    }

    public int GetUndoStackSize()
    {
        return _undoActionStack.Count();
    }

    public int GetRedoStackSize()
    {
        return _redoActionStack.Count();
    }

    public bool InEditMode(ref ResponseObject response)
    {
        //Check if we are in Edit mode:
        if (!HelperMethods.InEditMode())
        {
            response.Result = ResponseEnums.Failed;
            response.StringResponse = "Can't do this in Play/Pause mode!";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Default Undo/Redo GUI Element.
    /// </summary>
    public void UndoRedoGUI(Rect guiRect)
    {
        GUILayout.Space(30);

        // Insert flexible height space to push the redo system down
        GUILayout.FlexibleSpace();

        // Display PerformActionResult here:
        if (Response != null)
        {
            MessageType messageType;
            switch (Response.Result)
            {
                case ResponseEnums.Failed: //If action failed
                    if (string.IsNullOrEmpty(Response.StringResponse))
                    {
                        Response.StringResponse = "Action Failed!";
                    }
                    messageType = MessageType.Error;
                    EditorGUILayout.HelpBox(Response.StringResponse, messageType);

                    break;

                case ResponseEnums.SuccessWithWarning: //If action Succeeded with a warning
                    if (string.IsNullOrEmpty(Response.StringResponse))
                    {
                        Response.StringResponse = "Action Succeeded";
                    }
                    messageType = MessageType.Warning;
                    EditorGUILayout.HelpBox(Response.StringResponse, messageType);

                    break;
            }
        }



        float halfWindWidth = guiRect.width * 0.5f; //Get half width of the window
        // Undo and Redo Buttons
        EditorGUILayout.BeginHorizontal();

        //----------------
        // UndoButtonGroup
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(halfWindWidth));
        string undoContent = "Undo  " + this.GetUndoStackSize();
        if (GUILayout.Button(undoContent, GUILayout.ExpandWidth(true)))
        {

            this.Undo();
        }
        // Help Box
        EditorGUILayout.HelpBox(this.NxtUndoDsc(), MessageType.Info);

        EditorGUILayout.EndVertical();
        //----------------


        //----------------
        // RedoButtonGroup
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(halfWindWidth));
        string redoContent = "Redo  " + this.GetRedoStackSize();
        if (GUILayout.Button(redoContent, GUILayout.ExpandWidth(true)))
        {

            this.Redo();
        }
        // Help Box
        EditorGUILayout.HelpBox(this.NxtRedoDsc(), MessageType.Info);

        EditorGUILayout.EndVertical();
        //----------------

        //----------------
        EditorGUILayout.EndHorizontal();
    }
}




/// <summary>
/// Unity needs a non generic topmost class in order to serialize it
/// </summary>
[Serializable]
class ActionDropOutStack : DropOutStack<ActionLogicBase>
{
    public ActionDropOutStack(int capacity)
        : base(capacity)
    {
    }
}
