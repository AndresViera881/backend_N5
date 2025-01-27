using ErrorOr;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Common.Interfaces;

public interface IElasticsearchService
{
    Task<ErrorOr<bool>> IndexPermissionAsync(Permiso permiso);
}