using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class EditPointsForm : Form
    {
        private BindingList<PointData> points;

        public EditPointsForm()
        {
            InitializeComponent();

            // Initialize the BindingList for points
            points = new BindingList<PointData>();
            
            // Configure DataGridView columns explicitly
            dgv.AutoGenerateColumns = false;
            
            // Add X column
            DataGridViewTextBoxColumn colX = new DataGridViewTextBoxColumn();
            colX.Name = "X";
            colX.HeaderText = "X";
            colX.DataPropertyName = "X";
            colX.Width = 150;
            dgv.Columns.Add(colX);
            
            // Add Y column
            DataGridViewTextBoxColumn colY = new DataGridViewTextBoxColumn();
            colY.Name = "Y";
            colY.HeaderText = "Y";
            colY.DataPropertyName = "Y";
            colY.Width = 150;
            dgv.Columns.Add(colY);
            
            // Bind the BindingList to the DataGridView
            dgv.DataSource = points;
        }

        /// <summary>
        /// Constructor that accepts an existing list of points
        /// </summary>
        public EditPointsForm(BindingList<PointData> existingPoints) : this()
        {
            if (existingPoints != null)
            {
                // Copy existing points to avoid modifying the original list
                foreach (var point in existingPoints)
                {
                    points.Add(new PointData { X = point.X, Y = point.Y });
                }
            }
        }

        /// <summary>
        /// Gets the edited points
        /// </summary>
        public BindingList<PointData> GetPoints()
        {
            return points;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Add a new point with default values
            points.Add(new PointData { X = 0, Y = 0 });
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Remove the selected row
            if (dgv.SelectedRows.Count > 0 && !dgv.SelectedRows[0].IsNewRow)
            {
                int index = dgv.SelectedRows[0].Index;
                if (index >= 0 && index < points.Count)
                {
                    points.RemoveAt(index);
                }
            }
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Validate that the cell contains a valid number
            if (dgv.Columns[e.ColumnIndex].Name == "X" || dgv.Columns[e.ColumnIndex].Name == "Y")
            {
                double value;
                if (!double.TryParse(e.FormattedValue.ToString(), out value))
                {
                    e.Cancel = true;
                    MessageBox.Show("Please enter a valid number.", "Invalid Input", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }

    /// <summary>
    /// Data class to represent a point with X and Y coordinates
    /// </summary>
    public class PointData
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
