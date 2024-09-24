using Dapper;
using System.Data;
using System.Text.Json;

public class JsonTypeHandler<T> : SqlMapper.TypeHandler<List<T>>
{
    public override void SetValue(IDbDataParameter parameter, List<T> value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }

    public override List<T> Parse(object value)
    {
        if (value == null)
        {
            return new List<T>();
        }
        try
        {
            return JsonSerializer.Deserialize<List<T>>(value.ToString())!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao deserializar o valor em : {value}", ex);
        }
    }
}
