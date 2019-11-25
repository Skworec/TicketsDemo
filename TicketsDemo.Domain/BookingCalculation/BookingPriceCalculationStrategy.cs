using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;
using TicketsDemo.Domain.Interfaces;

namespace TicketsDemo.Domain.BookingImplementations.PriceCalculationStrategy
{
    public class BookingPriceCalculationStrategy : IPriceCalculationStrategy
    {
        private IRunRepository _runRepository;
        private ITrainRepository _trainRepository;

        public BookingPriceCalculationStrategy(IRunRepository runRepository, ITrainRepository trainRepository) {
            _runRepository = runRepository;
            _trainRepository = trainRepository;
        }

        public List<PriceComponent> CalculatePrice(PlaceInRun placeInRun)
        {
            var components = new List<PriceComponent>();

            var run = _runRepository.GetRunDetails(placeInRun.RunId);
            var train = _trainRepository.GetTrainDetails(run.TrainId);
            var place =
                train.Carriages
                    .Select(car => car.Places.FirstOrDefault(pl =>
                        pl.Number == placeInRun.Number &&
                        car.Number == placeInRun.CarriageNumber))
                    .SingleOrDefault(x => x != null);

            var cashDeskComponent = new PriceComponent()
            {
                Name = "Booking service tax",
                Value = place.Carriage.DefaultPrice * place.PriceMultiplier * Convert.ToDecimal(train.Booking.OverPrice)
            };
            components.Add(cashDeskComponent);

            return components;
        }
    }
}
