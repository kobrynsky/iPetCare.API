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

        public async Task<ServiceResponse<InstitutionsGetInstitutionDtoResponse>> GetInstitutionAsync(Guid institutionId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            if (institutionId == Guid.Empty)
                return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.NotFound, "Nieprawidłowy institutionId");

            var institution = await Context.Institutions.FindAsync(institutionId);

            if(institution == null)
                return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<InstitutionsGetInstitutionDtoResponse>(institution);

            return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<InstitutionsGetInstitutionsDtoResponse>> GetInstitutionsAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<InstitutionsGetInstitutionsDtoResponse>(HttpStatusCode.Unauthorized);

            var institutions = await Context.Institutions.ToListAsync();

            var dto = new InstitutionsGetInstitutionsDtoResponse();
            dto.Institutions = Mapper.Map<ICollection<InstitutionForInstitutionGetInstitutionDtoResponse>>(institutions);

            return new ServiceResponse<InstitutionsGetInstitutionsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>> UpdateInstitutionAsync(Guid institutionId, InstitutionsUpdateInstitutionDtoRequest dto)
        {
            if (institutionId == Guid.Empty)
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy institutionId");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.Forbidden);

            var institution = await Context.Institutions.FindAsync(institutionId);

            if (institution == null)
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.NotFound, "Nie znaleziono instytucji");

            institution.Address = dto.Address;
            institution.Name = dto.Name;

            if (await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas aktualizacji instytucji");

            var responseDto = Mapper.Map<InstitutionsUpdateInstitutionDtoResponse>(institution);

            return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        public async Task<ServiceResponse<InstitutionsCreateInstitutionDtoResponse>> CreateInstitutionAsync(InstitutionsCreateInstitutionDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<InstitutionsCreateInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<InstitutionsCreateInstitutionDtoResponse>(HttpStatusCode.Forbidden);

            if(dto.Id == Guid.Empty)
                dto.Id = Guid.NewGuid();

            var institution = Mapper.Map<Institution>(dto);

            await Context.Institutions.AddAsync(institution);

            if(await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse<InstitutionsCreateInstitutionDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas tworzenia instytucji");

            var responseDto = Mapper.Map<InstitutionsCreateInstitutionDtoResponse>(institution);

            return new ServiceResponse<InstitutionsCreateInstitutionDtoResponse>(HttpStatusCode.OK, responseDto);
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
                return new ServiceResponse(HttpStatusCode.NotFound, "Nie znaleziono instytucji");

            Context.Remove(institution);

            if(await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania instytucji");

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
                return new ServiceResponse(HttpStatusCode.NotFound, "Nie znaleziono instytucji");

            var institutionVets = await  Context.InstitutionVets.Where(x => x.InstitutionId == institutionId && x.VetId == CurrentlyLoggedUser.Vet.Id).ToListAsync();

            if(institutionVets.Any())
                return new ServiceResponse(HttpStatusCode.BadRequest, $"Weterynarz jest już zapisany do instytucji {institution.Name}");

            var institutionVet = new InstitutionVet() {Vet = CurrentlyLoggedUser.Vet, Institution = institution};
            Context.Add(institutionVet);

            if(await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Błąd podczas zapisu do bazy weterynarza do instytucji");

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
                return new ServiceResponse(HttpStatusCode.NotFound, "Nie znaleziono instytucji");

            var institutionVets = await Context.InstitutionVets.Where(x => x.InstitutionId == institutionId && x.VetId == CurrentlyLoggedUser.Vet.Id).SingleOrDefaultAsync();

            if (institutionVets == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, $"Weterynarz nie jest zapisany do instytucji {institution.Name}");

            Context.Remove(institutionVets);

            if (await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Błąd podczas zapisu do bazy wypisania weterynarza z instytucji");

            return new ServiceResponse(HttpStatusCode.OK);
        }
    }
}
