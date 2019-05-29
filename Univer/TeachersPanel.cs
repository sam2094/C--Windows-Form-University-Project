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
    public partial class TeachersPanel : Form
    {
        Score myScore= new Score();
        UniversityEntities db = new UniversityEntities();
        Teacher activeTeacher;
        Teacher editedTeacher;

        public TeachersPanel(Teacher teach)
        {
            InitializeComponent();
            activeTeacher = teach;
        }

        private void TeachersPanel_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = string.Format("{0} {1}, Welcome!", activeTeacher.Firstname, activeTeacher.Lastname);
            fillComboGroups();
            editedTeacher = db.Teachers.Find(activeTeacher.Id);
            txtFirstName.Text = editedTeacher.Firstname;
            txtLastName.Text = editedTeacher.Lastname;
            txtEmail.Text = editedTeacher.Email;
            txtPassword.Text = editedTeacher.Password;
            txtRepeat.Text = editedTeacher.Password;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string[] Inputs = new string[] { txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtPassword.Text, txtRepeat.Text};
            bool AllChecked = Extensions.IsNotEmpty(Inputs, string.Empty);
            if (AllChecked)
            {
                if (txtPassword.Text.Length >= 8)
                {
                    if (txtRepeat.Text == txtPassword.Text)
                    {
                        Teacher existEmail = db.Teachers.Where(t => t.Email == txtEmail.Text).FirstOrDefault();
                        if (existEmail == null || existEmail.Email == activeTeacher.Email)
                        {
                            editedTeacher.Firstname = txtFirstName.Text;
                            editedTeacher.Lastname = txtLastName.Text;
                            editedTeacher.Email = txtEmail.Text;
                            editedTeacher.Password = txtPassword.Text;
                            editedTeacher.Password = txtRepeat.Text;
                            db.SaveChanges();
                            MessageBox.Show("Your personal data was updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgwTeacher.DataSource = db.Teachers.Where(t => t.Id == activeTeacher.Id).ToList();
                            lblError.Visible = false;
                            txtFirstName.Visible = false;
                            txtLastName.Visible = false;
                            txtEmail.Visible = false;
                            txtPassword.Visible = false;
                            txtRepeat.Visible = false;
                            btnEdit.Visible = false;
                            label1.Visible = false;
                            label5.Visible = false;
                            label6.Visible = false;
                            label7.Visible = false;
                            label3.Visible = false;
                            button1.Visible = true;
                            label4.Visible = true;
                        }
                        else 
                        {
                            lblError.Text = "This email has been used before.";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Password and repeat password do not match.";
                         lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Password must contain at least 8 characters.";
                     lblError.Visible = true;
                }
            }
            else
            {
                 lblError.Text = "Fill in at least one parameter that you would like to change.";
                 lblError.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtFirstName.Visible = true;
            txtLastName.Visible = true;
            txtEmail.Visible = true;
            txtPassword.Visible = true;
            txtRepeat.Visible = true;
            btnEdit.Visible = true;
            label1.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label3.Visible = true;
            button1.Visible = false;
            label4.Visible = false;
        }

        private void fillComboGroups()
        {
            cmbGroups.Items.AddRange(db.TGS.Where(tgs => tgs.Teacher_Id == activeTeacher.Id).Select(tgs => tgs.Group.Name).ToArray());
        }

        private void fillComboStudents()
        {
            string groupName = cmbGroups.Text;
            cmbStudents.Items.Clear();
            cmbStudents.Items.AddRange(db.Students.Where(st => st.Group.Name == groupName).Select(st => st.Firstname + " "+ st.Lastname).ToArray());
        }

        private void cmbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillComboStudents();
        }

        private void btnScore_Click(object sender, EventArgs e)
        {
            int score = (int)nmScore.Value;
            string group = cmbGroups.Text;
            string student = cmbStudents.Text;
            myScore.Student_Id = db.Students.First(st => st.Firstname + " " + st.Lastname == student).Id;
            myScore.Subject_Id = db.TGS.First(t => t.Teacher_Id == activeTeacher.Id).Subject_Id;
            myScore.BeforeExamScore = score;
            if (!db.Scores.Any(sc => sc.Subject_Id == myScore.Subject_Id && sc.Student_Id == myScore.Student_Id))
            {
                db.Scores.Add(myScore);
                db.SaveChanges();
                MessageBox.Show("Scores was added to Student.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblError.Visible = false;
            }
            else
            {
                lblError.Text = "You have already assigned a score to this student.";
                lblError.Visible = true;
            }
        }
    }
}
