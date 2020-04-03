using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using Application.Interfaces;
using Application.Services.Utilities;
using Application.Dtos.Invitations;
using Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class InvitationService : Service, IInvitationService
    {
        public InvitationService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateInvitationDtoResponse>> CreateInvitationAsync(CreateInvitationDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Owner && CurrentlyLoggedUser.Role != Role.Vet)
                return new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.Forbidden);

            var pet = Context.Pets.Find(dto.PetId);
            if (pet == null)
                return new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            var didUserRequest = false;

            if (CurrentlyLoggedUser.Role == Role.Owner)
            {
                var owners = pet.OwnerPets.Where(x => x.OwnerId == CurrentlyLoggedUser.Owner.Id);
                if (!owners.Any())
                    didUserRequest = false;
                else didUserRequest = true;
            }

            var existInvitation = await Context.Requests.Where(x => x.PetId == dto.PetId && x.UserId == dto.UserId).ToListAsync();

            if (existInvitation.Any())
                return new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.BadRequest, $"Podany użytkownik został już zaproszony do zwierzaka {pet.Name}");

            if (dto.Id == Guid.Empty)
                dto.Id = Guid.NewGuid();

            var request = new Request()
            {
                UserId = dto.UserId,
                PetId = dto.PetId,
                DidUserRequest = didUserRequest,
                Id = dto.Id
            };

            Context.Requests.Add(request);
            var result = await Context.SaveChangesAsync();

            var responseDto = Mapper.Map<CreateInvitationDtoResponse>(request);

            return result > 0
                ? new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas tworzenia rasy");

        }

        public async Task<ServiceResponse> DeleteInvitationAsync(Guid InvitationId)
        {
            if (InvitationId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie istnieje takie zaproszenie w bazie danych");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var invitation = await Context.Requests.FindAsync(InvitationId);

            if (invitation == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Remove(invitation);

            if (await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania zaproszenia");

            return new ServiceResponse(HttpStatusCode.OK);
        }
    }
}
