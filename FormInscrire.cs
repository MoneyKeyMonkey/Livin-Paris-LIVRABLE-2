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
        NumericUpDown nudNombreUtilisateurs;

        public FormInscrire()
        {
            // Configuration de la fenêtre
            this.Text = "Inscription";
            this.Size = new System.Drawing.Size(350, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Création des labels et champs de texte
            Label lblNom = new Label { Text = "Nom:", Location = new System.Drawing.Point(20, 20) };
            txtNom = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 200 };

            Label lblPrenom = new Label { Text = "Prénom:", Location = new System.Drawing.Point(20, 60) };
            txtPrenom = new TextBox { Location = new System.Drawing.Point(120, 60), Width = 200 };

            Label lblMail = new Label { Text = "Email:", Location = new System.Drawing.Point(20, 100) };
            txtMail = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 200 };

            Label lblTelephone = new Label { Text = "Téléphone:", Location = new System.Drawing.Point(20, 140) };
            txtTelephone = new TextBox { Location = new System.Drawing.Point(120, 140), Width = 200 };

            Label lblAdresse = new Label { Text = "Adresse:", Location = new System.Drawing.Point(20, 180) };
            txtAdresse = new TextBox { Location = new System.Drawing.Point(120, 180), Width = 200 };

            Label lblPassword = new Label { Text = "Mot de passe:", Location = new System.Drawing.Point(20, 220) };
            txtPassword = new TextBox { Location = new System.Drawing.Point(120, 220), Width = 200, PasswordChar = '*' };

            Label lblRole = new Label { Text = "Rôle:", Location = new System.Drawing.Point(20, 260) };
            cmbRole = new ComboBox { Location = new System.Drawing.Point(120, 260), Width = 200 };
            cmbRole.Items.AddRange(new string[] { "client", "entreprise", "cuisinier" });
            cmbRole.SelectedIndex = 0; // Par défaut "client"

            Button btnInscrire = new Button { Text = "S'inscrire", Location = new System.Drawing.Point(120, 310), Width = 200 };
            btnInscrire.Click += BtnInscrire_Click;

            // Nouvelle section pour la génération d'utilisateurs
            Label lblSeparator = new Label { Text = "─────── Génération automatique ───────", Location = new System.Drawing.Point(20, 360), Width = 300, TextAlign = ContentAlignment.MiddleCenter };

            Label lblNombreUtilisateurs = new Label { Text = "Nombre:", Location = new System.Drawing.Point(20, 390) };
            nudNombreUtilisateurs = new NumericUpDown { Location = new System.Drawing.Point(120, 390), Width = 200, Minimum = 1, Maximum = 100, Value = 10 };

            Button btnGenerateUsers = new Button { Text = "Générer les utilisateurs", Location = new System.Drawing.Point(120, 430), Width = 200 };
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
            this.Controls.Add(lblSeparator);
            this.Controls.Add(lblNombreUtilisateurs);
            this.Controls.Add(nudNombreUtilisateurs);
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

            // Créer l'utilisateur à partir des champs du formulaire
            Utilisateur nouvelUtilisateur = new Utilisateur
            {
                Nom = txtNom.Text,
                Prenom = txtPrenom.Text,
                Email = txtMail.Text,
                Telephone = txtTelephone.Text,
                MotDePasse = txtPassword.Text,
                Type = cmbRole.SelectedItem.ToString(),
                EstActif = true
            };

            // Sauvegarder l'utilisateur dans la base de données
            if (nouvelUtilisateur.SauvegarderEnBDD())
            {
                MessageBox.Show("Inscription réussie!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void BtnGenerateUsers_Click(object sender, EventArgs e)
        {
            int nombreUtilisateurs = (int)nudNombreUtilisateurs.Value;

            Cursor = Cursors.WaitCursor;

            try
            {
                // Utiliser la méthode de génération automatique de la classe Utilisateur
                int compteur = Utilisateur.GenererEtSauvegarderUtilisateurs(nombreUtilisateurs);

                MessageBox.Show($"{compteur} utilisateurs sur {nombreUtilisateurs} ont été générés et sauvegardés dans la base de données!",
                                "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
