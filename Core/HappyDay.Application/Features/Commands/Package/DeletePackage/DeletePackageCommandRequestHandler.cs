using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Package.DeletePackage;

public class DeletePackageCommandRequestHandler : IRequestHandler<DeletePackageCommandRequest, GeneralResponse<DeletePackageCommandResponse>>
{
    private readonly IPackageRepository _repository;
    private readonly IMapper _mapper;

    public DeletePackageCommandRequestHandler(IPackageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<DeletePackageCommandResponse>> Handle(DeletePackageCommandRequest request, CancellationToken cancellationToken)
    {
        var package = await _repository.GetByIdAsync(request.Id);
        if (package == null)
        {
            return new GeneralResponse<DeletePackageCommandResponse>
            {
                isSuccess = false,
                Message = "Paket bulunamadı."
            };
        }

        await _repository.DeleteAsync(package.Id);

        var response = _mapper.Map<DeletePackageCommandResponse>(package);

        return new GeneralResponse<DeletePackageCommandResponse>
        {
            isSuccess = true,
            Message = "Paket başarıyla silindi.",
            Data = response
        };
    }
}