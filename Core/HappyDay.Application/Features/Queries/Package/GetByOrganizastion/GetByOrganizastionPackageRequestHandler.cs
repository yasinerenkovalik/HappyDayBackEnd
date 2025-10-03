using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Package.GetByCompany;

public class GetByOrganizastionPackageRequestHandler: IRequestHandler<GetByOrganizastionPackageRequest,GeneralResponse<List<GetByOrganizastionPackageResponse>>>
{
    private readonly IPackageRepository  _repository;
    private readonly IMapper _mapper;

    public GetByOrganizastionPackageRequestHandler(IPackageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetByOrganizastionPackageResponse>>> Handle(GetByOrganizastionPackageRequest request, CancellationToken cancellationToken)
    {
        var response = await _repository.GetByOrganization(request.Id);
        if (response == null)
        {
            return new GeneralResponse<List<GetByOrganizastionPackageResponse>>()
            {
                Message = "Package not found",
            };
        }
        var packages = _mapper.Map<List<GetByOrganizastionPackageResponse>>(response);

        return new GeneralResponse<List<GetByOrganizastionPackageResponse>>()
        {
            Data= packages,
            isSuccess = true
        };
    }
}