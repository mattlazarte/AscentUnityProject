using System;

namespace Ascent.Managers.Pools
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IncludeInPoolManagerAttribute : Attribute
    {
    }
}
