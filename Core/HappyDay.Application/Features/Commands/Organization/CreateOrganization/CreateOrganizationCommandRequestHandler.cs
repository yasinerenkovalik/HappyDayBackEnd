using AutoMapper;
using FluentValidation;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Interface.Services;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Organization.CreateOrganization;

public class CreateOrganizationCommandRequestHandler:IRequestHandler<CreateOrganizationCommandRequest, GeneralResponse<CreateOrganizationCommandResponse>>
{
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository  _repository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IValidator <CreateOrganizationCommandRequest> _validator;
    private readonly IFileService _fileService;

    public CreateOrganizationCommandRequestHandler(IMapper mapper, IOrganizationRepository repository, IValidator<CreateOrganizationCommandRequest> validator, ICompanyRepository companyRepository, IFileService fileService)
    {
        _mapper = mapper;
        _repository = repository;
        _validator = validator;
        _companyRepository = companyRepository;
        _fileService = fileService;
    }

    public async Task<GeneralResponse<CreateOrganizationCommandResponse>> Handle(CreateOrganizationCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new GeneralResponse<CreateOrganizationCommandResponse>
            {
                Message = Messages.MessageConstants.InvalidOrganizationData,
                isSuccess = false
            };
        }

        var company = await _companyRepository.GetByIdAsync(request.CompanyId);
        if (company == null)
        {
            return new GeneralResponse<CreateOrganizationCommandResponse>
            {
                Message = Messages.MessageConstants.CompanyNotFound,
                isSuccess = false
            };
        }

        // ✅ AutoMapper ile ana bilgileri eşleştir
        var organization = _mapper.Map<Domain.Entities.Organization>(request);
        organization.OrganizationImages = new List<Domain.Entities.OrganizationImage>();

        // ✅ Resimleri yükle ve OrganizationImage listesine ekle
        if (request.CoverPhoto != null)
        {
            var path = await _fileService.SaveFileAsync(request.CoverPhoto, "uploads/cover");
            organization.CoverPhotoPath = path;
        }
    
        if (request.Images != null && request.Images.Any())
        {
            var uploadedPaths = await _fileService.SaveFilesAsync(request.Images, "uploads");
            foreach (var path in uploadedPaths)
            {
                organization.OrganizationImages.Add(new Domain.Entities.OrganizationImage
                {
                    ImageUrl = path
                });
            }
        }

        await _repository.AddAsync(organization);

        return new GeneralResponse<CreateOrganizationCommandResponse>
        {
            Message = Messages.MessageConstants.OrganizationCreated,
            isSuccess = true
        };
    }

}