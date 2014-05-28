using System;

/// <summary>
/// Class ResponseObject. - Supplies the response from calling the following actions:
/// PerformAction(), Undo(), Redo(), ValidateBeforeUndo(), ValidateBeforeRedo()
/// </summary>
[Serializable]
public class ResponseObject
{
    public ResponseEnums Result = ResponseEnums.Success;
    public string StringResponse = String.Empty; // Optional response in the the ResponseObject object

    public void Clear()
    {
        this.Result = ResponseEnums.Success;
        this.StringResponse = String.Empty;
    }
}

public enum ResponseEnums
{
    Success, SuccessWithWarning, Failed
}