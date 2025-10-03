using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Package.UpdatePackage;

public class UpdatePackageCommandRequestHandler : IRequestHandler<UpdatePackageCommandRequest, GeneralResponse<UpdatePackageCommandResponse>>
{
    private readonly IPackageRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePackageCommandRequestHandler(IPackageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<UpdatePackageCommandResponse>> Handle(UpdatePackageCommandRequest request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id);
        if (package == null)
        {
            return new GeneralResponse<UpdatePackageCommandResponse>
            {
                isSuccess = false,
                Message = "Paket bulunamadı."
            };
        }

        // request -> package map (entity güncelleme)
        _mapper.Map(request, package);

        await _repository.UpdateAsync(package);

        var response = _mapper.Map<UpdatePackageCommandResponse>(package);

        return new GeneralResponse<UpdatePackageCommandResponse>
        {
            isSuccess = true,
            Message = "Paket başarıyla güncellendi.",
            Data = response
        };
    }
}