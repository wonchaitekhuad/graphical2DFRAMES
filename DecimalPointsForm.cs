using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    /// <summary>
    /// Form for displaying and managing decimal coordinate points with auto-scaling visualization.
    /// Supports adding, updating, removing, and clearing coordinate points.
    /// </summary>
    public partial class DecimalPointsForm : Form
    {
        private List<PointF> coordinates;
        private TextBox txtX;
        private TextBox txtY;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnRemove;
        private Button btnClear;
        private ListBox lstCoordinates;
        private PictureBox picCanvas;
        private CheckBox chkInvertY;
        private Label lblX;
        private Label lblY;
        private Label lblCoordinates;
        private int selectedIndex = -1;

        public DecimalPointsForm()
        {
            InitializeComponent();
            coordinates = new List<PointF>();
        }

        /// <summary>
        /// Initialize form components and layout.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Decimal Coordinates Manager";
            this.Width = 900;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Labels
            lblX = new Label { Text = "X:", Left = 10, Top = 10, Width = 30 };
            lblY = new Label { Text = "Y:", Left = 10, Top = 40, Width = 30 };
            lblCoordinates = new Label { Text = "Coordinates:", Left = 10, Top = 180, Width = 200 };

            // TextBoxes
            txtX = new TextBox { Left = 50, Top = 10, Width = 150 };
            txtY = new TextBox { Left = 50, Top = 40, Width = 150 };

            // Buttons
            btnAdd = new Button { Text = "Add", Left = 10, Top = 70, Width = 80 };
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button { Text = "Update", Left = 100, Top = 70, Width = 80 };
            btnUpdate.Click += BtnUpdate_Click;

            btnRemove = new Button { Text = "Remove", Left = 10, Top = 100, Width = 80 };
            btnRemove.Click += BtnRemove_Click;

            btnClear = new Button { Text = "Clear All", Left = 100, Top = 100, Width = 80 };
            btnClear.Click += BtnClear_Click;

            // CheckBox for inverting Y-axis
            chkInvertY = new CheckBox { Text = "Invert Y-Axis", Left = 10, Top = 140, Width = 150 };
            chkInvertY.CheckedChanged += ChkInvertY_CheckedChanged;

            // ListBox for displaying coordinates
            lstCoordinates = new ListBox { Left = 10, Top = 200, Width = 200, Height = 300 };
            lstCoordinates.SelectedIndexChanged += LstCoordinates_SelectedIndexChanged;

            // PictureBox for drawing
            picCanvas = new PictureBox
            {
                Left = 220,
                Top = 10,
                Width = 650,
                Height = 520,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            picCanvas.Paint += PicCanvas_Paint;

            // Add controls to form
            this.Controls.Add(lblX);
            this.Controls.Add(lblY);
            this.Controls.Add(lblCoordinates);
            this.Controls.Add(txtX);
            this.Controls.Add(txtY);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnRemove);
            this.Controls.Add(btnClear);
            this.Controls.Add(chkInvertY);
            this.Controls.Add(lstCoordinates);
            this.Controls.Add(picCanvas);

            this.ResumeLayout(false);
        }

        /// <summary>
        /// Handles the Add button click event. Parses X and Y values and adds them to the coordinates list.
        /// Uses CultureInfo.CurrentCulture for decimal separator compatibility.
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            float x, y;
            if (float.TryParse(txtX.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out x) &&
                float.TryParse(txtY.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out y))
            {
                coordinates.Add(new PointF(x, y));
                UpdateListBox();
                picCanvas.Invalidate();
                txtX.Clear();
                txtY.Clear();
                txtX.Focus();
            }
            else
            {
                MessageBox.Show("Please enter valid decimal numbers for X and Y coordinates.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Handles the Update button click event. Updates the selected coordinate with new values.
        /// </summary>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < coordinates.Count)
            {
                float x, y;
                if (float.TryParse(txtX.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out x) &&
                    float.TryParse(txtY.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out y))
                {
                    coordinates[selectedIndex] = new PointF(x, y);
                    UpdateListBox();
                    picCanvas.Invalidate();
                    txtX.Clear();
                    txtY.Clear();
                    txtX.Focus();
                }
                else
                {
                    MessageBox.Show("Please enter valid decimal numbers for X and Y coordinates.", "Invalid Input",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a coordinate to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles the Remove button click event. Removes the selected coordinate from the list.
        /// </summary>
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < coordinates.Count)
            {
                coordinates.RemoveAt(selectedIndex);
                selectedIndex = -1;
                UpdateListBox();
                picCanvas.Invalidate();
                txtX.Clear();
                txtY.Clear();
            }
            else
            {
                MessageBox.Show("Please select a coordinate to remove.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles the Clear All button click event. Removes all coordinates from the list.
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all coordinates?", "Confirm Clear",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                coordinates.Clear();
                selectedIndex = -1;
                UpdateListBox();
                picCanvas.Invalidate();
                txtX.Clear();
                txtY.Clear();
            }
        }

        /// <summary>
        /// Handles the ListBox selection change event. Loads selected coordinate into text boxes.
        /// </summary>
        private void LstCoordinates_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndex = lstCoordinates.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < coordinates.Count)
            {
                PointF point = coordinates[selectedIndex];
                txtX.Text = point.X.ToString("F4", CultureInfo.CurrentCulture);
                txtY.Text = point.Y.ToString("F4", CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Handles the Invert Y-Axis checkbox change event. Redraws the canvas.
        /// </summary>
        private void ChkInvertY_CheckedChanged(object sender, EventArgs e)
        {
            picCanvas.Invalidate();
        }

        /// <summary>
        /// Updates the ListBox with formatted coordinate values (F4 format - 4 decimal places).
        /// </summary>
        private void UpdateListBox()
        {
            lstCoordinates.Items.Clear();
            for (int i = 0; i < coordinates.Count; i++)
            {
                PointF point = coordinates[i];
                string formattedText = string.Format(CultureInfo.CurrentCulture,
                    "Point {0}: ({1:F4}, {2:F4})", i + 1, point.X, point.Y);
                lstCoordinates.Items.Add(formattedText);
            }
        }

        /// <summary>
        /// Handles the PictureBox paint event. Implements auto-scaling to map world coordinates to canvas.
        /// Draws coordinate points and labels.
        /// </summary>
        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (coordinates.Count == 0)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Calculate bounds for auto-scaling
            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;

            foreach (PointF point in coordinates)
            {
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            // Add padding to the bounds
            float paddingPercent = 0.1f;
            float rangeX = maxX - minX;
            float rangeY = maxY - minY;
            
            // Handle single point or points on a line
            if (rangeX < 0.0001f) rangeX = 1.0f;
            if (rangeY < 0.0001f) rangeY = 1.0f;

            float paddingX = rangeX * paddingPercent;
            float paddingY = rangeY * paddingPercent;

            minX -= paddingX;
            maxX += paddingX;
            minY -= paddingY;
            maxY += paddingY;

            rangeX = maxX - minX;
            rangeY = maxY - minY;

            // Calculate scaling factors
            float scaleX = (picCanvas.Width - 40) / rangeX;
            float scaleY = (picCanvas.Height - 40) / rangeY;
            float scale = Math.Min(scaleX, scaleY);

            // Draw coordinate axes
            using (Pen axisPen = new Pen(Color.LightGray, 1))
            {
                // Draw grid lines
                for (int i = 0; i <= 10; i++)
                {
                    float x = 20 + i * (picCanvas.Width - 40) / 10f;
                    g.DrawLine(axisPen, x, 20, x, picCanvas.Height - 20);

                    float y = 20 + i * (picCanvas.Height - 40) / 10f;
                    g.DrawLine(axisPen, 20, y, picCanvas.Width - 20, y);
                }
            }

            // Draw points and labels
            using (Brush pointBrush = new SolidBrush(Color.Red))
            using (Brush labelBrush = new SolidBrush(Color.Blue))
            using (Font labelFont = new Font("Arial", 8))
            {
                for (int i = 0; i < coordinates.Count; i++)
                {
                    PointF worldPoint = coordinates[i];
                    
                    // Transform world coordinates to canvas coordinates
                    float canvasX = 20 + (worldPoint.X - minX) * scale;
                    float canvasY;
                    
                    if (chkInvertY.Checked)
                    {
                        // Normal orientation (Y increases downward on screen)
                        canvasY = 20 + (worldPoint.Y - minY) * scale;
                    }
                    else
                    {
                        // Inverted Y (Y increases upward on screen, like Cartesian coordinates)
                        canvasY = picCanvas.Height - 20 - (worldPoint.Y - minY) * scale;
                    }

                    // Draw point as filled circle
                    float pointSize = 6;
                    g.FillEllipse(pointBrush, canvasX - pointSize / 2, canvasY - pointSize / 2, pointSize, pointSize);

                    // Draw label
                    string label = string.Format(CultureInfo.CurrentCulture, "P{0}\n({1:F4},{2:F4})", 
                        i + 1, worldPoint.X, worldPoint.Y);
                    g.DrawString(label, labelFont, labelBrush, canvasX + 5, canvasY + 5);
                }
            }

            // Draw border around canvas
            using (Pen borderPen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(borderPen, 20, 20, picCanvas.Width - 40, picCanvas.Height - 40);
            }
        }
    }
}
