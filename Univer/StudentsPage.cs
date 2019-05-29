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
    public partial class StudentsPage : Form
    {
        UniversityEntities db = new UniversityEntities();
        Student activeStudent;
        Student editedStudent;
        int myScore;
        int? afterscore;

        public StudentsPage(Student stu)
        {
            InitializeComponent();
            activeStudent = stu;
        }

        private void StudentsPage_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = string.Format("{0} {1}, Welcome!", activeStudent.Firstname, activeStudent.Lastname);
            editedStudent = db.Students.Find(activeStudent.Id);
            lblExam.Visible = true;
            txtFirstName.Text = editedStudent.Firstname;
            txtLastName.Text = editedStudent.Lastname;
            txtEmail.Text = editedStudent.Email;
            txtPassword.Text = editedStudent.Password;
            txtRepeat.Text = editedStudent.Password;
            lblError.Visible = false;
            cmbSubjects.Items.Clear();
            cmbSubjects.Items.AddRange(db.TGS.Where(tg => tg.Group_Id == activeStudent.Group_Id).Select(tg => tg.Subject.Name).ToArray());000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
            myScore = (int)db.Scores.FirstOrDefault(sc => sc.Student_Id == activeStudent.Id).BeforeExamScore;
            afterscore = (int?)db.Scores.FirstOrDefault(sc => sc.Student_Id == activeStudent.Id).AfterExamScore;

            if ( myScore > 17 && afterscore == null)
            {
                lblError.Visible = false;
                lblExam.Text = "Your subjects are enough to go to exams. Your scores :" + " " + myScore;
                btnExam.Visible = true;
            }
             else {
                lblError.Visible = false;
                lblExam.Text = "Your scores are not enough to go to exams or you have previously passed this exam. Your scores :" + " " + myScore;
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string[] Inputs = new string[] { txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtPassword.Text, txtRepeat.Text };
            bool AllChecked = Extensions.IsNotEmpty(Inputs, string.Empty);

            if (AllChecked)
            {
                if(txtPassword.Text.Length >= 8)
                {
                    if(txtPassword.Text == txtRepeat.Text)
                    {
                        Student existEmail = db.Students.Where(st => st.Email == txtEmail.Text).FirstOrDefault();
                        if (existEmail == null || existEmail.Email == activeStudent.Email)
                        {
                            editedStudent.Firstname = txtFirstName.Text;
                            editedStudent.Lastname = txtLastName.Text;
                            editedStudent.Email = txtEmail.Text;
                            editedStudent.Password = txtPassword.Text;
                            editedStudent.Password = txtRepeat.Text;
                            db.SaveChanges();
                            MessageBox.Show("Your personal data was updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnExam_Click(object sender, EventArgs e)
        {
            goExam ge = new goExam(activeStudent);
            ge.ShowDialog();
        }
    }
}
