using System;

namespace Task10.Infrastructure.CustomAttribures
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ValidatePropertyAttribute : Attribute
    {
    }
}
