using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DxControls.Progress
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class DCircleProgress : UserControl
    {
        public float percent = 0;
        
        float degrees;
        public double rayon;
        public short largeur;
        Color _Contour;

        public Color Contour { get { return _Contour; } set { _Contour = value; } }
        public DCircleProgress()
        {

            InitializeComponent();

            //line1.Point = new Point(rayon, rayon);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Changement de la couleur de contour
            FormCircle.Stroke = new SolidColorBrush(_Contour);

            progressFig.StartPoint = new Point(rayon * 2, rayon);
            

            arcExt.Size = new Size(rayon, rayon);
           
            arcInt.Size = new Size(rayon - largeur, rayon - largeur);
            degrees = 0;
            //Console.WriteLine($"Centre: {progressFig.StartPoint.ToString()} | Rayon: {rayon}");

            //Positionnement du block texte
            tbPercent.Margin = new Thickness(rayon - tbPercent.Width / 2, rayon - (tbPercent.Height/2), 0 , 0  );

            Update();
            

        }

        // todo peut être séparer le dessin du texte
        public void Update()
        {
            tbPercent.Text = $"{percent.ToString()}%";


            //Conversion pourcent en degrées
            degrees = percent * 360 / 100;

            // Todo Faire une formule directe percent => gradiants ? Tester avant à la calculatrice le rendu exploitables

            //Console.WriteLine(Contour.ToString());
            //Adaptation de l'arc en fonction de l'angle
            switch (degrees) {
                case float n when(n > 180 && n <360):
                    arcExt.IsLargeArc = true;
                    arcInt.IsLargeArc = true;
                    break;
                case float n when(n == 360):
                    arcExt.IsLargeArc = true;
                    arcInt.IsLargeArc = true;

                    degrees = 359.9F;
                    line1.Point = progressFig.StartPoint;
                    break;
                default:
                    arcExt.IsLargeArc = false;
                    arcInt.IsLargeArc = false;

                    break;
            }
            // Conversion de l'angle en radians
            double radians = Math.PI * degrees / 180;

            // Cos & Sin
            double cosAngle = Math.Cos(radians);
            double sinAngle = Math.Sin(radians);
            
            #region arcExt
            // Calcul des coordonnées
            double x = 50 + rayon * cosAngle;
            double y = 50 - (rayon * sinAngle);

            arcExt.Point = new Point(x,y );

           
            #endregion
            
            #region ligne
            double lx = largeur * cosAngle;
            double ly = largeur * sinAngle;
            line1.Point = new Point(arcExt.Point.X - lx, arcExt.Point.Y + ly);
            #endregion

            #region Arc intérieur
            arcInt.Point = new Point( progressFig.StartPoint.X - largeur, progressFig.StartPoint.Y );
            #endregion
            // Informations
            /*
            Console.WriteLine($"Percent: {percent} | Degrés: {degrees}° | Radians: {radians} rads");
            Console.WriteLine($"Cos X: {cosAngle}, Sin X: {sinAngle}");
            Console.WriteLine($"Debut Arc1: ({progressFig.StartPoint.X},{progressFig.StartPoint.Y}) | Fin Arc1: ({x},{y}) | Ligne: {line1.Point.ToString()} | Arc 2: {arcInt.Point.ToString()}");
            */
        }

        private void TextBlock_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void Slider_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            percent = (float)e.NewValue;
            Update();
        }
    }
}
