
using Domain.Entitys.Common;

namespace Domain.Entitys.MealModel
{
    public class MealTime:BaseEntity
    {
        public TimeOnly? BreakFastTimeStart { get; set; } = new TimeOnly(8, 0); // 8 AM
        public TimeOnly? BreakFastTimeEnd { get; set; } = new TimeOnly(11, 0); // 11 AM
        public TimeOnly? LunchTimeStart { get; set; } = new TimeOnly(12, 0); // 12 PM
        public TimeOnly? LunchTimeEnd { get; set; } = new TimeOnly(16, 0); // 4 PM
        public TimeOnly? DinnerTimeStart { get; set; } = new TimeOnly(19, 0); // 7 PM
        public TimeOnly? DinnerTimeEnd { get; set; } = new TimeOnly(23, 0); // 11 PM
    }
}
