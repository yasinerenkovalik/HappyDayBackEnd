using AutoMapper;
using FluentValidation;
using HappyDay.Application.Common.Security;
using HappyDay.Application.Features.EmailVerification; // IPasswordHasher
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Interface.Services;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Company.CreateCompany;

public class CreateCompanyCommandRequestHandler
    : IRequestHandler<CreateCompanyCommandRequest, GeneralResponse<CreateCompanyCommandResponse>>
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCompanyCommandRequest> _validator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly IFileService _fileService;

    public CreateCompanyCommandRequestHandler(
        ICompanyRepository repository,
        IMapper mapper,
        
        IValidator<CreateCompanyCommandRequest> validator,
        IPasswordHasher passwordHasher, IEmailVerificationService emailVerificationService, IFileService fileService)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _passwordHasher = passwordHasher;
        _emailVerificationService = emailVerificationService;
        _fileService = fileService;
    }

    public async Task<GeneralResponse<CreateCompanyCommandResponse>> Handle(
        CreateCompanyCommandRequest request,
        CancellationToken cancellationToken)
    {
        // 1) Validasyon
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreateCompanyCommandResponse>
            {
                Message = validationResult.Errors.First().ErrorMessage,
                isSuccess = false
            };
        }

        // 2) E-posta normalize + tekillik kontrolü
        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();
        var exists = await _repository.GetByEmailAsync(email) is not null;
        if (exists)
        {
            return new GeneralResponse<CreateCompanyCommandResponse>
            {
                Message = "Bu e-posta ile kayıtlı bir şirket zaten var.",
                isSuccess = false
            };
        }

        // 3) Map + hash
        var company = _mapper.Map<Domain.Entities.Company>(request);
        company.Email = email; // normalize edilmiş hali yaz
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            company.PasswordHash = _passwordHasher.Hash(request.Password);
        }
        var path = await _fileService.SaveFileAsync(request.CoverPhotoPath, "uploads/companycover");
        var fullPath = Path.Combine("uploads/companycover", path);

        // veritabanına tam yolu kaydet
        company.CoverPhotoPath = fullPath.Replace("\\", "/");

        // 4) Kaydet
        await _repository.AddAsync(company);
        await _emailVerificationService.GenerateAndSendAsync(company.Id, company.Email, cancellationToken);

        return new GeneralResponse<CreateCompanyCommandResponse>
        {
            Message = Messages.MessageConstants.CompanyCreated,
            isSuccess = true
        };
    }
}
