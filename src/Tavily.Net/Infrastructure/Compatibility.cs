#if NETSTANDARD2_0 || NET472
namespace System.Runtime.CompilerServices;

internal static class IsExternalInit { }

[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Method
        | AttributeTargets.Property,
    Inherited = false
)]
internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute
{
    public string FeatureName { get; } = featureName;
}

[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Field
        | AttributeTargets.Property,
    Inherited = false
)]
internal sealed class RequiredMemberAttribute : Attribute { }
#endif
