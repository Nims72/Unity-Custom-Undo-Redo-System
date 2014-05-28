using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

// Extension methods have to be inside a static class
// and they have to be static method.
public static class ExtensionMethods 
{
   
    // Extension method for calling GetInstance of ActionLogicBase from 
    // any class derived from ScriptableObject.
    public static T GetInstance<T>(this ScriptableObject scriptableObject, ActionRecorder actionRecorder)
        where T : ActionLogicBase
    {
        T instance = ActionLogicBase.CreateInstance<T>();
        instance._actionRecorder = actionRecorder;
        return instance;
    }

    // Extension method for calling GetInstance of ActionLogicBase from 
    // any class derived from ActionViewBase.
    // Inconvenient due to the fact you need to supply both T and B...
    public static T GetInstance<T, B>(this ScriptableObject scriptableObject, ActionRecorder actionRecorder)
        where B : ActionLogicBase
        where T : ActionViewBase<B>
    {
        T instance = ActionViewBase<B>.CreateInstance<T>();
        instance.ActionRecorder = actionRecorder;
        return instance;
    }
}

