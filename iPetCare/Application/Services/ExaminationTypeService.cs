using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Application.Interfaces;
using Application.Services.Utilities;
using Application.Dtos.ExaminationTypes;
using Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ExaminationTypeService : Service, IExaminationTypeService
    {
        public ExaminationTypeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateExaminationTypeDtoResponse>> CreateExaminationTypeAsync(CreateExaminationTypeDtoRequest dto)
        {
            if (await Context.ExaminationTypes.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<CreateExaminationTypeDtoResponse>(HttpStatusCode.BadRequest, "Podane badanie już istnieje");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateExaminationTypeDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<CreateExaminationTypeDtoResponse>(HttpStatusCode.Forbidden);

            var species = await Context.Species.FindAsync(dto.SpeciesId);
            if (species == null)
                return new ServiceResponse<ExaminationTypesCreateExaminationTypeDtoResponse>(HttpStatusCode.BadRequest, "Nie ma takiego gatunku");

            var examinationType = new ExaminationType()
            {
                Name = dto.Name,
                SpeciesId = dto.SpeciesId
            };

            Context.ExaminationTypes.Add(examinationType);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = new CreateExaminationTypeDtoResponse()
                {
                    Name = examinationType.Name,
                    SpeciesId = examinationType.SpeciesId,
                    Id = examinationType.Id
                };

                return new ServiceResponse<CreateExaminationTypeDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<CreateExaminationTypeDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu badania");
        }

        public async Task<ServiceResponse<GetAllExaminationTypesDtoResponse>> GetAllExaminationTypesAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllExaminationTypesDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationType = await Context.ExaminationTypes.ToListAsync();

            var dto = new GetAllExaminationTypesDtoResponse()
            {
                ExaminationTypes = Mapper.Map<List<ExaminationTypeForGetAllExaminationTypesDtoResponse>>(examinationType)
            };

            return new ServiceResponse<GetAllExaminationTypesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetExaminationTypeDtoResponse>> GetExaminationTypeAsync(int examinationTypeId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetExaminationTypeDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationType = await Context.ExaminationTypes.FindAsync(examinationTypeId);
            if (examinationType == null)
                return new ServiceResponse<GetExaminationTypeDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<GetExaminationTypeDtoResponse>(examinationType);

            return new ServiceResponse<GetExaminationTypeDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateExaminationTypeDtoResponse>> UpdateExaminationTypeAsync(int examinationTypeId, UpdateExaminationTypeDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.Forbidden);

            if (await Context.ExaminationTypes.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.BadRequest, "Podane badanie już istnieje");

            var examinationType = Context.ExaminationTypes.Find(examinationTypeId);
            var species = Context.Species.Find(dto.SpeciesId);

            if (species == null)
                return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowa rasa");

            if (examinationType == null)
                return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.NotFound);

            examinationType.Name = dto.Name;
            examinationType.SpeciesId = dto.SpeciesId;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<UpdateExaminationTypeDtoResponse>(examinationType);
                return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            if (result == 0)
                return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiła żadna zmiana");

            return new ServiceResponse<UpdateExaminationTypeDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu badania");
        }

        public async Task<ServiceResponse> DeleteExaminationTypeAsync(int examinationTypeId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var examinationType = Context.ExaminationTypes.Find(examinationTypeId);
            if (examinationType == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.ExaminationTypes.Remove(examinationType);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse(HttpStatusCode.OK);

            return new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania badania");
        }

        public async Task<ServiceResponse<ExaminationParametersGetAllForOneExaminationTypeDtoResponse>> GetAllForOneExaminationTypeAsync(int examinationTypeId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationParametersGetAllForOneExaminationTypeDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationType = await Context.ExaminationTypes.FindAsync(examinationTypeId);
            if (examinationType == null)
                return new ServiceResponse<ExaminationParametersGetAllForOneExaminationTypeDtoResponse>(HttpStatusCode.NotFound);

            var examinationParameter = await Context.ExaminationParameters.Where(x => x.ExaminationTypeId == examinationTypeId).ToListAsync();

            var dto = new ExaminationParametersGetAllForOneExaminationTypeDtoResponse()
            {
                Id = examinationType.Id,
                Name = examinationType.Name,
                ExaminationParameters = Mapper.Map<List<ExaminationParameterDetailsForExaminationTypeGetDtoResponse>>(examinationParameter)
            };

            return new ServiceResponse<ExaminationParametersGetAllForOneExaminationTypeDtoResponse>(HttpStatusCode.OK, dto);
        }
    }
}
