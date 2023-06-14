namespace Application.AutoMapping
{
    using System.Globalization;
    using AutoMapper;

	using Domain.Models;

	public class TrafficProfile : Profile
	{
		public TrafficProfile()
		{
			CreateMap<TrafficCSV, Traffic>().ForMember(x => x.LatestDate,
				y => y.MapFrom(z => DateTime.ParseExact(z.LatestDate, "M/d/yyyy", CultureInfo.InvariantCulture)));
        }
	}
}

