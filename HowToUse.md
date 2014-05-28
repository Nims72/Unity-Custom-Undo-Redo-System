## **The Framework:**

Is built around 3 main components which need to be implemented:
- The Action Logic.
- The Action View/GUI graphical representation (Implementation of this is optional).
- The ActionRecorder which holds the stacks for the redo and undo of actions.
	
Any Editor window which needs to record actions must have an instance of the ActionRecorder class.

### **How to use this framework:**
#### Action Logic
(For an example – look at RadialSpreadLogic Class)

Start a new C# file to implement your action code.  
The class must derive from the ActionLogicBase.  
Add the fields/properties you need to implement your logic (They must be public).

You must then implement the following methods:
1.	DoAction()
1.	UndoAction()
1.	RedoAction()

You may also override the following methods:  
These methods are used to insert any condition you want checked before performing their related actions and enable you to abort these actions (See Response object for more info).
- 	ValidateBeforeDo()
- 	ValidateBeforeUndo()
- 	ValidateBeforeRedo()

To change the action default description (which is by default the class name) – you may override the SetActionDesc() method to return a string/action description of your choice.

**Remember to decorate the class with the [Serializable] attribute.**

#### ActionView/GUI
(For an example – look at RadialSpreadView Class)

In the section will write the actionView/GUI class.  
Start a second C# file to implement the GUI of the action.  
This class must be derived from ActionViewBase.

When declaring the action it must be done in the following manner:  
class “YourClassGUI” : ActionViewBase<”YourClassLogic”>  
where ”YourClassLogic” is the class name you wrote in section 1.

You now have to implement the following methods:  
1. OnGUI() – This is your custom GUI.  
Members used in here must be defined at class level (not local), and they can be private.  
To register the action connect the “button” which executes the action to the action recorder, this is simply done by calling PerformAction(); when the button is pressed/executed.
2. ViewToModelParams() – This is where you transfer information from this GUI to the logic part of your action.

**Remember to decorate the class with the [Serializable] attribute.**

#### EditorWindow
(For an example – look at ExampleEditorWindow Class)

**Fields and properties:**  
It must contain an ActionRecorder field/property – this will keep track the Undo/Redo stacks, i.e.:

		private ActionRecorder _actionRecorder;
    
You must also define your Actions GUI/View classes as members of this window class (these will be the classes you wrote in section 2).
i.e.:  
        
        private RadialSpreadView _radialSpreadAction;

**OnEnable()**  
In the OnEnable() method of your window editor, add the following to initialize your recorder with the stack size of your choosing:

        if (_actionRecorder == null)
        {   
            //Don't use createInstance...use GetInstance(stackSize).  
            _actionRecorder = ActionRecorder.GetInstance(5); 
        }
        
Initialize your action View/GUI classes in the same manner. i.e.:

     if (_radialSpreadAction == null)  
     {
        //Don't use createInstance use GetInstance instead.
        _radialSpreadAction = RadialSpreadView.GetInstance<RadialSpreadView>(_actionRecorder);
     }
         
**OnGUI()**  
In order to display the GUI of your actions ( In the OnGUI() method of the editor window) call the OnGUI() method of the actionView/GUI class ( from section 2 - which you defined as a field at the beginning of this section).

In order to display the built in UndoRedoGUI (from the Action Recorder Class) make the following call from within the OnGUI() method of the editor window:

    _actionRecorder.UndoRedoGUI(this.position);

#### The ResponseObject

A response object is **returned** from calling the following actions:  
PerformAction(), Undo(), Redo()

A ResponseObject contains two memebers:
- Result – (Enumeration) which can have 1 of 3 values: Success, SuccessWithWarning, Failed
- StringResponse – (String) which allows you to supply more information.

The response object is **used** in the following 3 methods:  
*ValidateBeforeDo(), ValidateBeforeUndo(), ValidateBeforeRedo()*  
These methods have a reference to the response object – which you can be modified (Changing its result and string response). Returning a failed result in the response object will prevent the related action from being performed.

#### Notes:
1.	Important - Do not use events, delegates, generics, static members and System.Type as members for ActionLogic/ActionView as these are not serialized by Unity.
2.	If the default UndoRedoGUI implementation isn't used then:  
	Your custom Undo and Redo button's in the window editor must be linked to the Undo() and Redo() methods of the ActionRecorder class instance. The Undo and Redo methods return a response object.
3.	You do not have to implement the Action View/GUI graphical representation of an action, so if you only write out the logic of the action you can still perform it and record it in the ActionRecorder in the following fashion:

                //To perform an action which has no GUI from the EditorWindow call
                _actionRecorder.PerformAction(action);

