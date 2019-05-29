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
    public partial class AddTeachers : Form
    {
        UniversityEntities db = new UniversityEntities();
        Teacher selectedTeacher;

        public AddTeachers()
        {
            InitializeComponent();
        }

        private void AddTeachers_Load(object sender, EventArgs e)
        {
            fillDgwTeachers();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string identification = txtIdentification.Text;
            DateTime birthDay = dtpBirthDay.Value;
            string[] Inputs = new string[] {firstName, lastName,email,password,identification };
            bool AllChecked = Extensions.IsNotEmpty(Inputs, string.Empty);

            if (AllChecked)
            {
                lblError.Visible = false;
                if (password.Length >= 8)
                {
                    Teacher existIdentification = db.Teachers.FirstOrDefault(t => t.Identification == identification);
                    if(existIdentification == null)
                    {
                        Teacher existPassword = db.Teachers.FirstOrDefault(t => t.Password == password);
                        if (existPassword==null)
                        {
                            Teacher existEmail = db.Teachers.FirstOrDefault(t => t.Email == email);
                            if (existEmail==null) 
                            {
                                lblError.Visible = false;
                                db.Teachers.Add(new Teacher()
                                {
                                    Firstname = firstName,
                                    Lastname = lastName,
                                    Email = email,
                                    Password = password,
                                    Identification = identification,
                                    Birthdate = birthDay
                                });
                                db.SaveChanges();
                                MessageBox.Show(firstName + " " + lastName + " was added to Teachers.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                fillDgwTeachers();
                                clearAll();
                            }
                            else
                            {
                                lblError.Text = "Email must be unique";
                                lblError.Visible = true;
                            }
                        }
                        else
                        {
                            lblError.Text = "Password must be unique";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "The identification code must be unique";
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
                lblError.Text = "Please, fill all the fields.";
                lblError.Visible = true;
            }
        }

        private void fillDgwTeachers()
        {
            dgwTeachers.DataSource = db.Teachers.Select(tr => new
            {
                Id = tr.Id,
                Firstname = tr.Firstname,
                Lastname = tr.Lastname,
                Email = tr.Email,
                Password = tr.Password,
                Identification = tr.Identification,
                Birthday = tr.Birthdate
            }).ToList();
            dgwTeachers.Columns[0].Visible = false;
            dgwTeachers.Columns[4].Visible = false;
        }

        private void editMode()
        {
            btnAdd.Visible = false;
            btnDelete.Visible = false;
            btnEdit.Visible = true;
            label6.Visible = false;
            txtEmail.Visible = false;
            label7.Visible = false;
            txtPassword.Visible = false;
            label1.Visible = false;
            label8.Visible = false;
            txtIdentification.Visible = false;
        }

        private void AddMode()
        {
            btnAdd.Visible = true;
            btnDelete.Visible = false;
            btnEdit.Visible = false;
            label6.Visible = true;
            txtEmail.Visible = true;
            label7.Visible = true;
            txtPassword.Visible = true;
            label1.Visible = true;
            label8.Visible = true;
            txtIdentification.Visible = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedTeacher != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Teacher?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    db.Teachers.Remove(selectedTeacher);
                    db.SaveChanges();
                    fillDgwTeachers();
                    MessageBox.Show("Teacher was deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearAll();
                    AddMode();
                }
            }
        }

           private void clearAll()
        {
            foreach(Control ct in this.Controls)
            {
                if(ct is TextBox ||  ct is ComboBox)
                {
                    ct.Text = "";
                } else if(ct is NumericUpDown)
                {
                    var ctN = (NumericUpDown)ct;
                    ctN.Value = 0;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string firstname = txtFirstName.Text;
            string lastname = txtLastName.Text;
            DateTime birthDay = dtpBirthDay.Value;
            string[] Inputs = new string[] { firstname, lastname };
            bool AllChecked = Extensions.IsNotEmpty(Inputs, string.Empty);
            if (AllChecked)
            {
                selectedTeacher.Firstname = firstname;
                selectedTeacher.Lastname = lastname;
                selectedTeacher.Birthdate = birthDay;
                db.SaveChanges();
                lblError2.Visible = false;
                MessageBox.Show("Teacher was edited.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fillDgwTeachers();
                clearAll();
                AddMode();
            }
            else
            {
                lblError2.Text = "Fill in at least one parameter that you would like to change.";
                lblError2.Visible = true;
            }
        }

        private void dgwTeachers_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int teacherId = (int)dgwTeachers.Rows[e.RowIndex].Cells[0].Value;
            selectedTeacher = db.Teachers.Find(teacherId);
            txtFirstName.Text = selectedTeacher.Firstname;
            txtLastName.Text = selectedTeacher.Lastname;
            txtEmail.Text = selectedTeacher.Email;
            txtIdentification.Text = selectedTeacher.Identification;
            txtPassword.Text = selectedTeacher.Password;
            editMode();
        }
    }
}
