using MySql.Data.MySqlClient;
using System;
using System.IO;  // Pour File et StreamWriter
using System.Windows.Forms;

namespace LivinParisApp
{
    public class FormInscrire : Form
    {
        TextBox txtNom, txtPrenom, txtMail, txtTelephone, txtAdresse, txtPassword;
        ComboBox cmbRole;

        public FormInscrire()
        {
            // Configuration de la fenêtre
            this.Text = "Inscription";
            this.Size = new System.Drawing.Size(300, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Création des labels et champs de texte
            Label lblNom = new Label { Text = "Nom:", Location = new System.Drawing.Point(20, 20) };
            txtNom = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 150 };

            Label lblPrenom = new Label { Text = "Prénom:", Location = new System.Drawing.Point(20, 60) };
            txtPrenom = new TextBox { Location = new System.Drawing.Point(120, 60), Width = 150 };

            Label lblMail = new Label { Text = "Email:", Location = new System.Drawing.Point(20, 100) };
            txtMail = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 150 };

            Label lblTelephone = new Label { Text = "Téléphone:", Location = new System.Drawing.Point(20, 140) };
            txtTelephone = new TextBox { Location = new System.Drawing.Point(120, 140), Width = 150 };

            Label lblAdresse = new Label { Text = "Adresse:", Location = new System.Drawing.Point(20, 180) };
            txtAdresse = new TextBox { Location = new System.Drawing.Point(120, 180), Width = 150 };

            Label lblPassword = new Label { Text = "Mot de passe:", Location = new System.Drawing.Point(20, 220) };
            txtPassword = new TextBox { Location = new System.Drawing.Point(120, 220), Width = 150, PasswordChar = '*' };

            Label lblRole = new Label { Text = "Rôle:", Location = new System.Drawing.Point(20, 260) };
            cmbRole = new ComboBox { Location = new System.Drawing.Point(120, 260), Width = 150 };
            cmbRole.Items.AddRange(new string[] { "Cuisinier", "Client", "Entreprise" });

            Button btnInscrire = new Button { Text = "S'inscrire", Location = new System.Drawing.Point(100, 310) };
            btnInscrire.Click += BtnInscrire_Click;

            Button btnGenerateUsers = new Button { Text = "Générer 10 utilisateurs", Location = new System.Drawing.Point(100, 350) };
            btnGenerateUsers.Click += BtnGenerateUsers_Click;

            // Ajout des contrôles au formulaire
            this.Controls.Add(lblNom);
            this.Controls.Add(txtNom);
            this.Controls.Add(lblPrenom);
            this.Controls.Add(txtPrenom);
            this.Controls.Add(lblMail);
            this.Controls.Add(txtMail);
            this.Controls.Add(lblTelephone);
            this.Controls.Add(txtTelephone);
            this.Controls.Add(lblAdresse);
            this.Controls.Add(txtAdresse);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblRole);
            this.Controls.Add(cmbRole);
            this.Controls.Add(btnInscrire);
            this.Controls.Add(btnGenerateUsers);
        }

        private void BtnInscrire_Click(object sender, EventArgs e)
        {
            // Vérification des champs obligatoires
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtMail.Text) || string.IsNullOrWhiteSpace(txtTelephone.Text) ||
                string.IsNullOrWhiteSpace(txtAdresse.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Tous les champs obligatoires doivent être remplis.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Server=localhost;Database=food_delivery;User ID=root;Password=root";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO utilisateur (nom, prenom, email, num_tel, adresse, mdp, role) " +
                                   "VALUES (@Nom, @Prenom, @Email, @Telephone, @Adresse, SHA2(@MotDePasse, 256), @Role)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nom", txtNom.Text);
                        cmd.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
                        cmd.Parameters.AddWithValue("@Email", txtMail.Text);
                        cmd.Parameters.AddWithValue("@Telephone", txtTelephone.Text);
                        cmd.Parameters.AddWithValue("@Adresse", txtAdresse.Text);
                        cmd.Parameters.AddWithValue("@MotDePasse", txtPassword.Text);
                        cmd.Parameters.AddWithValue("@Role", cmbRole.SelectedItem.ToString().ToLower());

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Inscription réussie et sauvegardée dans la base de données !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("'utilisateur.email'")) // Code d'erreur pour une violation de contrainte unique
                    {
                        MessageBox.Show("L'email est déjà utilisé. Veuillez en choisir un autre.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnGenerateUsers_Click(object sender, EventArgs e)
        {
            string[] noms = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
            string[] prenoms = { "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles", "Christopher" };
            string[] roles = { "cuisinier", "client", "entreprise" };
            string[] addresses = {
                "1 Rue de Rivoli, 75001 Paris",
                "2 Avenue des Champs-Élysées, 75008 Paris",
                "3 Boulevard Saint-Germain, 75005 Paris",
                "4 Rue de la Paix, 75002 Paris",
                "5 Avenue Montaigne, 75008 Paris",
                "6 Rue du Faubourg Saint-Honoré, 75008 Paris",
                "7 Rue de la Boétie, 75008 Paris",
                "8 Rue de Vaugirard, 75006 Paris",
                "9 Rue de Rennes, 75006 Paris",
                "10 Rue de la Pompe, 75016 Paris"
            };

            Random random = new Random();

            string connectionString = "Server=localhost;Database=food_delivery;User ID=root;Password=root";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    for (int i = 0; i < 10; i++)
                    {
                        string nom = noms[random.Next(noms.Length)];
                        string prenom = prenoms[random.Next(prenoms.Length)];
                        string email = $"{prenom.ToLower()}.{nom.ToLower()}@example.com";
                        string telephone = "0123456789";    
                        string adresse = addresses[random.Next(addresses.Length)];
                        string motDePasse = prenom.ToLower();
                        string role = roles[random.Next(roles.Length)];

                        string query = "INSERT INTO utilisateur (nom, prenom, email, num_tel, adresse, mdp, role) " +
                                       "VALUES (@Nom, @Prenom, @Email, @Telephone, @Adresse, SHA2(@MotDePasse, 256), @Role)";

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Nom", nom);
                            cmd.Parameters.AddWithValue("@Prenom", prenom);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Telephone", telephone);
                            cmd.Parameters.AddWithValue("@Adresse", adresse);
                            cmd.Parameters.AddWithValue("@MotDePasse", motDePasse);
                            cmd.Parameters.AddWithValue("@Role", role);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("10 utilisateurs générés et sauvegardés dans la base de données !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
