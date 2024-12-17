using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class MainFormsUser : Form
    {
        enum RowState
        {
            Existed,
            New,
            Modified,
            ModifiedNew,
            Delete
        }
        int selectRow;
        public MainFormsUser()
        {
            InitializeComponent();
        }


        private void ReadSingleRow(DataGridView dgv, IDataRecord record, RowState state)
        {
            var values = new object[record.FieldCount + 1];
            record.GetValues(values);
            values[^1] = state;
            dgv.Rows.Add(values);
        }

        private void RefreshDataGrid(DataGridView dgv, string query)
        {
            dgv.Rows.Clear();
            DB.sqlCommand.CommandText = query;
            using (var reader = DB.sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    ReadSingleRow(dgv, reader, RowState.Existed);
                }
            }
        }

        private void CreateColumns(DataGridView dgv, params (string name, string header)[] columns)
        {
            dgv.Columns.Clear();

            foreach (var (name, header) in columns)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Name = name;
                column.HeaderText = header;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns.Add(column);
            }

        }
        private void MainFormsUser_Load(object sender, EventArgs e)
        {

            Btn_sort_pr.Click += Btn_sort_Click;
            Btn_sort_proc.Click += Btn_sort_Click;
            Btn_sort_type.Click += Btn_sort_Click;
            Btn_sort_prodyk.Click += Btn_sort_Click;
            Btn_sort_obs.Click += Btn_sort_Click;
            Btn_sort_log.Click += Btn_sort_Click;

            CreateColumnsForEquipment();
            RefreshDataGrid(dataGried_BD, "SELECT EquipmentID, EquipmentName, AreaID, EquipmentType, Status, Manufacturer FROM Equipment");

            CreateColumnsForAreas();
            RefreshDataGrid(dataGridView_area, "SELECT AreaID, AreaName, Description, FloorNumber, AreaCode FROM ProductionAreas");

            CreateColumnsForProcess();
            RefreshDataGrid(dataGridView_peocess, "SELECT ProcessID, ProcessName, EquipmentID, AreaID, ProcessStatus, DurationMinutes FROM Processes");

            CreateColumnsForTypeProd();
            RefreshDataGrid(dataGridView_type_prod, "SELECT ProductTypeID, ProductTypeName, Description, Category, UnitMeasure FROM ProductTypes");

            CreateColumnsForProduct();
            RefreshDataGrid(dataGridView_product, "SELECT ProductID, ProductTypeID, ProcessID, ProductStatus, BatchNumber, ProductionDate, ExpirationDate FROM Products");

            CreateColumnsForMaintenance();
            RefreshDataGrid(dataGridView_Maintenance, "SELECT MaintenanceID, EquipmentID, MaintenanceDate, MaintenanceDetails, MaintenanceType, MaintenanceCost FROM Maintenance");

            CreateColumnsForLogo();
            RefreshDataGrid(dataGridView_Logo, "SELECT LogID, EventTimestamp, EquipmentID, EventDescription, Severity, OperatorName, Comments FROM Logs");
        }

        //----------------------------------------------------------------------------------------------
        //Создание колонок
        private void CreateColumnsForEquipment()
        {
            CreateColumns(dataGried_BD,
                ("EquipmentID", "ID оборудования"),
                ("EquipmentName", "Название оборудования"),
                ("AreaID", "ID участка"),
                ("EquipmentType", "Тип оборудования"),
                ("Status", "Статус"),
                ("Manufacturer", "Производитель"));
        }

        private void CreateColumnsForAreas()
        {
            CreateColumns(dataGridView_area,
                ("AreaID", "ID участка"),
                ("AreaName", "Название участка"),
                ("Description", "Описание"),
                ("FloorNumber", "Номер этажа"),
                ("AreaCode", "Код области"));
        }

        private void CreateColumnsForProcess()
        {
            CreateColumns(dataGridView_peocess,
                ("ProcessID", "ProcessID"),
                ("ProcessName", "Название процесса"),
                ("EquipmentID", "EquipmentID"),
                ("AreaID", "AreaID"),
                ("ProcessStatus", "Состояние процесса"),
                ("DurationMinutes", "Продолжительность - минуты"));
        }

        private void CreateColumnsForTypeProd()
        {
            CreateColumns(dataGridView_type_prod,
                ("ProductTypeID", "ProductTypeID"),
                ("ProductTypeName", "Название продукта"),
                ("Description", "Описание"),
                ("Category", "Категория"),
                ("UnitMeasure", "Единица измерения"));
        }

        private void CreateColumnsForProduct()
        {
            CreateColumns(dataGridView_product,
                ("ProductID", "ProductID"),
                ("ProductTypeID", "ProductTypeID"),
                ("ProcessID", "ProcessID"),
                ("ProductStatus", "Статус продукта"),
                ("BatchNumber", "Номер партии"),
                ("ProductionDate", "Дата производства"),
                ("ExpirationDate", "Дата истечения срока действия"));
        }

        private void CreateColumnsForMaintenance()
        {
            CreateColumns(dataGridView_Maintenance,
                ("MaintenanceID", "MaintenanceID"),
                ("EquipmentID", "EquipmentID"),
                ("MaintenanceDate", "Срок технического обслуживания"),
                ("MaintenanceDetails", "Детали технического обслуживания"),
                ("MaintenanceType", "Тип обслуживания"),
                ("MaintenanceCost", "Затраты на техническое обслуживание"));
        }

        private void CreateColumnsForLogo()
        {
            CreateColumns(dataGridView_Logo,
                ("LogID", "LogID"),
                ("EventTimestamp", "Временная метка события"),
                ("EquipmentID", "EquipmentID"),
                ("EventDescription", "Описание события"),
                ("Severity", "Строгость"),
                ("OperatorName", "Имя оператора"),
                ("Comments", "Коментарий"));
        }


        private void SortAndFilterData(string tableName, ComboBox comboBox, TextBox textBox, DataGridView dataGridView)
        {
            if (comboBox.SelectedItem == null || string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
            {
                MessageBox.Show("Выберите поле для сортировки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string columnName = comboBox.SelectedItem.ToString();
            string filterValue = textBox.Text.Trim();
            if (!IsColumnNameValid(columnName))
            {
                MessageBox.Show("Недопустимое имя столбца.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query;

            if (string.IsNullOrEmpty(filterValue))
            {
                query = $"SELECT * FROM {tableName} ORDER BY [{columnName}]";
            }
            else
            {
                query = $"SELECT * FROM {tableName} WHERE [{columnName}] LIKE @filterValue ORDER BY [{columnName}]";
            }

            try
            {
                dataGridView.Rows.Clear();
                DB.sqlCommand.CommandText = query;
                DB.sqlCommand.Parameters.Clear();

                if (!string.IsNullOrEmpty(filterValue))
                {
                    DB.sqlCommand.Parameters.AddWithValue("@filterValue", $"%{filterValue}%");
                }

                using (var reader = DB.sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReadSingleRow(dataGridView, reader, RowState.Existed);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки и фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DB.sqlCommand.Parameters.Clear();
            }
        }

        private bool IsColumnNameValid(string columnName)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(columnName, "^[a-zA-Z0-9_]+$");
        }
        private void LoadColumnNamesForCurrentTab(string tableName, ComboBox comboBox)
        {
            comboBox.Items.Clear();

            switch (tableName)
            {
                case "Equipment":
                    comboBox.Items.AddRange(new[] { "EquipmentID", "EquipmentName", "AreaID", "EquipmentType", "Status", "Manufacturer" });
                    break;
                case "ProductionAreas":
                    comboBox.Items.AddRange(new[] { "AreaID", "AreaName", "Description", "FloorNumber", "AreaCode" });
                    break;
                case "Processes":
                    comboBox.Items.AddRange(new[] { "ProcessID", "ProcessName", "EquipmentID", "AreaID", "ProcessStatus", "DurationMinutes" });
                    break;
                case "ProductTypes":
                    comboBox.Items.AddRange(new[] { "ProductTypeID", "ProductTypeName", "Description", "Category", "UnitMeasure" });
                    break;
                case "Products":
                    comboBox.Items.AddRange(new[] { "ProductID", "ProductTypeID", "ProcessID", "ProductStatus", "BatchNumber", "ProductionDate", "ExpirationDate" });
                    break;
                case "Maintenance":
                    comboBox.Items.AddRange(new[] { "MaintenanceID", "EquipmentID", "MaintenanceDate", "MaintenanceDetails", "MaintenanceType", "MaintenanceCost" });
                    break;
                case "Logs":
                    comboBox.Items.AddRange(new[] { "LogID", "EventTimestamp", "EquipmentID", "EventDescription", "Severity", "OperatorName", "Comments" });
                    break;
                default:
                    MessageBox.Show("Unknown table name");
                    break;
            }
        }
        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                LoadColumnNamesForCurrentTab("Equipment", comboBox1);
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                LoadColumnNamesForCurrentTab("ProductionAreas", comboBox2);
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                LoadColumnNamesForCurrentTab("Processes", comboBox3);
            }
            else if (tabControl1.SelectedTab == tabPage4)
            {
                LoadColumnNamesForCurrentTab("ProductTypes", comboBox4);
            }
            else if (tabControl1.SelectedTab == tabPage5)
            {
                LoadColumnNamesForCurrentTab("Products", comboBox5);
            }
            else if (tabControl1.SelectedTab == tabPage6)
            {
                LoadColumnNamesForCurrentTab("Maintenance", comboBox6);
            }
            else if (tabControl1.SelectedTab == tabPage7)
            {
                LoadColumnNamesForCurrentTab("Logs", comboBox7);
            }
        }
       

        //----------------------------------------------------------------------------------------------
        //Вывод в TextBox значений из DataGridView

        private void dataGried_BD_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGried_BD.Rows[e.RowIndex];
                Txt_equ_id.Text = row.Cells["EquipmentID"].Value.ToString();
                Txt_name_equ.Text = row.Cells["EquipmentName"].Value.ToString();
                Txt_aria_ID.Text = row.Cells["AreaID"].Value.ToString();
                Txt_type_eque.Text = row.Cells["EquipmentType"].Value.ToString();
                Txt_status_equ.Text = row.Cells["Status"].Value.ToString();
                Txt_manuf.Text = row.Cells["Manufacturer"].Value.ToString();
            }
        }

        private void dataGridView_area_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_area.Rows[e.RowIndex];
                Txt_Id_aria.Text = row.Cells["AreaID"].Value.ToString();
                Txt_areaName.Text = row.Cells["AreaName"].Value.ToString();
                Txt_discrio.Text = row.Cells["Description"].Value.ToString();
                Txt_flowar.Text = row.Cells["FloorNumber"].Value.ToString();
                Txt_cod_ar.Text = row.Cells["AreaCode"].Value.ToString();
            }
        }

        private void dataGridView_peocess_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_peocess.Rows[e.RowIndex];
                Txt_proc_ID.Text = row.Cells["ProcessID"].Value.ToString();
                Txt_name_proc.Text = row.Cells["ProcessName"].Value.ToString();
                Txt_proccs_id.Text = row.Cells["EquipmentID"].Value.ToString();
                Txt_area_ids.Text = row.Cells["AreaID"].Value.ToString();
                Txt_sost_pr.Text = row.Cells["ProcessStatus"].Value.ToString();
                Txt_duration.Text = row.Cells["DurationMinutes"].Value.ToString();
            }
        }

        private void dataGridView_type_prod_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_type_prod.Rows[e.RowIndex];
                Txt_prodtype_id.Text = row.Cells["ProductTypeID"].Value.ToString();
                Txt_product_type.Text = row.Cells["ProductTypeName"].Value.ToString();
                Txt_opis_prod.Text = row.Cells["Description"].Value.ToString();
                Txt_categor_proc.Text = row.Cells["Category"].Value.ToString();
                Txt_ed_izm.Text = row.Cells["UnitMeasure"].Value.ToString();
            }
        }

        private void dataGridView_product_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_product.Rows[e.RowIndex];
                Txt_ProductId.Text = row.Cells["ProductID"].Value.ToString();
                Txt_product_type_ID.Text = row.Cells["ProductTypeID"].Value.ToString();
                Txt_Process_Id.Text = row.Cells["ProcessID"].Value.ToString();
                Txt_ProductStatus.Text = row.Cells["ProductStatus"].Value.ToString();
                Txt_BatchNumber.Text = row.Cells["BatchNumber"].Value.ToString();
                dateTimePicker_prod.Text = row.Cells["ProductionDate"].Value.ToString();
                dateTimePicker_exp.Text = row.Cells["ExpirationDate"].Value.ToString();
            }
        }

        private void dataGridView_Maintenance_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_Maintenance.Rows[e.RowIndex];
                Txt_MaintenanceID.Text = row.Cells["MaintenanceID"].Value.ToString();
                Txt_EquipmentID.Text = row.Cells["EquipmentID"].Value.ToString();
                dateTimePicker_MaintenanceDate.Text = row.Cells["MaintenanceDate"].Value.ToString();
                Txt_MaintenanceDetails.Text = row.Cells["MaintenanceDetails"].Value.ToString();
                Txt_MaintenanceType.Text = row.Cells["MaintenanceType"].Value.ToString();
                Txt_MaintenanceCost.Text = row.Cells["MaintenanceCost"].Value.ToString();

            }
        }

        private void dataGridView_Logo_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_Logo.Rows[e.RowIndex];
                Txt_LogID.Text = row.Cells["LogID"].Value.ToString();
                dateTimePicker_EventTimestamp.Text = row.Cells["EventTimestamp"].Value.ToString();
                Txt_Equipment_ID.Text = row.Cells["EquipmentID"].Value.ToString();
                Txt_EventDescription.Text = row.Cells["EventDescription"].Value.ToString();
                Txt_Severity.Text = row.Cells["Severity"].Value.ToString();
                Txt_OperatorName.Text = row.Cells["OperatorName"].Value.ToString();
                Txt_Comments.Text = row.Cells["Comments"].Value.ToString();
            }
        }


        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void Btn_sort_Click(object sender, EventArgs e)
        {
            string tableName = string.Empty;
            ComboBox comboBox = null;
            TextBox textBox = null;
            DataGridView dataGridView = null;

            if (tabControl1.SelectedTab == tabPage1)
            {
                tableName = "Equipment";
                comboBox = comboBox1;
                textBox = textBox1;
                dataGridView = dataGried_BD;
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                tableName = "ProductionAreas";
                comboBox = comboBox2;
                textBox = textBox2;
                dataGridView = dataGridView_area;
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                tableName = "Processes";
                comboBox = comboBox3;
                textBox = textBox3;
                dataGridView = dataGridView_peocess;
            }
            else if (tabControl1.SelectedTab == tabPage4)
            {
                tableName = "ProductTypes";
                comboBox = comboBox4;
                textBox = textBox4;
                dataGridView = dataGridView_type_prod;
            }
            else if (tabControl1.SelectedTab == tabPage5)
            {
                tableName = "Products";
                comboBox = comboBox5;
                textBox = textBox5;
                dataGridView = dataGridView_product;
            }
            else if (tabControl1.SelectedTab == tabPage6)
            {
                tableName = "Maintenance";
                comboBox = comboBox6;
                textBox = textBox6;
                dataGridView = dataGridView_Maintenance;
            }
            else if (tabControl1.SelectedTab == tabPage7)
            {
                tableName = "Logs";
                comboBox = comboBox7;
                textBox = textBox7;
                dataGridView = dataGridView_Logo;
            }

            if (string.IsNullOrEmpty(tableName) || comboBox == null || textBox == null || dataGridView == null)
            {
                MessageBox.Show("Не удалось определить параметры для сортировки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SortAndFilterData(tableName, comboBox, textBox, dataGridView);
        }

       
    }
}
