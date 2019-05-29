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
    public partial class AddStudents : Form
    {
        Student selectedStudent;
        UniversityEntities db = new UniversityEntities();

        public AddStudents()
        {
            InitializeComponent();
        }

        private void AddStudents_Load(object sender, EventArgs e)
        {
            fillDgwStudents();
            fillComboGroups();
        }

        private void fillComboGroups()
        {
            cmbGroups.Items.Clear();
            cmbGroups.Items.AddRange(db.Groups.Select(gp =>
            gp.Name).ToArray());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string identification = txtIdentification.Text;
            string group = cmbGroups.Text;
            DateTime birthDay = dtpBirthDay.Value;
            string[] Inputs = new string[] { firstName, lastName, email, password, identification,group};
            bool AllChecked = Extensions.IsNotEmpty(Inputs, string.Empty);
            if (AllChecked)
            {
                lblError.Visible = false;
                if (password.Length >= 8)
                {
                    Student existIdentification = db.Students.FirstOrDefault(st => st.Identification == identification);
                    if (existIdentification == null)
                    {
                        Student existPassword = db.Students.FirstOrDefault(st => st.Password == password);
                        if (existPassword == null)
                        {
                            Student existEmail = db.Students.FirstOrDefault(st => st.Email == email);
                            if (existEmail == null)
                            {
                                lblError.Visible = false;
                                db.Students.Add(new Student()
                                {
                                    Firstname = firstName,
                                    Lastname = lastName,
                                    Email = email,
                                    Group_Id = db.Groups.First(gp => gp.Name == group).Id,

                                    Password = password,
                                    Identification = identification,
                                    Birthdate = birthDay
                                });
                                db.SaveChanges();
                                MessageBox.Show(firstName + " " + lastName + " was added to Students.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                fillDgwStudents();
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
            cmbGroups.Visible = false;
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
            cmbGroups.Visible = true;
            label8.Visible = true;
            txtIdentification.Visible = true;
        }

        private void fillDgwStudents()
        {
            dgwStudents.DataSource = db.Students.Select(st => new
            {
                Id = st.Id,
                Firstname = st.Firstname,
                Lastname = st.Lastname,
                Email = st.Email,
                Password = st.Password,
                Group = st.Group.Name,
                Identification = st.Identification,
                Birthday = st.Birthdate
            }).ToList();

            dgwStudents.Columns[0].Visible = false;
            dgwStudents.Columns[4].Visible = false;
        }

        private void clearAll()
        {
            foreach (Control ct in this.Controls)
            {
                if (ct is TextBox || ct is ComboBox)
                {
                    ct.Text = "";
                }
                else if (ct is NumericUpDown)
                {
                    var ctN = (NumericUpDown)ct;
                    ctN.Value = 0;
                }
            }
        }

        private void dgwStudents_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int studentId = (int)dgwStudents.Rows[e.RowIndex].Cells[0].Value;
            selectedStudent = db.Students.Find(studentId);
            txtFirstName.Text = selectedStudent.Firstname;
            txtLastName.Text = selectedStudent.Lastname;
            txtEmail.Text = selectedStudent.Email;
            txtIdentification.Text = selectedStudent.Identification;
            txtPassword.Text = selectedStudent.Password;
            editMode();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Student?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    db.Students.Remove(selectedStudent);
                    db.SaveChanges();

                    fillDgwStudents();
                    MessageBox.Show("Student was deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearAll();
                    AddMode();
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
                selectedStudent.Firstname = firstname;
                selectedStudent.Lastname = lastname;
                selectedStudent.Birthdate = birthDay;
                db.SaveChanges();
                lblError2.Visible = false;
                MessageBox.Show("Student was edited.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddMode();
                clearAll();
                fillDgwStudents();
            }
            else
            {
                lblError2.Text = "Fill in at least one parameter that you would like to change.";
                lblError2.Visible = true;
            }
        }
    }
}
