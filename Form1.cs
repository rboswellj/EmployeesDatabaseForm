using System;
using System.Data;
using System.Windows.Forms;

/*  Robert Johnson
    CIT 243
    Employee Database Project
*/

namespace RobertJohnsonEmployeesDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // DB connection object and connection string
        DatabaseConnection objConnect;
        string conString;

        // Dataset
        DataSet ds;
        DataRow dRow;

        // Increment variable and last row index
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
                objConnect.Connection_string = conString;
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
            // Method to load values from current row of DB into text boxes
            // Also displays current Record number within form
            dRow = ds.Tables[0].Rows[inc];
            txtFirstName.Text = dRow.ItemArray.GetValue(1).ToString();
            txtLastName.Text = dRow.ItemArray.GetValue(2).ToString();
            txtJobTitle.Text = dRow.ItemArray.GetValue(3).ToString();
            txtDepartment.Text = dRow.ItemArray.GetValue(4).ToString();
            txtRecordNum.Text = $"Record {inc + 1} of {MaxRows}";
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            // If not yet at final index, go to next record
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
            // If not at first index, goes to previous record
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
            // If not already viewing first record, go to first
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
            // If not already at last record, go to last
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

        private void BtnAddNew_Click(object sender, EventArgs e)
        {
            // Clears all text boxes and enables the save and cancel buttons
            // within the add new record group
            txtFirstName.Clear();
            txtLastName.Clear();
            txtJobTitle.Clear();
            txtDepartment.Clear();
            btnAddNew.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Cancels the add new record process
            NavigateRecords();
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            btnAddNew.Enabled = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // saves new record
            if(txtFirstName.Text != String.Empty && txtLastName.Text != String.Empty)
            {
                DataRow row = ds.Tables[0].NewRow();
                row[1] = txtFirstName.Text;
                row[2] = txtLastName.Text;
                row[3] = txtJobTitle.Text;
                row[4] = txtDepartment.Text;

                ds.Tables[0].Rows.Add(row);
                try
                {
                    objConnect.UpdateDatabase(ds);
                    MaxRows++;
                    inc = MaxRows - 1;
                    NavigateRecords();
                    MessageBox.Show("Database Updated");
                    
                } 
                catch(Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                btnCancel.Enabled = false;
                btnSave.Enabled = false;
                btnAddNew.Enabled = true;

            }
            else
            {
                MessageBox.Show("Please enter a first and last name");
            }
            
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].Rows[inc];

            row[1] = txtFirstName.Text;
            row[2] = txtLastName.Text;
            row[3] = txtJobTitle.Text;
            row[4] = txtDepartment.Text;

            try
            {
                objConnect.UpdateDatabase(ds);
                MessageBox.Show("Record Updated");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds.Tables[0].Rows[inc].Delete();
                objConnect.UpdateDatabase(ds);

                MaxRows = ds.Tables[0].Rows.Count;
                inc--;
                NavigateRecords();

                MessageBox.Show("Record Deleted");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


    }
}
