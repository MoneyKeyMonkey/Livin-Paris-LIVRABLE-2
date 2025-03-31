using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LivinParisApp
{
    public class MainForm : Form
    {
        private MySqlConnection connection;

        public MainForm()
        {
            // Configuration de la fenêtre principale
            this.Text = "LivinParis";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Création du label titre
            Label titleLabel = new Label();
            titleLabel.Text = "LIVINPARIS";
            titleLabel.Font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new System.Drawing.Point(110, 50);
            this.Controls.Add(titleLabel);

            // Bouton Se connecter
            Button btnConnect = new Button();
            btnConnect.Text = "Se connecter";
            btnConnect.Location = new System.Drawing.Point(60, 150);
            btnConnect.Size = new System.Drawing.Size(120, 30);
            btnConnect.Click += (sender, e) => { new FormSeConnecter().Show(); };
            this.Controls.Add(btnConnect);

            // Bouton S'inscrire
            Button btnInscrire = new Button();
            btnInscrire.Text = "S'inscrire";
            btnInscrire.Location = new System.Drawing.Point(200, 150);
            btnInscrire.Size = new System.Drawing.Size(120, 30);
            btnInscrire.Click += (sender, e) => { new FormInscrire().Show(); };
            this.Controls.Add(btnInscrire);

            // Bouton Plan Metro Paris
            Button btnPlanMetro = new Button();
            btnPlanMetro.Text = "Plan Metro Paris";
            btnPlanMetro.Location = new System.Drawing.Point(60, 200);
            btnPlanMetro.Size = new System.Drawing.Size(120, 30);
            btnPlanMetro.Click += (sender, e) => { new Form1().Show(); };
            this.Controls.Add(btnPlanMetro);

            // Bouton Se Déconnecter
            Button btnDeconnecter = new Button();
            btnDeconnecter.Text = "Se Déconnecter";
            btnDeconnecter.Location = new System.Drawing.Point(200, 200);
            btnDeconnecter.Size = new System.Drawing.Size(120, 30);
            btnDeconnecter.Click += BtnDeconnecter_Click;
            this.Controls.Add(btnDeconnecter);

            // Initialiser la connexion à la base de données
            string connectionString = "Server=localhost;Database=food_delivery;User ID=root;Password=root";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        private void BtnDeconnecter_Click(object sender, EventArgs e)
        {
            // Logique de déconnexion de l'utilisateur
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                MessageBox.Show("Utilisateur déconnecté de la base de données.", "Déconnexion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Aucune connexion active à la base de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

