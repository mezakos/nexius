using Nexius.API.Contracts.Requests;
using Nexius.API.Contracts.Responses;
using Nexius.API.DAL.Models;
using Riok.Mapperly.Abstractions;

namespace Nexius.API
{
    [Mapper]
    public partial class Mapper
    {
        public partial TodoItem Map(TodoItemRequest source);

        public partial TodoItem Map(NewTodoItemRequest source);

        public partial TodoItemResponse Map(TodoItem source);

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Map<TDestination>(source);
        }

        public TDestination Map<TDestination>(dynamic? source)
        {
            var method = GetType()
                .GetMethods()
                .Where(x => x.ReturnType == typeof(TDestination)
                            && x.GetParameters().Length == 1
                            && x.GetParameters()[0].ParameterType == source?.GetType())
                .FirstOrDefault();
            if (method is null)
            {
                throw new NotImplementedException();
            }

            return (TDestination)method.Invoke(this, [source])!;
        }
    }
}
