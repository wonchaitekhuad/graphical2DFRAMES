// EditPointsForm.Designer.cs
using System;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    partial class EditPointsForm
    {
        private DataGridView dgv;
        private Button btnAdd, btnRemove, btnOK, btnCancel;

        /// <summary>
        /// Method required by designer — do not put runtime-only logic here.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // dgv
            this.dgv.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv.Height = 220;
            this.dgv.AutoGenerateColumns = false;
            var colX = new DataGridViewTextBoxColumn { HeaderText = "X", DataPropertyName = "X", ValueType = typeof(double), Width = 160 };
            var colY = new DataGridViewTextBoxColumn { HeaderText = "Y", DataPropertyName = "Y", ValueType = typeof(double), Width = 160 };
            this.dgv.Columns.AddRange(new DataGridViewColumn[] { colX, colY });
            this.dgv.AllowUserToAddRows = false;

            // buttons
            this.btnAdd.Text = "Add";
            this.btnAdd.Left = 10;
            this.btnAdd.Width = 80;
            this.btnAdd.Top = 230;

            this.btnRemove.Text = "Remove";
            this.btnRemove.Left = 100;
            this.btnRemove.Width = 80;
            this.btnRemove.Top = 230;

            this.btnOK.Text = "OK";
            this.btnOK.Left = 220;
            this.btnOK.Width = 80;
            this.btnOK.Top = 230;
            this.btnOK.DialogResult = DialogResult.OK;

            this.btnCancel.Text = "Cancel";
            this.btnCancel.Left = 310;
            this.btnCancel.Width = 80;
            this.btnCancel.Top = 230;
            this.btnCancel.DialogResult = DialogResult.Cancel;

            // wire-up minimal event handlers (implemented in EditPointsForm.cs)
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            this.dgv.CellValidating += new DataGridViewCellValidatingEventHandler(this.dgv_CellValidating);

            // form
            this.Text = "Edit Points (X,Y)";
            this.Width = 420;
            this.Height = 360;
            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);

            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
        }
    }
}
