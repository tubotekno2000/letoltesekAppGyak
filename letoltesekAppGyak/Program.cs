
using letoltesekApp;
using MySql.Data.MySqlClient;

namespace letoltesekAppGyak
{
    internal class Program
    {
        const string sqlConn = "server=localhost;userid=root;password=;database=letoltesekapp;";

        static void Main(string[] args)
        {

            //IDE ÍRD BE A NEVED: M.A
            {
                byte menuPont;
                do
                {
                    Console.Clear();
                    menuPont = menu();
                    if (menuPont == 1)
                    {
                        Letoltesek();
                    }

                    if (menuPont == 2)
                    {
                        Statisztika();
                    }

                    if (menuPont == 3)
                    {
                        Feltolt();
                    }

                    if (menuPont == 4)
                    {
                        Lekerdezes();
                    }
                } while (menuPont != 5);



            }
            static byte menu()
            {

                string[] menuElemek = new string[]
                    {   "Letöltések száma",
                    "Országonkénti statisztika",
                    "Új letöltés rögzítése",
                    "IP címosztályok számlálója",
                    "Kilépés"};
                for (byte k = 0; k < menuElemek.Length; k++)
                    Console.WriteLine($"{k + 1}. {menuElemek[k]}");
                Console.Write("\nVálassz a fenti lehetőségek közül: ");
                return byte.TryParse(Console.ReadLine(), out byte menuPont) ? menuPont : (byte)5;

            }
        }

        private static void Lekerdezes()
        {
            List<Letoltes> letoltes = getLetoltesek();

            var elsoOktet = letoltes.Select(x => int.Parse(x.letoltesIP.Split('.')[0])).ToList();

            var classA = elsoOktet.Count(octet => octet >= 1 && octet <= 127);
            var classB = elsoOktet.Count(octet => octet >= 128 && octet <= 191);
            var classC = elsoOktet.Count(octet => octet >= 192 && octet <= 223);

            Console.WriteLine($"A osztályú IP címek száma: {classA}");
            Console.WriteLine($"B osztályú IP címek száma: {classB}");
            Console.WriteLine($"C osztályú IP címek száma: {classC}");
            Console.ReadKey();
        }


        private static List<Letoltes> getLetoltesek()
        {
            MySqlConnection conn = new (sqlConn);
            MySqlCommand cmd = new("SELECT * FROM letoltesek", conn);
            List<Letoltes> letoltesek = new();
            try
            {
                conn.Open();
                cmd.Prepare();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    letoltesek.Add(new Letoltes(reader.GetString("orszagKod"), reader.GetString("orszag"), reader.GetString("alkalmazasNev"), reader.GetString("letoltesIP"), reader.GetString("alkalmazasVerzio"), reader.GetDateTime("letoltesDatum")));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                conn.Close();
            }
            return letoltesek;
        }

        private static void Feltolt()
        {
            Console.WriteLine("Új rekord felvitele");
            Console.WriteLine("Ország név:");
            string orszagNev = Console.ReadLine();
            Console.WriteLine("Ország kód:");
            string orszagKod = Console.ReadLine();
            Console.WriteLine("Alkalmazás név:");
            string alkalmazasNev = Console.ReadLine();
            Console.WriteLine("Alkalmazás verzió:");
            string alkalmazasVerzio = Console.ReadLine();
            Console.WriteLine("Letöltés IP címe:");
            string letoltesIP = Console.ReadLine();
            //YYYY-MM-DD 
            DateTime letoltesDatum = DateTime.Now;
            Letoltes letoltes = new(orszagKod, orszagNev, alkalmazasNev, letoltesIP, alkalmazasVerzio, letoltesDatum);
            MySqlConnection conn = new (sqlConn);
            MySqlCommand cmd = new("INSERT INTO letoltesek (orszag, orszagKod, alkalmazasNev, letoltesIP, alkalmazasVerzio, letoltesDatum) VALUES (@orszag, @orszagKod, @alkalmazasNev, @letoltesIP, @alkalmazasVerzio, @letoltesDatum)", conn);
            cmd.Parameters.AddWithValue("@orszag", letoltes.orszagNev);
            cmd.Parameters.AddWithValue("@orszagKod", letoltes.orszagKod);
            cmd.Parameters.AddWithValue("@alkalmazasNev", letoltes.alkalamzasNev);
            cmd.Parameters.AddWithValue("@letoltesIP", letoltes.letoltesIP);
            cmd.Parameters.AddWithValue("@alkalmazasVerzio", letoltes.alkalamzasVerzio);
            cmd.Parameters.AddWithValue("@letoltesDatum", letoltes.letoltesDatum.ToString("yyyy:mm:dd"));
            try
            {
                conn.Open();
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Sikeres feltöltés");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            Console.ReadKey();
            
        }

        private static void Statisztika()
        {
            //hány letoltes volt országonként

            MySqlConnection conn = new (sqlConn);
            MySqlCommand cmd = new("SELECT orszag, COUNT(*) FROM letoltesek GROUP BY orszag", conn);
            List<string> stat = new();
            try
            {
                conn.Open();
                cmd.Prepare();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stat.Add($"{reader.GetString("orszag")} - {reader.GetString("COUNT(*)")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            foreach (var item in stat)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }

        private static void Letoltesek()
        {
            MySqlConnection conn = new (sqlConn);
            //count records for database
            MySqlCommand cmd = new("SELECT COUNT(*) FROM letoltesek", conn);
            string? count = null;
            try
            {
                conn.Open();
                cmd.Prepare();
                count=cmd.ExecuteScalar().ToString();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            if(string.IsNullOrEmpty(count))
            {
                Console.WriteLine("Nincs adat");
                return;
            }
            else
            {
                Console.WriteLine($"A letöltések száma: {count}");
            }

            Console.ReadKey();
            
        }
    }
}
