using System;
using System.Collections.Generic;
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
            var username = CurrentlyLoggedUserName;
            if (string.IsNullOrWhiteSpace(username))
                return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            if (institutionId == Guid.Empty)
                return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.NotFound, "Nieprawidłowy institutionId");

            var institution = await Context.Institutions.FindAsync(institutionId);

            if(institution == null)
                return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.NotFound, "Nie znaleziono instytucji");

            var dto = Mapper.Map<InstitutionsGetInstitutionDtoResponse>(institution);

            return new ServiceResponse<InstitutionsGetInstitutionDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<InstitutionsGetInstitutionsDtoResponse>> GetInstitutionsAsync()
        {
            var username = CurrentlyLoggedUserName;
            if (string.IsNullOrWhiteSpace(username))
                return new ServiceResponse<InstitutionsGetInstitutionsDtoResponse>(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<InstitutionsGetInstitutionsDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            var institutions = await Context.Institutions.ToListAsync();

            var dto = new InstitutionsGetInstitutionsDtoResponse();
            dto.Institutions = Mapper.Map<ICollection<InstitutionForInstitutionGetInstitutionDtoResponse>>(institutions);

            return new ServiceResponse<InstitutionsGetInstitutionsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>> UpdateInstitutionAsync(Guid institutionId, InstitutionsUpdateInstitutionDtoRequest dto)
        {
            if (institutionId == Guid.Empty)
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.NotFound, "Nieprawidłowy institutionId");

            var username = CurrentlyLoggedUserName;
            if (string.IsNullOrWhiteSpace(username))
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            if(user.Role != Role.Administrator)
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
            var username = CurrentlyLoggedUserName;
            if (string.IsNullOrWhiteSpace(username))
                return new ServiceResponse<InstitutionsCreateInstitutionDtoResponse>(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<InstitutionsCreateInstitutionDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            if(user.Role != Role.Administrator)
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
                return new ServiceResponse(HttpStatusCode.NotFound, "Nieprawidłowy institutionId");

            var username = CurrentlyLoggedUserName;
            if (string.IsNullOrWhiteSpace(username))
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            if (user.Role != Role.Administrator)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var institution = await Context.Institutions.FindAsync(institutionId);

            if (institution == null)
                return new ServiceResponse(HttpStatusCode.NotFound, "Nie znaleziono instytucji");

            Context.Remove(institution);

            if(await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania instytucji");

            return new ServiceResponse(HttpStatusCode.OK);
        }
    }
}
