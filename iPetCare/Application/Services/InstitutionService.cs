using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Institutions;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class InstitutionService: Service, IInstitutionService
    {
        public InstitutionService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<GetInstitutionDtoResponse>> GetInstitutionAsync(Guid institutionId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            if (institutionId == Guid.Empty)
                return new ServiceResponse<GetInstitutionDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy institutionId");

            var institution = await Context.Institutions.FindAsync(institutionId);

            if(institution == null)
                return new ServiceResponse<GetInstitutionDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<GetInstitutionDtoResponse>(institution);

            return new ServiceResponse<GetInstitutionDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetInstitutionsDtoResponse>> GetInstitutionsAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetInstitutionsDtoResponse>(HttpStatusCode.Unauthorized);

            var institutions = await Context.Institutions.ToListAsync();

            var dto = new GetInstitutionsDtoResponse();
            dto.Institutions = Mapper.Map<ICollection<InstitutionForGetInstitutionDtoResponse>>(institutions);

            return new ServiceResponse<GetInstitutionsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateInstitutionDtoResponse>> UpdateInstitutionAsync(Guid institutionId, UpdateInstitutionDtoRequest dto)
        {
            if (institutionId == Guid.Empty)
                return new ServiceResponse<UpdateInstitutionDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy institutionId");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<UpdateInstitutionDtoResponse>(HttpStatusCode.Forbidden);

            var institution = await Context.Institutions.FindAsync(institutionId);

            if (institution == null)
                return new ServiceResponse<UpdateInstitutionDtoResponse>(HttpStatusCode.NotFound);

            institution.Address = dto.Address;
            institution.Name = dto.Name;

            if (await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse<UpdateInstitutionDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas aktualizacji placówki");

            var responseDto = Mapper.Map<UpdateInstitutionDtoResponse>(institution);

            return new ServiceResponse<UpdateInstitutionDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse<CreateInstitutionDtoResponse>> CreateInstitutionAsync(CreateInstitutionDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<CreateInstitutionDtoResponse>(HttpStatusCode.Forbidden);

            if(dto.Id == Guid.Empty)
                dto.Id = Guid.NewGuid();

            var institution = Mapper.Map<Institution>(dto);

            await Context.Institutions.AddAsync(institution);

            if(await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse<CreateInstitutionDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas tworzenia placówki");

            var responseDto = Mapper.Map<CreateInstitutionDtoResponse>(institution);

            return new ServiceResponse<CreateInstitutionDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse> DeleteInstitutionAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nieprawidłowy institutionId");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var institution = await Context.Institutions.FindAsync(institutionId);

            if (institution == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Remove(institution);

            if(await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania placówki");

            return new ServiceResponse(HttpStatusCode.OK);
        }

        public async Task<ServiceResponse> SignUpAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nieprawidłowy institutionId");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Vet)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var institution = Context.Institutions.Find(institutionId);

            if(institution == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var institutionVets = await  Context.InstitutionVets.Where(x => x.InstitutionId == institutionId && x.VetId == CurrentlyLoggedUser.Vet.Id).ToListAsync();

            if(institutionVets.Any())
                return new ServiceResponse(HttpStatusCode.BadRequest, $"Weterynarz jest już zapisany do placówki {institution.Name}");

            var institutionVet = new InstitutionVet() {Vet = CurrentlyLoggedUser.Vet, Institution = institution};
            Context.Add(institutionVet);

            if(await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Błąd podczas zapisu do bazy weterynarza do placówki");

            return new ServiceResponse(HttpStatusCode.OK);
        }

        public async Task<ServiceResponse> SignOutAsync(Guid institutionId)
        {
            if (institutionId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nieprawidłowy institutionId");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Vet)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var institution = Context.Institutions.Find(institutionId);

            if (institution == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var institutionVets = await Context.InstitutionVets.Where(x => x.InstitutionId == institutionId && x.VetId == CurrentlyLoggedUser.Vet.Id).SingleOrDefaultAsync();

            if (institutionVets == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, $"Weterynarz nie jest zapisany do placówki {institution.Name}");

            Context.Remove(institutionVets);

            if (await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Błąd podczas zapisu do bazy wypisania weterynarza z placówki");

            return new ServiceResponse(HttpStatusCode.OK);
        }
    }
}
