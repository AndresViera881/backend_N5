using ErrorOr;
using FluentAssertions;
using Moq;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Application.Permissions.Commands.ModifyPermission;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.UnitTests
{
    public class ModifyPermissionCommandHandlerTests
    {
        private readonly Mock<IPermisosRepository> _mockPermisosRepository;
        private readonly Mock<ITipoPermisosRepository> _mockTipoPermisosRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ModifyPermissionCommandHandler _handler;

        public ModifyPermissionCommandHandlerTests()
        {
            _mockPermisosRepository = new Mock<IPermisosRepository>();
            _mockTipoPermisosRepository = new Mock<ITipoPermisosRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _handler = new ModifyPermissionCommandHandler(
                _mockPermisosRepository.Object,
                _mockTipoPermisosRepository.Object,
                _mockUnitOfWork.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundError_When_Permiso_Does_Not_Exist()
        {
            //Arrange
            var command = new ModifyPermissionCommand(1, "Andres", "Viera", 1);

            _mockPermisosRepository
                .Setup(repo => repo.GetPermisosByIdAsync(command.PermisoId))
                .ReturnsAsync((Permiso)null); //simulo que no hay permisos

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            result.FirstError.Description.Should().Be("No se encontró el permiso con ID 1.");
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundError_When_TipoPermiso_Does_Not_Exist()
        {
            //Arrange
            var command = new ModifyPermissionCommand(1, "Andres", "Viera", 2);
            var permiso = new Permiso("Juan", "Salinas", new TipoPermiso(1, "Vacaciones"));

            _mockPermisosRepository
                .Setup(repo => repo.GetPermisosByIdAsync(command.PermisoId))
                .ReturnsAsync(permiso);

            _mockTipoPermisosRepository
                .Setup(repo => repo.GetTipoPermisoByIdAsync(command.TipoPermisoId))
                .ReturnsAsync((TipoPermiso)null);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            result.FirstError.Description.Should().Be("No se encontró el tipo de permiso con ID 2.");
        }

        [Fact]
        public async Task Handle_Should_Update_Permiso_Successfully()
        {
            //Arrange
            var command = new ModifyPermissionCommand(1, "Andres", "Viera", 1);

            var permiso = new Permiso("Juana", "Lopez", new TipoPermiso(1, "Vacaciones"));
            var tipoPermiso = new TipoPermiso(2, "Enfermedad");

            _mockPermisosRepository
                .Setup(repo => repo.GetPermisosByIdAsync(command.PermisoId))
                .ReturnsAsync(permiso);

            _mockTipoPermisosRepository
                .Setup(repo => repo.GetTipoPermisoByIdAsync(command.TipoPermisoId))
                .ReturnsAsync(tipoPermiso);

            _mockPermisosRepository
                .Setup(repo => repo.UpdatePermisoAsync(command.PermisoId, It.IsAny<Permiso>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(uow => uow.CommitChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.NombreEmpleado.Should().Be(command.NombreEmpleado);
            result.Value.ApellidoEmpleado.Should().Be(command.ApellidoEmpleado);
            result.Value.TipoPermiso.Should().Be(tipoPermiso);

            _mockPermisosRepository.Verify(repo => repo.UpdatePermisoAsync(command.PermisoId, It.IsAny<Permiso>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitChangesAsync(), Times.Once);
        }
    }
}