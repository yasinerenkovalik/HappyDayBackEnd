using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Company.UpdateCompany;

public class UpdateCompanyCommandRequestHandler:IRequestHandler<UpdateCompanyCommandRequest,GeneralResponse<UpdateCompanyCommandResponse>>
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator <UpdateCompanyCommandRequest> _validator;

    public UpdateCompanyCommandRequestHandler(ICompanyRepository repository, IMapper mapper, IValidator<UpdateCompanyCommandRequest> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<GeneralResponse<UpdateCompanyCommandResponse>> Handle(UpdateCompanyCommandRequest request, CancellationToken cancellationToken)
    {
        var  response = await _repository.GetByIdAsync(request.Id);
        if (response.Adress.Length <=0)
        {
            return new GeneralResponse<UpdateCompanyCommandResponse>
            {
                Message = Messages.MessageConstants.CompanyNotFound
            };
        }
         var mapped = _mapper.Map<Domain.Entities.Company>(response);
       await _repository.UpdateAsync(mapped);
       return new GeneralResponse<UpdateCompanyCommandResponse>()
       {
           Message = Messages.MessageConstants.CompanyUpdated
       };
    }
}