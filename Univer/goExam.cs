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
    public partial class goExam : Form
    {
        RadioButton selectedButton = new RadioButton();
        Student activeStudent;
        UniversityEntities db = new UniversityEntities();
        List<Exam> allQuestions = new List<Exam>();
        int subjectId;
        List<string> answers = new List<string>();
        int i = 0;

        public goExam(Student stu)
        {
            InitializeComponent();
            activeStudent = stu;
            subjectId = (int)db.TGS.Where(t => t.Group_Id == activeStudent.Group_Id).FirstOrDefault().Subject_Id;
            allQuestions = db.Exams.Where(e => e.Subject_Id == subjectId).ToList();

            for(var j = 0; j < 10; j++)
            {
                answers.Insert(j, null);
            }
        }

        private void goExam_Load(object sender, EventArgs e)
        {
            lblQuest.Text = allQuestions[i].Question;
            rA.Text = allQuestions[i].A;
            rB.Text = allQuestions[i].B;
            rC.Text = allQuestions[i].C;
            rD.Text = allQuestions[i].D;
            rE.Text = allQuestions[i].E;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            selectedButton = this.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
            string buttonValue = selectedButton?.Text;
            answers[i] = buttonValue;
            if (selectedButton!= null)
            {
                selectedButton.Checked = false;
            }
            btnPrev.Visible = true;
            i++;
            lblQuest.Text = allQuestions[i].Question;
            rA.Text = allQuestions[i].A;
            rB.Text = allQuestions[i].B;
            rC.Text = allQuestions[i].C;
            rD.Text = allQuestions[i].D;
            rE.Text = allQuestions[i].E;
            
            if(i == allQuestions.Count-1)
            {
                btnNext.Visible = false;
                btnFinish.Visible = true;
            }
            if (answers[i] != null)
            {
                ButonSelector(answers[i]);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            selectedButton = this.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
            string buttonValue = selectedButton?.Text;
            answers[i] = buttonValue;
            if (selectedButton != null)
            {
                selectedButton.Checked = false;
            }
            btnPrev.Visible = true;
            if (i > 0)
            {
                i--;
                btnNext.Visible = true;
                lblQuest.Text = allQuestions[i].Question;
                rA.Text = allQuestions[i].A;
                rB.Text = allQuestions[i].B;
                rC.Text = allQuestions[i].C;
                rD.Text = allQuestions[i].D;
                rE.Text = allQuestions[i].E;
            }
            else
            {
                btnPrev.Visible = false;
            }
            if (answers[i] != null)
            {
                ButonSelector(answers[i]);
            }
        }

        private void ButonSelector(string selectedAnswer)
        {
            foreach (var  rb in this.Controls.OfType<RadioButton>())
            {
                if (rb.Text == selectedAnswer)
                {
                    rb.Checked = true;
                }
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            selectedButton = this.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
            string buttonValue = selectedButton?.Text;
            answers[i] = buttonValue;
            int total = 0;
            for (var i = 0; i < answers.Count; i++)
            {
                if (answers[i] == allQuestions[i].A)
                {
                    total += 5;
                }
            }
            Score result = db.Scores.First(sc => sc.Student_Id == activeStudent.Id && sc.Subject.Id == subjectId);
            result.AfterExamScore = total;
            db.SaveChanges();
            MessageBox.Show("Exam succesfully completed !");
            this.Close();
        }
    }
}
