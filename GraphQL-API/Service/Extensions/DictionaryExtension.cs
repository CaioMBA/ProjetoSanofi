using Dapper;

namespace Service.Extensions
{
    public static class DictionaryExtension
    {
        public static DynamicParameters ToDynamicParameters(this Dictionary<string, object> obj)
        {
            var parameters = new DynamicParameters();
            foreach (var kv in obj)
            {
                var value = kv.Value;
                if (value != null && !value.ToString().ToLower().StartsWith("in") && !value.ToString().ToLower().StartsWith("between"))
                {
                    parameters.Add($"@{kv.Key}", value.ToString().GetSQLValue());
                }
            }

            return parameters;
        }

        public static string ToSQLWhere(this Dictionary<string, object> obj)
        {
            var sWhereSQL = string.Join(
                " and ",
                obj
                    .Where(kv => kv.Value != null)
                    .Select(kv => $"{kv.Value.ToString().ToSQLWhere(kv.Key)}") // Corrigindo a chamada do método
            );

            if (String.IsNullOrEmpty(sWhereSQL.Trim()))
            {
                return "";
            }

            return $" where {sWhereSQL}";
        }
    }
}
