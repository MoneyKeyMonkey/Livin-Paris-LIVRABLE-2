using liv_inParis;
using System.Text;
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Déclarez les variables comme membres de la classe
        private MetroDataService metroData;
        private List<Station> stations;
        private List<Lien> connexions;
        private Graphe graphe;
        private Station? stationSelectionnee;
        private Station? stationDepart;
        private Station? stationArrivee;

        private Button btnAfficherStations;
        private Button btnAfficherLiens;
        private Button btnDijkstra;
        private Button btnAfficherTemps; // Ajoutez ce bouton
        private TextBox txtRechercheStation;
        private ListBox lstStations;

        public Form1()
        {
            InitializeComponent();

            // Créez une instance de MetroDataService
            metroData = new MetroDataService();

            // Accédez aux propriétés via l'instance metroData
            stations = metroData.Stations;  // Au lieu de MetroDataService.Stations
            connexions = metroData.Connexions;  // Au lieu de MetroDataService.Connexions
            // Créez une instance de Graphe
            graphe = new Graphe(connexions);

            // Abonnez-vous à l'événement Paint
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.MouseClick += new MouseEventHandler(Form1_MouseClick);

            // Ajoutez les contrôles
            btnAfficherStations = new Button { Text = "Afficher Stations", Location = new Point(10, 10) };
            btnAfficherLiens = new Button { Text = "Afficher Liens", Location = new Point(150, 10) };
            btnDijkstra = new Button { Text = "Dijkstra", Location = new Point(290, 10) };
            btnAfficherTemps = new Button { Text = "Afficher Temps", Location = new Point(400, 10) }; // Ajoutez ce bouton
            txtRechercheStation = new TextBox { Location = new Point(10, 50), Width = 200 };
            lstStations = new ListBox { Location = new Point(10, 80), Width = 200, Height = 100 };
            // Ajoute les boutons cliquables
            btnAfficherStations.Click += BtnAfficherStations_Click;
            btnAfficherLiens.Click += BtnAfficherLiens_Click;
            btnDijkstra.Click += BtnDijkstra_Click;
            btnAfficherTemps.Click += BtnAfficherTemps_Click; // Ajoutez cet événement
            txtRechercheStation.TextChanged += TxtRechercheStation_TextChanged;
            lstStations.MouseDoubleClick += LstStations_MouseDoubleClick;

            this.Controls.Add(btnAfficherStations);
            this.Controls.Add(btnAfficherLiens);
            this.Controls.Add(btnDijkstra);
            this.Controls.Add(btnAfficherTemps); // Ajoutez ce bouton
            this.Controls.Add(txtRechercheStation);
            this.Controls.Add(lstStations);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Permet de dessiner les stations et les connexions sur le formulaire.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Dessinez les stations et les connexions
            graphe.AfficherStations(e.Graphics, stations, stationSelectionnee);
            graphe.AfficherConnexions(e.Graphics, stations);

            if (stationDepart != null && stationArrivee != null)
            {
                var (path, totalTime) = graphe.GetShortestPath(stationDepart, stationArrivee);
                graphe.AfficherChemin(e.Graphics, path);

            }
        }

        /// <summary>
        /// Permet de sélectionner une station en cliquant dessus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var station in stations)
            {
                int rayon = 6;
                var rect = new Rectangle(station.Position.X - rayon, station.Position.Y - rayon, 2 * rayon, 2 * rayon);

                if (rect.Contains(e.Location))
                {
                    if (stationDepart == null)
                    {
                        stationDepart = station;
                    }
                    else if (stationArrivee == null)
                    {
                        stationArrivee = station;
                    }
                    else
                    {
                        stationDepart = station;
                        stationArrivee = null;
                    }

                    stationSelectionnee = station;
                    this.Invalidate(); // Redessiner le formulaire pour mettre à jour la couleur de la station sélectionnée
                    break;
                }
            }
        }

        /// <summary>
        /// Affiche la liste des stations dans une boîte de dialogue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAfficherStations_Click(object sender, EventArgs e)
        {
            metroData.AfficherStations();
        }

        /// <summary>
        /// Affiche la liste des connexions dans une boîte de dialogue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAfficherLiens_Click(object sender, EventArgs e)
        {
            metroData.AfficherLiens();
        }

        /// <summary>
        /// Recherche le chemin le plus court entre la station de départ et la station d'arrivée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDijkstra_Click(object sender, EventArgs e)
        {
            if (stationDepart != null && stationArrivee != null)
            {
                var (path, totalTime) = graphe.GetShortestPath(stationDepart, stationArrivee);

                // Afficher le résultat ici
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Temps de trajet total : {totalTime} minutes");
                sb.AppendLine("Stations empruntées :");
                sb.AppendLine(string.Join(" --> ", path.Select(s => s.Nom)));
                MessageBox.Show(sb.ToString(), "Chemin le plus court");

                this.Invalidate(); // Redessiner le formulaire pour afficher le chemin
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une station de départ et une station d'arrivée.");
            }
        }

        /// <summary>
        /// Permet d'afficher ou de masquer les temps de trajet sur les connexions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAfficherTemps_Click(object sender, EventArgs e)
        {
            graphe.AfficherTemps = !graphe.AfficherTemps;
            this.Invalidate(); // Redessiner le formulaire pour afficher/masquer les temps de trajet
        }

        /// <summary>
        /// Permet de filtrer les stations par nom.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtRechercheStation_TextChanged(object sender, EventArgs e)
        {
            string recherche = txtRechercheStation.Text.ToLower();
            var resultats = stations.Where(s => s.Nom.ToLower().StartsWith(recherche)).ToList();
            lstStations.DataSource = resultats;
            lstStations.DisplayMember = "Nom";
        }

        /// <summary>
        /// Permet de sélectionner une station en double-cliquant dessus dans le menu déroulant.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstStations_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstStations.SelectedItem is Station station)
            {
                if (stationDepart == null)
                {
                    stationDepart = station;
                }
                else if (stationArrivee == null)
                {
                    stationArrivee = station;
                }
                else
                {
                    stationDepart = station;
                    stationArrivee = null;
                }

                stationSelectionnee = station;
                this.Invalidate(); // Redessiner le formulaire pour mettre à jour la couleur de la station sélectionnée
            }
        }
    }
}
