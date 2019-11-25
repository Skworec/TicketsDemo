using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;
using TicketsDemo.Domain.BookingImplementations.PriceCalculationStrategy;
using TicketsDemo.Domain.DefaultImplementations.PriceCalculationStrategy;
using TicketsDemo.Domain.Interfaces;

namespace TicketsDemo.Domain.UniversalImplementations.PriceCalculationStrategy
{
    public class UniversalPriceCalculationStrategy : IPriceCalculationStrategy
    {
        private IRunRepository _runRepository;
        private ITrainRepository _trainRepository;

        public UniversalPriceCalculationStrategy(IRunRepository runRepository, ITrainRepository trainRepository)
        {
            _runRepository = runRepository;
            _trainRepository = trainRepository;
        }

        public List<PriceComponent> CalculatePrice(PlaceInRun placeInRun)
        {
            var run = _runRepository.GetRunDetails(placeInRun.RunId);
            var train = _trainRepository.GetTrainDetails(run.TrainId);

            var components = new DefaultPriceCalculationStrategy(_runRepository, _trainRepository).
                CalculatePrice(placeInRun);

            if (_trainRepository.GetTrainDetails(placeInRun.Run.TrainId).Booking != null)
            {
                components.AddRange(new BookingPriceCalculationStrategy(_runRepository, _trainRepository).
                    CalculatePrice(placeInRun));
            }

            return components;
        }
    }
}
