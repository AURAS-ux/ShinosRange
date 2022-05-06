using Godot;
using System;


public class Player : KinematicBody2D
{
    [Export] readonly int _moveSpeed=20;
    [Export] readonly int _gravityMultiplyer = 20;

    [Export] readonly float _jumpHeight = 100;
    [Export] readonly float _jumpTimeToPeak = 0.5f;
    [Export] readonly float _jumpTimeToDescend = 0.4f;

    private Vector2 velocity ;
    private Camera2D _camera;
    private AnimationPlayer _playerAnim,_idleAnimation,_jumpAnimation;
    private Sprite _playerSpirte;
    private float _gravity = 9.8f;
    private bool _allowJump = true;
    private float _jumpTimper =0;

    private float _jumpVelocity;
    private float _jumpGravity;
    private float _fallGravity;
    private Position2D _positionShooting;
    private CollisionPolygon2D _hitBoxPlayer;

    PackedScene bullet = GD.Load<PackedScene>("res://Prefabs/Bullet.tscn");
    public override void _Ready()
    {

        _jumpVelocity = ((2 * _jumpHeight) / _jumpTimeToPeak) * -1.0f; ;
        _jumpGravity = ((-2 * _jumpHeight) / (_jumpTimeToPeak*_jumpTimeToPeak))*-1.0f;
        _fallGravity = ((-2 * _jumpHeight) / (_jumpTimeToDescend*_jumpTimeToDescend))*-1.0f;

        _camera = GetNode<Camera2D>("/root/MainLevel/MainCam");
        _playerAnim = GetNode<AnimationPlayer>("runAnimation");
        _playerSpirte = GetNode<Sprite>("PlayerSprite");
        _idleAnimation = GetNode<AnimationPlayer>("idleStatus");
        _jumpAnimation = GetNode<AnimationPlayer>("jumpAnimation");
        _idleAnimation.Play("idle");
        _positionShooting = GetNode<Position2D>("Shooting_Position");
        _hitBoxPlayer = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
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
            _positionShooting.Position *= -1;
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
            //if (_allowJump && _jumpTimper>10)
            //{
            //    velocity -= new Vector2(0,_jumpImpulse*100);
            //    _allowJump = false;
            //    _jumpTimper = 0;
            //}
            if (_allowJump && _jumpTimper>10)
            {
                velocity.y += GetGravity() * delta;
                PlayerJump();
                _allowJump = false;
                _jumpTimper = 0;
            }
        }

        if(Input.IsActionPressed("M1_pressed"))
        {
            Shoot();
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


    public void PlayerJump()
    {
        velocity.y = _jumpVelocity;
    }

    public float GetGravity()
    {
        if (velocity.y < 0)
            return _jumpGravity;
        else
            return _fallGravity;
    }

    public void OnDoorBodyEntered(Area2D collision)
    {
        if (collision == GetNode("."))
        {
            collision.Position = new Vector2(2731, 803);
            _camera.Position = new Vector2(2731, 723);
        }
    }

    public void Shoot()
    {
        var b = bullet.Instance();
        _positionShooting.AddChild(b);
    }
}
