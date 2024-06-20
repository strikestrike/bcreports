using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace bcreports
{
    public partial class frmReport : Form
    {
        private ArrayList doctorIDs = new ArrayList();
        private string searchedDoctorName;
        private string startDate, endDate;
        private Dictionary<int, Dictionary<string, string>> doctorApptTypeIds = new Dictionary<int, Dictionary<string, string>>();
        private int[] periods = { -1, 0, 7, 14, 21, 30 };
        public frmReport()
        {
            InitializeComponent();
            LoadDoctorData();
            LoadAppTypeData();
            SetPeriodData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
            SaveAppointmentTypes();
        }

        private void SearchData()
        {
            string whereSql = "";

            if (cbPeriod.SelectedIndex <= 0)
            {
                whereSql = " Date >= '" + GetPastDays(dtStart.Value.Date) + "' ";
                whereSql += " AND Date <= '" + GetPastDays(dtEnd.Value.Date) + "' ";

                startDate = dtStart.Value.ToString("dd/MM/yyyy");
                endDate = dtEnd.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                DateTime end = DateTime.Now.AddDays(periods[cbPeriod.SelectedIndex]);
                whereSql = " Date >= '" + GetPastDays(DateTime.Now) + "' ";
                whereSql += " AND Date <= '" + GetPastDays(end) + "' ";

                startDate = DateTime.Now.ToString("dd/MM/yyyy");
                endDate = end.ToString("dd/MM/yyyy");
            }

            searchedDoctorName = "";

            if (cbDoctor.SelectedIndex > 0)
            {
                whereSql += " AND PractID = " + doctorIDs[cbDoctor.SelectedIndex - 1] + " ";
                searchedDoctorName = cbDoctor.Text;
            }

            string query = "SELECT * FROM BCPeAppt WHERE " + whereSql;

            List<Record> records = new List<Record>();

            using (SqlConnection connection = new SqlConnection(frmConnect.connectString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int doctorId = Convert.ToInt32(reader["PractID"]);
                                    int apptTypeId = Convert.ToInt32(reader["ApptTypeID"]);
                                    int appointmentDateCount = Convert.ToInt32(reader["Date"]);

                                    DateTime appointmentDate = new DateTime(1, 1, 1).AddDays(appointmentDateCount - 1);
                                    string appointmentDateFormatted = appointmentDate.ToString("dd/MM/yyyy");

                                    int appointmentTimeCount = Convert.ToInt32(reader["Time"]);
                                    string appointmentTimeFormatted = TimeSpan.FromSeconds(appointmentTimeCount).ToString(@"hh\:mm\:ss");

                                    string appointmentUnixFormatted = $"{appointmentDate:yyyy-MM-dd} {appointmentTimeFormatted}";
                                    long appointmentUnix = new DateTimeOffset(DateTime.Parse(appointmentUnixFormatted)).ToUnixTimeSeconds();

                                    string comment = reader["Comment"].ToString().Trim();

                                    // Search on Patient table
                                    string queryP = $"SELECT * FROM BCPePatient WHERE PatientId = '{reader["PatientID"]}'";
                                    using (SqlCommand commandP = new SqlCommand(queryP, connection))
                                    {
                                        using (SqlDataReader readerP = commandP.ExecuteReader())
                                        {
                                            if (readerP.HasRows)
                                            {
                                                readerP.Read();
                                                string firstName = readerP["Given"].ToString();
                                                string lastName = readerP["Surname"].ToString();
                                                string fullName = $"{firstName} {lastName}";

                                                int dobDateCount = Convert.ToInt32(readerP["DOB"]);
                                                DateTime dobDate = new DateTime(1, 1, 1).AddDays(dobDateCount - 1);
                                                string dob = dobDate.ToString("dd/MM/yyyy");

                                                string medicareCardNo = readerP["MedicareCardNo"].ToString();
                                                string medicareRef = readerP["MedicareRef"].ToString();
                                                string veteranCardNo = readerP["VeteranCardNo"].ToString();
                                                string address = readerP["Address1"].ToString();

                                                // Get Address Details
                                                string queryS = $"SELECT * FROM BCPeSuburb WHERE SuburbID = '{readerP["SuburbID"]}'";
                                                using (SqlCommand commandS = new SqlCommand(queryS, connection))
                                                {
                                                    using (SqlDataReader readerS = commandS.ExecuteReader())
                                                    {
                                                        if (readerS.HasRows)
                                                        {
                                                            readerS.Read();
                                                            string suburb = readerS["Name"].ToString();
                                                            string state = readerS["State"].ToString();
                                                            string postcode = readerS["Postcode"].ToString();

                                                            // Get type description
                                                            string typeDbVal = "N/A";
                                                            string columnName = $"ApptDesc{apptTypeId}";
                                                            string queryD = $"SELECT {columnName} FROM BCPePract WHERE PractID='{doctorId}'";
                                                            using (SqlCommand commandD = new SqlCommand(queryD, connection))
                                                            {
                                                                using (SqlDataReader readerD = commandD.ExecuteReader())
                                                                {
                                                                    if (readerD.HasRows)
                                                                    {
                                                                        readerD.Read();
                                                                        typeDbVal = readerD[columnName].ToString();
                                                                    }
                                                                }
                                                            }

                                                            // Search on type
                                                            bool addIt = true;
                                                            if (listAppointmentType.SelectedItems.Count > 0 && listAppointmentType.SelectedIndex != 0)
                                                            {
                                                                if (!listAppointmentType.SelectedItems.Contains(typeDbVal))
                                                                {
                                                                    addIt = false;
                                                                }
                                                            }

                                                            if (addIt)
                                                            {
                                                                records.Add(new Record
                                                                {
                                                                    PatientId = reader["PatientID"].ToString(),
                                                                    Date = appointmentDateFormatted,
                                                                    Time = appointmentTimeFormatted,
                                                                    Type = typeDbVal,
                                                                    Comment = comment,
                                                                    Name = fullName,
                                                                    DOB = dob,
                                                                    MedicareNo = medicareCardNo,
                                                                    Ref = medicareRef,
                                                                    DVA = veteranCardNo,
                                                                    Address = address,
                                                                    Suburb = suburb,
                                                                    State = state,
                                                                    Postcode = postcode,
                                                                });
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            dataGridView1.DataSource = records;
        }

        private void LoadDoctorData()
        {
            string query = "SELECT * FROM BCPePract";

            // Create a connection object
            using (SqlConnection connection = new SqlConnection(frmConnect.connectString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    cbDoctor.Items.Clear();
                    cbDoctor.Items.Add("Choose");
                    cbDoctor.SelectedIndex = 0;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        cbDoctor.Items.Add(row["Title"].ToString() + " " + row["Given"].ToString() + " " + row["Surname"].ToString());
                        doctorIDs.Add(Convert.ToInt32(row["PractID"]));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void LoadAppTypeData()
        {
            try
            {
                listAppointmentType.Items.Clear();
                listAppointmentType.Items.Add("Choose");
                if (Properties.Settings.Default.appointment_types == null)
                {
                    listAppointmentType.SelectedIndex = 0;
                }

                using (SqlConnection connection = new SqlConnection(frmConnect.connectString))
                {
                    connection.Open();

                    ArrayList practIDs = new ArrayList();
                    ArrayList apptTypeIDs = new ArrayList();

                    string query1 = "SELECT PractID, ApptTypeID FROM BCPeAppt";
                    using (SqlCommand command1 = new SqlCommand(query1, connection))
                    {
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                practIDs.Add(Convert.ToInt32(reader["PractID"]));
                                apptTypeIDs.Add(Convert.ToInt32(reader["ApptTypeID"]));

                            }
                        }
                    }

                    string query2 = "SELECT * FROM BCPePract WHERE PractID=@practID";
                    for (int i = 0; i < practIDs.Count; ++i)
                    {
                        int doctorID = Convert.ToInt32(practIDs[i]);
                        if (!doctorApptTypeIds.ContainsKey(doctorID))
                        {
                            doctorApptTypeIds[doctorID] = new Dictionary<string, string>();
                        }

                        using (SqlCommand command2 = new SqlCommand(query2, connection))
                        {
                            command2.Parameters.AddWithValue("@practID", doctorID);
                            using (SqlDataReader reader2 = command2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    string typeColName = "ApptDesc" + apptTypeIDs[i];
                                    string typeName = reader2[typeColName].ToString();
                                    if (!listAppointmentType.Items.Contains(typeName))
                                    {
                                        listAppointmentType.Items.Add(typeName);
                                    }
                                    doctorApptTypeIds[doctorID][typeColName] = typeName;
                                }
                            }
                        }
                    }

                    StringCollection appointmentTypes = Properties.Settings.Default.appointment_types;

                    if (appointmentTypes != null)
                    {
                        foreach (string item in appointmentTypes)
                        {
                            int idx = listAppointmentType.Items.IndexOf(item);
                            if (idx != -1)
                            {
                                listAppointmentType.SelectedIndices.Add(idx);
                            }
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SetPeriodData()
        {
            cbPeriod.Items.Clear();
            cbPeriod.Items.Add("Choose");
            cbPeriod.Items.Add("Today");
            cbPeriod.Items.Add("Next 7 days");
            cbPeriod.Items.Add("Next 14 days");
            cbPeriod.Items.Add("Next 21 days");
            cbPeriod.Items.Add("Next 30 days");
            cbPeriod.SelectedIndex = 5;
        }

        private int GetPastDays(DateTime date)
        {
            DateTime startDate = new DateTime(1, 1, 1);
            TimeSpan timePassed = date - startDate;
            return timePassed.Days;
        }

        private void btnSaveAsPDF_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select the folder where the report PDF file will be saved";
                    folderDialog.ShowNewFolderButton = true;
                    folderDialog.SelectedPath = Properties.Settings.Default.output_path;

                    DialogResult result = folderDialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                    {
                        string selectedFolderPath = folderDialog.SelectedPath;
                        Properties.Settings.Default.output_path = selectedFolderPath;
                        Properties.Settings.Default.Save();

                        using (PdfWriter writer = new PdfWriter(selectedFolderPath + "/report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"))
                        {
                            // Create PDF with landscape orientation
                            PdfDocument pdf = new PdfDocument(writer);
                            pdf.SetDefaultPageSize(PageSize.A4.Rotate()); // Landscape orientation

                            // Create document
                            Document document = new Document(pdf);
                            document.SetMargins(20, 20, 20, 20); // Set margins

                            // Add header information
                            float fontSize = 8f; // Adjust the font size as needed

                            // Add logo image from resources
                            Bitmap logoBitmap = bcreports.res.logo;
                            byte[] logoBytes;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                logoBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                logoBytes = ms.ToArray();
                            }
                            ImageData imageData = ImageDataFactory.Create(logoBytes);
                            iText.Layout.Element.Image logo = new iText.Layout.Element.Image(imageData).ScaleToFit(200, 60); // Adjust size as needed

                            // Create a table for the header
                            Table headerTable = new Table(2);
                            headerTable.SetWidth(UnitValue.CreatePercentValue(148));

                            // Create cell for the header text
                            Cell headerTextCell = new Cell();
                            headerTextCell.SetBorder(Border.NO_BORDER);

                            Paragraph header = new Paragraph("BLUECHIP APPOINTMENT REPORT")
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetFontSize(12)
                                .SetBold();
                            headerTextCell.Add(header);

                            Paragraph doctorName = new Paragraph("Doctor Name: " + searchedDoctorName)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetBold()
                                .SetFontSize(8);
                            headerTextCell.Add(doctorName);

                            Paragraph filters = new Paragraph("Filters: " + string.Join(", ", Properties.Settings.Default.appointment_types.Cast<string>().ToArray()))
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetBold()
                                .SetFontSize(8);
                            headerTextCell.Add(filters);

                            Paragraph dateRange = new Paragraph("Date Range: " + startDate  + " to " + endDate)
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetBold()
                                .SetFontSize(8);
                            headerTextCell.Add(dateRange);

                            headerTable.AddCell(headerTextCell);

                            // Create cell for the logo image
                            Cell logoCell = new Cell();
                            logoCell.SetBorder(Border.NO_BORDER);
                            logoCell.Add(logo).SetTextAlignment(TextAlignment.RIGHT);
                            headerTable.AddCell(logoCell);

                            // Add header table to document
                            document.Add(headerTable);

                            // Create table
                            Table table = new Table(dataGridView1.Columns.Count);

                            // Add table headers
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                table.AddHeaderCell(new Cell().Add(new Paragraph(column.HeaderText).SetFontSize(fontSize)));
                            }

                            // Add table rows
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        table.AddCell(new Cell().Add(new Paragraph(cell.Value?.ToString() ?? string.Empty).SetFontSize(fontSize)));
                                    }
                                }
                            }

                            // Set table width to fit page
                            table.SetWidth(UnitValue.CreatePercentValue(100));

                            // Add table to document
                            document.Add(table);
                            document.Close();
                        }

                        MessageBox.Show("PDF generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void SaveAppointmentTypes()
        {
            Properties.Settings.Default.appointment_types = new StringCollection();
            foreach (string item in listAppointmentType.SelectedItems)
            {
                Properties.Settings.Default.appointment_types.Add(item);
            }
            Properties.Settings.Default.Save();
        }


        public class Record
        {
            public string PatientId { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Type { get; set; }
            public string Comment { get; set; }
            public string Name { get; set; }
            public string DOB { get; set; }
            public string MedicareNo { get; set; }
            public string Ref { get; set; }
            public string DVA { get; set; }
            public string Address { get; set; }
            public string Suburb { get; set; }
            public string State { get; set; }
            public string Postcode { get; set; }
        }

        private void frmReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnSaveAsExcel_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the folder where the report Excel file will be saved";
                folderDialog.ShowNewFolderButton = true;
                folderDialog.SelectedPath = Properties.Settings.Default.output_path;

                DialogResult result = folderDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    string selectedFolderPath = folderDialog.SelectedPath;
                    Properties.Settings.Default.output_path = selectedFolderPath;
                    Properties.Settings.Default.Save();


                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sheet1");

                        // Add the headers
                        //for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        //{
                        //    worksheet.Cell(1, i + 1).Value = dataGridView1.Columns[i].HeaderText;
                        //}

                        // Add the data
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                worksheet.Cell(i + 2, j + 1).Value = dataGridView1.Rows[i].Cells[j].Value?.ToString();
                            }
                        }

                        // Save the workbook
                        workbook.SaveAs(selectedFolderPath + "/report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
                        MessageBox.Show("Excel generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }
    }
}