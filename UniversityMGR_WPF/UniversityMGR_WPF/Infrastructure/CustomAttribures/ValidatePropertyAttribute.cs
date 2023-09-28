using System;

namespace UniversityMGR_WPF.Infrastructure.CustomAttribures
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ValidatePropertyAttribute : Attribute
    {
    }
}
