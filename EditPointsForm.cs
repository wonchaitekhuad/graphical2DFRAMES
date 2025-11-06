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
            
            // Bind the BindingList to the DataGridView
            dgv.DataSource = points;
            
            // Configure DataGridView columns
            if (dgv.Columns.Count >= 2)
            {
                dgv.Columns[0].Name = "X";
                dgv.Columns[0].HeaderText = "X";
                dgv.Columns[0].Width = 150;
                dgv.Columns[1].Name = "Y";
                dgv.Columns[1].HeaderText = "Y";
                dgv.Columns[1].Width = 150;
            }
        }

        /// <summary>
        /// Constructor that accepts an existing list of points
        /// </summary>
        public EditPointsForm(BindingList<PointData> existingPoints) : this()
        {
            if (existingPoints != null)
            {
                points = existingPoints;
                dgv.DataSource = points;
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
