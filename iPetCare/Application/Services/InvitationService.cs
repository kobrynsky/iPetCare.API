using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

            if (CurrentlyLoggedUser.Role == Role.Owner)
            {
                var owners = pet.OwnerPets.Where(x => x.Owner.UserId == CurrentlyLoggedUser.Id);
                if (owners.Any())
                    return new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.BadRequest, "Użytkownik ma już dostęp do zwierzaka");
            }
            else if (CurrentlyLoggedUser.Role == Role.Vet)
            {
                var vets = pet.VetPets.Where(x => x.Vet.UserId == CurrentlyLoggedUser.Id);
                if (vets.Any())
                    return new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.BadRequest, "Użytkownik ma już dostęp do zwierzaka");
            }

            var existInvitation = await Context.Requests.Where(x => x.PetId == dto.PetId && x.UserId == CurrentlyLoggedUser.Id).AnyAsync();

            if (existInvitation)
                return new ServiceResponse<CreateInvitationDtoResponse>(HttpStatusCode.BadRequest, $"Podany użytkownik został już zaproszony do zwierzaka {pet.Name}");

            var request = new Request()
            {
                UserId = CurrentlyLoggedUser.Id,
                PetId = dto.PetId,
                Id = Guid.NewGuid(),
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

            if (CurrentlyLoggedUser.Id == invitation.UserId && pet.OwnerPets.Any(x => x.Owner.User.Id == CurrentlyLoggedUser.Id))
                return new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.Forbidden);

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
                        Owner = owner,
                        MainOwner = false,
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

            if (!dto.IsAccepted)
            {
                Context.Requests.Remove(invitation);
            }

            int result = await Context.SaveChangesAsync();

            return result >= 0
                ? new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<ChangeStatusInvitationDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zmiany statusu zaproszenia");
        }

        public async Task<ServiceResponse> AcceptInvitationAsync(Guid invitationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var invitation = await Context.Requests.FindAsync(invitationId);

            if(invitation == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if(CurrentlyLoggedUser.Id != invitation.Pet.OwnerPets.FirstOrDefault(x => x.MainOwner)?.Owner.UserId)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var user = await UserManager.FindByIdAsync(invitation.UserId);

            if(user == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (user.Role == Role.Vet)
            {
                var vetPet = new VetPet()
                {
                    Vet = user.Vet,
                    Pet = invitation.Pet,
                };

                await Context.VetPets.AddAsync(vetPet);
            }
            else if (user.Role == Role.Owner)
            {
                var ownerPet = new OwnerPet()
                {
                    Owner = user.Owner,
                    Pet = invitation.Pet,
                    MainOwner = false,
                };
                await Context.OwnerPets.AddAsync(ownerPet);
            }

            Context.Requests.Remove(invitation);

            int result = await Context.SaveChangesAsync();

            return result >= 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zmiany statusu zaproszenia");
        }

        public async Task<ServiceResponse> DeclineInvitationAsync(Guid invitationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var invitation = await Context.Requests.FindAsync(invitationId);

            if (invitation == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (CurrentlyLoggedUser.Id != invitation.Pet.OwnerPets.FirstOrDefault(x => x.MainOwner)?.Owner.UserId)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var user = await UserManager.FindByIdAsync(invitation.UserId);

            if (user == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Requests.Remove(invitation);

            int result = await Context.SaveChangesAsync();

            return result >= 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zmiany statusu zaproszenia");

        }

        public async Task<ServiceResponse> DeleteAccessAsync(string userId, Guid petId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.FindAsync(petId);

            if (pet == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var mainOwner = pet.OwnerPets.FirstOrDefault(x => x.MainOwner);

            if (mainOwner != null && (mainOwner.Owner.UserId == CurrentlyLoggedUser.Id || user.Id == CurrentlyLoggedUser.Id))
            {
                if (user.Role == Role.Vet)
                {
                    var vetPet = pet.VetPets.FirstOrDefault(x => x.Vet.UserId == userId);
                    if (vetPet != null)
                        Context.VetPets.Remove(vetPet);
                }

                if (user.Role == Role.Owner)
                {
                    var ownerPet = pet.OwnerPets.FirstOrDefault(x => x.Owner.UserId == userId);
                    if (ownerPet != null)
                        Context.OwnerPets.Remove(ownerPet);
                }
            }

            int result = await Context.SaveChangesAsync();

            return result >= 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zmiany statusu zaproszenia");
        }

        public async Task<ServiceResponse> RevokeInvitationAsync(Guid invitationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var invitation = await Context.Requests.FindAsync(invitationId);

            if (invitation == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (CurrentlyLoggedUser.Id != invitation.UserId)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var user = await UserManager.FindByIdAsync(invitation.UserId);

            if (user == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Requests.Remove(invitation);

            int result = await Context.SaveChangesAsync();

            return result >= 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zmiany statusu zaproszenia");
        }
    }
}
