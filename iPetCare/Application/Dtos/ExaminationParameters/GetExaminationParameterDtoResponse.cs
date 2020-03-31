namespace Application.Dtos.ExaminationParameters
{
    public class GetExaminationParameterDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
        public ExaminationTypeForGetExaminationParameterDtoResponse ExaminationType { get; set; }
    }

    public class ExaminationTypeForGetExaminationParameterDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
