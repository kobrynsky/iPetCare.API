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

            var pet = await Context.Pets.FindAsync(dto.PetId);
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

            var existInvitation = await Context.Requests.Where(x => x.PetId == dto.PetId && x.UserId == dto.UserId).AnyAsync();

            if (existInvitation)
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

        public async Task<ServiceResponse> DeleteInvitationAsync(Guid invitationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (invitationId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie istnieje takie zaproszenie w bazie danych");

            var invitation = await Context.Requests.FindAsync(invitationId);

            if (invitation == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Remove(invitation);

            if (await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania zaproszenia");

            return new ServiceResponse(HttpStatusCode.OK);
        }

        public async Task<ServiceResponse<ChangeStatusInvitationDtoResponse>> ChangeInvitationStatusAsync(ChangeStatusInvitationDtoRequest dto, Guid invitationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.Unauthorized);

            var invitation = await Context.Requests.FindAsync(invitationId);

            if (invitation == null)
                return new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.NotFound);

            var pet = await Context.Pets.FindAsync(invitation.PetId);

            if (CurrentlyLoggedUser.Id == invitation.UserId && pet.OwnerPets.Where(x => x.Owner.User.Id == CurrentlyLoggedUser.Id).Any())
                return new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.Forbidden);

            invitation.IsAccepted = dto.IsAccepted;

            var responseDto = Mapper.Map<ChangeStatusInvitationDtoResponse>(invitation);

            if (dto.IsAccepted)
            {
                var User = UserManager.FindByIdAsync(invitation.UserId).Result;

                if (User.Role == Role.Owner)
                {
                    var owner = await Context.Owners.Where(x => x.UserId == invitation.UserId).SingleOrDefaultAsync();

                    var existInvitation = await Context.OwnerPets.Where(x => x.PetId == invitation.PetId && x.OwnerId == owner.Id).ToListAsync();
                    if (existInvitation.Any())
                        return new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.BadRequest, "Podany użytkownik został zatwierdzony do tego zwierzaka");

                    Context.OwnerPets.Add(new OwnerPet
                    {
                        Pet = pet,
                        Owner = owner
                    });
                }

                if (User.Role == Role.Vet)
                {
                    var vet = await Context.Vets.Where(x => x.UserId == invitation.UserId).SingleOrDefaultAsync();

                    var existInvitation = await Context.VetPets.Where(x => x.PetId == invitation.PetId && x.VetId == vet.Id).AnyAsync();
                    if (existInvitation)
                        return new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.BadRequest, "Podany użytkownik został zatwierdzony do tego zwierzaka");

                    Context.VetPets.Add(new VetPet
                    {
                        Pet = pet,
                        Vet = vet
                    });
                }
            }

            int result = await Context.SaveChangesAsync();

            return result >= 0
                ? new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zmiany statusu zaproszenia");
        }
    }
}
