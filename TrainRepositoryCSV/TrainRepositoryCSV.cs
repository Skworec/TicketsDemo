using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Collections.Generic;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;
using System.Globalization;

namespace TicketsDemo.CSV
{
    public class TrainRepositoryCSV : ITrainRepository
    {
        public static TextFieldParser MyCSVParser;
        private List<Train> Trains = new List<Train>();

        #region Parse
        public Train TrainFromCSV(string[] format)
        {
            Train returningTrain = new Train();
            returningTrain.Id = int.Parse(format[0]);
            returningTrain.Number = int.Parse(format[1]);
            returningTrain.StartLocation = format[2];
            returningTrain.EndLocation = format[3];
            returningTrain.Carriages = new List<Carriage>();
            if (format[4] != null)
                returningTrain.BookingId = int.Parse(format[4]);
            return returningTrain;
        }

        public Carriage CarriageFromCSV(string[] format)
        {
            Carriage returningCarriage = new Carriage();
            returningCarriage.Id = int.Parse(format[0]);
            switch (int.Parse(format[1]))
            {
                case 1:
                    {
                        returningCarriage.Type = CarriageType.Sedentary;
                        break;
                    }
                case 2:
                    {
                        returningCarriage.Type = CarriageType.FirstClassSleeping;
                        break;
                    }
                case 3:
                    {
                        returningCarriage.Type = CarriageType.SecondClassSleeping;
                        break;
                    }
            }
            returningCarriage.DefaultPrice = int.Parse(format[2]);
            returningCarriage.TrainId = int.Parse(format[3]);
            returningCarriage.Number = int.Parse(format[4]);
            returningCarriage.Places = new List<Place>();
            return returningCarriage;
        }

        public BookingCompany BookingFromCSV(string[] format)
        {
            BookingCompany returningCompany = new BookingCompany();
            returningCompany.Id = int.Parse(format[0]);
            returningCompany.Name = format[1];
            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            returningCompany.OverPrice = float.Parse(format[2], culture);
            return returningCompany;
        }

        /// <summary>
        /// Сделать нормально
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public Place PlaceFromCSV(string[] format)
        {
            Place returningPlace = new Place();
            returningPlace.Id = int.Parse(format[0]);
            returningPlace.Number = int.Parse(format[1]);
            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            returningPlace.PriceMultiplier = decimal.Parse(format[2], culture);
            returningPlace.CarriageId = int.Parse(format[3]);
            return returningPlace;
        }
        public void InitiateParser(string filename)
        {
            string path = "D:\\Git\\TicketsDemo\\TicketsDemo.Data\\";
            MyCSVParser = new TextFieldParser(string.Format("{0}{1}.txt", path, filename));
            MyCSVParser.Delimiters = new string[] { "," };
        }
        #endregion Parse

        #region ITrainRepository Members
        public List<Train> GetAllTrains()
        {
            InitiateParser("Train");
            while (true)
            {
                string[] field = MyCSVParser.ReadFields();
                if (field == null)
                {
                    break;
                }
                Trains.Add(TrainFromCSV(field));
            }
            return Trains;
        }

        public Train GetTrainDetails(int trainId)
        {
            List<Train> tr = GetAllTrains();
            Train returningTrain = new Train();
            tr.ForEach(train =>
        {
            if (train.Id == trainId)
                returningTrain = train;
        });
            InitiateParser("Carriages");
            List<Carriage> carriages = new List<Carriage>();
            while (true)
            {
                string[] field = MyCSVParser.ReadFields();
                if (field == null)
                {
                    break;
                }
                carriages.Add(CarriageFromCSV(field));
            }

            InitiateParser("BookingCompanies");
            List<BookingCompany> bookingCompanies = new List<BookingCompany>();
            while (true)
            {
                string[] field = MyCSVParser.ReadFields();
                if (field == null)
                {
                    break;
                }
                bookingCompanies.Add(BookingFromCSV(field));
            }
            InitiateParser("Places");
            List<Place> places = new List<Place>();
            while (true)
            {
                string[] field = MyCSVParser.ReadFields();
                if (field == null)
                {
                    break;
                }
                places.Add(PlaceFromCSV(field));
            }
            foreach (Train x in Trains)
            {
                if (x.Id == trainId)
                {
                    foreach (Carriage y in carriages)
                    {
                        if (x.Id == y.TrainId)
                        {
                            x.Carriages.Add(y);
                            y.Train = x;

                            foreach (Place p in places)
                            {
                                if (p.CarriageId == y.Id)
                                {
                                    y.Places.Add(p);
                                    p.Carriage = y;
                                }
                            }
                        }
                    }
                    foreach (BookingCompany y in bookingCompanies)
                    {
                        if (x.BookingId == y.Id)
                        {
                            x.Booking = y;
                        }
                    }
                    returningTrain = x;
                }
            }

            return returningTrain;
        }

        public void CreateTrain(Train train)
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string file = "Train";
            string fullPath = string.Format("'{0}'\'{1}'.txt", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), file);
            StreamWriter streamWriter = new StreamWriter(fullPath, true);
            streamWriter.WriteLine(train.ToString());
            if (train.Carriages != null)
            {
                file = "Carriages";
                fullPath = string.Format("'{0}'\'{1}'.txt", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), file);
                streamWriter = new StreamWriter(fullPath, true);
                streamWriter.WriteLine(train.Carriages.ToString());

            }
            if (train.Booking != null)
            {
                file = "BookingCompanies";
                fullPath = string.Format("'{0}'\'{1}'.txt", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), file);
                streamWriter = new StreamWriter(fullPath, true);
                streamWriter.WriteLine(train.Booking.ToString());
            }
            streamWriter.Close();
        }

        public void DeleteTrain(Train train)
        {
            string tempFilePath = "TrainTmp";
            string fullTempPath = string.Format("'{0}'\'{1}'.txt", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), tempFilePath);
            string fullPath = string.Format("'{0}'\'{1}'.txt", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Train");
            StreamReader sr = new StreamReader(fullPath);
            StreamWriter sw = new StreamWriter(fullTempPath);
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                if (line != train.ToString())
                    sw.WriteLine(line);
            }

            File.Delete(fullPath);
            File.Move(fullTempPath, fullPath);
        }

        public void UpdateTrain(Train train)
        {
            string tempFilePath = "TrainTmp";
            string fullTempPath = string.Format("'{0}'\'{1}'.txt", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), tempFilePath);
            string fullPath = string.Format("'{0}'\'{1}'.txt", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Train");
            StreamReader sr = new StreamReader(fullPath);
            StreamWriter sw = new StreamWriter(fullTempPath);
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                if (line != train.ToString())
                    sw.WriteLine(line);
                else
                {
                    sw.WriteLine(train.ToString());
                }
            }

            File.Delete(fullPath);
            File.Move(fullTempPath, fullPath);
        }
        #endregion ITrainRepository Members
    }
}
