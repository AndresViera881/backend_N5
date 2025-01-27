using MediatR;
using ErrorOr;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Application.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery() : IRequest<ErrorOr<List<Permiso>>>;