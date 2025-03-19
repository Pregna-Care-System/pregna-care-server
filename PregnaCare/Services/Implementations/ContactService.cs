using PregnaCare.Api.Models.Requests.ContactSubscriberRequestModel;
using PregnaCare.Api.Models.Responses.ContactSubscriberResponseModel;
using PregnaCare.Common.Api;
using PregnaCare.Common.Constants;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;
using PregnaCare.Utils;

namespace PregnaCare.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly IGenericRepository<ContactSubscriber, Guid> _contactRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="emailService"></param>
        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _contactRepository = unitOfWork.GetRepository<ContactSubscriber, Guid>();
        }

        public async Task<CreateContactResponse> CreateContactAsync(CreateContactRequest request)
        {
            var response = new CreateContactResponse { Success = false };
            var detailErrorList = new List<DetailError>();

            if (string.IsNullOrEmpty(request.Email))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    MessageId = Messages.E00005,
                    Message = Messages.GetMessageById(Messages.E00005)
                });
            }

            if (!ValidationUtils.IsValidEmail(request.Email))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    MessageId = Messages.E00006,
                    Message = Messages.GetMessageById(Messages.E00006)
                });
            }

            if (string.IsNullOrEmpty(request.FullName))
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.FullName),
                    Value = request.FullName,
                    MessageId = Messages.E00005,
                    Message = Messages.GetMessageById(Messages.E00005)
                });
            }

            var existingSubscriber = (await _contactRepository.FindAsync(x => x.Email == request.Email)).FirstOrDefault();
            if (existingSubscriber != null)
            {
                detailErrorList.Add(new DetailError
                {
                    FieldName = nameof(request.Email),
                    Value = request.Email,
                    MessageId = Messages.E00011,
                    Message = Messages.GetMessageById(Messages.E00011)
                });
            }

            if (detailErrorList.Count > 0)
            {
                response.MessageId = Messages.E00001;
                response.Response = Messages.GetMessageById(Messages.E00001);
                response.DetailErrorList = detailErrorList;
                return response;
            }

            var contact = new ContactSubscriber
            {
                FullName = request.FullName,
                Email = request.Email,
                Message = request.Message
            };

            await _contactRepository.AddAsync(contact);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Response = Messages.GetMessageById(Messages.I00001);
            return response;
        }

        public async Task<bool> DeleteContactAsync(string email)
        {
            var contact = (await _contactRepository.FindAsync(x => x.Email == email)).FirstOrDefault();
            if (contact == null) return false;

            _contactRepository.Remove(contact);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<SelectContactResponse> SelectContactAsync()
        {
            var response = new SelectContactResponse { Success = false };

            var contacts = await _contactRepository.GetAllAsync();
            if (contacts == null || contacts.Count() == 0)
            {
                response.MessageId = Messages.E00013;
                response.Message = Messages.GetMessageById(Messages.E00013);
                return response;
            }

            var responseEntities = contacts.Select(x => new SelectContactEntity
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Message = x.Message,
                CreatedAt = x.CreatedAt
            }).OrderByDescending(x => x.CreatedAt).ToList();

            response.Success = true;
            response.MessageId = Messages.I00001;
            response.Message = Messages.GetMessageById(Messages.I00001);
            response.Response = responseEntities;
            return response;
        }
    }

}
