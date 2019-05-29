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
    public partial class HomePage : Form
    {
        UniversityEntities db = new UniversityEntities();

        public HomePage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (rbAdmin.Checked)
            {
                string email = txtEmail.Text;
                string password = txtPassword.Text;
                if (email != string.Empty && password != string.Empty)
                {
                    Admin adm = db.Admins.FirstOrDefault(a => a.Email == email);
                    if (adm != null)
                    {
                        if(adm.Password == password)
                        {
                            AdminPanel ap = new AdminPanel(adm);
                            ap.ShowDialog();
                            txtEmail.Text = string.Empty;
                            txtPassword.Text = string.Empty;
                            lblError.Visible = false;
                        }
                        else
                        {
                            lblError.Text = "Password is not correct";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Email is not correct";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Please fill all the fields";
                    lblError.Visible = true;
                }
            }
            else if (rbTeacher.Checked)
            {
                string email = txtEmail.Text;
                string password = txtPassword.Text;
                if (email != string.Empty && password != string.Empty)
                {
                    Teacher teach = db.Teachers.FirstOrDefault(t => t.Email == email);
                    if (teach != null)
                    {
                        if (teach.Password == password)
                        {
                            TeachersPanel techPan = new TeachersPanel(teach);
                            techPan.ShowDialog();
                            txtEmail.Text = string.Empty;
                            txtPassword.Text = string.Empty;
                            lblError.Visible = false;
                        }
                        else
                        {
                            lblError.Text = "Password is not correct";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Email is not correct";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Please fill all the fields";
                     lblError.Visible = true;
                }
            }
            else if (rbStudent.Checked)
            {
                string email = txtEmail.Text;
                string password = txtPassword.Text;
                if (email != string.Empty && password != string.Empty)
                {
                    Student stu = db.Students.FirstOrDefault(st => st.Email == email);
                    if (stu != null)
                    {
                        if (stu.Password == password)
                        {
                            StudentsPage stPa = new StudentsPage(stu);
                            stPa.ShowDialog();
                            txtEmail.Text = string.Empty;
                            txtPassword.Text = string.Empty;
                            lblError.Visible = false;
                        }
                        else
                        {
                            lblError.Text = "Password is not correct";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Email is not correct";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Please fill all the fields";
                    lblError.Visible = true;
                }
            }
        }
    }
}
