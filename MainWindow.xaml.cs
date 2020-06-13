using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CallsignToCountry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // all data goes in here for quick searching
        Dictionary<string, data> lookup = new Dictionary<string, data>();

        public MainWindow()
        {
            InitializeComponent();

            LoadFile();
        }

        //List<data> lookupData = new List<data>();

        private void LoadFile()
        {
            int counter = 0;
            string line;
            data d=null;

            Trace.WriteLine("LoadFile");

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"C:\Users\grmur\source\repos\CallsignToCountry\CallsignToCountry\Data\callsigns.txt");
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                string[] split = line.Split('\t');


                //if (split[0].Length == 2)
                {
                    d = new data();

                    d.Callsign = line;
                    d.Country = split[1];

                    if (split[0].Contains('�'))
                    {

                        Console.WriteLine("Needs work:" + d.Callsign);

                        specialSplit(split[0], d.Country);

                        counter++;
                    }
                    else
                    {
                        // add now
                        d = new data();
                        d.Callsign = split[0];
                        d.Country = split[1];

                        lookup.Add(split[0], d);
                    }
                }
                      

                //else
                //{
                //    d = new data();

                //    d.Callsign = split[0];
                //    d.Country = split[1];

                //    lookup.Add(d.Callsign, d);
                //}
            }

        }

        private void specialSplit(string line, string country)
        {
            Trace.WriteLine("LoadFile");

            string[] split = line.Split('�');

            Console.WriteLine("Split: " + split[0] + " " + split[1]);

           // if (split[0].Length == 1)
            {

                char start = split[0][1];
                char end = split[1][1];

                Console.WriteLine("found " + start + "  " + end);

                string nstring = split[0];

                for (char x = start; x < end + 1; x++)
                {
                    char c = split[0][0];
                    // nstring = (char) c + (char) x;

                    string tmp = x.ToString();
                    nstring = split[0][0] + tmp;


                    Console.WriteLine("RES = " + nstring);

                    data d = new data();
                    d.Country = country;
                    d.Callsign = nstring;

                    if (!lookup.ContainsKey(nstring))
                    {
                        lookup.Add(nstring, d);
                    }

                }
            }
        }

        private void RunQuery(object sender, RoutedEventArgs e)
        {
            data d=null;

            string sign = theCallsign.Text;
             sign=sign.ToUpper();

            string sub = sign.Substring(0, 2);

            if (lookup.ContainsKey(sub))
            { 
                d = lookup[sub];
            }
            else
            {
                if (!lookup.ContainsKey(sub))
                {
                    sub = sign.Substring(0, 1);
                    d = lookup[sub];
                }
                else
                {
                    Result.Content = "Not a clue";
                    return;
                }
            }

            Result.Content = d.Country;

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
