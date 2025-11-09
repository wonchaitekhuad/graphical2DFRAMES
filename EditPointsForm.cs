// EditPointsForm.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class EditPointsForm : Form
    {
        private BindingList<PointXY> _points;

        // คืนค่า list ที่แก้ไขแล้ว
        public List<PointXY> Points => _points.Select(p => new PointXY(p.X, p.Y)).ToList();

        public EditPointsForm(IEnumerable<PointXY> initialPoints = null)
        {
            InitializeComponent();
            var list = (initialPoints ?? new List<PointXY>()).Select(p => new PointXY(p.X, p.Y)).ToList();
            _points = new BindingList<PointXY>(list);
            dgv.DataSource = _points;
        }

        // Event handlers
        private void btnAdd_Click(object sender, EventArgs e)
        {
            _points.Add(new PointXY(0, 0));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                var item = dgv.CurrentRow.DataBoundItem as PointXY;
                if (item != null) _points.Remove(item);
            }
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = dgv.Columns[e.ColumnIndex];
            if (col.DataPropertyName == "X" || col.DataPropertyName == "Y")
            {
                string formatted = Convert.ToString(e.FormattedValue);
                if (!double.TryParse(formatted, out _))
                {
                    e.Cancel = true;
                    MessageBox.Show("กรุณาใส่ค่าเป็นตัวเลข (double).", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
