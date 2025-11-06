using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    /// <summary>
    /// A WinForms form for managing decimal coordinates with auto-scaling canvas drawing.
    /// Supports Add/Update/Remove/Clear operations and Y-axis inversion.
    /// </summary>
    public class DecimalPointsForm : Form
    {
        // UI Controls
        private TextBox txtX;
        private TextBox txtY;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnRemove;
        private Button btnClear;
        private ListBox lstPoints;
        private PictureBox picCanvas;
        private CheckBox chkInvertY;
        private Label lblX;
        private Label lblY;
        private Label lblPoints;
        private Label lblCanvas;

        // Data storage - using List of PointF for decimal coordinates
        private List<PointF> points = new List<PointF>();

        public DecimalPointsForm()
        {
            InitializeComponents();
            SetupEventHandlers();
        }

        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Decimal Coordinates Manager";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Label for X
            lblX = new Label
            {
                Text = "X:",
                Location = new Point(20, 20),
                Size = new Size(30, 20)
            };

            // TextBox for X coordinate
            txtX = new TextBox
            {
                Location = new Point(55, 18),
                Size = new Size(100, 20)
            };

            // Label for Y
            lblY = new Label
            {
                Text = "Y:",
                Location = new Point(170, 20),
                Size = new Size(30, 20)
            };

            // TextBox for Y coordinate
            txtY = new TextBox
            {
                Location = new Point(205, 18),
                Size = new Size(100, 20)
            };

            // Add button
            btnAdd = new Button
            {
                Text = "Add",
                Location = new Point(320, 16),
                Size = new Size(75, 23)
            };

            // Update button
            btnUpdate = new Button
            {
                Text = "Update",
                Location = new Point(405, 16),
                Size = new Size(75, 23),
                Enabled = false
            };

            // Remove button
            btnRemove = new Button
            {
                Text = "Remove",
                Location = new Point(490, 16),
                Size = new Size(75, 23),
                Enabled = false
            };

            // Clear button
            btnClear = new Button
            {
                Text = "Clear All",
                Location = new Point(575, 16),
                Size = new Size(75, 23)
            };

            // Label for Points List
            lblPoints = new Label
            {
                Text = "Points List:",
                Location = new Point(20, 55),
                Size = new Size(100, 20)
            };

            // ListBox for points
            lstPoints = new ListBox
            {
                Location = new Point(20, 80),
                Size = new Size(280, 450)
            };

            // CheckBox for Y-axis inversion
            chkInvertY = new CheckBox
            {
                Text = "Invert Y-axis",
                Location = new Point(320, 80),
                Size = new Size(120, 20)
            };

            // Label for Canvas
            lblCanvas = new Label
            {
                Text = "Canvas (Auto-scaled):",
                Location = new Point(320, 105),
                Size = new Size(150, 20)
            };

            // PictureBox for canvas
            picCanvas = new PictureBox
            {
                Location = new Point(320, 130),
                Size = new Size(550, 400),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Add all controls to form
            this.Controls.Add(lblX);
            this.Controls.Add(txtX);
            this.Controls.Add(lblY);
            this.Controls.Add(txtY);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnRemove);
            this.Controls.Add(btnClear);
            this.Controls.Add(lblPoints);
            this.Controls.Add(lstPoints);
            this.Controls.Add(chkInvertY);
            this.Controls.Add(lblCanvas);
            this.Controls.Add(picCanvas);
        }

        private void SetupEventHandlers()
        {
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnRemove.Click += BtnRemove_Click;
            btnClear.Click += BtnClear_Click;
            lstPoints.SelectedIndexChanged += LstPoints_SelectedIndexChanged;
            chkInvertY.CheckedChanged += ChkInvertY_CheckedChanged;
            picCanvas.Paint += PicCanvas_Paint;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!TryParseCoordinates(out float x, out float y))
            {
                return;
            }

            // Add point to list
            points.Add(new PointF(x, y));

            // Update ListBox with F4 formatting
            RefreshListBox();

            // Clear input fields
            txtX.Clear();
            txtY.Clear();
            txtX.Focus();

            // Redraw canvas
            picCanvas.Invalidate();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a point to update.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryParseCoordinates(out float x, out float y))
            {
                return;
            }

            // Update the selected point
            int index = lstPoints.SelectedIndex;
            points[index] = new PointF(x, y);

            // Refresh ListBox
            RefreshListBox();

            // Restore selection
            lstPoints.SelectedIndex = index;

            // Redraw canvas
            picCanvas.Invalidate();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a point to remove.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = lstPoints.SelectedIndex;
            points.RemoveAt(index);

            // Refresh ListBox
            RefreshListBox();

            // Clear text boxes and disable update/remove buttons
            txtX.Clear();
            txtY.Clear();
            btnUpdate.Enabled = false;
            btnRemove.Enabled = false;

            // Redraw canvas
            picCanvas.Invalidate();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (points.Count == 0)
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all points?", 
                "Confirm Clear", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                points.Clear();
                RefreshListBox();
                txtX.Clear();
                txtY.Clear();
                btnUpdate.Enabled = false;
                btnRemove.Enabled = false;
                picCanvas.Invalidate();
            }
        }

        private void LstPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndex >= 0)
            {
                PointF point = points[lstPoints.SelectedIndex];
                txtX.Text = point.X.ToString("F4", CultureInfo.CurrentCulture);
                txtY.Text = point.Y.ToString("F4", CultureInfo.CurrentCulture);
                btnUpdate.Enabled = true;
                btnRemove.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
                btnRemove.Enabled = false;
            }

            // Redraw canvas to highlight selected point
            picCanvas.Invalidate();
        }

        private void ChkInvertY_CheckedChanged(object sender, EventArgs e)
        {
            picCanvas.Invalidate();
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (points.Count == 0)
            {
                return;
            }

            // Calculate bounds for auto-scaling
            float minX = points.Min(p => p.X);
            float maxX = points.Max(p => p.X);
            float minY = points.Min(p => p.Y);
            float maxY = points.Max(p => p.Y);

            // Add margin (10% on each side)
            float rangeX = maxX - minX;
            float rangeY = maxY - minY;

            // Handle case where all points have same X or Y
            if (rangeX < 0.0001f) rangeX = 1.0f;
            if (rangeY < 0.0001f) rangeY = 1.0f;

            float marginX = rangeX * 0.1f;
            float marginY = rangeY * 0.1f;

            minX -= marginX;
            maxX += marginX;
            minY -= marginY;
            maxY += marginY;

            float worldWidth = maxX - minX;
            float worldHeight = maxY - minY;

            // Canvas dimensions
            int canvasWidth = picCanvas.Width - 20; // Leave 10px margin on each side
            int canvasHeight = picCanvas.Height - 20;

            // Calculate scaling factors
            float scaleX = canvasWidth / worldWidth;
            float scaleY = canvasHeight / worldHeight;

            // Use the smaller scale to maintain aspect ratio
            float scale = Math.Min(scaleX, scaleY);

            // Drawing function to map world coordinates to canvas coordinates
            PointF WorldToCanvas(PointF worldPoint)
            {
                float x = (worldPoint.X - minX) * scale + 10;
                float y = chkInvertY.Checked 
                    ? (worldPoint.Y - minY) * scale + 10
                    : canvasHeight - (worldPoint.Y - minY) * scale + 10;
                return new PointF(x, y);
            }

            // Draw axes
            using (Pen axisPen = new Pen(Color.LightGray, 1))
            {
                // X-axis at y=0 if 0 is in range
                if (minY <= 0 && maxY >= 0)
                {
                    PointF p1 = WorldToCanvas(new PointF(minX, 0));
                    PointF p2 = WorldToCanvas(new PointF(maxX, 0));
                    g.DrawLine(axisPen, p1, p2);
                }

                // Y-axis at x=0 if 0 is in range
                if (minX <= 0 && maxX >= 0)
                {
                    PointF p1 = WorldToCanvas(new PointF(0, minY));
                    PointF p2 = WorldToCanvas(new PointF(0, maxY));
                    g.DrawLine(axisPen, p1, p2);
                }
            }

            // Draw points and labels
            using (Brush pointBrush = new SolidBrush(Color.Blue))
            using (Brush selectedBrush = new SolidBrush(Color.Red))
            using (Brush textBrush = new SolidBrush(Color.Black))
            using (Font font = new Font("Arial", 8))
            {
                for (int i = 0; i < points.Count; i++)
                {
                    PointF worldPoint = points[i];
                    PointF canvasPoint = WorldToCanvas(worldPoint);

                    // Determine if this point is selected
                    bool isSelected = (lstPoints.SelectedIndex == i);
                    Brush brush = isSelected ? selectedBrush : pointBrush;
                    float radius = isSelected ? 6f : 4f;

                    // Draw point
                    g.FillEllipse(brush, canvasPoint.X - radius, canvasPoint.Y - radius, 
                        radius * 2, radius * 2);

                    // Draw label with F4 formatting
                    string label = $"({worldPoint.X.ToString("F4", CultureInfo.CurrentCulture)}, " +
                                   $"{worldPoint.Y.ToString("F4", CultureInfo.CurrentCulture)})";
                    g.DrawString(label, font, textBrush, canvasPoint.X + 8, canvasPoint.Y - 8);
                }
            }

            // Draw connection lines between points
            if (points.Count > 1)
            {
                using (Pen linePen = new Pen(Color.LightBlue, 1))
                {
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        PointF p1 = WorldToCanvas(points[i]);
                        PointF p2 = WorldToCanvas(points[i + 1]);
                        g.DrawLine(linePen, p1, p2);
                    }
                }
            }
        }

        private bool TryParseCoordinates(out float x, out float y)
        {
            x = 0;
            y = 0;

            // Parse X coordinate
            if (!float.TryParse(txtX.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out x))
            {
                MessageBox.Show(
                    $"Invalid X coordinate: '{txtX.Text}'. Please enter a valid decimal number.",
                    "Parse Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtX.Focus();
                txtX.SelectAll();
                return false;
            }

            // Parse Y coordinate
            if (!float.TryParse(txtY.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out y))
            {
                MessageBox.Show(
                    $"Invalid Y coordinate: '{txtY.Text}'. Please enter a valid decimal number.",
                    "Parse Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtY.Focus();
                txtY.SelectAll();
                return false;
            }

            return true;
        }

        private void RefreshListBox()
        {
            int selectedIndex = lstPoints.SelectedIndex;
            lstPoints.Items.Clear();

            for (int i = 0; i < points.Count; i++)
            {
                PointF point = points[i];
                // Format with F4 (4 decimal places)
                string item = $"Point {i + 1}: ({point.X.ToString("F4", CultureInfo.CurrentCulture)}, " +
                             $"{point.Y.ToString("F4", CultureInfo.CurrentCulture)})";
                lstPoints.Items.Add(item);
            }

            // Restore selection if valid
            if (selectedIndex >= 0 && selectedIndex < lstPoints.Items.Count)
            {
                lstPoints.SelectedIndex = selectedIndex;
            }
        }
    }
}
