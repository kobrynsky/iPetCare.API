﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Examinations
{
    public class CreateExaminationDtoRequest
    {
        [Required]
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }
        [Required]
        public Guid PetId { get; set; }
    }
}