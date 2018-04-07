using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace AppDataBase
{

    public partial class MainWindow : Window
    {
        OleDbConnection conector;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
            //conectar con la bdd
            conector = new OleDbConnection();
            conector.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\empleados.mdb";
            BindGrid();
        }

        //mostrar LA BDD en gridview

        private void BindGrid()
        {

            OleDbCommand cmd = new OleDbCommand();
            if (conector.State != ConnectionState.Open)
                conector.Open();
            cmd.Connection = conector;
            //Si Search no está vacío busco de acuerdo a Search.txt
            if (FEmpSearch.Text != "")
            {
                cmd.CommandText = "select * from tbl_emple where " + FEmpSelField.Text + " like '%" + FEmpSearch.Text + "%' order by Id";
            }
            //Sino lista toda la tabla
            else
                cmd.CommandText = "select * from tbl_emple order by Id";

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            tBL_EMPLEDataGrid.ItemsSource = dt.AsDataView();


            if (dt.Rows.Count > 0)
            {
                lbl_grid.Visibility = Visibility.Hidden;
                tBL_EMPLEDataGrid.Visibility = Visibility.Visible;
            }
            else
            {
                lbl_grid.Visibility = Visibility.Visible;
                tBL_EMPLEDataGrid.Visibility = Visibility.Hidden;
            }
        }

        //alta a un empleado
        private void btn_guardar_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (conector.State != ConnectionState.Open)
                conector.Open();
            cmd.Connection = conector;

            if (FEmpNombre.Text != "" && FEmpApellido.Text != "" && FEmpFechanac.Text != "" &&
                FEmpDoc.Text != "" && FEmpDomicilio.Text != "")
            {
                  cmd.CommandText = "INSERT INTO TBL_EMPLE(Nombre,Apellido,Fecha_nac,Genero,Documento,Email,Domicilio) Values ('" + FEmpNombre.Text + "','" + FEmpApellido.Text + "','" + FEmpFechanac.Text + "','" + FEmpGenero.Text + "','" + FEmpDoc.Text + "','" + FEmpEmail.Text + "','" + FEmpDomicilio.Text + "')";
                  cmd.ExecuteNonQuery();
                  BindGrid();
                  MessageBox.Show("Registro Agregado Correctamente");
                  //pbar.Value = 100;
                  ClearAll();
             }
             else
             {
                    MessageBox.Show("Complete los campos, para guardar el registro");
              }
            }
        //modificar un empleado
        private void btn_modificar_Click(object sender, RoutedEventArgs e)
        {
            if (tBL_EMPLEDataGrid.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)tBL_EMPLEDataGrid.SelectedItems[0];
                FEmpId.Text = row["Id"].ToString();
                FEmpNombre.Text = row["Nombre"].ToString();
                FEmpApellido.Text = row["Apellido"].ToString();
                FEmpFechanac.Text = row["Fecha_nac"].ToString();
                FEmpGenero.Text = row["Genero"].ToString();
                FEmpDoc.Text = row["Documento"].ToString();
                FEmpEmail.Text = row["Email"].ToString();
                FEmpDomicilio.Text = row["Domicilio"].ToString();
                FEmpId.IsEnabled = false;
                activarBtns();
            }
            else
            {
                MessageBox.Show("Seleccione un registro para actualizar");
            }
        }
        //actualizar la modificacion del empleado
        private void btn_actualizar_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (conector.State != ConnectionState.Open)
                conector.Open();
            cmd.Connection = conector;
            DataRowView row = (DataRowView)tBL_EMPLEDataGrid.SelectedItems[0];
            cmd.CommandText = "update TBL_EMPLE set Nombre='" + FEmpNombre.Text + "',Apellido='" + FEmpApellido.Text + "',Fecha_nac='" + FEmpFechanac.Text + "',Genero='" + FEmpGenero.Text + "',Documento='" + FEmpDoc.Text + "',Email='" + FEmpEmail.Text + "',Domicilio='" + FEmpDomicilio.Text + "' where Id=" + FEmpId.Text;
            cmd.ExecuteNonQuery();
            BindGrid();
            MessageBox.Show("Registro Actualizado Correctamente");
            //pbar.Value = 100;
            ClearAll();
            desactivarBtns();
        }
        //eliminar un empleado
        private void btn_borrar_Click(object sender, RoutedEventArgs e)
        {
            if (tBL_EMPLEDataGrid.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)tBL_EMPLEDataGrid.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (conector.State != ConnectionState.Open)
                    conector.Open();
                cmd.Connection = conector;
                cmd.CommandText = "delete from TBL_EMPLE where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Registro borrado exitosamente");
                //pbar.Value = 100;
                ClearAll();
                tBL_EMPLEDataGrid.ItemsSource = dt.AsDataView();

            }
            else
            {
                MessageBox.Show("Seleccione un registro para eliminar");
            }
        }
        //Buscar un empleado
        private void buscar_Click(object sender, RoutedEventArgs e)
        {
            BindGrid();
        }
        //Borrar todos los campos cuando cancelo la operacion
        private void ClearAll()
        {
            FEmpNombre.Text = "";
            FEmpApellido.Text = "";
            FEmpFechanac.Text = "";
            FEmpGenero.SelectedIndex = 0;
            FEmpDoc.Text = "";
            FEmpEmail.Text = "";
            FEmpDomicilio.Text = "";
            FEmpId.IsEnabled = false;
        }

        private void tBL_EMPLEDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //Cancelo la modificacion
        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            //borro todos los campos y desactivo btns actualizar y cancelar
            ClearAll();
            desactivarBtns();
        }
        //activar botones [actualizar][cancelar] * desactivar el resto de botones
        private void activarBtns()
        {
            btn_Actualizar.IsEnabled = true;
            btn_Actualizar.Visibility = Visibility;
            btn_Cancelar.IsEnabled = true;
            btn_Cancelar.Visibility = Visibility;
            btn_Guardar.IsEnabled = false;
            //btn_Guardar.Visibility = Hidden;
            btn_Modificar.IsEnabled = false;
            //btn_Modificar.Visibility = Hide;
            btn_Eliminar.IsEnabled = false;
        }
        //desactivar botones [actualizar][cancelar] y activar el resto de botones
        private void desactivarBtns()
        {
            btn_Actualizar.IsEnabled = false;
            btn_Actualizar.Visibility = Visibility.Hidden;
            btn_Cancelar.IsEnabled = false;
            btn_Cancelar.Visibility = Visibility.Hidden;
            btn_Guardar.IsEnabled = true;
            //btn_Guardar.Visibility = Hidden;
            btn_Modificar.IsEnabled = true;
            //btn_Modificar.Visibility = Hide;
            btn_Eliminar.IsEnabled = true;
        }
    }
}





