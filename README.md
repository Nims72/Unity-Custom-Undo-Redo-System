### Unity-Custom-Undo-Redo-System


This is a Unity3d Framework for adding “Actions” to the editor window coupled with a custom Undo/Redo system.

The framework has its own optional default implementation for the GUI component of the Undo/Redo.

You cannot Undo/Redo/Perform an action if you’re not in Editing mode (for example being in play mode), but the system will persist the undo/redo data after existing play mode.

At the moment the framework will not persist the Undo/Redo data if the editor-window is closed or if the Unity editor layout is reset.

Included in the project is “How to use” text guide, and an example.

See Unity forums for discussion:  
http://forum.unity3d.com/threads/248451-Decoupling-GUI-and-Logic-in-Editor-windows-and-a-Custom-Undo-Redo-system

Short video of the included example:
https://www.youtube.com/watch?v=ayaPmhzvyIM
