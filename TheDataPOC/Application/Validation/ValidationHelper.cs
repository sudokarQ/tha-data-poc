namespace Application.Validation
{
    using DTOs;

    public class ValidationHelper
    {
        public static bool Validate(CrashUpdateDto dto)
        {
            bool latitudeCheck = true;
            bool longitudeCheck = true;

            if (dto.Latitude is not null)
            {
                latitudeCheck = IsLatitudeValid(dto.Latitude);
            }

            if (dto.Longitude is not null)
            {
                longitudeCheck = IsLongitudeValid(dto.Longitude);
            }

            return latitudeCheck && longitudeCheck;
        }

        public static bool Validate(TrafficUpdateDto dto)
        {
            bool latitudeCheck = true;
            bool longitudeCheck = true;

            if (dto.Latitude is not null)
            {
                latitudeCheck = IsLatitudeValid(dto.Latitude);
            }

            if (dto.Longitude is not null)
            {
                longitudeCheck = IsLongitudeValid(dto.Longitude);
            }

            return latitudeCheck && longitudeCheck;
        }

        private static bool IsLongitudeValid(double? longitude)
            => longitude >= -180 && longitude <= 180;


        private static bool IsLatitudeValid(double? latitude)
            => latitude >= -180 && latitude <= 180;
    }
}
