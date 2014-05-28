using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class SomeActionClass : ActionLogicBase
{
    public override void DoAction()
    {
        Debug.Log("This action has no GUI\nSomeActionClass DoAction() Called.");

    }

    public override void UndoAction()
    {
        Debug.Log("Some Action Class UndoAction Called");
    }

    public override void RedoAction()
    {
        Debug.Log("Some Action Class RedoAction Called");
    }
}

class FailUndoAction : ActionLogicBase
{
    public override void DoAction()
    {
        //Debug.Log("FailUndoActionClass DoAction Called");
    }

    public override void UndoAction()
    {
        //Debug.Log("FailUndoActionClass UndoAction Called");
    }

    public override void RedoAction()
    {

    }

    public override void ValidateBeforeUndo(ref ResponseObject response)
    {
       
        response.Result = ResponseEnums.Failed;
    }
}

class FailRedoAction : ActionLogicBase
{
    public override void DoAction()
    {
        //Debug.Log("FailRedoActionClass DoAction Called");
    }

    public override void UndoAction()
    {
        //Debug.Log("FailRedoActionClass UndoAction Called");
    }

    public override void RedoAction()
    {

    }

    public override void ValidateBeforeRedo(ref ResponseObject response)
    {
       
        response.Result = ResponseEnums.Failed;
    }
}