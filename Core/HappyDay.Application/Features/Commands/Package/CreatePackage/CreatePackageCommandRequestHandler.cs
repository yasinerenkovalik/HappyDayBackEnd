using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Package.CreatePackage;

public class CreatePackageCommandRequestHandler: IRequestHandler<CreatePackageCommandRequest, GeneralResponse<CreatePackageCommandResponse>>
{
    private IValidator<CreatePackageCommandRequest>  _validator;
    private readonly IMapper _mapper;
    private readonly IPackageRepository  _repository;

    public CreatePackageCommandRequestHandler(IPackageRepository repository, IValidator<CreatePackageCommandRequest>  validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<CreatePackageCommandResponse>> Handle(CreatePackageCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreatePackageCommandResponse>
            {
                Message = validationResult.Errors.First().ErrorMessage,
                isSuccess = false
            };
        }
        var package = _mapper.Map<global::Package>(request);
        await _repository.AddAsync(package);
        return new GeneralResponse<CreatePackageCommandResponse>
        {
            Message = "Package created",
            isSuccess = true
        };
        
        
    }
}