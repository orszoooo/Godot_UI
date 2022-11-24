using Godot;
using System;

public class Main : Panel
{
    // Declare member variables here.
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode("Button").Connect("pressed", this, nameof(_OnButtonPressed));
    }

    public void _OnButtonPressed()
    {
        var label = (Label)GetNode("Label");
        label.Text = "Hello World!";
    }
}
