using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using OptiRiskAI.Authors;
using OptiRiskAI.Books;
namespace OptiRiskAI.Web;
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OptiRiskAIWebMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);
    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OptiRiskAIAuthorDtoToCreateUpdateAuthorDtoMapper : MapperBase<AuthorDto, CreateUpdateAuthorDto>
{
    public override partial CreateUpdateAuthorDto Map(AuthorDto source);
    public override partial void Map(AuthorDto source, CreateUpdateAuthorDto destination);
}
