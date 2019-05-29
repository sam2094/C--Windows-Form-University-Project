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
    public partial class AdminPanel : Form
    {
        UniversityEntities db = new UniversityEntities();
        Admin activeAdmin;
        Admin editedAdmin;

        public AdminPanel( Admin adm)
        {
            InitializeComponent();
            activeAdmin = adm;
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            editedAdmin = db.Admins.Find(activeAdmin.Id);
            txtEmail.Text = editedAdmin.Email;
            txtPassword.Text = editedAdmin.Password;
            txtRepeat.Text = editedAdmin.Password;
        }

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            AddSubjects adSub = new AddSubjects();
            adSub.ShowDialog();
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            AddGroups adGp = new AddGroups();
            adGp.ShowDialog();
        }

        private void btnAddTeacher_Click(object sender, EventArgs e)
        {
            AddTeachers adTe = new AddTeachers();
            adTe.ShowDialog();
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            AddStudents adSt = new AddStudents();
            adSt.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TeacherEdit tEd = new TeacherEdit();
            tEd.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnEdit.Visible = true;
            txtEmail.Visible = true;
            txtPassword.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            button2.Visible = false;
            label7.Visible = false;
            label10.Visible = true;
            txtRepeat.Visible = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editedAdmin.Email = txtEmail.Text;
            editedAdmin.Password = txtPassword.Text;
            editedAdmin.Password = txtPassword.Text;

            if(txtPassword.Text.Length >= 8)
            {
                if(txtRepeat.Text == txtPassword.Text)
                {
                    editedAdmin.Email = txtEmail.Text;
                    editedAdmin.Password = txtPassword.Text;
                    editedAdmin.Password = txtRepeat.Text;
                    db.SaveChanges();
                    MessageBox.Show("Your personal data was updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblError.Visible = false;
                    txtEmail.Visible = false;
                    txtPassword.Visible = false;
                    txtRepeat.Visible = false;
                    btnEdit.Visible = false;
                    label10.Visible = false;
                    label9.Visible = false;
                    label8.Visible = false;
                    button2.Visible = true;
                    label7.Visible = true;
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
    }
}
