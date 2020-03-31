namespace Application.Dtos.ExaminationParameters
{
    public class UpdateExaminationParameterDtoRequest
    {
        public string Name { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
    }
}
