using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Univer.Model;

namespace Univer
{
    public partial class AddGroups : Form
    {
        Group selecktedGroup;
        UniversityEntities db = new UniversityEntities();

        public AddGroups()
        {
            InitializeComponent();
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            fillDgwGroups();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string groupname = txtGroupName.Text;
            if (groupname != String.Empty)
            {
                if (db.Groups.Any(gp => gp.Name == groupname))
                {
                    lblError.Text = "This group exists";
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                    db.Groups.Add(new Group()
                    {
                    Name = groupname,
                    });
                    db.SaveChanges();
                    MessageBox.Show(groupname + " was added to Subjects.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillDgwGroups();
                    txtGroupName.Text = string.Empty;
                }
            }
            else
            {
                lblError.Text = "Please, fill all the fields.";
                lblError.Visible = true;
            }
        }

        private void fillDgwGroups()
        {
            dgwGroups.DataSource = db.Groups.Select(gr => new
            {
                Id = gr.Id,
                Name = gr.Name
            }).ToList();
            dgwGroups.Columns[0].Visible = false;
        }

        private void dgwGroups_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int GroupId = (int)dgwGroups.Rows[e.RowIndex].Cells[0].Value;
            selecktedGroup = db.Groups.Find(GroupId);
            txtGroupName.Text = selecktedGroup.Name;
            editMode();
        }

        private void editMode()
        {
            btnAdd.Visible = false;
            btnDelete.Visible = false;
            btnEdit.Visible = true;
        }

        private void AddMode()
        {
            btnAdd.Visible = true;
            btnDelete.Visible = false;
            btnEdit.Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string groupName = txtGroupName.Text;
            if (groupName != String.Empty)
            {
                selecktedGroup.Name = groupName;
                if (db.Groups.Any(gp => gp.Name == groupName))
                {
                    lblError.Text = "This group exists";
                    lblError.Visible = true;
                }
                else
                {
                    db.SaveChanges();
                    fillDgwGroups();
                    MessageBox.Show(groupName + " was updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblError.Visible = false;
                    txtGroupName.Text = string.Empty;
                    AddMode();
                }
            }
            else
            {
                lblError.Text = "Please, fill all the fields.";
                lblError.Visible = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selecktedGroup != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Group?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    db.Groups.Remove(selecktedGroup);
                    db.SaveChanges();
                    txtGroupName.Text = string.Empty;
                    fillDgwGroups();
                    MessageBox.Show("Group was deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddMode();
                }
            }
        }
    }
}
