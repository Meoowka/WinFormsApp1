using System;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.Logging;
using OfficeOpenXml;

namespace WinFormsApp1
{
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Delete
    }

    public partial class MainFormsAdmin : Form
    {
        int selectRow;

        public MainFormsAdmin()
        {
            InitializeComponent();
        }

        //----------------------------------------------------------------------------------------------
        //Доп. Методы создания
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
            DB.sqlCommand.Parameters.Clear();
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

        private void MainFormsAdmin_Load(object sender, EventArgs e)
        {

            Pb_refresh_pd_ych.Click += Pb_refresh_Click;
            Pb_refresh_proc.Click += Pb_refresh_Click;
            Pb_refresh_type.Click += Pb_refresh_Click;
            Pb_refresh_prod.Click += Pb_refresh_Click;
            Pb_refresh_obs.Click += Pb_refresh_Click;
            Pb_refresh_log.Click += Pb_refresh_Click;

            Crate_btn_area.Click += Create_btn_Click;
            Btn_sozd_proc.Click += Create_btn_Click;
            Btn_sozd_type.Click += Create_btn_Click;
            Btn_sozd_prod.Click += Create_btn_Click;
            Btn_sozd_obsl.Click += Create_btn_Click;
            Btn_sozd_log.Click += Create_btn_Click;

            Delete_btn_area.Click += Delete_btn_Click;
            Delete_btn_proc.Click += Delete_btn_Click;
            Delete_btn_type.Click += Delete_btn_Click;
            Delete_btn_prod.Click += Delete_btn_Click;
            Delete_btn_man.Click += Delete_btn_Click;
            Delete_btn_log.Click += Delete_btn_Click;

            Pb_ers_area.Click += Pb_ers_Click;
            Pb_ers_proc.Click += Pb_ers_Click;
            Pb_ers_type.Click += Pb_ers_Click;
            Pb_ers_prod.Click += Pb_ers_Click;
            Pb_ers_obs.Click += Pb_ers_Click;
            Pb_ers_log.Click += Pb_ers_Click;

            Edit_btn_aria.Click += Edit_btn_Click;
            Edit_btn_proc.Click += Edit_btn_Click;
            Edit_btn_type.Click += Edit_btn_Click;
            Edit_btn_prod.Click += Edit_btn_Click;
            Edit_btn_obsle.Click += Edit_btn_Click;
            Edit_btn_log.Click += Edit_btn_Click;


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
        //----------------------------------------------------------------------------------------------
        //Вывод в TextBox значений из DataGridView
        private void dataGried_BD_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView_area_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView_peocess_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView_type_prod_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView_product_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView_Maintenance_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView_Logo_CellClick(object sender, DataGridViewCellEventArgs e)
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
        //----------------------------------------------------------------------------------------------
        //Методы для обработки событий для кнопок

        private (string ProcedureName, Dictionary<string, object> Parameters) GetProcedureAndParameters(string action)
        {
            string procedureName = string.Empty;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (tabControl1.SelectedTab == tabPage1) // Equipment
            {
                switch (action)
                {
                    case "Insert":
                        procedureName = "InsertEquipment";
                        parameters = new Dictionary<string, object>
                {
                    { "@Name", Txt_name_equ.Text },
                    { "@AreaID", Convert.ToInt32(Txt_aria_ID.Text) },
                    { "@Type", Txt_type_eque.Text },
                    { "@Status", Txt_status_equ.Text },
                    { "@Manufacturer", Txt_manuf.Text }
                };
                        break;

                    case "Update":
                        procedureName = "UpdateEquipment";
                        parameters = new Dictionary<string, object>
                {
                    { "@EquipmentID", Convert.ToInt32(Txt_equ_id.Text) },
                    { "@EquipmentName", Txt_name_equ.Text },
                    { "@AreaID", Convert.ToInt32(Txt_aria_ID.Text) },
                    { "@EquipmentType", Txt_type_eque.Text },
                    { "@Status", Txt_status_equ.Text },
                    { "@Manufacturer", Txt_manuf.Text }
                };
                        break;

                    case "Delete":
                        procedureName = "DeleteEquipment";
                        parameters = new Dictionary<string, object>
                {
                    { "@EquipmentID", Convert.ToInt32(Txt_equ_id.Text) }
                };
                        break;
                }
            }

            else if (tabControl1.SelectedTab == tabPage2) // ProductionAreas
            {
                switch (action)
                {
                    case "Insert":
                        procedureName = "InsertProductionArea";
                        parameters = new Dictionary<string, object>
                {
                    { "@AreaName", Txt_areaName.Text },
                    { "@Description", Txt_discrio.Text },
                    { "@FloorNumber", Txt_flowar.Text },
                    { "@AreaCode", Txt_cod_ar.Text }
                };
                        break;

                    case "Update":
                        procedureName = "UpdateProductionArea";
                        parameters = new Dictionary<string, object>
                {
                    { "@AreaID", Convert.ToInt32(Txt_Id_aria.Text) },
                    { "@AreaName", Txt_areaName.Text },
                    { "@Description", Txt_discrio.Text },
                    { "@FloorNumber", Txt_flowar.Text },
                    { "@AreaCode", Txt_cod_ar.Text }
                };
                        break;

                    case "Delete":
                        procedureName = "DeleteProductionArea";
                        parameters = new Dictionary<string, object>
                {
                    { "@AreaID", Convert.ToInt32(Txt_Id_aria.Text) }
                };
                        break;
                }
            }

            else if (tabControl1.SelectedTab == tabPage3) // Processes
            {
                switch (action)
                {
                    case "Insert":
                        procedureName = "InsertProcess";
                        parameters = new Dictionary<string, object>
                {
                    { "@Name", Txt_name_proc.Text },
                    { "@EquipmentID", Convert.ToInt32(Txt_proccs_id.Text) },
                    { "@AreaID", Convert.ToInt32(Txt_area_ids.Text) },
                    { "@Status", Txt_sost_pr.Text },
                    { "@Duration", Convert.ToInt32(Txt_duration.Text) }
                };
                        break;

                    case "Update":
                        procedureName = "UpdateProcess";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProcessID", Convert.ToInt32(Txt_proc_ID.Text) },
                    { "@Name", Txt_name_proc.Text },
                    { "@EquipmentID", Convert.ToInt32(Txt_proccs_id.Text) },
                    { "@AreaID", Convert.ToInt32(Txt_area_ids.Text) },
                    { "@PStatus", Txt_sost_pr.Text },
                    { "@Duration", Convert.ToInt32(Txt_duration.Text) }
                };
                        break;

                    case "Delete":
                        procedureName = "DeleteProcess";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProcessID", Convert.ToInt32(Txt_proc_ID.Text) }
                };
                        break;
                }
            }

            else if (tabControl1.SelectedTab == tabPage4) // ProductTypes
            {
                switch (action)
                {
                    case "Insert":
                        procedureName = "InsertProductType";
                        parameters = new Dictionary<string, object>
                {
                    { "@Name", Txt_product_type.Text },
                    { "@Description", Txt_opis_prod.Text },
                    { "@Category", Txt_categor_proc.Text },
                    { "@Unit", Txt_ed_izm.Text }
                };
                        break;

                    case "Update":
                        procedureName = "UpdateProductType";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProductTypeID", Convert.ToInt32(Txt_prodtype_id.Text) },
                    { "@Name", Txt_product_type.Text },
                    { "@Description", Txt_opis_prod.Text },
                    { "@Category", Txt_categor_proc.Text },
                    { "@Unit", Txt_ed_izm.Text }
                };
                        break;

                    case "Delete":
                        procedureName = "DeleteProductType";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProductTypeID", Convert.ToInt32(Txt_prodtype_id.Text) }
                };
                        break;
                }
            }

            else if (tabControl1.SelectedTab == tabPage5) // Products
            {
                switch (action)
                {
                    case "Insert":
                        procedureName = "InsertProduct";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProductTypeID", Convert.ToInt32(Txt_product_type_ID.Text) },
                    { "@ProcessID", Convert.ToInt32(Txt_Process_Id.Text) },
                    { "@PStatus", Txt_ProductStatus.Text },
                    { "@BatchNumber", Txt_BatchNumber.Text },
                    { "@ProductionDate", dateTimePicker_prod.Value },
                    { "@ExpirationDate", dateTimePicker_exp.Value }
                };
                        break;

                    case "Update":
                        procedureName = "UpdateProduct";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProductID", Convert.ToInt32(Txt_ProductId.Text) },
                    { "@ProductTypeID", Convert.ToInt32(Txt_product_type_ID.Text) },
                    { "@ProcessID", Convert.ToInt32(Txt_Process_Id.Text) },
                    { "@Status", Txt_ProductStatus.Text },
                    { "@BatchNumber", Txt_BatchNumber.Text },
                    { "@ProductionDate", dateTimePicker_prod.Value },
                    { "@ExpirationDate", dateTimePicker_exp.Value }
                };
                        break;

                    case "Delete":
                        procedureName = "DeleteProduct";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProductID", Convert.ToInt32(Txt_ProductId.Text) }
                };
                        break;
                }
            }

            else if (tabControl1.SelectedTab == tabPage6) // Maintenance
            {
                switch (action)
                {
                    case "Insert":
                        procedureName = "InsertMaintenance";
                        parameters = new Dictionary<string, object>
                {
                    { "@EquipmentID", Txt_product_type.Text },
                    { "@MaintenanceDate", dateTimePicker_MaintenanceDate.Value },
                    { "@Details", Txt_MaintenanceDetails.Text },
                    { "@Type", Txt_MaintenanceType.Text },
                    { "@Cost", Txt_MaintenanceCost.Text }
                };
                        break;

                    case "Update":
                        procedureName = "UpdateMaintenance";
                        parameters = new Dictionary<string, object>
                {
                    { "@EquipmentID", Txt_product_type.Text },
                    { "@MaintenanceDate", dateTimePicker_MaintenanceDate.Value },
                    { "@Details", Txt_MaintenanceDetails.Text },
                    { "@Type", Txt_MaintenanceType.Text },
                    { "@Cost", Txt_MaintenanceCost.Text }
                };
                        break;

                    case "Delete":
                        procedureName = "DeleteProductType";
                        parameters = new Dictionary<string, object>
                {
                    { "@ProductTypeID", Convert.ToInt32(Txt_prodtype_id.Text) }
                };
                        break;
                }
            }

            else if (tabControl1.SelectedTab == tabPage7) // Products
            {
                switch (action)
                {
                    case "Insert":
                        procedureName = "InsertLog";
                        parameters = new Dictionary<string, object>
                {
                    { "@EventTimestamp", Convert.ToInt32(dateTimePicker_EventTimestamp.Value) },
                    { "@EquipmentID", Convert.ToInt32(Txt_Equipment_ID.Text) },
                    { "@Description", Txt_EventDescription.Text },
                    { "@Severity", Txt_Severity.Text },
                    { "@OperatorName", Txt_OperatorName.Text },
                    { "@Comments", Txt_Comments.Text }
                };
                        break;

                    case "Update":
                        procedureName = "UpdateLog";
                        parameters = new Dictionary<string, object>
                {
                    { "@LogID", Convert.ToInt32(Txt_LogID.Text) },
                    { "@EventTimestamp", Convert.ToInt32(dateTimePicker_EventTimestamp.Value) },
                    { "@EquipmentID", Convert.ToInt32(Txt_Equipment_ID.Text) },
                    { "@Description", Txt_EventDescription.Text },
                    { "@Severity", Txt_Severity.Text },
                    { "@OperatorName", Txt_OperatorName.Text },
                    { "@Comments", Txt_Comments.Text }
                };
                        break;

                    case "Delete":
                        procedureName = "DeleteLog";
                        parameters = new Dictionary<string, object>
                {
                    { "@Txt_LogID", Convert.ToInt32(Txt_LogID.Text) }
                };
                        break;
                }
            }

            return (procedureName, parameters);
        }


        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == "tabPage2")
            {
                CreateColumnsForAreas();
                RefreshDataGrid(dataGridView_area, "SELECT AreaID, AreaName, Description, FloorNumber, AreaCode FROM ProductionAreas");
            }
            if (e.TabPage.Name == "tabPage3")
            {
                CreateColumnsForProcess();
                RefreshDataGrid(dataGridView_peocess, "SELECT ProcessID, ProcessName, EquipmentID, AreaID, ProcessStatus, DurationMinutes FROM Processes");
            }
            if (e.TabPage.Name == "tabPage4")
            {
                CreateColumnsForTypeProd();
                RefreshDataGrid(dataGridView_type_prod, "SELECT ProductTypeID, ProductTypeName, Description, Category, UnitMeasure FROM ProductTypes");
            }
            if (e.TabPage.Name == "tabPage5")
            {
                CreateColumnsForProduct();
                RefreshDataGrid(dataGridView_product, "SELECT ProductID, ProductTypeID, ProcessID, ProductStatus, BatchNumber, ProductionDate, ExpirationDate FROM Products");
            }
            if (e.TabPage.Name == "tabPage6")
            {
                CreateColumnsForMaintenance();
                RefreshDataGrid(dataGridView_Maintenance, "SELECT MaintenanceID, EquipmentID, MaintenanceDate, MaintenanceDetails, MaintenanceType, MaintenanceCost FROM Maintenance");
            }
            if (e.TabPage.Name == "tabPage7")
            {
                CreateColumnsForLogo();
                RefreshDataGrid(dataGridView_Logo, "SELECT LogID, EventTimestamp, EquipmentID, EventDescription, Severity, OperatorName, Comments FROM Logs");
            }
        }
        private DataGridView GetDataGridForContext(string context)
        {
            switch (context)
            {
                case "Equipment":
                    return dataGried_BD;
                case "ProductionAreas":
                    return dataGridView_area;
                case "Processes":
                    return dataGridView_peocess;
                case "ProductTypes":
                    return dataGridView_type_prod;
                case "Products":
                    return dataGridView_product;
                case "Maintenance":
                    return dataGridView_Maintenance;
                case "Logs":
                    return dataGridView_Logo;
                default:
                    throw new InvalidOperationException("Неизвестный контекст для DataGridView");
            }
        }
        private Dictionary<string, string> GetFieldsForContext(string context)
        {

            return context switch
            {
                "Equipment" => new Dictionary<string, string>
        {
            { "EquipmentID", Guid.NewGuid().ToString() },
            { "EquipmentName", "Название оборудования" },
            { "EquipmentType", "Тип оборудования" },
            { "Status", "Статус" },
            { "Manufacturer", "Производитель" },
            { "AreaID", GetForeignKeyValue("ProductionAreas", "AreaID") }
        },

                "ProductionAreas" => new Dictionary<string, string>
        {
            { "AreaID", Guid.NewGuid().ToString() },
            { "AreaName", "Название участка" },
            { "Description", "Описание" },
            { "FloorNumber", "Этаж" },
            { "AreaCode", "Код участка" }
        },
                "Processes" => new Dictionary<string, string>
        {
            { "ProcessID", Guid.NewGuid().ToString() },
            { "ProcessName", "Название процесса" },
            { "DurationMinutes", "Длительность (мин)" },
            { "AreaID", GetForeignKeyValue("ProductionAreas", "AreaID") },
            { "EquipmentID", GetForeignKeyValue("Equipment", "EquipmentID") }
        },
                "ProductTypes" => new Dictionary<string, string>
        {
            { "ProductTypeID", Guid.NewGuid().ToString() },
            { "ProductTypeName", "Название типа продукции" },
            { "Description", "Описание" },
            { "Category", "Категория" },
            { "UnitMeasure", "Единица измерения" }
        },
                "Products" => new Dictionary<string, string>
        {
            { "ProductID", Guid.NewGuid().ToString() },
            { "ProductTypeID", GetForeignKeyValue("ProductTypes", "ProductTypeID") },
            { "ProcessID", GetForeignKeyValue("Processes", "ProcessID") },
            { "ProductStatus", "Статус продукта" },
            { "BatchNumber", "Номер партии" },
            { "ProductionDate", DateTime.Now.ToString("yyyy-MM-dd") },
            { "ExpirationDate", DateTime.Now.AddMonths(12).ToString("yyyy-MM-dd") }
        },
                "Maintenance" => new Dictionary<string, string>
        {
            { "MaintenanceID", Guid.NewGuid().ToString() },
            { "EquipmentID", GetForeignKeyValue("Equipment", "EquipmentID") },
            { "MaintenanceDate", DateTime.Now.ToString("yyyy-MM-dd") },
            { "MaintenanceDetails", "Подробности обслуживания" },
            { "MaintenanceType", "Тип обслуживания" },
            { "MaintenanceCost", "Стоимость обслуживания" }
        },
                "Logs" => new Dictionary<string, string>
        {
            { "LogID", Guid.NewGuid().ToString() },
            { "EventTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "EquipmentID", GetForeignKeyValue("Equipment", "EquipmentID") },
            { "EventDescription", "Описание события" },
            { "Severity", "Серьезность события" },
            { "OperatorName", "Имя оператора" },
            { "Comments", "Комментарии" }
        },
                _ => new Dictionary<string, string>()
            };
        }

        private readonly Dictionary<string, string> tablePrimaryKeys = new Dictionary<string, string>
        {
            { "Equipment", "EquipmentID" },
            { "ProductionAreas", "AreaID" },
            { "Processes", "ProcessID" },
            { "ProductTypes", "ProductTypeID" },
            { "Products", "ProductID" },
            { "Maintenance", "MaintenanceID" },
            { "Logs", "LogID" }
        };

        private readonly Dictionary<string, List<string>> tableColumns = new Dictionary<string, List<string>>
        {
            { "Equipment", new List<string> { "EquipmentID", "Name", "Type", "Location", "Status", "PurchaseDate", "WarrantyEnd" } },
            { "ProductionAreas", new List<string> { "AreaID", "AreaName", "Description", "Supervisor", "Location" } },
            { "Processes", new List<string> { "ProcessID", "ProcessName", "Description", "StartDate", "EndDate", "Status" } },
            { "ProductTypes", new List<string> { "ProductTypeID", "TypeName", "Category", "Description" } },
            { "Products", new List<string> { "ProductID", "ProductName", "ProductTypeID", "Price", "Stock", "Manufacturer", "ExpiryDate" } },
            { "Maintenance", new List<string> { "MaintenanceID", "EquipmentID", "MaintenanceDate", "MaintenanceDetails", "MaintenanceType", "MaintenanceCost" } },
            { "Logs", new List<string> { "LogID", "Timestamp", "Action", "User", "Details" } }
        };
        private void RefreshDataGridForContext(string context)
        {
            string query = "";

            switch (context)
            {
                case "Equipment":
                    query = "SELECT * FROM Equipment";
                    break;
                case "ProductionAreas":
                    query = "SELECT * FROM ProductionAreas";
                    break;
                case "Processes":
                    query = "SELECT * FROM Processes";
                    break;
                case "ProductTypes":
                    query = "SELECT * FROM ProductTypes";
                    break;
                case "Products":
                    query = "SELECT * FROM Products";
                    break;
                case "Maintenance":
                    query = "SELECT * FROM Maintenance";
                    break;
                case "Logs":
                    query = "SELECT * FROM Logs";
                    break;
                default:
                    MessageBox.Show("Неизвестный контекст");
                    return;
            }
            DataGridView dgv = GetDataGridForContext(context);
            RefreshDataGrid(dgv, query);
        }

        private string GetForeignKeyValue(string tableName, string columnName)
        {
            DB.sqlCommand.CommandText = $"SELECT TOP 1 {columnName} FROM {tableName} ORDER BY {columnName} DESC";

            DB.sqlCommand.Parameters.Clear();

            var result = DB.sqlCommand.ExecuteScalar();

            return result != null ? result.ToString() : "NULL";
        }

        private void SaveData(string context, Dictionary<string, string> data)
        {
            if (data.Values.Any(v => string.IsNullOrEmpty(v)))
            {
                MessageBox.Show("Не все поля заполнены. Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsDuplicateRecord(context, data))
            {
                MessageBox.Show("Запись с такими данными уже существует в базе.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DB.sqlCommand.CommandText = $"INSERT INTO {context} ({string.Join(",", data.Keys)}) VALUES ({string.Join(",", data.Keys.Select(k => "@" + k))})";

            DB.sqlCommand.Parameters.Clear();
            foreach (var item in data)
            {
                DB.sqlCommand.Parameters.AddWithValue("@" + item.Key, item.Value ?? (object)DBNull.Value);
            }
            DB.sqlCommand.ExecuteNonQuery();

            MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool IsDuplicateRecord(string context, Dictionary<string, string> data)
        {
            string query = $"SELECT COUNT(1) FROM {context} WHERE {string.Join(" AND ", data.Keys.Select(k => $"{k} = @{k}"))}";

            DB.sqlCommand.CommandText = query;
            DB.sqlCommand.Parameters.Clear();

            foreach (var item in data)
            {
                DB.sqlCommand.Parameters.AddWithValue("@" + item.Key, item.Value ?? (object)DBNull.Value);
            }
            int count = Convert.ToInt32(DB.sqlCommand.ExecuteScalar());
            return count > 0;
        }

        private void DeleteRow()
        {
            string context = "";
            DataGridView selectedGrid = null;

            if (tabControl1.SelectedTab == tabPage1)
            {
                context = "Equipment";
                selectedGrid = dataGried_BD;
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                context = "ProductionAreas";
                selectedGrid = dataGridView_area;
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                context = "Processes";
                selectedGrid = dataGridView_peocess;
            }
            else if (tabControl1.SelectedTab == tabPage4)
            {
                context = "ProductTypes";
                selectedGrid = dataGridView_type_prod;
            }
            else if (tabControl1.SelectedTab == tabPage5)
            {
                context = "Products";
                selectedGrid = dataGridView_product;
            }
            else if (tabControl1.SelectedTab == tabPage6)
            {
                context = "Maintenance";
                selectedGrid = dataGridView_Maintenance;
            }
            else if (tabControl1.SelectedTab == tabPage7)
            {
                context = "Logs";
                selectedGrid = dataGridView_Logo;
            }

            if (selectedGrid == null)
            {
                MessageBox.Show("Выбранный DataGridView отсутствует.");
                return;
            }

            if (selectedGrid.Rows.Count == 0)
            {
                MessageBox.Show("В таблице нет строк для удаления.");
                return;
            }

            if (selectedGrid.CurrentCell == null)
            {
                MessageBox.Show("Нет выделенной строки для удаления.");
                return;
            }

            int index = selectedGrid.CurrentCell.RowIndex;

            if (index < 0 || index >= selectedGrid.Rows.Count || selectedGrid.Rows[index].IsNewRow)
            {
                MessageBox.Show("Некорректный индекс строки или выбрана пустая строка.");
                return;
            }

            if (selectedGrid.Columns.Count == 0)
            {
                MessageBox.Show("В таблице нет столбцов.");
                return;
            }

            if (selectedGrid.Rows[index].Cells[0].Value == null || string.IsNullOrEmpty(selectedGrid.Rows[index].Cells[0].Value.ToString()))
            {
                MessageBox.Show("Значение в первой ячейке отсутствует.");
                return;
            }

            int intCellValue = Convert.ToInt32(selectedGrid.Rows[index].Cells[0].Value);

            if (selectedGrid.Columns.Count > 6)
            {
                selectedGrid.Rows[index].Cells[6].Value = RowState.Delete;
            }

            selectedGrid.Rows[index].Visible = false;

            DeleteRecordFromTable(context, intCellValue);
        }

        private void DeleteRecordFromTable(string tableName, int recordId)
        {
            if (recordId == -1)
            {
                MessageBox.Show("Запись не выбрана для удаления.");
                return;
            }
            if (!tablePrimaryKeys.ContainsKey(tableName))
            {
                MessageBox.Show($"Не найден первичный ключ для таблицы {tableName}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string primaryKeyColumn = tablePrimaryKeys[tableName];

            try
            {
                string deleteQuery = $"DELETE FROM {tableName} WHERE {primaryKeyColumn} = {recordId}";
                DB.sqlCommand.CommandText = deleteQuery;
                DB.sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Search(DataGridView dwg, string tableName, string searchText)
        {
            dwg.Rows.Clear();

            try
            {
                string query;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    query = $"SELECT * FROM {tableName}";
                }
                else
                {
                    var columns = new List<string>();
                    foreach (DataGridViewColumn column in dwg.Columns)
                    {
                        columns.Add($"CAST([{column.Name}] AS NVARCHAR(MAX)) LIKE @searchText");
                    }
                    query = $"SELECT * FROM {tableName} WHERE {string.Join(" OR ", columns)}";
                }

                DB.sqlCommand.CommandText = query;
                DB.sqlCommand.Parameters.Clear();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    DB.sqlCommand.Parameters.AddWithValue("@searchText", $"%{searchText}%");
                }

                using (SqlDataReader reader = DB.sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int rowIndex = dwg.Rows.Add();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dwg.Rows[rowIndex].Cells[i].Value = reader.GetValue(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearTextBoxesForCurrentTab()
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                Txt_equ_id.Clear();
                Txt_name_equ.Clear();
                Txt_aria_ID.Clear();
                Txt_type_eque.Clear();
                Txt_status_equ.Clear();
                Txt_manuf.Clear();
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                Txt_Id_aria.Clear();
                Txt_areaName.Clear();
                Txt_discrio.Clear();
                Txt_flowar.Clear();
                Txt_cod_ar.Clear();
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                Txt_proc_ID.Clear();
                Txt_name_proc.Clear();
                Txt_proccs_id.Clear();
                Txt_area_ids.Clear();
                Txt_sost_pr.Clear();
                Txt_duration.Clear();
            }
            else if (tabControl1.SelectedTab == tabPage4)
            {
                Txt_prodtype_id.Clear();
                Txt_product_type.Clear();
                Txt_opis_prod.Clear();
                Txt_categor_proc.Clear();
                Txt_ed_izm.Clear();
            }
            else if (tabControl1.SelectedTab == tabPage5)
            {
                Txt_ProductId.Clear();
                Txt_product_type_ID.Clear();
                Txt_Process_Id.Clear();
                Txt_ProductStatus.Clear();
                Txt_BatchNumber.Clear();
                dateTimePicker_prod.Value = DateTime.MinValue;
                dateTimePicker_exp.Value = DateTime.MinValue;
            }
            else if (tabControl1.SelectedTab == tabPage6)
            {
                Txt_MaintenanceID.Clear();
                Txt_EquipmentID.Clear();
                dateTimePicker_MaintenanceDate.Value = DateTime.MinValue;
                Txt_MaintenanceDetails.Clear();
                Txt_MaintenanceType.Clear();
                Txt_MaintenanceCost.Clear();

            }
            else if (tabControl1.SelectedTab == tabPage7)
            {
                Txt_LogID.Clear();
                dateTimePicker_EventTimestamp.Value = DateTime.MinValue;
                Txt_Equipment_ID.Clear();
                Txt_EventDescription.Clear();
                Txt_Severity.Clear();
                Txt_OperatorName.Clear();
                Txt_Comments.Clear();
            }
        }

        public void ImportFromExcelAndUpdateDB(DataGridView grid, string tableName)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var package = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        grid.Rows.Clear();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            int gridRowIndex = grid.Rows.Add();
                            List<string> values = new List<string>();

                            for (int col = 1; col <= colCount; col++)
                            {
                                object cellValue = worksheet.Cells[row, col].Value ?? string.Empty;
                                grid.Rows[gridRowIndex].Cells[col - 1].Value = cellValue.ToString();
                                values.Add(cellValue.ToString());
                            }

                            UpdateDatabaseRecord(tableName, values);
                        }

                        MessageBox.Show("Импорт завершён успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateDatabaseRecord(string tableName, List<string> values)
        {
            try
            {
                string primaryKey = tablePrimaryKeys[tableName];
                string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE {primaryKey} = @primaryKey";

                DB.sqlCommand.CommandText = checkQuery;
                DB.sqlCommand.Parameters.Clear();
                DB.sqlCommand.Parameters.AddWithValue("@primaryKey", values[0]);

                int exists = (int)DB.sqlCommand.ExecuteScalar();
                string query = exists > 0
                    ? GenerateUpdateQuery(tableName, values)
                    : GenerateInsertQuery(tableName, values);

                Console.WriteLine("Generated Query: " + query);

                DB.sqlCommand.CommandText = query;
                DB.sqlCommand.Parameters.Clear();

                int paramIndex = 0;

                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] != null && values[i] != string.Empty)
                    {
                        string paramName = $"@param{paramIndex}";
                        Console.WriteLine($"Adding Parameter: {paramName} = {values[i]}");
                        DB.sqlCommand.Parameters.AddWithValue(paramName, values[i]);
                        paramIndex++;
                    }
                    else
                    {
                        string paramName = $"@param{paramIndex}";
                        Console.WriteLine($"Adding Parameter: {paramName} = NULL");
                        DB.sqlCommand.Parameters.AddWithValue(paramName, DBNull.Value);
                        paramIndex++;
                    }
                }

                Console.WriteLine("Parameters:");
                foreach (SqlParameter param in DB.sqlCommand.Parameters)
                {
                    Console.WriteLine($"{param.ParameterName} = {param.Value}");
                }

                DB.sqlCommand.ExecuteNonQuery();

                MessageBox.Show(
                    exists > 0
                        ? "Запись успешно обновлена в базе данных."
                        : "Запись успешно добавлена в базу данных.",
                    "Успех",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления базы данных: {ex.Message}\n\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //try
            //{
            //    string primaryKey = tablePrimaryKeys[tableName]; // Получаем первичный ключ из словаря
            //    string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE {primaryKey} = @primaryKey";

            //    // Проверяем, существует ли запись с таким первичным ключом
            //    DB.sqlCommand.CommandText = checkQuery;
            //    DB.sqlCommand.Parameters.Clear();
            //    DB.sqlCommand.Parameters.AddWithValue("@primaryKey", values[0]);

            //    int exists = (int)DB.sqlCommand.ExecuteScalar();

            //    if (exists > 0)
            //    {
            //        // Формируем запрос на обновление
            //        string updateQuery = GenerateUpdateQuery(tableName, values);
            //        DB.sqlCommand.CommandText = updateQuery;
            //    }
            //    else
            //    {
            //        // Формируем запрос на вставку
            //        string insertQuery = GenerateInsertQuery(tableName, values);
            //        DB.sqlCommand.CommandText = insertQuery;
            //    }

            //    // Добавляем параметры для выполнения запроса
            //    DB.sqlCommand.Parameters.Clear();
            //    for (int i = 0; i < values.Count; i++)
            //    {
            //        DB.sqlCommand.Parameters.AddWithValue($"@param{i}", values[i]);
            //    }

            //    // Выполняем запрос
            //    DB.sqlCommand.ExecuteNonQuery();
            //    MessageBox.Show("Операция успешно выполнена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка обновления базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private string GenerateUpdateQuery(string tableName, List<string> values)
        {
            string primaryKey = tablePrimaryKeys[tableName];
            List<string> columns = tableColumns[tableName];

            string setClause = string.Join(", ", columns.Skip(1).Select((col, i) => $"{col} = @param{i + 1}"));

            return $"UPDATE {tableName} SET {setClause} WHERE {primaryKey} = @param0";
        }

        private string GenerateInsertQuery(string tableName, List<string> values)
        {
            List<string> columns = tableColumns[tableName];
            string columnNames = string.Join(", ", columns);
            string parameters = string.Join(", ", values.Select((_, i) => $"@param{i}"));

            return $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameters})";
        }

        private DataGridView GetSelectedGrid(TabControl tabControl)
        {
            if (tabControl.SelectedTab == tabPage1) return dataGried_BD;
            if (tabControl.SelectedTab == tabPage2) return dataGridView_area;
            if (tabControl.SelectedTab == tabPage3) return dataGridView_peocess;
            if (tabControl.SelectedTab == tabPage4) return dataGridView_type_prod;
            if (tabControl.SelectedTab == tabPage5) return dataGridView_product;
            if (tabControl.SelectedTab == tabPage6) return dataGridView_Maintenance;
            if (tabControl.SelectedTab == tabPage7) return dataGridView_Logo;
            return null; // Если не найдено, возвращаем null
        }

        private string GetTableNameForTab(TabPage tabPage)
        {
            if (tabPage == tabPage1) return "Equipment";
            if (tabPage == tabPage2) return "ProductionAreas";
            if (tabPage == tabPage3) return "Processes";
            if (tabPage == tabPage4) return "ProductTypes";
            if (tabPage == tabPage5) return "Products";
            if (tabPage == tabPage6) return "Maintenance";
            if (tabPage == tabPage7) return "Logs";
            return string.Empty;
        }

        private void BackupDatabase(string databaseName, string backupFilePath)
        {
            try
            {

                string backupQuery = $@"
            BACKUP DATABASE [{databaseName}]
            TO DISK = @backupFilePath
            WITH FORMAT, INIT, NAME = 'Full Backup of {databaseName}', SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                DB.sqlCommand.CommandText = backupQuery;
                DB.sqlCommand.Parameters.Clear();
                DB.sqlCommand.Parameters.AddWithValue("@backupFilePath", backupFilePath);

                DB.sqlCommand.ExecuteNonQuery();

                MessageBox.Show($"Резервное копирование базы данных '{databaseName}' успешно выполнено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка резервного копирования: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestoreDatabase(string databaseName, string backupFilePath)
        {
            try
            {
                // Убедитесь, что соединение с базой закрыто, так как она будет перезаписана.
                DB.CloseConnection();

                // Формируем команду для восстановления
                string restoreQuery = $@"
            RESTORE DATABASE [{databaseName}]
            FROM DISK = @backupFilePath
            WITH REPLACE, RECOVERY, STATS = 10";

                // Открываем соединение с master, так как восстанавливаем другую БД
                using (SqlConnection masterConnection = new SqlConnection("Data Source=.;Initial Catalog=master;Integrated Security=True"))
                {
                    masterConnection.Open();

                    using (SqlCommand restoreCmd = new SqlCommand(restoreQuery, masterConnection))
                    {
                        restoreCmd.Parameters.AddWithValue("@backupFilePath", backupFilePath);
                        restoreCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"База данных '{databaseName}' успешно восстановлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Повторно открываем соединение
                DB.GetSqlConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка восстановления базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Change()
        {
            string tableName = "";
            DataGridView selectedGrid = null;
            List<string> columnValues = new List<string>();
            if (tabControl1.SelectedTab == tabPage1)
            {
                tableName = "Equipment";
                selectedGrid = dataGried_BD;

                columnValues.Add(Txt_equ_id.Text);
                columnValues.Add(Txt_name_equ.Text);
                columnValues.Add(Txt_aria_ID.Text);
                columnValues.Add(Txt_type_eque.Text);
                columnValues.Add(Txt_status_equ.Text);
                columnValues.Add(Txt_manuf.Text);
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                tableName = "ProductionAreas";
                selectedGrid = dataGridView_area;

                columnValues.Add(Txt_Id_aria.Text);
                columnValues.Add(Txt_areaName.Text);
                columnValues.Add(Txt_discrio.Text);
                columnValues.Add(Txt_flowar.Text);
                columnValues.Add(Txt_cod_ar.Text);
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                tableName = "Processes";
                selectedGrid = dataGridView_peocess;

                columnValues.Add(Txt_proc_ID.Text);
                columnValues.Add(Txt_name_proc.Text);
                columnValues.Add(Txt_proccs_id.Text);
                columnValues.Add(Txt_area_ids.Text);
                columnValues.Add(Txt_sost_pr.Text);
                columnValues.Add(Txt_duration.Text);
            }
            else if (tabControl1.SelectedTab == tabPage4)
            {
                tableName = "ProductTypes";
                selectedGrid = dataGridView_type_prod;

                columnValues.Add(Txt_prodtype_id.Text);
                columnValues.Add(Txt_product_type.Text);
                columnValues.Add(Txt_opis_prod.Text);
                columnValues.Add(Txt_categor_proc.Text);
                columnValues.Add(Txt_ed_izm.Text);
            }
            else if (tabControl1.SelectedTab == tabPage5)
            {
                tableName = "Products";
                selectedGrid = dataGridView_product;

                columnValues.Add(Txt_ProductId.Text);
                columnValues.Add(Txt_product_type_ID.Text);
                columnValues.Add(Txt_Process_Id.Text);
                columnValues.Add(Txt_ProductStatus.Text);
                columnValues.Add(Txt_BatchNumber.Text);
                columnValues.Add(dateTimePicker_prod.Text);
                columnValues.Add(dateTimePicker_exp.Text);
            }
            else if (tabControl1.SelectedTab == tabPage6)
            {
                tableName = "Maintenance";
                selectedGrid = dataGridView_Maintenance;

                columnValues.Add(Txt_MaintenanceID.Text);
                columnValues.Add(Txt_EquipmentID.Text);
                columnValues.Add(dateTimePicker_MaintenanceDate.Text);
                columnValues.Add(Txt_MaintenanceDetails.Text);
                columnValues.Add(Txt_MaintenanceType.Text);
                columnValues.Add(Txt_MaintenanceCost.Text);
            }
            else if (tabControl1.SelectedTab == tabPage7)
            {
                tableName = "Logs";
                selectedGrid = dataGridView_Logo;

                columnValues.Add(Txt_LogID.Text);
                columnValues.Add(dateTimePicker_EventTimestamp.Text);
                columnValues.Add(Txt_Equipment_ID.Text);
                columnValues.Add(Txt_EventDescription.Text);
                columnValues.Add(Txt_Severity.Text);
                columnValues.Add(Txt_OperatorName.Text);
                columnValues.Add(Txt_Comments.Text);
            }

            if (string.IsNullOrEmpty(tableName) || selectedGrid == null)
            {
                MessageBox.Show("Не удалось определить текущую вкладку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRowIndex = selectedGrid.CurrentCell?.RowIndex;
            if (selectedRowIndex == null || selectedRowIndex < 0)
            {
                MessageBox.Show("Не выбрана строка для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(columnValues[0]))
            {
                MessageBox.Show("Ошибка: отсутствует значение первичного ключа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < columnValues.Count; i++)
            {
                if (string.IsNullOrEmpty(columnValues[i]))
                {
                    columnValues[i] = selectedGrid.Rows[(int)selectedRowIndex].Cells[i].Value.ToString();
                }
            }

            string query = "UPDATE " + tableName + " SET ";
            for (int i = 1; i < columnValues.Count; i++)
            {
                query += $"{selectedGrid.Columns[i].Name} = @param{i}, ";
            }

            query = query.TrimEnd(',', ' ');

            query += " WHERE " + selectedGrid.Columns[0].Name + " = @param0";

            try
            {
                DB.sqlCommand.CommandText = query;
                DB.sqlCommand.Parameters.Clear();

                for (int i = 0; i < columnValues.Count; i++)
                {
                    DB.sqlCommand.Parameters.AddWithValue($"@param{i}", columnValues[i]);
                }
                DB.sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Запись успешно обновлена в базе данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            //try
            //{
            //    string tableName = "";
            //    DataGridView selectedGrid = null;
            //    List<string> columnValues = new List<string>();

            //    // Определяем текущую вкладку и заполняем значения
            //    if (tabControl1.SelectedTab == tabPage1)
            //    {
            //        tableName = "Equipment";
            //        selectedGrid = dataGried_BD;

            //        columnValues.Add(Txt_equ_id.Text);
            //        columnValues.Add(Txt_name_equ.Text);
            //        columnValues.Add(Txt_aria_ID.Text);
            //        columnValues.Add(Txt_type_eque.Text);
            //        columnValues.Add(Txt_status_equ.Text);
            //        columnValues.Add(Txt_manuf.Text);
            //    }
            //    else if (tabControl1.SelectedTab == tabPage2)
            //    {
            //        tableName = "ProductionAreas";
            //        selectedGrid = dataGridView_area;

            //        columnValues.Add(Txt_Id_aria.Text);
            //        columnValues.Add(Txt_areaName.Text);
            //        columnValues.Add(Txt_discrio.Text);
            //        columnValues.Add(Txt_flowar.Text);
            //        columnValues.Add(Txt_cod_ar.Text);
            //    }
            //    else if (tabControl1.SelectedTab == tabPage3)
            //    {
            //        tableName = "Processes";
            //        selectedGrid = dataGridView_peocess;

            //        columnValues.Add(Txt_proc_ID.Text);
            //        columnValues.Add(Txt_name_proc.Text);
            //        columnValues.Add(Txt_proccs_id.Text);
            //        columnValues.Add(Txt_area_ids.Text);
            //        columnValues.Add(Txt_sost_pr.Text);
            //        columnValues.Add(Txt_duration.Text);
            //    }
            //    else if (tabControl1.SelectedTab == tabPage4)
            //    {
            //        tableName = "ProductTypes";
            //        selectedGrid = dataGridView_type_prod;

            //        columnValues.Add(Txt_prodtype_id.Text);
            //        columnValues.Add(Txt_product_type.Text);
            //        columnValues.Add(Txt_opis_prod.Text);
            //        columnValues.Add(Txt_categor_proc.Text);
            //        columnValues.Add(Txt_ed_izm.Text);
            //    }
            //    else if (tabControl1.SelectedTab == tabPage5)
            //    {
            //        tableName = "Products";
            //        selectedGrid = dataGridView_product;

            //        columnValues.Add(Txt_ProductId.Text);
            //        columnValues.Add(Txt_product_type_ID.Text);
            //        columnValues.Add(Txt_Process_Id.Text);
            //        columnValues.Add(Txt_ProductStatus.Text);
            //        columnValues.Add(Txt_BatchNumber.Text);
            //        columnValues.Add(dateTimePicker_prod.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            //        columnValues.Add(dateTimePicker_exp.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            //    }
            //    else if (tabControl1.SelectedTab == tabPage6)
            //    {
            //        tableName = "Maintenance";
            //        selectedGrid = dataGridView_Maintenance;

            //        columnValues.Add(Txt_MaintenanceID.Text);
            //        columnValues.Add(Txt_EquipmentID.Text);
            //        columnValues.Add(dateTimePicker_MaintenanceDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            //        columnValues.Add(Txt_MaintenanceDetails.Text);
            //        columnValues.Add(Txt_MaintenanceType.Text);
            //        columnValues.Add(Txt_MaintenanceCost.Text);
            //    }
            //    else if (tabControl1.SelectedTab == tabPage7)
            //    {
            //        tableName = "Logs";
            //        selectedGrid = dataGridView_Logo;

            //        columnValues.Add(Txt_LogID.Text);
            //        columnValues.Add(dateTimePicker_EventTimestamp.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            //        columnValues.Add(Txt_Equipment_ID.Text);
            //        columnValues.Add(Txt_EventDescription.Text);
            //        columnValues.Add(Txt_Severity.Text);
            //        columnValues.Add(Txt_OperatorName.Text);
            //        columnValues.Add(Txt_Comments.Text);
            //    }

            //    // Проверяем, определены ли таблица и DataGridView
            //    if (string.IsNullOrEmpty(tableName) || selectedGrid == null)
            //    {
            //        MessageBox.Show("Не удалось определить текущую вкладку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    // Проверяем, выбрана ли строка
            //    var selectedRowIndex = selectedGrid.CurrentCell?.RowIndex;
            //    if (selectedRowIndex == null || selectedRowIndex < 0)
            //    {
            //        MessageBox.Show("Не выбрана строка для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    // Проверяем первичный ключ
            //    if (string.IsNullOrEmpty(columnValues[0]))
            //    {
            //        MessageBox.Show("Ошибка: отсутствует значение первичного ключа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    // Сравниваем количество колонок в DataGridView с количеством значений
            //    if (columnValues.Count != selectedGrid.ColumnCount - 1)
            //    {
            //        MessageBox.Show("Количество введённых значений не соответствует количеству столбцов в таблице.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    // Обновляем значения в DataGridView
            //    for (int i = 0; i < columnValues.Count; i++)
            //    {
            //        selectedGrid.Rows[(int)selectedRowIndex].Cells[i].Value = columnValues[i];
            //    }

            //    // Помечаем строку как изменённую
            //    selectedGrid.Rows[(int)selectedRowIndex].Cells[selectedGrid.Columns.Count - 1].Value = RowState.Modified;

            //    // Обновляем запись в базе данных
            //    UpdateDatabaseRecord(tableName, columnValues);

            //    MessageBox.Show("Запись успешно обновлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }

        private async Task CallStoredProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                DB.sqlCommand.CommandText = procedureName;
                DB.sqlCommand.CommandType = CommandType.StoredProcedure;

                DB.sqlCommand.Parameters.Clear();

                foreach (var param in parameters)
                {
                    DB.sqlCommand.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
                int affectedRows = await DB.sqlCommand.ExecuteNonQueryAsync();


                MessageBox.Show($"Операция успешно выполнена. Затронуто строк: {affectedRows}.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения хранимой процедуры: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //try
            //{
            //    DB.sqlCommand.CommandText = procedureName;
            //    DB.sqlCommand.CommandType = CommandType.StoredProcedure;
            //    DB.sqlCommand.Parameters.Clear();
            //    foreach (var param in parameters)
            //    {
            //        DB.sqlCommand.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            //    }

            //    int affectedRows = DB.sqlCommand.ExecuteNonQuery();

            //    MessageBox.Show($"Операция успешно выполнена. Затронуто строк: {affectedRows}.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка выполнения хранимой процедуры: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }

        private void DisplayData(string query, DataGridView dataGrid)
        {
            try
            {
                DB.sqlCommand.CommandText = query;
                DB.sqlCommand.CommandType = CommandType.Text;

                using (SqlDataAdapter adapter = new SqlDataAdapter(DB.sqlCommand))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGrid.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //----------------------------------------------------------------------------------------------
        //Методы кнопок
        private void Create_btn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string context = "";

                if (clickedButton == Create_btn)
                {
                    context = "Equipment";
                }
                else if (clickedButton == Crate_btn_area)
                {

                    context = "ProductionAreas";
                }
                else if (clickedButton == Btn_sozd_proc)
                {

                    context = "Processes";
                }
                else if (clickedButton == Btn_sozd_type)
                {

                    context = "ProductTypes";
                }
                else if (clickedButton == Btn_sozd_prod)
                {

                    context = "Products";
                }
                else if (clickedButton == Btn_sozd_obsl)
                {

                    context = "Maintenance";
                }
                else if (clickedButton == Btn_sozd_log)
                {

                    context = "Logs";
                }


                Dictionary<string, string> fields = GetFieldsForContext(context);

                AddRecordForm addForm = new AddRecordForm(context);
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    RefreshDataGridForContext(context);
                }
            }
        }
        private void Delete_btn_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var login = new Login();
            login.Show();
        }
        private void Txt_search_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == Txt_search)
                Search(dataGried_BD, "Equipment", textBox.Text.Trim());
            else if (textBox == Txt_search_area)
                Search(dataGridView_area, "ProductionAreas", textBox.Text.Trim());
            else if (textBox == Txt_search_proc)
                Search(dataGridView_peocess, "Processes", textBox.Text.Trim());
            else if (textBox == Txt_search_prod)
                Search(dataGridView_type_prod, "ProductTypes", textBox.Text.Trim());
            else if (textBox == Txt_search_proddd)
                Search(dataGridView_product, "Products", textBox.Text.Trim());
            else if (textBox == Txt_search_obs)
                Search(dataGridView_Maintenance, "Maintenance", textBox.Text.Trim());
            else if (textBox == Txt_search_log)
                Search(dataGridView_Logo, "Logs", textBox.Text.Trim());
        }
        private void Pb_ers_Click(object sender, EventArgs e)
        {
            ClearTextBoxesForCurrentTab();
        }
        private void экспортДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dataOperations = new DataOperations();

            DataGridView selectedGrid = GetSelectedGrid(tabControl1);
            if (selectedGrid != null)
            {
                dataOperations.ExportToExcel(selectedGrid, GetTableNameForTab(tabControl1.SelectedTab));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите вкладку с данными для экспорта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void импортДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dataOperations = new DataOperations();
            DataGridView selectedGrid = GetSelectedGrid(tabControl1);
            if (selectedGrid != null)
            {
                dataOperations.ImportFromExcel(selectedGrid, GetTableNameForTab(tabControl1.SelectedTab));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите вкладку с данными для импорта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void резервнаяКопияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Backup files (*.bak)|*.bak",
                Title = "Сохранить резервную копию"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string backupPath = saveFileDialog.FileName;
                BackupDatabase("MicroSystemTechDB", backupPath);
            }
        }
        private void загрузитьРезервнуюКопиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Backup files (*.bak)|*.bak",
                Title = "Выберите файл резервной копии"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string backupPath = openFileDialog.FileName;
                RestoreDatabase("MicroSystemTechDB", backupPath);
            }
        }
        private void Edit_btn_Click(object sender, EventArgs e)
        {
            Change();
        }
        private void Pb_refresh_Click(object sender, EventArgs e)
        {
            PictureBox clickedButton = sender as PictureBox;
            if (clickedButton != null)
            {
                string query = "";
                DataGridView targetDataGridView = null;

                if (clickedButton == Pb_refresh)
                {
                    targetDataGridView = dataGried_BD;
                    query = "SELECT * FROM Equipment";
                }
                else if (clickedButton == Pb_refresh_pd_ych)
                {
                    targetDataGridView = dataGridView_area;
                    query = "SELECT * FROM ProductionAreas";
                }
                else if (clickedButton == Pb_refresh_proc)
                {
                    targetDataGridView = dataGridView_peocess;
                    query = "SELECT * FROM Processes";
                }
                else if (clickedButton == Pb_refresh_type)
                {
                    targetDataGridView = dataGridView_type_prod;
                    query = "SELECT * FROM ProductTypes";
                }
                else if (clickedButton == Pb_refresh_prod)
                {
                    targetDataGridView = dataGridView_product;
                    query = "SELECT * FROM Products";
                }
                else if (clickedButton == Pb_refresh_obs)
                {
                    targetDataGridView = dataGridView_Maintenance;
                    query = "SELECT * FROM Maintenance";
                }
                else if (clickedButton == Pb_refresh_log)
                {
                    targetDataGridView = dataGridView_Logo;
                    query = "SELECT * FROM Logs";
                }

                // Обновление выбранного DataGridView
                if (targetDataGridView != null && !string.IsNullOrEmpty(query))
                {
                    RefreshDataGrid(targetDataGridView, query);
                }
            }
        }
        private void запуститьСкриптДляPowerShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptPath = @"..\\BackupScript.ps1";
            System.Diagnostics.Process.Start("powershell.exe", $"-ExecutionPolicy Bypass -File \"{scriptPath}\"");
        }

        private void импортСкриптовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (procedureName, parameters) = GetProcedureAndParameters("Insert");
            if (!string.IsNullOrEmpty(procedureName))
            {
                CallStoredProcedure(procedureName, parameters);
            }
        }

        private void хрПрInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (procedureName, parameters) = GetProcedureAndParameters("Update");
            if (!string.IsNullOrEmpty(procedureName))
            {
                CallStoredProcedure(procedureName, parameters);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (procedureName, parameters) = GetProcedureAndParameters("Delete");
            if (!string.IsNullOrEmpty(procedureName))
            {
                CallStoredProcedure(procedureName, parameters);
            }
        }





        //----------------------------------------------------------------------------------------------
    }
}


