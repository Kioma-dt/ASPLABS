using Web_453503_Avramenko.API.Data;

namespace Web_453503_Avramenko.API.UseCases;

public sealed record GetListOfPets(
    string? NormalizedSpeciesName,
    int PageNo = 1,
    int PageSize = 3)
    : IRequest<ResponseData<ListModel<Pet>>>;

public class GetListOfPetsHandler(AppDbContext db)
    : IRequestHandler<GetListOfPets, ResponseData<ListModel<Pet>>>
{
    readonly int _maxPageSize = 20;
    
    public async Task<ResponseData<ListModel<Pet>>> Handle(GetListOfPets request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize <=  _maxPageSize ? request.PageSize : _maxPageSize;

        var count = await db.Pets
            .Include(p => p.Species)
            .CountAsync(p => request.NormalizedSpeciesName == null
                            || p.Species!.NormalizedName.Equals(request.NormalizedSpeciesName));;
        
        var totalPages = (int) Math.Ceiling((double)count / pageSize);
        
        var items = await db.Pets
            .AsQueryable()
            .Where(p => request.NormalizedSpeciesName == null
                        || (p.Species.NormalizedName.Equals(request.NormalizedSpeciesName)))
            .Skip((request.PageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var data = new ListModel<Pet>()
        {
            Items = items,
            CurrentPage = request.PageNo,
            TotalPages = totalPages
        };

        var result = ResponseData<ListModel<Pet>>.Success(data);

        return ResponseData<ListModel<Pet>>.Success(data);
    }
}