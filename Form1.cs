using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobertJohnsonEmployeesDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DatabaseConnection objConnect;
        string conString;

        DataSet ds;
        DataRow dRow;

        int MaxRows;
        int inc = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                objConnect = new DatabaseConnection();
                // This is set up in the project settings. It is the connection string for the DB file
                conString = Properties.Settings.Default.EmployeesConnectionString;

                //Set connection string for new DB connection object
                objConnect.connection_string = conString;
                // Loads the SQL statement saved to settings into object (Select *)
                objConnect.Sql = Properties.Settings.Default.SQL;
                // Handing dataset over from connection
                ds = objConnect.GetConnection;

                // Our dataset only has one table. This gets row count on that table
                MaxRows = ds.Tables[0].Rows.Count;

                // Call method to populate the form fields with data from DB
                NavigateRecords();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void NavigateRecords()
        {
            dRow = ds.Tables[0].Rows[inc];
            txtFirstName.Text = dRow.ItemArray.GetValue(1).ToString();
            txtLastName.Text = dRow.ItemArray.GetValue(2).ToString();
            txtJobTitle.Text = dRow.ItemArray.GetValue(3).ToString();
            txtDepartment.Text = dRow.ItemArray.GetValue(4).ToString();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if(inc != MaxRows -1)
            {
                inc++;
                NavigateRecords();
            }
            else
            {
                MessageBox.Show("No More Rows");
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (inc > 0)
            {
                inc--;
                NavigateRecords();
            }
            else
            {
                MessageBox.Show("First Record");
            }
        }

        private void BtnFirst_Click(object sender, EventArgs e)
        {
            if(inc != 0)
            {
                inc = 0;
                NavigateRecords();
            }
            else
            {
                MessageBox.Show("First Record");
            }
        }

        private void BtnLast_Click(object sender, EventArgs e)
        {
            if(inc != MaxRows - 1)
            {
                inc = MaxRows - 1;
                NavigateRecords();
            }
            else
            {
                MessageBox.Show("Last Record");
            }
        }
    }
}
