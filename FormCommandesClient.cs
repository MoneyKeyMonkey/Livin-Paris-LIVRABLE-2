//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinFormsApp1
//{
//    public class FormCommandesClient : Form
//    {
//        private DataGridView dataGridView;
//        private string clientId;

//        public FormCommandesClient(string clientId)
//        {
//            this.clientId = clientId;
//            this.Text = "Commandes du client";
//            this.Size = new Size(800, 400);

//            dataGridView = new DataGridView { Dock = DockStyle.Fill };
//            this.Controls.Add(dataGridView);

//            LoadCommandes();
//        }

//        private void LoadCommandes()
//        {
//            string connectionString = "Server=localhost;Database=livin_paris;User ID=root;Password=root";
//            string query = @"SELECT c.id_commande, c.date_commande, c.statut, 
//                        GROUP_CONCAT(p.nom SEPARATOR ', ') AS plats
//                        FROM commande c
//                        JOIN commande_plat cp ON c.id_commande = cp.id_commande
//                        JOIN plat p ON cp.id_plat = p.id_plat
//                        WHERE c.id_client = @clientId
//                        GROUP BY c.id_commande";

//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                MySqlCommand cmd = new MySqlCommand(query, connection);
//                cmd.Parameters.AddWithValue("@clientId", clientId);

//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                DataTable dt = new DataTable();
//                adapter.Fill(dt);

//                dataGridView.DataSource = dt;
//            }
//        }
//    }
//}
