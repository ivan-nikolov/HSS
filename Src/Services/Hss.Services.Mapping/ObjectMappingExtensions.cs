namespace Hss.Services.Mapping
{
    using System;

    public static class ObjectMappingExtensions
    {
        public static TDestination To<TDestination>(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return AutoMapperConfig.MapperInstance.Map<TDestination>(obj);
        }
    }
}
