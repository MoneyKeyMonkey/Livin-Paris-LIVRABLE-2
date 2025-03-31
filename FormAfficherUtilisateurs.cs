//using MySql.Data.MySqlClient;
//using System;
//using System.Data;
//using System.Windows.Forms;

//namespace WinFormsApp1
//{
//    public class FormAfficherUtilisateurs : Form
//    {
//        DataGridView dataGridView;

//        public FormAfficherUtilisateurs()
//        {
//            this.Text = "Liste des Utilisateurs";
//            this.Size = new System.Drawing.Size(600, 400);
//            this.StartPosition = FormStartPosition.CenterScreen;

//            dataGridView = new DataGridView { Dock = DockStyle.Fill };
//            this.Controls.Add(dataGridView);

//            LoadUsers();
//        }

//        private void LoadUsers()
//        {
//            string connectionString = "Server=localhost;Database=livin_paris;User ID=root;Password=root";
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    // Vérifier d'abord si la colonne role existe
//                    string checkColumnQuery = "SELECT COUNT(*) FROM information_schema.columns " +
//                                            "WHERE table_schema = 'livin_paris' " +
//                                            "AND table_name = 'utilisateur' " +
//                                            "AND column_name = 'role'";

//                    using (MySqlCommand checkCmd = new MySqlCommand(checkColumnQuery, connection))
//                    {
//                        int columnExists = Convert.ToInt32(checkCmd.ExecuteScalar());

//                        string mainQuery;
//                        if (columnExists > 0)
//                        {
//                            mainQuery = "SELECT id, nom, prenom, email, adresse, role FROM utilisateur WHERE role = 'client'";
//                        }
//                        else
//                        {
//                            mainQuery = "SELECT id, nom, prenom, email, adresse FROM utilisateur";
//                            MessageBox.Show("La colonne 'role' n'existe pas encore. Veuillez mettre à jour la structure de la base de données.",
//                                          "Information",
//                                          MessageBoxButtons.OK,
//                                          MessageBoxIcon.Information);
//                        }

//                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(mainQuery, connection))
//                        {
//                            DataTable dataTable = new DataTable();
//                            adapter.Fill(dataTable);
//                            dataGridView.DataSource = dataTable;
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Erreur: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }
//        // Gestion du clic sur le bouton "Voir commandes"
//        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.ColumnIndex == dataGridView.Columns["Commandes"].Index && e.RowIndex >= 0)
//            {
//                string idClient = dataGridView.Rows[e.RowIndex].Cells["id"].Value.ToString();
//                FormCommandesClient formCommandes = new FormCommandesClient(idClient);
//                formCommandes.ShowDialog();
//            }
//        }
//    }
//}
