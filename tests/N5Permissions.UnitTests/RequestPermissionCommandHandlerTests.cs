using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Application.Permissions.Commands.RequestPermission;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.UnitTests
{
    public class RequestPermissionCommandHandlerTests
    {
        private readonly Mock<IPermisosRepository> _mockPermisosRepository;
        private readonly Mock<ITipoPermisosRepository> _mockTipoPermisosRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<RequestPermissionCommandHandler>> _mockLogger;
        private readonly RequestPermissionCommandHandler _handler;

        public RequestPermissionCommandHandlerTests()
        {
            _mockPermisosRepository = new Mock<IPermisosRepository>();
            _mockTipoPermisosRepository = new Mock<ITipoPermisosRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<RequestPermissionCommandHandler>>();

            _handler = new RequestPermissionCommandHandler(
                _mockPermisosRepository.Object,
                _mockTipoPermisosRepository.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundError_When_TipoPermiso_Does_Not_Exist()
        {
            //Arrange
            var command = new RequestPermissionCommand("Andres", "Viera", 99);//Un tipo de permiso inexistente

            _mockTipoPermisosRepository
                .Setup(repo => repo.GetTipoPermisoByIdAsync(command.TipoPermisoId))
                .ReturnsAsync((TipoPermiso)null);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            result.FirstError.Description.Should().Be("No se encontró el tipo de permiso con ID 99, utilice un TipoPermisoId 1,2 o 3 (data semilla).");
        }

        [Fact]
        public async Task Handle_Should_Create_Permiso_Successfully()
        {
            //Arrange
            var command = new RequestPermissionCommand("Andres", "Viera", 1);//Un tipo de permiso que si existe


            var tipoPermiso = new TipoPermiso(1, "Vacaciones");
            var permiso = new Permiso(command.NombreEmpleado, command.ApellidoEmpleado, tipoPermiso);

            _mockTipoPermisosRepository
                .Setup(repo => repo.GetTipoPermisoByIdAsync(command.TipoPermisoId))
                .ReturnsAsync(tipoPermiso);

            _mockPermisosRepository
                .Setup(repo => repo.CreatePermisoAsync(It.IsAny<Permiso>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(uow => uow.CommitChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.NombreEmpleado.Should().Be(command.NombreEmpleado);
            result.Value.ApellidoEmpleado.Should().Be(command.ApellidoEmpleado);
            result.Value.TipoPermiso.Should().Be(tipoPermiso);

            _mockPermisosRepository.Verify(repo => repo.CreatePermisoAsync(It.IsAny<Permiso>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitChangesAsync(), Times.Once);
        }
    }
}
