To create a simple image loader in C#, we can use **Windows Forms** and the `PictureBox` control to load and display images. Below is a basic example of an image loader that allows you to open and display an image using a `OpenFileDialog`.

### Steps:
1. Create a Windows Forms application.
2. Use a `PictureBox` control to display the image.
3. Use an `OpenFileDialog` to select an image from the file system.

### Example Code: Image Loader in C#

```csharp
using System;
using System.Windows.Forms;
using System.Drawing;

public class ImageLoaderForm : Form
{
    private PictureBox pictureBox;
    private Button loadImageButton;

    public ImageLoaderForm()
    {
        // Initialize the form
        this.Text = "Simple Image Loader";
        this.Width = 800;
        this.Height = 600;

        // Initialize the PictureBox control to display the image
        pictureBox = new PictureBox();
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox.Dock = DockStyle.Fill;
        this.Controls.Add(pictureBox);

        // Initialize the button to load the image
        loadImageButton = new Button();
        loadImageButton.Text = "Load Image";
        loadImageButton.Dock = DockStyle.Bottom;
        loadImageButton.Height = 40;
        loadImageButton.Click += new EventHandler(OnLoadImageClick);
        this.Controls.Add(loadImageButton);
    }

    // Event handler to load the image
    private void OnLoadImageClick(object sender, EventArgs e)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            // Set the file dialog to filter for image files only
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            // Show the file dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Load the image into the PictureBox
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
    }

    // Main entry point for the application
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new ImageLoaderForm());
    }
}
```

### Explanation:

1. **Form Setup**:
   - The form (`ImageLoaderForm`) has a `PictureBox` that automatically adjusts the image size using `SizeMode = PictureBoxSizeMode.Zoom`, and a button (`loadImageButton`) to trigger the loading of the image.
   - The button is positioned at the bottom of the form using `DockStyle.Bottom`, while the `PictureBox` takes the remaining space with `DockStyle.Fill`.

2. **OpenFileDialog**:
   - When the user clicks the button, an `OpenFileDialog` is displayed, allowing the user to choose an image file.
   - The file dialog is filtered to only show common image formats (`.jpg`, `.jpeg`, `.png`, `.bmp`, `.gif`).

3. **Loading the Image**:
   - Once an image is selected, the program attempts to load the image using `Image.FromFile()` and displays it in the `PictureBox`.
   - If the image loading fails (for example, due to a file error), an error message is displayed using a `MessageBox`.

4. **Main Entry**:
   - The `Main` method is the entry point of the application. It initializes the form and starts the application loop.

### How to Run:

1. Open **Visual Studio**.
2. Create a new **Windows Forms Application** project.
3. Replace the contents of the `Form1.cs` (or equivalent) with the code provided above.
4. Run the project.

### Example Output:

- The form will display an empty `PictureBox` with a "Load Image" button at the bottom.
- Clicking the "Load Image" button will open a file dialog to choose an image from your computer.
- After selecting an image, the image will be displayed inside the form, scaled to fit within the `PictureBox`.

This is a simple image loader with basic functionality, but it can be extended to add features like:
- **Resizing the form to fit the image size**.
- **Adding support for dragging and dropping images**.
- **Displaying image metadata (e.g., dimensions, file size)**.