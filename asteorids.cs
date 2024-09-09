Creating a simple **Asteroids-like** game in C# can be done using **Windows Forms** or **Unity**. For this example, we'll use **Windows Forms** to create a basic version of the classic arcade game, where the player controls a ship that can rotate and shoot bullets to destroy asteroids.

### Features:
- **Player-controlled ship**: The player can rotate the ship, move forward, and shoot bullets.
- **Asteroids**: Asteroids float around the screen and can be shot by the player.
- **Collision Detection**: The player loses if the ship hits an asteroid.

### Implementation in C# Using Windows Forms

```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class AsteroidsGame : Form
{
    private Timer gameTimer;
    private Ship player;
    private List<Asteroid> asteroids;
    private List<Bullet> bullets;
    private Random random;
    private int score;

    public AsteroidsGame()
    {
        // Initialize the form
        this.Text = "Asteroids Game";
        this.Width = 800;
        this.Height = 600;
        this.DoubleBuffered = true;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize game objects
        player = new Ship(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
        asteroids = new List<Asteroid>();
        bullets = new List<Bullet>();
        random = new Random();
        score = 0;

        // Initialize the game timer
        gameTimer = new Timer();
        gameTimer.Interval = 20; // 50 FPS
        gameTimer.Tick += GameLoop;
        gameTimer.Start();

        // Handle key events
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;

        // Add initial asteroids
        for (int i = 0; i < 5; i++)
        {
            AddAsteroid();
        }
    }

    // Main game loop
    private void GameLoop(object sender, EventArgs e)
    {
        player.Update(this.ClientSize);

        foreach (var asteroid in asteroids)
        {
            asteroid.Update(this.ClientSize);
        }

        foreach (var bullet in bullets)
        {
            bullet.Update(this.ClientSize);
        }

        CheckCollisions();
        RemoveOffscreenBullets();
        this.Invalidate(); // Redraw the form
    }

    // Handle key inputs for ship movement
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Left:
                player.RotationSpeed = -5;
                break;
            case Keys.Right:
                player.RotationSpeed = 5;
                break;
            case Keys.Up:
                player.Accelerating = true;
                break;
            case Keys.Space:
                Shoot();
                break;
        }
    }

    // Stop ship rotation and acceleration when keys are released
    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Left:
            case Keys.Right:
                player.RotationSpeed = 0;
                break;
            case Keys.Up:
                player.Accelerating = false;
                break;
        }
    }

    // Shoot a bullet from the player's position
    private void Shoot()
    {
        bullets.Add(new Bullet(player.Position, player.Rotation));
    }

    // Check for collisions between asteroids, bullets, and the player
    private void CheckCollisions()
    {
        for (int i = asteroids.Count - 1; i >= 0; i--)
        {
            // Check if player hits an asteroid
            if (player.CollidesWith(asteroids[i]))
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over! Your score: " + score);
                Application.Exit();
            }

            // Check if bullets hit asteroids
            for (int j = bullets.Count - 1; j >= 0; j--)
            {
                if (bullets[j].CollidesWith(asteroids[i]))
                {
                    bullets.RemoveAt(j);
                    asteroids.RemoveAt(i);
                    score += 100;
                    AddAsteroid(); // Add a new asteroid when one is destroyed
                    break;
                }
            }
        }
    }

    // Remove bullets that go off-screen
    private void RemoveOffscreenBullets()
    {
        bullets.RemoveAll(b => !b.OnScreen(this.ClientSize));
    }

    // Add a new asteroid at a random position
    private void AddAsteroid()
    {
        int x = random.Next(this.ClientSize.Width);
        int y = random.Next(this.ClientSize.Height);
        asteroids.Add(new Asteroid(x, y));
    }

    // Render the game objects
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Draw the player
        player.Draw(g);

        // Draw the asteroids
        foreach (var asteroid in asteroids)
        {
            asteroid.Draw(g);
        }

        // Draw the bullets
        foreach (var bullet in bullets)
        {
            bullet.Draw(g);
        }

        // Draw the score
        g.DrawString("Score: " + score, new Font("Arial", 16), Brushes.White, 10, 10);

        base.OnPaint(e);
    }

    // Main entry point
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new AsteroidsGame());
    }
}

// Class for the player's ship
public class Ship
{
    public PointF Position;
    public float Rotation;
    public float Speed;
    public float RotationSpeed;
    public bool Accelerating;
    private const float MaxSpeed = 5;
    private const float Acceleration = 0.1f;

    public Ship(float x, float y)
    {
        Position = new PointF(x, y);
        Rotation = 0;
        Speed = 0;
        Accelerating = false;
    }

    // Update the ship's position and rotation
    public void Update(Size clientSize)
    {
        if (Accelerating)
        {
            Speed = Math.Min(Speed + Acceleration, MaxSpeed);
        }
        else
        {
            Speed = Math.Max(Speed - Acceleration, 0);
        }

        // Rotate the ship
        Rotation += RotationSpeed;

        // Update position based on rotation
        Position.X += (float)(Math.Cos(Rotation * Math.PI / 180) * Speed);
        Position.Y += (float)(Math.Sin(Rotation * Math.PI / 180) * Speed);

        // Wrap the ship around the screen
        if (Position.X < 0) Position.X = clientSize.Width;
        if (Position.X > clientSize.Width) Position.X = 0;
        if (Position.Y < 0) Position.Y = clientSize.Height;
        if (Position.Y > clientSize.Height) Position.Y = 0;
    }

    // Draw the ship
    public void Draw(Graphics g)
    {
        // Simple triangle shape for the ship
        PointF[] shipPoints = new PointF[]
        {
            new PointF(Position.X + 10 * (float)Math.Cos(Rotation * Math.PI / 180), Position.Y + 10 * (float)Math.Sin(Rotation * Math.PI / 180)),
            new PointF(Position.X + 10 * (float)Math.Cos((Rotation + 140) * Math.PI / 180), Position.Y + 10 * (float)Math.Sin((Rotation + 140) * Math.PI / 180)),
            new PointF(Position.X + 10 * (float)Math.Cos((Rotation + 220) * Math.PI / 180), Position.Y + 10 * (float)Math.Sin((Rotation + 220) * Math.PI / 180)),
        };
        g.FillPolygon(Brushes.White, shipPoints);
    }

    // Check if the ship collides with an asteroid
    public bool CollidesWith(Asteroid asteroid)
    {
        return Distance(Position, asteroid.Position) < 20; // Simple collision detection
    }

    // Helper to calculate distance between two points
    private float Distance(PointF p1, PointF p2)
    {
        return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}

// Class for asteroids
public class Asteroid
{
    public PointF Position;
    public float Rotation;
    public float Speed;
    private Random random;

    public Asteroid(float x, float y)
    {
        Position = new PointF(x, y);
        random = new Random();
        Rotation = random.Next(360);
        Speed = (float)(random.NextDouble() * 2 + 1); // Random speed between 1 and 3
    }

    // Update the asteroid's position
    public void Update(Size clientSize)
    {
        Position.X += (float)(Math.Cos(Rotation * Math.PI / 180) * Speed);
        Position.Y += (float)(Math.Sin(Rotation * Math.PI / 180) * Speed);

        // Wrap around the screen
        if (Position.X < 0) Position.X = clientSize.Width;
        if (Position.X > clientSize.Width) Position.X = 0;
        if (Position.Y < 0) Position.Y = clientSize.Height;
        if (Position.Y > clientSize.Height) Position.Y = 0;
    }

    // Draw the asteroid
    public void Draw(Graphics g)
    {
        g.FillEllipse(Brushes.Gray, Position.X - 15, Position.Y - 15, 30, 30); // Draw asteroid as a circle


    }
}

// Class for bullets
public class Bullet
{
    public PointF Position;
    public float Rotation;
    private const float Speed = 7;

    public Bullet(PointF position, float rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    // Update the bullet's position
    public void Update(Size clientSize)
    {
        Position.X += (float)(Math.Cos(Rotation * Math.PI / 180) * Speed);
        Position.Y += (float)(Math.Sin(Rotation * Math.PI / 180) * Speed);
    }

    // Check if the bullet is still on screen
    public bool OnScreen(Size clientSize)
    {
        return Position.X >= 0 && Position.X <= clientSize.Width && Position.Y >= 0 && Position.Y <= clientSize.Height;
    }

    // Draw the bullet
    public void Draw(Graphics g)
    {
        g.FillEllipse(Brushes.Yellow, Position.X - 2, Position.Y - 2, 4, 4); // Draw bullet as a small circle
    }

    // Check if the bullet hits an asteroid
    public bool CollidesWith(Asteroid asteroid)
    {
        return Distance(Position, asteroid.Position) < 15; // Simple collision detection
    }

    // Helper to calculate distance between two points
    private float Distance(PointF p1, PointF p2)
    {
        return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}
```

### Explanation:

1. **Player's Ship**:
   - The ship can rotate using the left and right arrow keys.
   - The ship accelerates forward using the up arrow key.
   - The ship can shoot bullets using the spacebar.

2. **Asteroids**:
   - Asteroids are circles that move around the screen randomly.
   - If the player hits an asteroid, the game ends.

3. **Bullets**:
   - Bullets are shot in the direction the ship is facing, and they disappear if they go off-screen.

4. **Collision Detection**:
   - The game checks for collisions between the player's ship and asteroids, as well as between bullets and asteroids.

5. **Game Over**:
   - When the player's ship collides with an asteroid, the game stops, and the final score is displayed.

### How to Run the Game:

1. Open **Visual Studio** or another C# IDE.
2. Create a new **Windows Forms Application** project.
3. Replace the code in `Form1.cs` with the code provided above.
4. Run the project.

### Controls:
- **Left/Right Arrow Keys**: Rotate the ship.
- **Up Arrow Key**: Accelerate the ship forward.
- **Spacebar**: Shoot bullets.

This is a basic implementation of an **Asteroids-like** game in C#. It simulates the core mechanics of the original game, with player movement, shooting, asteroid movement, and simple collision detection.