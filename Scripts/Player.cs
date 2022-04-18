using Godot;

public class Player : KinematicBody2D
{
    [Export] readonly int _moveSpeed=20;
    Vector2 velocity ;
    Camera2D _camera;
    AnimationPlayer _playerAnim,_idleAnimation;
    Sprite _playerSpirte;
    public override void _Ready()
    {
        _camera = GetNode<Camera2D>("/root/MainLevel/MainCam");
        _playerAnim = GetNode<AnimationPlayer>("runAnimation");
        _playerSpirte = GetNode<Sprite>("PlayerSprite");
        _idleAnimation = GetNode<AnimationPlayer>("idleStatus");
        _idleAnimation.Play("idle");
    }
    public override void _PhysicsProcess(float delta)
    {
        movePlayer(delta);
        _camera.Position = new Vector2(this.GlobalPosition.x,_camera.Position.y) ;
    }
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public void movePlayer(float delta)
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
        MoveAndSlide(velocity);
    }
}
