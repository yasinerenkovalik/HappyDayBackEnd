using AutoMapper;
using HappyDay.Application.Features.Commands.Organization.UpdateOrganization;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Interface.Services;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Organization.UpdateOrganization
{
    public class UpdateOrganizationCommandRequestHandler : IRequestHandler<UpdateOrganizationCommandRequest, GeneralResponse<UpdateOrganizationCommandResponse>>
    {
        private readonly IOrganizationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateOrganizationCommandRequestHandler(IOrganizationRepository repository, IMapper mapper, IFileService fileService)
        {
            _repository = repository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<GeneralResponse<UpdateOrganizationCommandResponse>> Handle(UpdateOrganizationCommandRequest request, CancellationToken cancellationToken)
        {
            var organization = await _repository.GetByIdAsync(request.Id);
            if (organization == null)
            {
                return new GeneralResponse<UpdateOrganizationCommandResponse>
                {
                    Message = Messages.MessageConstants.InvalidOrganizationData,
                    isSuccess = false
                };
            }

            // 1. Kapak fotoğrafı varsa kaydet
            if (request.CoverPhoto != null)
            {
                var coverPath = await _fileService.SaveFileAsync(request.CoverPhoto, "uploads/cover");
                organization.CoverPhotoPath = coverPath;
            }

            // 2. Diğer tüm alanları güncelle
            _mapper.Map(request, organization);

            // 3. Veritabanında güncelle
            await _repository.UpdateAsync(organization);

            return new GeneralResponse<UpdateOrganizationCommandResponse>
            {
                Message = Messages.MessageConstants.OrganizationUpdated,
                isSuccess = true,
                Data = new UpdateOrganizationCommandResponse
                {
                    
                }
            };
        }
    }
}
