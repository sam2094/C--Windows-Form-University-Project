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
    public partial class TeacherEdit : Form
    {
        UniversityEntities db = new UniversityEntities();

        public TeacherEdit()
        {
            InitializeComponent();
        }

        private void TeacherEdit_Load(object sender, EventArgs e)
        {
                fillDgwTgs();
                fillComboTeachers();
                fillComboSubjects();
                fillComboGroups();
        }

        private void fillComboTeachers()
        {
            cmbTeachers.Items.Clear();
            cmbTeachers.Items.AddRange(db.Teachers.Select(t =>
            t.Firstname + " " + t.Lastname).ToArray());
        }

        private void fillComboSubjects()
        {
            cmbSubjects.Items.Clear();
            cmbSubjects.Items.AddRange(db.Subjects.Select(sb =>
            sb.Name).ToArray());
        }

        private void fillComboGroups()
        {
            cmbGroups.Items.Clear();
            cmbGroups.Items.AddRange(db.Groups.Select(gp =>
            gp.Name).ToArray());
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            string teacher = cmbTeachers.Text;
            string group = cmbGroups.Text;
            string subject = cmbSubjects.Text;

            if(teacher != string.Empty && subject != string.Empty && group != string.Empty)
            {
                db.TGS.Add(new TG()
                {
                    Teacher_Id = db.Teachers.First(t => t.Firstname + " " + t.Lastname == teacher).Id,
                    Group_Id = db.Groups.First(gp => gp.Name == group).Id,
                    Subject_Id = db.Subjects.First(sb => sb.Name == subject).Id,
                });
                db.SaveChanges();
                MessageBox.Show("This group" + " " + group + " "+ "and this subject" + " " + subject + " was added to Teacher.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fillDgwTgs();
            }
            else
            {
                lblError.Text = "Please,fill all the fields ";
                lblError.Visible = true;
            }
        }

        private void fillDgwTgs()
        {
            dgwTgs.DataSource = db.TGS.Select(tg => new {
                Id = tg.Id,
                Teacher = tg.Teacher.Firstname + " " + tg.Teacher.Lastname,
                Groups = tg.Group.Name,
                Subjects = tg.Subject.Name,
            }).ToList();
           dgwTgs.Columns[0].Visible = false;
        }
    }
}
