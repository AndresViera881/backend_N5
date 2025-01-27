namespace N5Permissions.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}