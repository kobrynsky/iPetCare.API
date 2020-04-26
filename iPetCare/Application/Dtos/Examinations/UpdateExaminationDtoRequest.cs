﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Examinations
{
    public class UpdateExaminationDtoRequest
    {
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public string Content { get; set; }

        [Required]
        public Guid PetId { get; set; }
    }
}
