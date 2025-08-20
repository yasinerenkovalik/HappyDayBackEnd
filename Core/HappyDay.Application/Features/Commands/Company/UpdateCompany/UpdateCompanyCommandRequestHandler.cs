using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
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

        public UpdateCompanyCommandRequestHandler(
            ICompanyRepository repository,
            IMapper mapper,
            IValidator<UpdateCompanyCommandRequest> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
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

            // 2) Mevcut şirketi tracked olarak çek
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null)
            {
                return new GeneralResponse<UpdateCompanyCommandResponse>
                {
                    Message = MessageConstants.CompanyNotFound
                };
            }

            // 3) Yeni instance oluşturma! Var olan tracked entity’nin ÜZERİNE map et
            _mapper.Map(request, existing);

            // 4) Repository’yi değiştirmeden UpdateAsync ile kaydet
            await _repository.UpdateAsync(existing);

            return new GeneralResponse<UpdateCompanyCommandResponse>
            {
                Message = MessageConstants.CompanyUpdated
                // İstersen:
                // Data = new UpdateCompanyCommandResponse { Id = existing.Id }
            };
        }
    }
}
