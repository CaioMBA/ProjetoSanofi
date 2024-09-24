using GraphQL.Types;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extensions
{
    public static class TypeExtension
    {
        public static IGraphType GetGraphQLType(this Type propertyType)
        {
            if (propertyType == typeof(string))
                return new StringGraphType();
            else if (propertyType == typeof(int))
                return new IntGraphType();
            else if (propertyType == typeof(long))
                return new LongGraphType();
            else if (propertyType == typeof(float))
                return new FloatGraphType();
            else if (propertyType == typeof(double))
                return new FloatGraphType();
            else if (propertyType == typeof(bool))  // Adicione esta verificação
                return new BooleanGraphType();
            else
                return new StringGraphType();
        }
    }
}