using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Interface.Services;
using HappyDay.Application.Wrappers;
using HappyDay.Domain.Entities;
using MediatR;

namespace HappyDay.Application.Features.Commands.OrganizationImages.CreateOrganizationImages
{
    public class CreateOrganizationImagesCommandRequestHandler : IRequestHandler<CreateOrganizationImagesCommandRequest, GeneralResponse<CreateOrganizationImagesCommandResponse>>
    {
        private readonly IOrganizationImagesRepository _organizationImagesRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CreateOrganizationImagesCommandRequestHandler(IOrganizationImagesRepository organizationImagesRepository, IMapper mapper, IFileService fileService)
        {
            _organizationImagesRepository = organizationImagesRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<GeneralResponse<CreateOrganizationImagesCommandResponse>> Handle(CreateOrganizationImagesCommandRequest request, CancellationToken cancellationToken)
        {
         
            var imagePath = await _fileService.SaveFileAsync(request.OrganizationImage, "uploads/organization");
            var organization = new OrganizationImage()
            {
                ImageUrl = imagePath,
                OrganizationId = request.OrganizationId
                // Id verme! EF Core otomatik alsın.
            };


  

            // 3. Veritabanına kaydet
            await _organizationImagesRepository.AddAsync(organization);

            // 4. Başarılı cevap dön
            return new GeneralResponse<CreateOrganizationImagesCommandResponse>
            {
                isSuccess = true,
                Message = "Organizasyon görseli başarıyla yüklendi.",
                Data = new CreateOrganizationImagesCommandResponse
                {
                   
                }
            };
        }
    }
}
