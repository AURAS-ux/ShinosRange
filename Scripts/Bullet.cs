using Godot;
using System;

public class Bullet : KinematicBody2D
{
    
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(new Vector2(1000, 0));
    }

    public void BulletLeftScreen()
    {
        QueueFree();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
