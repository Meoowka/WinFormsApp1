using Microsoft.Data.SqlClient;
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
    public partial class AddRecordForm : Form
    {
        private string context;

        public AddRecordForm(string context)
        {
            InitializeComponent();
            this.context = context;
            this.StartPosition = FormStartPosition.CenterScreen;
            GenerateDynamicControls();
        }
        private void CreateLabelAndTextBox(string name, string labelText, ref int currentHeight, int controlHeight, int horizontalSpacing, ref int maxWidth, bool isReadOnly = false)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Font = new Font("Times New Roman", 14),
                AutoSize = true,
                Location = new Point(horizontalSpacing, currentHeight) 
            };

            TextBox txt = new TextBox
            {
                Name = "txt" + name,
                Font = new Font("Times New Roman", 14),
                Width = 300,
                Location = new Point(lbl.Left + 200, currentHeight),
                ReadOnly = isReadOnly
            };

            Controls.Add(lbl);
            Controls.Add(txt);
            currentHeight += controlHeight + 20;
            maxWidth = Math.Max(maxWidth, txt.Right + horizontalSpacing);
        }

      
        private void GenerateDynamicControls()
        {
            int controlHeight = 30;
            int verticalSpacing = 20;
            int horizontalSpacing = 20;

            int maxWidth = 0;
            int currentHeight = 10;

            switch (context)
            {
                case "ProductionAreas":
                    CreateLabelAndTextBox("AreaName", "Название участка", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("Description", "Описание", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("FloorNumber", "Этаж", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("AreaCode", "Код участка", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    break;

                case "Equipment":
                    CreateLabelAndTextBox("EquipmentName", "Название оборудования", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("EquipmentType", "Тип оборудования", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("Status", "Статус", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("Manufacturer", "Производитель", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("AreaID", "Участок (AreaID)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    break;

                case "Processes":
                    CreateLabelAndTextBox("ProcessName", "Название процесса", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("DurationMinutes", "Длительность (мин)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("AreaID", "Участок (AreaID)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("EquipmentID", "Оборудование (EquipmentID)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    break;

                case "ProductTypes":
                    CreateLabelAndTextBox("ProductTypeName", "Название типа продукции", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("Description", "Описание", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("Category", "Категория", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("UnitMeasure", "Единица измерения", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    break;

                case "Products":
                    CreateLabelAndTextBox("ProductStatus", "Статус продукта", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("BatchNumber", "Номер партии", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("ProductionDate", DateTime.Now.ToString("yyyy-MM-dd"), ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("ExpirationDate", DateTime.Now.AddMonths(12).ToString("yyyy-MM-dd"), ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("ProductTypeID", "Тип продукции (ProductTypeID)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("ProcessID", "Процесс (ProcessID)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    break;

                case "Maintenance":
                    CreateLabelAndTextBox("MaintenanceDate", DateTime.Now.ToString("yyyy-MM-dd"), ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("MaintenanceDetails", "Подробности обслуживания", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("MaintenanceType", "Тип обслуживания", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("MaintenanceCost", "Стоимость обслуживания", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("EquipmentID", "Оборудование (EquipmentID)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    break;

                case "Logs":
                    CreateLabelAndTextBox("EventTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("EventDescription", "Описание события", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("Severity", "Серьезность события", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("OperatorName", "Имя оператора", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("Comments", "Комментарии", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    CreateLabelAndTextBox("EquipmentID", "Оборудование (EquipmentID)", ref currentHeight, controlHeight, horizontalSpacing, ref maxWidth);
                    break;
            }

            Button btnSave = new Button
            {
                Text = "Сохранить",
                Font = new Font("Times New Roman", 14),
                Size = new Size(150, 40),
                Location = new Point((maxWidth - 150) / 2, currentHeight + verticalSpacing) 
            };
            btnSave.Click += Btn_Save_Click;

            Controls.Add(btnSave);
            this.ClientSize = new Size(maxWidth + horizontalSpacing * 2, btnSave.Bottom + verticalSpacing * 2);
        }
        private string GetForeignKeyTableName(string fieldName)
        {
            switch (fieldName)
            {
                case "AreaID":
                    return "ProductionAreas";
                case "EquipmentID":
                    return "Equipment";
                case "ProcessID":
                    return "Processes";
                case "ProductTypeID":
                    return "Product_Types";
                case "ProductID":
                    return "Products";
                case "MaintenanceID":
                    return "Maintenance";
                case "LogID":
                    return "Logs";
                default:
                    return string.Empty;
            }
        } 
        private bool CheckForDuplicates(string tableName, Dictionary<string, string> data)
        {
            string whereClause = string.Join(" AND ", data.Select(d => $"{d.Key} = '{d.Value}'"));
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {whereClause}";

            DB.sqlCommand.CommandText = query;
            int count = (int)DB.sqlCommand.ExecuteScalar();

            return count > 0;
        }
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            foreach (Control ctrl in Controls)
            {
                if (ctrl is TextBox txt)
                {
                    string fieldKey = txt.Name.Replace("txt", "");
                    data[fieldKey] = txt.Text;
                }
            }
            if (data.Values.Any(value => string.IsNullOrEmpty(value)))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }
            if (CheckForDuplicates(context, data))
            {
                MessageBox.Show("Запись с такими данными уже существует.");
                return;
            }
            InsertDataToDatabase(context, data);
            MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        private bool IsForeignKeyField(string fieldName)
        { 
            var foreignKeyFields = new List<string>
            {
            "AreaID",
            "EquipmentID", 
            "ProcessID", 
            "ProductTypeID", 
            "ProductID", 
            "MaintenanceID", 
            "LogID", 
            };

            return foreignKeyFields.Contains(fieldName);
        }
        private void InsertDataToDatabase(string context, Dictionary<string, string> data)
        {

            DB.sqlCommand.CommandText = $"INSERT INTO {context} ({string.Join(",", data.Keys)}) VALUES ({string.Join(",", data.Keys.Select(k => "@" + k))})";
            DB.sqlCommand.Parameters.Clear();
            foreach (var item in data)
            {
                DB.sqlCommand.Parameters.AddWithValue("@" + item.Key, string.IsNullOrEmpty(item.Value) ? DBNull.Value : item.Value);
            }
            DB.sqlCommand.ExecuteNonQuery();
        }
    }
}
