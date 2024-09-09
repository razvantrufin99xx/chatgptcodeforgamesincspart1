Creating a small game in C# inspired by **"River Raid"** from the **Atari 2600** involves a few key concepts like movement, obstacles, and collision detection. While it won't be a full replica, we can create a simple 2D scrolling shooter with basic mechanics using **Windows Forms** or **Unity**. For this example, I will use **Windows Forms** to create a simplified version of the gameplay experience.

### Features of the Game:
- **Player-controlled ship**: The player moves up and down while the background scrolls vertically.
- **Obstacles (enemies)**: Random obstacles scroll toward the player.
- **Shooting**: The player can shoot bullets to destroy obstacles.
- **Collision Detection**: Detect when the player hits an obstacle.

Here's a basic implementation using **Windows Forms**:

### Small "River" Game in C# Using Windows Forms

```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class RiverRaidGame : Form
{
    private Timer gameTimer;
    private Rectangle player;
    private List<Rectangle> obstacles;
    private List<Rectangle> bullets;
    private int playerSpeed = 5;
    private int obstacleSpeed = 5;
    private int bulletSpeed = 10;
    private Random random;
    private int score = 0;

    public RiverRaidGame()
    {
        // Initialize the form
        this.Text = "Simple River Game";
        this.Width = 400;
        this.Height = 600;
        this.DoubleBuffered = true;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize game objects
        player = new Rectangle(this.Width / 2 - 20, this.Height - 60, 40, 40);
        obstacles = new List<Rectangle>();
        bullets = new List<Rectangle>();
        random = new Random();

        // Initialize the game timer
        gameTimer = new Timer();
        gameTimer.Interval = 20; // 50 FPS
        gameTimer.Tick += GameLoop;
        gameTimer.Start();

        // Handle key events
        this.KeyDown += OnKeyDown;
    }

    // Main game loop
    private void GameLoop(object sender, EventArgs e)
    {
        MovePlayer();
        MoveObstacles();
        MoveBullets();
        DetectCollisions();
        AddObstacle();
        this.Invalidate(); // Redraw the form
    }

    // Move the player within bounds
    private void MovePlayer()
    {
        if (player.Y > 0 && player.Y < this.ClientSize.Height - player.Height)
        {
            // Let player move freely within vertical bounds
        }
    }

    // Move obstacles downward
    private void MoveObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            obstacles[i] = new Rectangle(obstacles[i].X, obstacles[i].Y + obstacleSpeed, obstacles[i].Width, obstacles[i].Height);

            // Remove obstacles that go off the screen
            if (obstacles[i].Y > this.ClientSize.Height)
            {
                obstacles.RemoveAt(i);
                i--;
                score += 10; // Increment score when obstacles pass the player
            }
        }
    }

    // Move bullets upward
    private void MoveBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i] = new Rectangle(bullets[i].X, bullets[i].Y - bulletSpeed, bullets[i].Width, bullets[i].Height);

            // Remove bullets that go off the screen
            if (bullets[i].Y < 0)
            {
                bullets.RemoveAt(i);
                i--;
            }
        }
    }

    // Add new obstacles at random positions
    private void AddObstacle()
    {
        if (random.Next(0, 100) < 5) // 5% chance to add a new obstacle each frame
        {
            int width = random.Next(20, 60);
            int height = random.Next(20, 60);
            int xPos = random.Next(0, this.ClientSize.Width - width);
            obstacles.Add(new Rectangle(xPos, -height, width, height));
        }
    }

    // Detect collisions between player, obstacles, and bullets
    private void DetectCollisions()
    {
        // Check if player hits an obstacle
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (player.IntersectsWith(obstacles[i]))
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over! Your score: " + score);
                Application.Exit();
            }
        }

        // Check if bullets hit an obstacle
        for (int i = 0; i < bullets.Count; i++)
        {
            for (int j = 0; j < obstacles.Count; j++)
            {
                if (bullets[i].IntersectsWith(obstacles[j]))
                {
                    bullets.RemoveAt(i);
                    obstacles.RemoveAt(j);
                    score += 50; // Increment score for hitting an obstacle
                    return;
                }
            }
        }
    }

    // Handle key inputs to control the player and shoot bullets
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Left:
                if (player.X > 0) player.X -= playerSpeed;
                break;
            case Keys.Right:
                if (player.X < this.ClientSize.Width - player.Width) player.X += playerSpeed;
                break;
            case Keys.Up:
                if (player.Y > 0) player.Y -= playerSpeed;
                break;
            case Keys.Down:
                if (player.Y < this.ClientSize.Height - player.Height) player.Y += playerSpeed;
                break;
            case Keys.Space:
                Shoot();
                break;
        }
    }

    // Shoot a bullet from the player
    private void Shoot()
    {
        Rectangle bullet = new Rectangle(player.X + player.Width / 2 - 5, player.Y, 10, 20);
        bullets.Add(bullet);
    }

    // Render the game objects
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Draw the player
        g.FillRectangle(Brushes.Blue, player);

        // Draw obstacles
        foreach (var obstacle in obstacles)
        {
            g.FillRectangle(Brushes.Red, obstacle);
        }

        // Draw bullets
        foreach (var bullet in bullets)
        {
            g.FillRectangle(Brushes.Yellow, bullet);
        }

        // Draw score
        g.DrawString("Score: " + score, new Font("Arial", 16), Brushes.White, 10, 10);

        base.OnPaint(e);
    }

    // Main entry point
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new RiverRaidGame());
    }
}
```

### Explanation:

1. **Form Setup**:
   - The form is sized to 400x600 pixels, simulating a vertical scrolling game.
   - The game uses double-buffering to prevent flickering during rendering.

2. **Player**:
   - The player is represented by a blue rectangle at the bottom of the screen, and it can move left, right, up, and down within the game window using the arrow keys.
   - The player can shoot bullets using the spacebar.

3. **Obstacles**:
   - Obstacles are randomly generated at the top of the screen and move downward at a fixed speed.
   - If an obstacle passes the player, the score is increased by 10 points.

4. **Bullets**:
   - When the player presses the spacebar, a bullet is shot upward from the player's position.
   - Bullets are yellow rectangles that move upward and are removed when they go off-screen or hit an obstacle.

5. **Collisions**:
   - If the player's rectangle intersects with any obstacle, the game ends and displays the player's score.
   - If a bullet hits an obstacle, both the bullet and obstacle are removed, and the score increases by 50 points.

6. **Scoring**:
   - The score is displayed at the top left of the screen, and the player earns points for dodging obstacles and shooting them.

### How to Run the Game:

1. Open **Visual Studio** or another C# IDE.
2. Create a new **Windows Forms Application** project.
3. Replace the code in `Form1.cs` with the code provided above.
4. Run the project.

### Controls:

- **Arrow Keys**: Move the player (left, right, up, and down).
- **Spacebar**: Shoot bullets.

### Potential Enhancements:

- **Add Fuel/Power-Ups**: Add fuel or power-ups that the player needs to collect to keep playing.
- **Add Lives**: Introduce lives or health so the player doesn't lose on the first hit.
- **Enhanced Graphics**: Replace rectangles with images to simulate a more realistic river, player, and obstacles.
- **Enemy Behavior**: Add smarter enemies or obstacles that move sideways or change speed.

This simple game captures the essence of an old-school **"River Raid"**-like game, with a player dodging and shooting obstacles while moving through a scrolling environment.