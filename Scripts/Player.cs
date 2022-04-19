using Godot;
using System;


public class Player : KinematicBody2D
{
    [Export] readonly int _moveSpeed=20;
    [Export] readonly int _gravityMultiplyer = 20;
    Vector2 velocity ;
    Camera2D _camera;
    AnimationPlayer _playerAnim,_idleAnimation,_jumpAnimation;
    Sprite _playerSpirte;
    float _gravity = 9.8f;
    bool _allowJump = true;
    float _jumpImpulse=30;
    float _jumpTimper = 0;
    public override void _Ready()
    {
        _camera = GetNode<Camera2D>("/root/MainLevel/MainCam");
        _playerAnim = GetNode<AnimationPlayer>("runAnimation");
        _playerSpirte = GetNode<Sprite>("PlayerSprite");
        _idleAnimation = GetNode<AnimationPlayer>("idleStatus");
        _jumpAnimation = GetNode<AnimationPlayer>("jumpAnimation");
        _idleAnimation.Play("idle");
    }
    public override void _PhysicsProcess(float delta)
    {
        MovePlayer(delta);
        AddGravity(delta);
        _camera.Position = new Vector2(this.GlobalPosition.x, _camera.Position.y);
    }
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public void MovePlayer(float delta)
    {
        velocity = new Vector2(0, 0);
        if (Input.IsKeyPressed((int)KeyList.A))
        {
            velocity += new Vector2(-10 * _moveSpeed * delta, 0);
            _playerAnim.Play("run");
            _playerSpirte.FlipH = true;
            _idleAnimation.Stop();
        }
        else if (Input.IsKeyPressed((int)KeyList.D))
        {
            velocity += new Vector2(10 * _moveSpeed * delta, 0);
            _playerAnim.Play("run");
            _playerSpirte.FlipH = false;
            _idleAnimation.Stop();
        }
        else
        {
            _playerAnim.Stop();
            _idleAnimation.Play("idle");
        }

        if (Input.IsKeyPressed((int)KeyList.Space))
        {
            _playerAnim.Stop();
            _jumpAnimation.Play("jump");
            if (_allowJump && _jumpTimper>10)
            {
                velocity -= new Vector2(0,_jumpImpulse*100);
                _allowJump = false;
                _jumpTimper = 0;
            }
        }
        MoveAndSlide(velocity);
    }
    
    public void AddGravity(float delta)
    {
        velocity = Vector2.Zero;
        velocity.y += _gravity * _gravityMultiplyer * delta;
        velocity = MoveAndSlide(velocity,new Vector2(0,-1));
        if(IsOnFloor())
        {
            _allowJump = true;
            _jumpTimper++;
            _jumpAnimation.Stop();
        }
    }
}
