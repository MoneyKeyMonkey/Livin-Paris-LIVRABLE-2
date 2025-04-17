// Modèle Utilisateur
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormsApp1;

public class Utilisateur
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string MotDePasse { get; set; }
    public string Type { get; set; }
    public bool EstActif { get; set; }
    
    // Variables statiques pour la génération d'utilisateurs
    private static List<string> prenoms = new List<string>();
    private static List<string> noms = new List<string>();
    private static Random random = new Random();
    private static bool estCharge = false;
    
    // Charge les prénoms et noms depuis le fichier nppi.csv
    public static void ChargerNomsDepuisCSV()
    {
        if (estCharge)
            return;
            
        try
        {
            // Chemin vers le fichier CSV
            string cheminFichier = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nppi.csv");
            
            // Lecture des lignes du fichier
            string[] lignes = File.ReadAllLines(cheminFichier);
            
            // Ignorer l'en-tête
            for (int i = 1; i < lignes.Length; i++)
            {
                string[] colonnes = lignes[i].Split(';');
                if (colonnes.Length >= 2)
                {
                    prenoms.Add(colonnes[0]);
                    noms.Add(colonnes[1]);
                }
            }
            
            estCharge = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors du chargement du fichier: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    // Génère un numéro de téléphone à 10 chiffres commençant par 06
    private static string GenererTelephone()
    {
        StringBuilder sb = new StringBuilder("06");
        for (int i = 0; i < 8; i++)
        {
            sb.Append(random.Next(10));
        }
        return sb.ToString();
    }
    
    // Génère un mot de passe en mélangeant les lettres du prénom
    private static string GenererMotDePasse(string prenom)
    {
        char[] chars = prenom.ToCharArray();
        
        // Algorithme de mélange Fisher-Yates
        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            char temp = chars[i];
            chars[i] = chars[j];
            chars[j] = temp;
        }
        
        return new string(chars);
    }
    
    // Détermine le type d'utilisateur selon les probabilités spécifiées
    private static string DeterminerType()
    {
        int val = random.Next(15);
        if (val < 3) // 3/15 = 1/5 chance
            return "cuisinier";
        else if (val == 3) // 1/15 chance
            return "entreprise";
        else
            return "client";
    }
    
    // Génère un utilisateur aléatoire
    public static Utilisateur GenererUtilisateurAleatoire()
    {
        // S'assure que les noms sont chargés
        ChargerNomsDepuisCSV();
        
        if (prenoms.Count == 0 || noms.Count == 0)
            return null;
            
        string prenom = prenoms[random.Next(prenoms.Count)];
        string nom = noms[random.Next(noms.Count)];
        string email = $"{prenom.ToLower()}{nom.ToLower()}@livin.p";
        string telephone = GenererTelephone();
        string motDePasse = GenererMotDePasse(prenom);
        string type = DeterminerType();
        
        return new Utilisateur
        {
            Prenom = prenom,
            Nom = nom,
            Email = email,
            Telephone = telephone,
            MotDePasse = motDePasse,
            Type = type,
            EstActif = true
        };
    }
    
    // Génère plusieurs utilisateurs aléatoires
    public static List<Utilisateur> GenererUtilisateursAleatoires(int nombre)
    {
        List<Utilisateur> utilisateurs = new List<Utilisateur>();
        for (int i = 0; i < nombre; i++)
        {
            Utilisateur utilisateur = GenererUtilisateurAleatoire();
            if (utilisateur != null)
                utilisateurs.Add(utilisateur);
        }
        return utilisateurs;
    }
    
    // Méthode pour sauvegarder un utilisateur dans la base de données
    public bool SauvegarderEnBDD()
    {
        string connectionString = "Server=localhost;Database=livin_paris;Uid=root;Pwd=;";
        
        using (MySqlConnection connexion = new MySqlConnection(connectionString))
        {
            try
            {
                connexion.Open();
                string requete = @"INSERT INTO utilisateurs (nom, prenom, email, telephone, mot_de_passe, type, est_actif) 
                                 VALUES (@nom, @prenom, @email, @telephone, @motDePasse, @type, @estActif)";
                
                using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@nom", this.Nom);
                    commande.Parameters.AddWithValue("@prenom", this.Prenom);
                    commande.Parameters.AddWithValue("@email", this.Email);
                    commande.Parameters.AddWithValue("@telephone", this.Telephone);
                    commande.Parameters.AddWithValue("@motDePasse", this.MotDePasse);
                    commande.Parameters.AddWithValue("@type", this.Type);
                    commande.Parameters.AddWithValue("@estActif", this.EstActif);
                    
                    commande.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
    
    // Méthode statique pour sauvegarder une liste d'utilisateurs
    public static int SauvegarderListeEnBDD(List<Utilisateur> utilisateurs)
    {
        int compteurSuccess = 0;
        foreach (var utilisateur in utilisateurs)
        {
            if (utilisateur.SauvegarderEnBDD())
                compteurSuccess++;
        }
        return compteurSuccess;
    }
    
    // Méthode pour générer et enregistrer plusieurs utilisateurs directement
    public static int GenererEtSauvegarderUtilisateurs(int nombre)
    {
        List<Utilisateur> utilisateurs = GenererUtilisateursAleatoires(nombre);
        return SauvegarderListeEnBDD(utilisateurs);
    }
}
