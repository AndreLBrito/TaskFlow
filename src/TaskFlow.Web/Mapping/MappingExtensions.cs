using Mapster;

namespace TaskFlow.Web.Mapping;

public static class MappingExtensions
{
    public static TDestination To<TDestination>(this object source)
    {
        return source.Adapt<TDestination>();
    }
}