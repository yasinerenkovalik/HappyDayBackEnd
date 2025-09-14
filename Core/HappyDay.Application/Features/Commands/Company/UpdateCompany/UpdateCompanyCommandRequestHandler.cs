using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Interface.Services;
using HappyDay.Application.Messages;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Company.UpdateCompany
{
    public class UpdateCompanyCommandRequestHandler 
        : IRequestHandler<UpdateCompanyCommandRequest, GeneralResponse<UpdateCompanyCommandResponse>>
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCompanyCommandRequest> _validator;
        private readonly IFileService _fileService;

        public UpdateCompanyCommandRequestHandler(
            ICompanyRepository repository,
            IMapper mapper,
            IValidator<UpdateCompanyCommandRequest> validator, IFileService fileService)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _fileService = fileService;
        }

        public async Task<GeneralResponse<UpdateCompanyCommandResponse>> Handle(
            UpdateCompanyCommandRequest request, 
            CancellationToken cancellationToken)
        {
            // 1) Validasyon
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return new GeneralResponse<UpdateCompanyCommandResponse>
                {
                    Message = MessageConstants.CompanyUpdated,
                   
                };
            }

     
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null)
            {
                return new GeneralResponse<UpdateCompanyCommandResponse>
                {
                    Message = MessageConstants.CompanyNotFound
                };
            }

            
            _mapper.Map(request, existing);

            // 4) Repository’yi değiştirmeden UpdateAsync ile kaydet
            var path = await _fileService.SaveFileAsync(request.CoverPhoto, "uploads/companycover");
            var fullPath = Path.Combine("uploads/companycover", path);

            // veritabanına tam yolu kaydet
            existing.CoverPhotoPath = fullPath.Replace("\\", "/");
            await _repository.UpdateAsync(existing);

            return new GeneralResponse<UpdateCompanyCommandResponse>
            {
                Message = MessageConstants.CompanyUpdated,
                isSuccess = true
              
            };
        }
    }
}
