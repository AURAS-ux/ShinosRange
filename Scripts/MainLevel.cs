using Godot;
using System;

public class MainLevel : Node2D
{
    public override void _Ready()
    {
               
    }
    public override void _Process(float delta)
    {
        if(Input.IsKeyPressed((int)KeyList.Escape))
        {
            GetTree().Quit();
        }
    }
}
