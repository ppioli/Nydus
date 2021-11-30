using System;

namespace Nydus.EntityHelper.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
    {
    }


    public EntityNotFoundException(Type type, object id) : base($"Could not find entity {type.Name} with id {id}")
    {
    }
}