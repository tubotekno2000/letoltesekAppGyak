using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace letoltesekApp
{
    class Letoltes
    {
        public int Id { get; }
        public string orszagKod { get; }
        public string orszagNev { get; }
        public string alkalamzasNev { get; }
        public string letoltesIP { get; }
        public string alkalamzasVerzio { get; }
        public DateTime letoltesDatum { get; }

        public Letoltes(string orszagKod, string orszagNev, string alkalamzasNev, string letoltesIP, string alkalamzasVerzio, DateTime letoltesDatum)
        {
            this.orszagKod = orszagKod;
            this.orszagNev = orszagNev;
            this.alkalamzasNev = alkalamzasNev;
            this.letoltesIP = letoltesIP;
            this.alkalamzasVerzio = alkalamzasVerzio;
            this.letoltesDatum = letoltesDatum;
        }
        

    }
}
