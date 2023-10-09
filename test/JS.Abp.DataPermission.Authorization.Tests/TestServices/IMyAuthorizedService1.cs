namespace JS.Abp.DataPermission.Authorization.Tests.TestServices;

public interface IMyAuthorizedService1
{
    Task<int> Anonymous();

    Task<int> AnonymousAsync();

    Task<int> ProtectedByClass();

    Task<int> ProtectedByClassAsync();
}
