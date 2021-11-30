using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nydus.EntityHelper;

public class PageHelper<TEntity>
{
    // private readonly int _pageSize = 20;
    //
    // private readonly PageHelperSorter<TEntity> _sorter;
    //
    // public PageHelper()
    // {
    //     _sorter = new PageHelperSorter<TEntity>();
    // }
    //
    // public string EncodeCursor<TFilter>(PageCursor<TFilter, TEntity> cursor) where TFilter : IFilter<TEntity>
    // {
    //     var serialized = JsonSerializer.Serialize(cursor);
    //     var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(serialized);
    //     return Convert.ToBase64String(plainTextBytes);
    // }
    //
    // public PageCursor<TFilter, TEntity> DecodeCursor<TFilter>(string cursor) where TFilter : IFilter<TEntity>
    // {
    //     var base64EncodedBytes = Convert.FromBase64String(cursor);
    //     var decoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    //
    //     return JsonSerializer.Deserialize<PageCursor<TFilter, TEntity>>(decoded);
    // }
    //
    // public PageResult<TEntity> GetPaged<TFilter>(string cursor, IQueryable<TEntity> queryable)
    //     where TFilter : IFilter<TEntity>
    // {
    //     var decoded = DecodeCursor<TFilter>(cursor);
    //
    //     return GetPaged(decoded, queryable);
    // }
    //
    // public PageResult<TEntity> GetPaged<TFilter>(PageCursor<TFilter, TEntity> cursor, IQueryable<TEntity> queryable)
    //     where TFilter : IFilter<TEntity>
    // {
    //     if (cursor.Filter != null)
    //     {
    //         queryable = cursor.Filter.Apply(queryable);
    //     }
    //
    //     queryable = _sorter.Sort(queryable, cursor.Order, cursor.LastItem);
    //
    //     var count = queryable.Count();
    //     var items = queryable.Take(_pageSize)
    //         .ToList();
    //
    //     string nextPage = null;
    //
    //     if (count > _pageSize)
    //     {
    //         var nextCursor = new PageCursor<TFilter, TEntity>()
    //         {
    //             Filter = cursor.Filter,
    //             Order = cursor.Order,
    //             LastItem = _sorter.ExtractKey(items.Last())
    //         };
    //         nextPage = EncodeCursor(nextCursor);
    //     }
    //
    //     return new PageResult<TEntity>()
    //     {
    //         Content = items,
    //         PageInfo = new PageInfo()
    //         {
    //             NextPage = nextPage
    //         }
    //     };
    // }

    private class PageHelperSorter<TPage>
    {
        private readonly PropertyInfo _property;

        public PageHelperSorter()
        {
            _property = typeof(TPage).GetProperties()
                .FirstOrDefault(p => p.Name == "Id");

            if (_property == null) throw new Exception($"Could not find sort property for type {typeof(TPage).Name}");
        }

        private Expression<Func<TEntity, bool>> GreaterThanExpression(List<KeyProperty> key)
        {
            var value = Get(_property, key).Value;
            if (value == null) throw new Exception($"The key value {_property.Name} is null");

            var param = Expression.Parameter(typeof(TEntity), _property.Name);
            var expression = Expression.LessThan(
                Expression.Property(param, _property),
                Expression.Constant(value));

            return Expression.Lambda<Func<TEntity, bool>>(expression, param);
        }

        public List<KeyProperty> ExtractKey(TPage obj)
        {
            var value = _property.GetValue(obj);
            if (value == null)
                throw new Exception($"The object of type {typeof(TPage).Name} sort param {_property.Name} is null");

            var key = new List<KeyProperty>
            {
                new()
                {
                    Name = _property.Name,
                    Type = _property.PropertyType.GetKeyType(),
                    Value = value,
                },
            };

            return key;
        }


        public KeyProperty Get(PropertyInfo property, List<KeyProperty> key)
        {
            return key.FirstOrDefault(k => k.Name.Equals(property.Name)) ??
                   throw new Exception($"Could not find property {property.Name} on provided keys");
        }
    }
}

[JsonConverter(typeof(KeyPropertyConverter))]
public class KeyProperty
{
    public string Name { get; set; }
    public KeyType Type { get; set; }
    public object Value { get; set; }
}

public enum KeyType
{
    Int,
    String,
}

public static class TypeExtension
{
    public static KeyType GetKeyType(this Type type)
    {
        if (type == typeof(int)) return KeyType.Int;
        if (type == typeof(string)) return KeyType.String;
        throw new NotSupportedException();
    }
}

public class KeyPropertyConverter : JsonConverter<KeyProperty>
{
    public override KeyProperty Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();

        var property = new KeyProperty();
        KeyType? type = null;
        object value = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                if (type == null || value == null) throw new JsonException();

                property.Type = type.Value;
                switch (property.Type)
                {
                    case KeyType.Int:
                        property.Value = (int)value;
                        break;
                    case KeyType.String:
                        property.Value = (string)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return property;
            }


            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Expected PropertyName token");

            var propName = reader.GetString();
            reader.Read();

            switch (propName)
            {
                case nameof(KeyProperty.Name):
                    property.Name = reader.GetString();
                    break;
                case nameof(KeyProperty.Type):
                    type = (KeyType)reader.GetInt32();
                    break;
                case nameof(KeyProperty.Value):
                    // TODO this is ok only cuz we support two types of keys: Integers and strings
                    value = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : reader.GetString();
                    break;
            }
        }

        throw new JsonException("Expected EndObject token");
    }


    public override void Write(
        Utf8JsonWriter writer,
        KeyProperty property,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(KeyProperty.Name), property.Name);
        writer.WriteNumber(nameof(KeyProperty.Type), (int)property.Type);
        switch (property.Type)
        {
            case KeyType.Int:
                writer.WriteNumber(nameof(KeyProperty.Value), (int)property.Value);
                break;
            case KeyType.String:
                writer.WriteString(nameof(KeyProperty.Value), (string)property.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        writer.WriteEndObject();
    }
}