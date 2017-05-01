
namespace System.Linq.Expressions
{
    using Collections.Generic;

    public static class ExpressionExtensions
    {
        public static string MemberPath(this Expression expression)
        {
            if (true == expression is UnaryExpression)
            {
                return (expression as UnaryExpression).Operand.MemberPath();
            }

            if (false == expression is MemberExpression)
            {
                return string.Empty;
            }

            var memberExpression = expression as MemberExpression;
            if (memberExpression.Expression is ParameterExpression)
            {
                return memberExpression.Member.Name;
            }

            return string.Format("{0}.{1}", memberExpression.Expression.MemberPath(), memberExpression.Member.Name);
        }

        public static IEnumerable<string> MemberPaths(this IEnumerable<Expression> expressions)
        {
            if (null == expressions)
            {
                yield break;
            }

            foreach (var expression in expressions)
            {
                yield return expression.MemberPath();
            }
        }
    }
}
