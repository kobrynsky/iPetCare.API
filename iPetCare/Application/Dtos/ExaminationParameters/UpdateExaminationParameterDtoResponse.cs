namespace Application.Dtos.ExaminationParameters
{
    public class UpdateExaminationParameterDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
    }
}
