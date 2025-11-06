using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class DecimalPointsForm : Form
    {
        private List<PointF> coordinates;
        private int selectedIndex = -1;
        
        // UI Controls
        private TextBox txtX;
        private TextBox txtY;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnRemove;
        private Button btnClear;
        private ListBox lstCoordinates;
        private PictureBox pictureBox;
        private CheckBox chkInvertY;
        private Label lblX;
        private Label lblY;
        
        public DecimalPointsForm()
        {
            InitializeComponent();
            coordinates = new List<PointF>();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Decimal Coordinates Manager";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Labels
            lblX = new Label();
            lblX.Text = "X:";
            lblX.Location = new Point(10, 10);
            lblX.Size = new Size(30, 20);
            this.Controls.Add(lblX);
            
            lblY = new Label();
            lblY.Text = "Y:";
            lblY.Location = new Point(10, 40);
            lblY.Size = new Size(30, 20);
            this.Controls.Add(lblY);
            
            // TextBoxes
            txtX = new TextBox();
            txtX.Location = new Point(45, 10);
            txtX.Size = new Size(100, 20);
            this.Controls.Add(txtX);
            
            txtY = new TextBox();
            txtY.Location = new Point(45, 40);
            txtY.Size = new Size(100, 20);
            this.Controls.Add(txtY);
            
            // Buttons
            btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(160, 10);
            btnAdd.Size = new Size(75, 23);
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);
            
            btnUpdate = new Button();
            btnUpdate.Text = "Update";
            btnUpdate.Location = new Point(240, 10);
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.Click += BtnUpdate_Click;
            this.Controls.Add(btnUpdate);
            
            btnRemove = new Button();
            btnRemove.Text = "Remove";
            btnRemove.Location = new Point(320, 10);
            btnRemove.Size = new Size(75, 23);
            btnRemove.Click += BtnRemove_Click;
            this.Controls.Add(btnRemove);
            
            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(400, 10);
            btnClear.Size = new Size(75, 23);
            btnClear.Click += BtnClear_Click;
            this.Controls.Add(btnClear);
            
            // CheckBox for Y inversion
            chkInvertY = new CheckBox();
            chkInvertY.Text = "Invert Y-Axis";
            chkInvertY.Location = new Point(160, 40);
            chkInvertY.Size = new Size(100, 20);
            chkInvertY.CheckedChanged += ChkInvertY_CheckedChanged;
            this.Controls.Add(chkInvertY);
            
            // ListBox
            lstCoordinates = new ListBox();
            lstCoordinates.Location = new Point(10, 70);
            lstCoordinates.Size = new Size(200, 480);
            lstCoordinates.SelectedIndexChanged += LstCoordinates_SelectedIndexChanged;
            this.Controls.Add(lstCoordinates);
            
            // PictureBox
            pictureBox = new PictureBox();
            pictureBox.Location = new Point(220, 70);
            pictureBox.Size = new Size(560, 480);
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.BackColor = Color.White;
            pictureBox.Paint += PictureBox_Paint;
            this.Controls.Add(pictureBox);
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (TryParseCoordinates(out float x, out float y))
            {
                coordinates.Add(new PointF(x, y));
                UpdateListBox();
                ClearInputs();
                pictureBox.Invalidate();
            }
            else
            {
                MessageBox.Show("Invalid coordinates. Please enter valid decimal numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < coordinates.Count)
            {
                if (TryParseCoordinates(out float x, out float y))
                {
                    coordinates[selectedIndex] = new PointF(x, y);
                    UpdateListBox();
                    ClearInputs();
                    pictureBox.Invalidate();
                }
                else
                {
                    MessageBox.Show("Invalid coordinates. Please enter valid decimal numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a coordinate to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= 0 && selectedIndex < coordinates.Count)
            {
                coordinates.RemoveAt(selectedIndex);
                selectedIndex = -1;
                UpdateListBox();
                ClearInputs();
                pictureBox.Invalidate();
            }
            else
            {
                MessageBox.Show("Please select a coordinate to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void BtnClear_Click(object sender, EventArgs e)
        {
            coordinates.Clear();
            selectedIndex = -1;
            UpdateListBox();
            ClearInputs();
            pictureBox.Invalidate();
        }
        
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
        
        private void ChkInvertY_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }
        
        private bool TryParseCoordinates(out float x, out float y)
        {
            bool xValid = float.TryParse(txtX.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out x);
            bool yValid = float.TryParse(txtY.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out y);
            return xValid && yValid;
        }
        
        private void UpdateListBox()
        {
            int currentIndex = lstCoordinates.SelectedIndex;
            lstCoordinates.Items.Clear();
            
            for (int i = 0; i < coordinates.Count; i++)
            {
                PointF point = coordinates[i];
                string formattedPoint = string.Format(CultureInfo.CurrentCulture, 
                    "({0:F4}, {1:F4})", point.X, point.Y);
                lstCoordinates.Items.Add(formattedPoint);
            }
            
            if (currentIndex >= 0 && currentIndex < coordinates.Count)
            {
                lstCoordinates.SelectedIndex = currentIndex;
            }
        }
        
        private void ClearInputs()
        {
            txtX.Text = "";
            txtY.Text = "";
        }
        
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (coordinates.Count == 0)
                return;
                
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            // Calculate bounds for auto-scaling
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            
            foreach (PointF point in coordinates)
            {
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }
            
            // Add padding (10% margin)
            float rangeX = maxX - minX;
            float rangeY = maxY - minY;
            
            if (rangeX < 0.0001f) rangeX = 1.0f;
            if (rangeY < 0.0001f) rangeY = 1.0f;
            
            float paddingX = rangeX * 0.1f;
            float paddingY = rangeY * 0.1f;
            
            minX -= paddingX;
            maxX += paddingX;
            minY -= paddingY;
            maxY += paddingY;
            
            rangeX = maxX - minX;
            rangeY = maxY - minY;
            
            // Calculate scaling factors
            float scaleX = (pictureBox.Width - 40) / rangeX;
            float scaleY = (pictureBox.Height - 40) / rangeY;
            float scale = Math.Min(scaleX, scaleY);
            
            // Draw axes
            Pen axisPen = new Pen(Color.Gray, 1);
            axisPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            
            // Y-axis (vertical line at x=0 if visible)
            if (minX <= 0 && maxX >= 0)
            {
                float screenX = 20 + (0 - minX) * scale;
                g.DrawLine(axisPen, screenX, 20, screenX, pictureBox.Height - 20);
            }
            
            // X-axis (horizontal line at y=0 if visible)
            if (minY <= 0 && maxY >= 0)
            {
                float screenY = chkInvertY.Checked ? 
                    (pictureBox.Height - 20 - (0 - minY) * scale) : 
                    (20 + (maxY - 0) * scale);
                g.DrawLine(axisPen, 20, screenY, pictureBox.Width - 20, screenY);
            }
            
            // Draw points and labels
            Brush pointBrush = new SolidBrush(Color.Red);
            Pen pointPen = new Pen(Color.DarkRed, 2);
            Font labelFont = new Font("Arial", 8);
            Brush labelBrush = new SolidBrush(Color.Black);
            
            for (int i = 0; i < coordinates.Count; i++)
            {
                PointF worldPoint = coordinates[i];
                
                // Map world coordinates to screen coordinates
                float screenX = 20 + (worldPoint.X - minX) * scale;
                float screenY;
                
                if (chkInvertY.Checked)
                {
                    // Standard coordinate system (Y increases upward)
                    screenY = pictureBox.Height - 20 - (worldPoint.Y - minY) * scale;
                }
                else
                {
                    // Inverted coordinate system (Y increases downward)
                    screenY = 20 + (maxY - worldPoint.Y) * scale;
                }
                
                // Draw point
                g.FillEllipse(pointBrush, screenX - 4, screenY - 4, 8, 8);
                g.DrawEllipse(pointPen, screenX - 4, screenY - 4, 8, 8);
                
                // Draw label
                string label = string.Format(CultureInfo.CurrentCulture, 
                    "({0:F4}, {1:F4})", worldPoint.X, worldPoint.Y);
                g.DrawString(label, labelFont, labelBrush, screenX + 6, screenY - 10);
            }
            
            axisPen.Dispose();
            pointBrush.Dispose();
            pointPen.Dispose();
            labelFont.Dispose();
            labelBrush.Dispose();
        }
    }
}
