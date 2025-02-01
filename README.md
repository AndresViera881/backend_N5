# backend_N5
# Permisos API REST 🚀

![.NET 9](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2023/03/asp_blog_image.png)

Este es el Coding Challenge de **N5** hecho con **.NET 8**.

## Características principales

- **Escalabilidad**: ✅ Clean Architecture.
- **Resiliencia**: Implementación de Results Pattern en los responses.
- **Buenas prácticas**: Patrones repository, unit of work, CQRS, Configuration, SOLID, Inyeccion de dependencias, elastic search.

## **Tecnologías**
[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/entity-framework-core)](https://learn.microsoft.com/es-es/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red)](https://www.microsoft.com/en-us/sql-server)
[![Elastic Search](https://img.shields.io/badge/elastic-search-green)](https://www.elastic.co/)
[![Elastic Search](https://img.shields.io/badge/docker-container-blue)](https://www.docker.com/)

## **Primeros pasos**

1. **Actualizar la cadena de conexión**
   * Navega hasta appsettings.development.json.
   * Actualiza la cadena de conexión si fuera necesario.   

2. **Ejecutar el proyecto:**
   * El proyecto tiene configurado migraciones automáticas, así que en la priemra ejecución se creará la base de datos y sus objetos.
   * Se ha añadido dos PermisoType como data semilla con ids: 1 (vacaciones), 2 (enfermedad) o 3 (maternidad) (verificar en la base de datos si lo requiere).

3. **Elastic search**
	* Navega hasta appsettings.development.json, allí esta configurado el puerto para el servicio de elasticsearch.
    * Levantar contenedor con Elastic search, para eso ejecutar en el terminal:
	 ```bash
     docker run --name elasticsearch -d -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "xpack.security.enabled=false" -e "xpack.security.http.ssl.enabled=false" docker.elastic.co/elasticsearch/elasticsearch:8.6.2
     ```	
	* Esperar a que inicie completamente el servicio y luego probar ingresando a http://localhost:9200/
	* Cuando se ejecuta el endpoint POST /api/permisos además de la BD se guardará también un índice en elasticsearch
	* Puedes obtener todos los registros de elasticsearch en http://localhost:9200/_search o localizar un registro en particular con un query param en: http://localhost:9200/_search?q=nombreEmpleado:xxx donde xxx es un el nombre del empleado usado en el POST.
