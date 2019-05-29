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
    public partial class AddSubjects : Form
    {
        Subject selectedSubject;
        UniversityEntities db = new UniversityEntities();

        public AddSubjects()
        {
            InitializeComponent();
        }

        private void AddSubjects_Load(object sender, EventArgs e)
        {
            fillDgwSubjects();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string subjectName = txtSubjectName.Text;
            if (subjectName != String.Empty)
            {
                if (db.Subjects.Any(sb => sb.Name == subjectName ))
                {
                    lblError.Text = "This subject exists";
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                    db.Subjects.Add(new Subject()
                    {
                        Name = subjectName,

                    });
                    db.SaveChanges();   
                    MessageBox.Show(subjectName + " was added to Subjects.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillDgwSubjects();
                    txtSubjectName.Text = string.Empty;
                }
            }
            else
            {
                lblError.Text = "Please, fill all the fields.";
                lblError.Visible = true;
            }
        }

        private void fillDgwSubjects()
        {
            dgwSubjects.DataSource = db.Subjects.Select(sb => new
            {
                Id = sb.Id,
                Name = sb.Name
            }).ToList();
            dgwSubjects.Columns[0].Visible = false;
        }

        private void dgwSubjects_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int SubjectId = (int)dgwSubjects.Rows[e.RowIndex].Cells[0].Value;
            selectedSubject = db.Subjects.Find(SubjectId);
            txtSubjectName.Text = selectedSubject.Name;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedSubject != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Subject?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    db.Subjects.Remove(selectedSubject);
                    db.SaveChanges();
                    txtSubjectName.Text = string.Empty;
                    fillDgwSubjects();
                    MessageBox.Show("Subject was deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddMode();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string subjectName = txtSubjectName.Text;   
            if (subjectName != String.Empty)
            {
                selectedSubject.Name = subjectName;
                if (db.Subjects.Any(sb => sb.Name == subjectName))
                {
                    lblError.Text = "This subject exists";
                    lblError.Visible = true;
                }
                else
                {
                    db.SaveChanges();
                    fillDgwSubjects();
                    MessageBox.Show(subjectName + " was updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblError.Visible = false;
                    AddMode();
                    txtSubjectName.Text = string.Empty;
                }
            }
            else
            {
                lblError.Text = "Please, fill all the fields.";
                lblError.Visible = true;
            }
        }
    }
}
