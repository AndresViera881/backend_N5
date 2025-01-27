using ErrorOr;
using FluentAssertions;
using Moq;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Application.Permissions.Queries.GetPermissions;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.UnitTests
{
    public class GetPermissionsQueryHandlerTests
    {
        private readonly Mock<IPermisosRepository> _mockPermisosRepository;
        private readonly GetPermissionsQueryHandler _handler;

        public GetPermissionsQueryHandlerTests()
        {
            _mockPermisosRepository = new Mock<IPermisosRepository>();
            _handler = new GetPermissionsQueryHandler(_mockPermisosRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ListOfPermisos_When_PermisosExist()
        {
            //Arrange
            var permisos = new List<Permiso>
            {
                new Permiso("Andres", "Viera", new TipoPermiso(1, "Vacaciones")),
                new Permiso("Juan", "Lopez", new TipoPermiso(2, "Enfermedad")),
                new Permiso("Yasmin", "Azabache", new TipoPermiso(3, "Maternidad")),
            };//creo mis permisos validos

            _mockPermisosRepository
                .Setup(repo => repo.GetPermisosAsync())
                .ReturnsAsync(permisos);

            var query = new GetPermissionsQuery();

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(permisos.Count);
            result.Value.Should().BeEquivalentTo(permisos);

            _mockPermisosRepository.Verify(repo => repo.GetPermisosAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFoundError_When_NoPermisosExist()
        {
            //Arrange
            _mockPermisosRepository
                .Setup(repo => repo.GetPermisosAsync())
                .ReturnsAsync(new List<Permiso>()); //Aqui retorno como si no hubierapermisos

            var query = new GetPermissionsQuery();

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            result.FirstError.Description.Should().Be("No se encontró ningun permiso.");

            _mockPermisosRepository.Verify(repo => repo.GetPermisosAsync(), Times.Once);
        }
    }
}
