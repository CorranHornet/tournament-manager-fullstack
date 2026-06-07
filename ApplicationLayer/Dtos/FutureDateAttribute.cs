using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.Dtos
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date > DateTime.Now;
            }

            return false;
        }
    }
}